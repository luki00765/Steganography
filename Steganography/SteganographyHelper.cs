using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Steganography
{
	class SteganographyHelper
	{
		public static void Encrypt(BitmapImage bmp, string text)
		{
			WriteableBitmap writeBitmap = new WriteableBitmap(bmp);
			byte R = 0, G = 0, B = 0, A = 0; //red grenn blue alpha (przezroczystość)

			int bmpWidth = writeBitmap.PixelWidth; // szerokość obrazka
			int bmpHeight = writeBitmap.PixelHeight; // wysokość obrazka
			
			int stride = bmpWidth * 4; // Suma wszystkich pikseli w jednym wierszu
			int size = bmpHeight * stride; // suma wszystkich pikseli w wierszy * długość obrazka = suma wszystkich pikseli w obrazie
			byte[] pixels = new byte[size];
			writeBitmap.CopyPixels(pixels, stride, 0);

			for (int i = 0; i < bmpHeight; i++)
			{
				for (int j = 0; j < bmpWidth; j++) // pętla idzie po szerokości obrazka ----->
				{
					int index = i * stride + 4 * j; // wyznaczamy indeks w tablicy jednowymiarowej "pixels". Chodzimy co 4 ponieważ w każdym pikselu mamy 4 składkowe RGBA
					R = pixels[index];
					G = pixels[index + 1];
					B = pixels[index + 2];
					A = pixels[index + 3];
				}
			}
		}

		public static void Decrypt(BitmapImage bmp)
		{

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

	}
}
