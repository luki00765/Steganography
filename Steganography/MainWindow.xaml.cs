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
using MahApps.Metro.Controls;
using System.Threading;

namespace Steganography
{
	/// <summary>
	/// Logika interakcji dla klasy MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		public BitmapImage bmp = null;
		string path;

		public MainWindow()
		{
			InitializeComponent();
			MyImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/noImage.jpg"));
			prompt.Text = "Prompt: firstly You should load the Image";
			prompt.Foreground = Brushes.DarkRed;
		}

		private void OpenImage(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Images (*.bmp;*.jpeg;*.png;*.jpg;*.gif)|*.bmp;*.jpeg;*.png;*jpg;*gif";
			dlg.Title = "Open Image File";
			Nullable<bool> result = dlg.ShowDialog();

			if (result == true)
			{
				path = dlg.FileName;
				bmp = new BitmapImage(new Uri(path));
				MyImage.Source = bmp;

				if (MyImage.Source != null)
				{
					TglButton.Visibility = System.Windows.Visibility.Visible;
					DecodeBtn.Visibility = System.Windows.Visibility.Visible;
					MessageText.Text = "";
					prompt.Text = "";
					DecodeResult.Source = null;
				}
				else
				{
					prompt.Foreground = Brushes.DarkRed;
					prompt.Text = "Prompt: Image not load correctly";
				}
			}
		}

		private void SaveImage(object sender, RoutedEventArgs e)
		{
			if (MyImage.Source != null && bmp != null)
			{
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.Filter = "Png Image|*.png|Bitmap Image|*.bmp";
				dlg.Title = "Save Image File";
				dlg.DefaultExt = "png";
				Nullable<bool> result = dlg.ShowDialog();

				if (result == true)
				{
					FileStream fs = new FileStream(dlg.FileName, FileMode.OpenOrCreate);
					try
					{
						switch (dlg.FilterIndex)
						{
							case 1:
								{
									PngBitmapEncoder encoder = new PngBitmapEncoder();
									encoder.Frames.Add(BitmapFrame.Create(bmp));
									encoder.Save(fs);
									break;
								}
							case 2:
								{
									BmpBitmapEncoder encoder = new BmpBitmapEncoder();
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
				MessageBox.Show("You should Load Image before Saving");
			}
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
			DecodeResult.Source = null;
			MessageText.Text = SteganographyHelper.Decrypt(bmp);

			if (MessageText.Text != null && MessageText.Text != "Secret message NOT FOUND!")
			{
				DecodeResult.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/ok.jpg"));
			}
			// Sprawdź wszystkie przypadki; przypadek == "" jest dla zdjęć czarno-białych; przypadek Secret message NOT FOUND to jest zwrócona wiadomość która nie jest nullem, ale informuje o tym, że nic nie odnaleziono
			if (MessageText.Text == "" || MessageText.Text == null || MessageText.Text == "Secret message NOT FOUND!")
			{
				MessageText.Text = "Secret message NOT FOUND!"; // przypadek dla obrazów czarno-białych które teoretycznie wykrywają wiadomość ale w postaci samych zer
				DecodeResult.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/X.jpg"));
			}
		}

		private void HideMethod(object sender, RoutedEventArgs e)
		{
			if(MessageToHide.Text != "") // sprawdź czy wiadomość nie jest pusta
			{
				MessageText.Text = "";
				var modifiedImage = SteganographyHelper.Encrypt(bmp, MessageToHide.Text);
				MyImage.Source = modifiedImage;
				bmp = ConvertWriteableBitmapToBitmapImage(modifiedImage);
				MessageToHide.Text = "";
				TglButton.IsChecked = false;
				DecodeResult.Source = null;

				bool answer = SteganographyHelper.isMessageLargerThanImage;
				if (answer == false)
				{
					prompt.Foreground = Brushes.DarkGreen;
					prompt.Text = "Prompt: Your text was hidden in the image SUCCESSFULLY! \nDont forget about to save the Image";
				}
				else
				{
					prompt.Foreground = Brushes.DarkRed;
					prompt.Text = "Prompt: Message is to long. Change the image for a larger or write smaller text";
				}
				
			}
			else
			{
				prompt.Foreground = Brushes.DarkRed;
				prompt.Text = "Prompt: Message what do you want to hide, cannot be empty!";
			}
			
		}

		// konwerter z Typu WriteableBitmap który jest wynikiem obrazu z ukrytą wiadomością -> na BitmapImage, który jest obsłużony standardowo w aplikacji.
		public BitmapImage ConvertWriteableBitmapToBitmapImage(WriteableBitmap wbm)
		{
			bmp = new BitmapImage();
			using (MemoryStream stream = new MemoryStream())
			{
				PngBitmapEncoder encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(wbm));
				encoder.Save(stream);
				bmp.BeginInit();
				bmp.CacheOption = BitmapCacheOption.OnLoad;
				bmp.StreamSource = stream;
				bmp.EndInit();
				bmp.Freeze();
			}
			return bmp;
		}
	}
}
