using HumanityWPF;
using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Diagnostics;
using System.Windows;

namespace HumanityWPF
{
    public partial class ModeSelectWindow : Window
    {
        public ModeSelectWindow()
        {
            InitializeComponent();
        }

        private void GraphicButton_Click(object sender, RoutedEventArgs e)
        {
            
            var gameWindow = new StartupWindow();
            gameWindow.Show();

            this.Close(); // zamknij okno wyboru
        }

        private void TextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(
                    @"F:\PULPIT\Humanity\bin\Debug\net8.0\Humanity.exe"
                );

                // opcjonalnie zamknij WPF, jeśli chcesz zostawić tylko konsolę
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udało się uruchomić wersji tekstowej: " + ex.Message);
            }
        }
    }
}
