using System.Windows;
using System.Windows.Input;
using HumanityWPF.ViewModel;
using HumanityWPF.Controller;

namespace HumanityWPF
{
    public partial class MainWindow : Window
    {
        private readonly GameViewModel _viewModel;
        private readonly WPFGameController _gameController;

        public MainWindow()
        {
            InitializeComponent();

            // Inicjalizacja ViewModel
            _viewModel = new GameViewModel();
            DataContext = _viewModel;

            // Inicjalizacja kontrolera gry
            _gameController = new WPFGameController(_viewModel);

            // Uruchomienie gry
            StartGame();
        }

        private async void StartGame()
        {
            // Focus na pole input
            InputTextBox.Focus();

            // Uruchomienie intro i głównej pętli gry
            await _gameController.Run();
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (_viewModel.WaitingForKey)
                {
                    // Jeśli czekamy na klawisz, tylko potwierdź
                    _viewModel.NotifyKeyPressed();
                    e.Handled = true;
                }
                else
                {
                    // Normalnie przetwórz komendę
                    ProcessCommand();
                }
            }
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.WaitingForKey)
            {
                _viewModel.NotifyKeyPressed();
            }
            else
            {
                ProcessCommand();
            }
        }

        private void ProcessCommand()
        {
            string command = _viewModel.InputText?.Trim() ?? "";

            if (!string.IsNullOrEmpty(command))
            {
                // Wyślij komendę do kontrolera
                _gameController.HandleInput(command);

                // Wyczyść pole input
                _viewModel.InputText = string.Empty;

                // Scroll do dołu output
             
                InputTextBox.Focus();
            }
        }
    }
}