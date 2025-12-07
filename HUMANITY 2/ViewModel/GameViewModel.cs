using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HumanityWPF.Model;

namespace HumanityWPF.ViewModel
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private readonly GameModel _gameModel;
        private readonly ItemModel _itemModel;

        // Właściwości do bindowania w XAML
        private string _currentRoomName;
        private int _sanityValue;
        private string _outputText;
        private ImageSource _currentImage;
        private string _inputText;
        private bool _waitingForKey = false;
        private TaskCompletionSource<bool> _keyPressTask;

        public GameViewModel()
        {
            _gameModel = new GameModel();
            _itemModel = new ItemModel();

            // Inicjalizacja wartości
            CurrentRoomName = "LABORATORY";
            SanityValue = 100;
            OutputText = "Initializing...";

            // Placeholder image (czarne tło)
            CurrentImage = GetRoomImage(0);
        }

        // Właściwości publiczne (bindowane do UI)
        public string CurrentRoomName
        {
            get => _currentRoomName;
            set
            {
                _currentRoomName = value;
                OnPropertyChanged();
            }
        }

        public int SanityValue
        {
            get => _sanityValue;
            set
            {
                _sanityValue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SanityPercentage));
                OnPropertyChanged(nameof(SanityColor));
            }
        }

        public double SanityPercentage => SanityValue / 100.0;

        public Brush SanityColor
        {
            get
            {
                if (SanityValue >= 75) return new SolidColorBrush(Colors.LimeGreen);
                if (SanityValue >= 50) return new SolidColorBrush(Colors.Yellow);
                if (SanityValue >= 25) return new SolidColorBrush(Colors.Orange);
                return new SolidColorBrush(Colors.Red);
            }
        }

        public string OutputText
        {
            get => _outputText;
            set
            {
                _outputText = value;
                OnPropertyChanged();
            }
        }

        public ImageSource CurrentImage
        {
            get => _currentImage;
            set
            {
                _currentImage = value;
                OnPropertyChanged();
            }
        }

        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged();
            }
        }

        public bool WaitingForKey
        {
            get => _waitingForKey;
            set
            {
                _waitingForKey = value;
                OnPropertyChanged();
            }
        }

        // Dostęp do modeli
        public GameModel GameModel => _gameModel;
        public ItemModel ItemModel => _itemModel;

        // Metody pomocnicze
        public void AppendOutput(string text)
        {
            OutputText += text + "\n";
        }

        public void ClearOutput()
        {
            OutputText = string.Empty;
        }

        public async Task WaitForKeyPress()
        {
            _keyPressTask = new TaskCompletionSource<bool>();
            WaitingForKey = true;
            AppendOutput("\n[Press ENTER to continue...]");
            await _keyPressTask.Task;
            WaitingForKey = false;
        }

        public void NotifyKeyPressed()
        {
            if (_keyPressTask != null && !_keyPressTask.Task.IsCompleted)
            {
                _keyPressTask.SetResult(true);
            }
        }

        public void UpdateRoomDisplay(int roomIndex)
        {
            CurrentRoomName = _gameModel.RoomName(roomIndex);
            SanityValue = _gameModel.sanity;

            // Zmiana obrazu pokoju
            CurrentImage = GetRoomImage(roomIndex);
        }

        public void UpdateItemDisplay(string itemName)
        {
            // Zmiana obrazu na przedmiot
            CurrentImage = GetItemImage(itemName);
        }

        // Tworzenie placeholder obrazów (tymczasowe, póki nie masz grafik)
        private ImageSource CreatePlaceholderImage(string text)
        {
            var drawingVisual = new DrawingVisual();
            using (DrawingContext dc = drawingVisual.RenderOpen())
            {
                dc.DrawRectangle(Brushes.Black, null, new System.Windows.Rect(0, 0, 800, 600));

                var formattedText = new FormattedText(
                    text,
                    System.Globalization.CultureInfo.CurrentCulture,
                    System.Windows.FlowDirection.LeftToRight,
                    new Typeface("Arial"),
                    48,
                    Brushes.DarkRed,
                    1.0);

                dc.DrawText(formattedText, new System.Windows.Point(400 - formattedText.Width / 2, 300));
            }

            var renderBitmap = new RenderTargetBitmap(800, 600, 96, 96, PixelFormats.Pbgra32);
            renderBitmap.Render(drawingVisual);
            return renderBitmap;
        }

        private ImageSource TryLoadImageByCandidates(string nameNoExt)
        {
            string assembly = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name ?? "UnknownAssembly";
            string[] candidates = new[]
            {
                // jako osadzony Resource (pack URI)
                $"pack://application:,,,/Assety/Grafika/{nameNoExt}.png",
                // alternatywna forma z nazwą assembly
                $"/{assembly};component/Assety/Grafika/{nameNoExt}.png",
                // jako plik skopiowany do folderu wyjściowego (relative)
                $"Assety/Grafika/{nameNoExt}.png",
                // absolutna ścieżka do katalogu aplikacji (na wszelki wypadek)
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assety", "Grafika", $"{nameNoExt}.png")
            };

            foreach (var candidate in candidates)
            {
                try
                {
                    Uri uri;
                    // jeśli ścieżka wygląda jak lokalna ścieżka pliku, użyj UriKind.Absolute
                    if (System.IO.Path.IsPathRooted(candidate))
                        uri = new Uri(candidate, UriKind.Absolute);
                    else
                        uri = new Uri(candidate, UriKind.RelativeOrAbsolute);

                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = uri;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    System.Diagnostics.Debug.WriteLine($"Loaded image: {candidate}");
                    return bitmap;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"TryLoadImage failed for '{candidate}': {ex.Message}");
                    // dalej próbuj następnego kandydata
                }
            }

            return null;
        }

        private ImageSource GetRoomImage(int roomIndex)
        {
            string roomName = _gameModel.RoomName(roomIndex) ?? "Unknown";
            // jeżeli pliki mają spacje, dostosuj zgodnie z rzeczywistymi nazwami plików
            string cleaned = roomName.Replace(" ", "");
            var img = TryLoadImageByCandidates(cleaned);
            if (img != null) return img;

            System.Diagnostics.Debug.WriteLine($"All image load attempts failed for room '{roomName}'. Returning placeholder.");
            return CreatePlaceholderImage(roomName);
        }

        private ImageSource GetItemImage(string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
                return CreatePlaceholderImage("UNKNOWN");

            string cleaned = itemName.Replace(" ", "");
            var img = TryLoadImageByCandidates(cleaned);
            if (img != null) return img;

            System.Diagnostics.Debug.WriteLine($"All image load attempts failed for item '{itemName}'. Returning placeholder.");
            return CreatePlaceholderImage(itemName.ToUpper());
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}