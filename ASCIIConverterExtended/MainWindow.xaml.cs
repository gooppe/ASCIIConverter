using ASCIIConverterCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ASCIIConverterExtended
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BitmapImage SelectedImage;

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void ConvertImage()
        {
            if (ImageAdress.Text == string.Empty)
                return;
            try
            {
                int Height = int.Parse(TextHeight.Text);
                int Width = int.Parse(TextWidth.Text);
                int PaletteIndex = PalettePicker.SelectedIndex;
                string Pal = PaletteIndex != 4 ? Palette.DefaultPatterns[PaletteIndex] : " ";
                if (PaletteIndex == 4 && CustomPalette.Text.Length != 0)
                {
                    Pal = CustomPalette.Text;
                }
                string[] text = Converter.GetText(ImageAdress.Text, new Palette(Pal), Height, Width);

                ArtTextBox.Text = string.Join(Environment.NewLine, text);
            }
            catch
            {
                MessageBox.Show("Couldn't open file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateTextSize()
        {
            if (ImageAdress.Text == string.Empty)
                return;
            try
            {
                TextHeight.Text = (Math.Round(SelectedImage.PixelHeight * ResizeSlider.Value)).ToString();
                TextWidth.Text = (Math.Round(SelectedImage.PixelWidth * ResizeSlider.Value)).ToString();
            }
            catch
            {

            }
        }

        private void OpenImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog chooseFile = new OpenFileDialog
            {
                Filter = "Images(*.BMP;*.GIF;*.EXIF;*.JPG;*.PNG;*.TIFF;*.JPEG)|*.BMP;*.GIF;*.EXIF;*.JPG;*.PNG;*.TIFF;*.JPEG",
                Multiselect = false,
                CheckFileExists = true
            };
            if (chooseFile.ShowDialog() == true)
            {
                try
                {
                    SelectedImage = new BitmapImage(new Uri(chooseFile.FileName));
                    PreviewImage.Source = SelectedImage;
                    ImageAdress.Text = chooseFile.FileName;
                    UpdateTextSize();
                    ConvertImage();
                    ImgWorkspace.Visibility = Visibility.Visible;
                }
                catch
                {
                    MessageBox.Show("Couldn't open image", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ResizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateTextSize();
            ConvertImage();
        }

        private void PalettePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (PalettePicker.SelectedIndex == 4)
                {
                    CustomPalette.Visibility = Visibility.Visible;
                }
                else
                {
                    CustomPalette.Visibility = Visibility.Collapsed;
                }
                ConvertImage();
            }
            catch
            {

            }
        }

        private void SaveArtButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog chooseFile = new OpenFileDialog
            {
                Filter = "Text documents|*.TXT",
                Multiselect = false,
                CheckFileExists = true
            };
            if (chooseFile.ShowDialog() == true)
            {
                try
                {
                    using (StreamWriter sw = File.CreateText(chooseFile.FileName))
                    {
                        sw.Write(ArtTextBox.Text);
                    }
                }
                catch
                {
                    MessageBox.Show("Couldn't save file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }
    }
}
