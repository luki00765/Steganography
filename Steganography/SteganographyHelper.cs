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
			byte R = 0, G = 0, B = 0, A = 0; //red grenn blue alpha

			int bmpWidth = writeBitmap.PixelWidth; // szerokość obrazka
			int bmpHeight = writeBitmap.PixelHeight; // wysokość obrazka
			int bitsPerPixel; //ilość kanałów na każdy piksel
			if (bmp.Format.BitsPerPixel < 8)
			{
				bitsPerPixel = 1;
			}
			else
			{
				bitsPerPixel = bmp.Format.BitsPerPixel / 8;
			}
			int stride = bmpWidth * bitsPerPixel; // Suma wszystkich pikseli w jednym wierszu
			int size = bmpHeight * stride; // suma wszystkich pikseli w wierszy * długość obrazka = suma wszystkich pikseli w obrazie
			byte[] pixels = new byte[size];
			writeBitmap.CopyPixels(pixels, stride, 0);
			byte[] message = Encoding.ASCII.GetBytes(text + "\0");
			int j = 0; // j potrzebne do iteracji wiadomości. Jest potrzebny w momencie kiedy ukrywam wiadomość omijając kanał alpha
			if (bmp.Format.BitsPerPixel >= 32)
			{
				for (int i = 0; i < message.Length * 8 + message.Length * 8 / 3; i++) // ograniczenie: ilość wiadomości pomnożona przez ich długość daje nam możliwość ukrycia jej w każdym pikselu; jednakże aby pomijać kanały alpha, należy podzielić tą długość przez 3
				{
					if (message.Length * 8 + message.Length * 8 / 3 > size) // jeżeli wiadomość jest dłuższa niż wielkość obrazu
					{
						isMessageLargerThanImage = true;
						break;
					}

					if (bmp.Format.BitsPerPixel >= 32) // dla obrazów 32-bit
					{
						if ((i % 4) == 3) //w każdej iteracji pomiń ALPHA
							continue;
					}

					bool bitMessage = (message[j / 8] & (128 >> (j % 8))) != 0; // itereujemy po każdym bicie wiadomośći
					pixels[i] = (byte)(pixels[i] & (~1)); // ostatni bit zmieniamy pod wpływem negacji. Metodą LSB - najmniej znaczących bitów
					pixels[i] |= (bitMessage) ? (byte)1 : (byte)0; // ustaw 1 dla true, 0 dla false
					j++;
				}
			}
			else
			{
				for (int i = 0; i < message.Length * 8; i++)
				{
					if (message.Length * 8 > size)
					{
						isMessageLargerThanImage = true;
						break;
					}

					bool bitMessage = (message[j / 8] & (128 >> (j % 8))) != 0; // itereujemy po każdym bicie wiadomośći
					pixels[i] = (byte)(pixels[i] & (~1)); // ostatni bit zmieniamy pod wpływem negacji. Metoda LSB - najmniej znaczących bitów
					pixels[i] |= (bitMessage) ? (byte)1 : (byte)0; // ustaw 1 dla true, 0 dla false
					j++;
				}
			}

			var rect = new Int32Rect(0, 0, bmpWidth, bmpHeight);
			writeBitmap.WritePixels(rect, pixels, stride, 0); // Nadpisz oryginalny obraz, obrazem z ukrytą wiadomością.
			return writeBitmap;
		}

		public static string Decrypt(BitmapImage bmp)
		{
			string message = "";
			byte letter = 0;
			int findNull = 1;
			int bitsPerPixel;
			if (bmp.Format.BitsPerPixel < 8)
			{
				bitsPerPixel = 1;
			}
			else
			{
				bitsPerPixel = bmp.Format.BitsPerPixel / 8;
			}
			int stride = bmp.PixelWidth * bitsPerPixel;
			int size = bmp.PixelHeight * stride;
			byte[] pixels = new byte[size];
			bmp.CopyPixels(pixels, stride, 0);

			for (int i = 0; i < size; i++)
			{
				if (bmp.Format.BitsPerPixel >= 32)
				{
					if ((i % 4) == 3) //w każdej iteracji pomiń ALPHA
						continue;
				}

				letter = (byte)((byte)(letter << 1) | (byte)(pixels[i] & 1)); // << przesunięcie bitu o 1 np. 3 << 2 czyli: 0011 << 2 = 1100. wynik 12.
				// Przeszukanie szuka litery zapisanej w postaci dziesiętnej; funkcja używa | alternatywy;  & koniunkcji

				if ((findNull % 8) == 0) // szuka nulli
				{
					message += (char)letter;

					if (letter == 0) //znak null kończący napis
					{
						message = message.Replace("\0", String.Empty); // usuń \0
						return message;
					}
					if (letter >= 128) // jeżeli litera przekorczy limit ( 128 - znaki ASCII ) to wtedy nie ma ukrytej wiadomości
					{
						return "";
					}
					letter = 0;
				}
				findNull++;
			}

			return "";
		}
	}
}
