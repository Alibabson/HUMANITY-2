using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace HumanityWPF
{
    public partial class StartupWindow : Window
    {
        private bool _canProceed = false;
        private string _fullTitle = "H U M A N I T Y  2";
        private int _currentChar = 0;

        public StartupWindow()
        {
            InitializeComponent();
            this.Focusable = true;
            this.Focus();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Animacja typu typewriter dla tytułu
            await System.Threading.Tasks.Task.Delay(500);

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(150);
            timer.Tick += (s, args) =>
            {
                if (_currentChar < _fullTitle.Length)
                {
                    TitleText.Text += _fullTitle[_currentChar];
                    _currentChar++;

                    // Fade in dla każdej litery
                    var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(100));
                    TitleText.BeginAnimation(OpacityProperty, fadeIn);
                }
                else
                {
                    timer.Stop();
                    ShowSubtitle();
                }
            };
            timer.Start();
        }

        private async void ShowSubtitle()
        {
            await System.Threading.Tasks.Task.Delay(500);

            // Fade in subtitle
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
            SubtitlePanel.BeginAnimation(OpacityProperty, fadeIn);

            await System.Threading.Tasks.Task.Delay(1500);

            // Fade in "press any key"
            var fadeInKey = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
            PressKeyText.BeginAnimation(OpacityProperty, fadeInKey);

            _canProceed = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_canProceed)
            {
                ProceedToGame();
            }
        }

        private void ProceedToGame()
        {
            // Fade out całego okna
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
            fadeOut.Completed += (s, e) =>
            {
                // Otwórz główne okno gry
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            };
            this.BeginAnimation(OpacityProperty, fadeOut);
        }
    }
}