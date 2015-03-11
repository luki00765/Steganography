using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Steganography
{
	/// <summary>
	/// Logika interakcji dla klasy MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private BitmapImage bmp = null;
		public MainWindow()
		{
			InitializeComponent();
		}

		private void OpenImage(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Image Files (*.bmp;*.jpeg;*.png;*.jpg)|*.bmp;*.jpeg;*.png;*jpg";
			dlg.Title = "Open Image File";
			Nullable<bool> result = dlg.ShowDialog();

			if (result == true)
			{
				string path = dlg.FileName;
				ImageSource imgSource = new BitmapImage(new Uri(path));
				MyImage.Source = imgSource;
				bmp = (BitmapImage)MyImage.Source;

				if (MyImage.Source != null)
				{
					TglButton.Visibility = System.Windows.Visibility.Visible;
					DecodeBtn.Visibility = System.Windows.Visibility.Visible;
				}
			}
		}

		private void SaveImage(object sender, RoutedEventArgs e)
		{
			if (MyImage.Source != null)
			{
				SaveFileDialog dlg = new SaveFileDialog();
				string extensionImage = CheckTheExtensionOnImageFile(bmp);
				dlg.Filter = "Image File (*." + extensionImage + ")|*." + extensionImage;
				dlg.Title = "Save Image File";
				Nullable<bool> result = dlg.ShowDialog();

				if (result == true)
				{
					FileStream fs = new FileStream(dlg.FileName, FileMode.OpenOrCreate);
					try
					{
						switch (extensionImage)
						{
							case "bmp":
								{
									BmpBitmapEncoder encoder = new BmpBitmapEncoder();
									encoder.Frames.Add(BitmapFrame.Create(bmp));
									encoder.Save(fs);
									break;
								}
							case "jpg":
							case "jpeg":
								{
									JpegBitmapEncoder encoder = new JpegBitmapEncoder();
									encoder.Frames.Add(BitmapFrame.Create(bmp));
									encoder.Save(fs);
									break;
								}
							case "png":
								{
									PngBitmapEncoder encoder = new PngBitmapEncoder();
									encoder.Frames.Add(BitmapFrame.Create(bmp));
									encoder.Save(fs);
									break;
								}
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
					fs.Close();
				}


			}
			else
			{
				MessageBox.Show("You should use Encrypt function");
			}
		}

		private string CheckTheExtensionOnImageFile(BitmapImage image)
		{
			string[] splitExtension = image.UriSource.AbsolutePath.Split(new Char[] { '.' });
			return splitExtension[1];
		}

		private void CloseApp(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void AboutApp(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Praca magisterska. Uniwersytet Gdański 2015");
		}

		private void DecodeMethod(object sender, RoutedEventArgs e)
		{
			PBDecode.Visibility = System.Windows.Visibility.Visible;
			PBDecode.Value += 5;
			if(PBDecode.Value == 100)
			{
				MessageBox.Show("Done2");
			}
		}

		private void HideMethod(object sender, RoutedEventArgs e)
		{
			/*PBHide.Visibility = System.Windows.Visibility.Visible;
			PBHide.Value += 5;
			if(PBHide.Value == 100)
			{
				MessageBox.Show("Done");
			}*/
			SteganographyHelper.Encrypt(bmp, MessageText.Text);
		}
	}
}
