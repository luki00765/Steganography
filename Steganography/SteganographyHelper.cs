using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Steganography
{
	class SteganographyHelper
	{
		public static bool isMessageLargerThanImage;

		public static WriteableBitmap Encrypt(BitmapImage bmp, string text)
		{
			WriteableBitmap writeBitmap = new WriteableBitmap(bmp);
			byte R = 0, G = 0, B = 0, A = 0; //red grenn blue alpha (przezroczystość)

			int bmpWidth = writeBitmap.PixelWidth; // szerokość obrazka
			int bmpHeight = writeBitmap.PixelHeight; // wysokość obrazka
			
			int stride = bmpWidth * 4; // Suma wszystkich pikseli w jednym wierszu
			int size = bmpHeight * stride; // suma wszystkich pikseli w wierszy * długość obrazka = suma wszystkich pikseli w obrazie
			byte[] pixels = new byte[size];
			writeBitmap.CopyPixels(pixels, stride, 0);

			List<string> listBinaryText = new List<string>();
			string[] binaryPixels = new string[size];

			for (int i = 0; i < bmpHeight; i++)
			{
				for (int j = 0; j < bmpWidth; j++) // pętla idzie po szerokości obrazka ----->
				{
					int index = i * stride + 4 * j; // wyznaczamy indeks w tablicy jednowymiarowej "pixels". Chodzimy co 4 ponieważ w każdym pikselu mamy 4 składkowe RGBA
					R = pixels[index];
					G = pixels[index + 1];
					B = pixels[index + 2];
					A = pixels[index + 3];

					binaryPixels[index] = decToBin(R);
					binaryPixels[index + 1] = decToBin(G);
					binaryPixels[index + 2] = decToBin(B);
					binaryPixels[index + 3] = decToBin(A);
				}
			}

			// przekształć tekst na postać binarną (8 bit) i dodaj do listy
			foreach(char character in text)
			{
				string binaryCharacter = System.Convert.ToString(character, 2);
				if (binaryCharacter.Length < 8)
				{
					string tmp = binaryCharacter;
					binaryCharacter = "";
					for (int i = 0; i < 8 - tmp.Length; i++)
					{
						binaryCharacter += "0";
					}
					binaryCharacter += tmp;
				}
				listBinaryText.Add(binaryCharacter);
			}

			binaryPixels = HideMessageInImage(binaryPixels, listBinaryText);

			if (isMessageLargerThanImage == true)
			{
				return writeBitmap;
			}

			byte[] tableWithMessage = BinToByteArray(binaryPixels);

			var rect = new Int32Rect(0, 0, bmpWidth, bmpHeight);
			writeBitmap.WritePixels(rect, tableWithMessage, stride, 0); // Nadpisz oryginalny obraz, obrazem z ukrytą wiadomością.
			return writeBitmap;
		}

		public static string decToBin(int number)
		{
			string decToBin = Convert.ToString(number, 2);
			string result = "";
			if(decToBin.Length < 8)
			{
				for (int i = 0; i < 8 - decToBin.Length; i++ )
				{
					result += "0";
				}
				result += decToBin;
				return result;
			}
			return decToBin;
		}

		public static byte[] BinToByteArray(string[] binaryPixels)
		{
			byte[] tableWithMessage = new byte[binaryPixels.Length];
			for (int i = 0; i < binaryPixels.Length; i++ )
			{
				string tmp = binaryPixels[i];
				tableWithMessage[i] = Convert.ToByte(tmp.Substring(0, 8), 2);
			}
			return tableWithMessage;
		}

		public static string[] HideMessageInImage(string[] binaryTab, List<string> message)
		{
			int j = 0;
			int checkALPHApixel = 3;
			for (int i = 0; i < binaryTab.Length; ) // pobierz długość tablicy z pikselami
			{
				for (; j < message.Count; j++ ) // j - ile jest liter do ukrycia
				{
					var tmp = message[j];
					for (int k = 0; k < tmp.Length; k++ ) // k - pobierz długość wiadomości (8 zawsze) a następnie zastosuj algorytm LSB 
					{
						if(message.Count * tmp.Length + 8 >= binaryTab.Length - 2) // jeżeli ilość wiadomości pomnożona przez długość będzie większa od dosępnych pikseli to oznacza, że nie ma miejsca w tablicy aby pomieścić wiadomość. 8 - to jest dodatkowe miejsce na null, ponieważ będzie on potrzebny przy dekodowaniu wiadmości. Od długości tablicy zostało odjęte 2 ponieważ na każde 8bitów (RGB 0000 0000 ) przypadają dwie składowe ALPHA.
						{
							//MessageBox.Show("Message is to long. Change the image for a larger or write smaller text");
							isMessageLargerThanImage = true;
							return binaryTab;
						}
						// Algorytm LSB. ostatni element w tablicy binarnej zamieniamy na pierwsze element liczby binarnej, następnie kolejny itp.
						/*Przykład:  tablica 11111111 11111111 11111111		wiadomość: 00 
						 *  Rezultat: tablica 11111110 11111110 11111111
						 * wybieramy najmniej znaczące bity i wstawiamy w nie wiadomość
						 */
						if(i == checkALPHApixel) // warunek, który pomija wszystkie składowe APLHA - nie obługujemy ich.
						{
							checkALPHApixel += 4;  // jeżeli weszliśmy do warunku to zwiększ wartość co 4, ponieważ za 4 iteracje znowu będzie występować ALPHA
							i++; // pomin wartość APLHA w tablicy - na tej wartości nie będą dokonywane żadne zmiany
							k--; // k odpowiada za iteracje po danej literze zapisanej binarnie. Zmniejszamy k ponieważ nie chcemy pominąć żadnej litery; k powróci do swojego stanu zaraz po zakończeniu tego warunku
							continue;
						}
						else
						{
							string tmpString = binaryTab[i];
							tmpString = tmpString.Remove(7) + tmp[k];
							binaryTab[i] = tmpString;
							i++;
						}
						
					}
				}

				// po ukryciu wiadomości w obrazie zapisz po niej null czyli 0000 0000 aby później móc odkodować wiadomość.
				for (int l = 0; l < 8; l++ )
				{
					if (i == checkALPHApixel)
					{
						checkALPHApixel += 4;
						i++;
						l--;
						continue;
					}
					else
					{
						string tmpString = binaryTab[i];
						tmpString = tmpString.Remove(7) + "0";
						binaryTab[i] = tmpString;
						i++;
					}
				}
				break; // jeżeli ukryjesz wiadomość i dopiszesz dodatkowo osiem zer to wyjdź z pętli.
			}

			return binaryTab;
		}

		private static List<string> MessageBinaryPixels = new List<string>(); // lista która będzie przechowywać tajną wiadomość
		public static string Decrypt(BitmapImage bmp)
		{
			//odczytaj długość wiadomości w ostatnim pikselu; iteruj od początku obrazka po długości wiadomości.
			// za ukrytą wiadomością wstaw null: 0000 0000 później sprawdzaj 0,8,16,32,64  % 8 == 0 warunek konieczny, musi być 8 zer
			MessageBinaryPixels.Clear();
			string message = "";
			WriteableBitmap writeBitmap = new WriteableBitmap(bmp);
			byte R = 0, G = 0, B = 0, A = 0; //red grenn blue alpha (przezroczystość)

			int bmpWidth = writeBitmap.PixelWidth; // szerokość obrazka
			int bmpHeight = writeBitmap.PixelHeight; // wysokość obrazka

			int stride = bmpWidth * 4; // Suma wszystkich pikseli w jednym wierszu
			int size = bmpHeight * stride; // suma wszystkich pikseli w wierszy * długość obrazka = suma wszystkich pikseli w obrazie
			byte[] pixels = new byte[size];
			writeBitmap.CopyPixels(pixels, stride, 0);

			int strideBinaryPixels = bmpWidth * 3;
			int sizeBinaryPixels = bmpHeight * strideBinaryPixels;
			string[] binaryPixels = new string[sizeBinaryPixels];

			for (int i = 0; i < bmpHeight; i++)
			{
				for (int j = 0; j < bmpWidth; j++) // pętla idzie po szerokości obrazka ----->
				{
					int index = i * stride + 4 * j; // wyznaczamy indeks w tablicy jednowymiarowej "pixels". Chodzimy co 4 ponieważ w każdym pikselu mamy 4 składkowe RGBA
					R = pixels[index];
					G = pixels[index + 1];
					B = pixels[index + 2];
					A = pixels[index + 3];

					int indexBinaryPixels = i * strideBinaryPixels + 3 * j;
					binaryPixels[indexBinaryPixels] = decToBin(R);
					binaryPixels[indexBinaryPixels + 1] = decToBin(G);
					binaryPixels[indexBinaryPixels + 2] = decToBin(B);
					//binaryPixels[index + 3] = decToBin(A);
				}
			}

			bool isMessage = FindNull(binaryPixels);
			if(isMessage != true) // jeśli wiadomość zostanie odnaleziona to ją odszyfruj i zwróć, w przeciwnym wypadku zwróć inofrmacje o tym, że wiadomość nie została odnaleziona
			{
				return "Secret message NOT FOUND!";
			}

			message = ExtractTheMessageFromList(); // zwróć wiadomosć

			return message;
		}

		
		// Metoda która szuka nulla, czyli 0000 0000. Został on zapisany zaraz po ukrytej wiadomości
		public static bool FindNull(string[] binaryPixels)
		{
			int counterZero = 0;
			string tmpString = "";

			for (int i = 0; i < binaryPixels.Length; i = i + 8 ) // każda litera jest zapisana w postaci binarnej 8bitowej; dlatego pętla skacze co 8 pozycji: 0, 7, 16, 32 ... i szuka ośmiu zer
			{
				for (int j = i; j < j + 8; j++ )
				{
					tmpString = binaryPixels[j];
					tmpString = tmpString.Substring(tmpString.Length - 1, 1); // wyciągnij ostatni element z danej wartości binarnej i sprawdź czy jest zerem
					if(tmpString == "0")
					{
						counterZero++; // zliczamy zera
						if(counterZero == 8) // jeżeli uzbieramy osiem zer, to oznacza, że przed nimi znajduje się ukryta wiadomość, więc wystarczy ją teraz tylko wyciągnąć
						{
							for (int m = 0; m < i; m++ ) // wyciąganie ukrytej wiadomości; dodajemy ją do listy
							{
								MessageBinaryPixels.Add(binaryPixels[m]);
							}
							return true;
						}
					}
					else
					{
						counterZero = 0;
						break;
					}
				}
			}
			return false;
		}

		// funkcja, która wyodrębnia wiadomość z listy; następnie ją dekoduje i zwraca jako tekst
		public static string ExtractTheMessageFromList()
		{
			string message = "";
			string tmp = "";

			for (int i = 0; i < MessageBinaryPixels.Count; i++)
			{
				tmp = MessageBinaryPixels[i];
				tmp = tmp.Substring(tmp.Length - 1, 1);
				message += tmp;
			}
			//message = DecodeMessage(message);
			var data = DecodeMessage(message); // data zawiera wartość DEC ASCII każdej wartości binarnej
			var text = Encoding.ASCII.GetString(data); // DEC jest zamieniana na literę.

			for (int i = 0; i < data.Length; i++ ) // jeśli w tablicy "data" o typie Byte będzie większa wartość niż 127 to znaczy, że nia ma żadnej uktytej wiadomości.
			{
				if(data[i] > 127)
				{
					text = "";
					return text;
				}
			}

			return text;
		}

		// funkcja odszyfruje wartość binarną i zwróci w DEC
		public static Byte[] DecodeMessage(String binary)
		{
			var list = new List<Byte>();

			for (int i = 0; i < binary.Length; i += 8)
			{
				String t = binary.Substring(i, 8);

				list.Add(Convert.ToByte(t, 2));
			}

			return list.ToArray();
		}
	}
}
