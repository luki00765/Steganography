using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Steganography
{
	class SteganographyHelper
	{
		public static BitmapImage Encrypt(BitmapImage bmp, string text)
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
			int strideToBinary = bmpWidth * 3;
			string[] binaryPixels = new string[bmpHeight * strideToBinary];

			for (int i = 0; i < bmpHeight; i++)
			{
				for (int j = 0; j < bmpWidth; j++) // pętla idzie po szerokości obrazka ----->
				{
					int index = i * stride + 4 * j; // wyznaczamy indeks w tablicy jednowymiarowej "pixels". Chodzimy co 4 ponieważ w każdym pikselu mamy 4 składkowe RGBA
					R = pixels[index];
					G = pixels[index + 1];
					B = pixels[index + 2];
					//A = pixels[index + 3];

					int indexToBinaray = i * strideToBinary + 3 * j;
					binaryPixels[indexToBinaray] = decToBin(R);
					binaryPixels[indexToBinaray + 1] = decToBin(G);
					binaryPixels[indexToBinaray + 2] = decToBin(B);
					//binaryPixels[index + 3] = decToBin(A);
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
			int[] tableWithMessage = BinToDec(binaryPixels);
			writeBitmap.WritePixels(bmp.SourceRect, tableWithMessage, stride, 0); // NIE WIADOMO CZY TO DZIAŁA !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			
			byte[] tmp22 = new byte[size]; // W TAMACH TESTÓW
			writeBitmap.CopyPixels(tmp22, stride, 0);

			return bmp;

			//TODO:
			//funkcja, która zmienia ostatnie wartości w każdej składowej na pierwszą wartość binarną danej litery
			// potrzebna funkcja SetPixels
			// długość całej wiadomości zapisz w ostatnim pikselu w składowej ALPHA. Przezroczystość ulegnie dużej zmianie; jednakże jest to lepsze niż zamiana koloru.
			// za ukrytą wiadomością wstaw null: 0000 0000 później sprawdzaj 0,8,16,32,64  % 8 == 0 warunek konieczny, musi być 8 zer
		}

		public static void Decrypt(BitmapImage bmp)
		{
			//odczytaj długość wiadomości w ostatnim pikselu; iteruj od początku obrazka po długości wiadomości.
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

		public static int[] BinToDec(string[] binaryPixels)
		{
			int[] tableWithMessage = new int[binaryPixels.Length];
			for (int i = 0; i < binaryPixels.Length; i++ )
			{
				string tmp = binaryPixels[i];
				tableWithMessage[i] = Convert.ToInt32(tmp, 2);
			}
			return tableWithMessage;
		}

		public static string[] HideMessageInImage(string[] binaryTab, List<string> message)
		{
			int j = 0;
			for (int i = 0; i < binaryTab.Length; ) // pobierz długość tablicy z pikselami
			{
				for (; j < message.Count; j++ ) // j - ile jest liter to ukrycia
				{
					var tmp = message[j];
					for (int k = 0; k < tmp.Length; k++ ) // k - pobierz długość wiadomości (8 zawsze) a następnie zastosuj algorytm LSB 
					{
						if(message.Count * tmp.Length > binaryTab.Length) // jeżeli ilość wiadomości pomnożona przez długość będzie większa od dosępnych pikseli to oznacza, że nie ma miejsca w tablicy aby pomieścić wiadomość
						{
							MessageBox.Show("Message is to long. Change the image for a larger or write smaller text");
							return binaryTab;
						}
						// Algorytm LSB. ostatni element w tablicy binarnej zamieniamy na pierwsze element liczby binarnej, następnie kolejny itp.
						/*Przykład:  tablica 11111111 11111111 11111111		wiadomość: 00 
						 *  Rezultat: tablica 11111110 11111110 11111111
						 * wybieramy najmniej znaczące bity i wstawiamy w nie wiadomość
						 */
						string tmpString = binaryTab[i];
						tmpString = tmpString.Remove(7) + tmp[k];
						binaryTab[i] = tmpString;
						i++;
					}
				}

				// po ukryciu wiadomości w obrazie zapisz po niej null czyli 0000 0000 aby później móc odkodować wiadomość.
				for (int l = 0; l < 8; l++ )
				{
					string tmpString = binaryTab[i];
					tmpString = tmpString.Remove(7) + "0";
					binaryTab[i] = tmpString;
					i++;
				}
				break;
			}

			return binaryTab;
		}

	}
}
