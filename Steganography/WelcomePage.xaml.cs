﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace Steganography
{
	/// <summary>
	/// Logika interakcji dla klasy WelcomePage.xaml
	/// </summary>
	public partial class WelcomePage : MetroWindow
	{
		MainWindow main = new MainWindow();

		public WelcomePage()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Duration duration = new Duration(TimeSpan.FromSeconds(3));
			DoubleAnimation doubleanimation = new DoubleAnimation(progresBar.Maximum, duration);

			doubleanimation.Completed += new EventHandler(Start); // dodana metoda Start

			progresBar.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);
		}

		public void Start(object sender, EventArgs e)
		{
			this.Hide();
			main.Show();
		}
	}
}
