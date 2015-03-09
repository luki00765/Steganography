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

		private void EnctyptMethod(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Image Files (*.bmp;*.jpeg;*.png;*.jpg)|*.bmp;*.jpeg;*.png;*jpg";
			dlg.Title = "Open Image File";
			Nullable<bool> result = dlg.ShowDialog();

			if(result == true)
			{
				string path = dlg.FileName;
				ImageSource imgSource = new BitmapImage(new Uri(path));
				MyImage.Source = imgSource;
				bmp = (BitmapImage)MyImage.Source;

				if(MyImage.Source != null)
				{
					TglButton.Visibility = System.Windows.Visibility.Visible;
					// ukryj wiadomość
				}
			}
		}

		private void DecryptMethod(object sender, RoutedEventArgs e)
		{
			if(MyImage.Source != null)
			{
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.Filter = "Image Files (*.bmp;*.jpeg;*.png;*.jpg)|*.bmp;*.jpeg;*.png;*jpg";
				dlg.Title = "Save Image File";
				Nullable<bool> result = dlg.ShowDialog();

				if(result == true)
				{
					FileStream fs = new FileStream(dlg.FileName, FileMode.OpenOrCreate);
					// tablica dzięki której wyciągnę rozsrzeszenie pliku
					string[] splitExtension = dlg.SafeFileName.Split(new Char[] { '.' });
					
					try
					{
						switch (splitExtension[1])
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

		private void CloseApp(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

	}
}
