using System;
using System.IO;
using System.Media;
using System.Windows.Media;

namespace HumanityWPF
{
    public static class SoundManager
    {
        private static MediaPlayer _bgPlayer = new MediaPlayer();
        private static MediaPlayer _sfxPlayer = new MediaPlayer();

        // ===== DŹWIĘKI TŁA =====
        public static void PlayIntroAmbient()
        {
            // Cichy ambient dla intro (jeśli masz plik)
            // PlaySound("intro_ambient.mp3", _bgPlayer, 0.3, true);
        }

        public static void PlayGameAmbient()
        {
            // Ambient dla gry
            // PlaySound("game_ambient.mp3", _bgPlayer, 0.2, true);
        }

        // ===== DŹWIĘKI EFEKTÓW =====
        public static void PlayRoomTransition()
        {
            // Dźwięk zmiany pokoju
            Beep(400, 100);
        }

        public static void PlayGhostAppear()
        {
            // Straszny dźwięk ducha
            Beep(200, 300);
        }

        public static void PlayItemCheck()
        {
            // Sprawdzanie przedmiotu
            Beep(600, 80);
        }

        public static void PlaySuccess()
        {
            // Sukces (znalezienie klucza, rozwiązanie zagadki)
            Beep(800, 100);
            System.Threading.Tasks.Task.Delay(100).Wait();
            Beep(1000, 100);
        }

        public static void PlayError()
        {
            // Błąd
            Beep(300, 150);
            System.Threading.Tasks.Task.Delay(100).Wait();
            Beep(200, 150);
        }

        public static void PlayTypewriter()
        {
            // Dźwięk pisania na maszynie (dla intro)
            Beep(1200, 30);
        }

        public static void PlaySanityDrop()
        {
            // Spadek sanity
            Beep(250, 200);
        }

        // ===== POMOCNICZE =====
        private static void PlaySound(string filename, MediaPlayer player, double volume = 0.5, bool loop = false)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", filename);
                if (File.Exists(path))
                {
                    player.Open(new Uri(path));
                    player.Volume = volume;
                    if (loop)
                    {
                        player.MediaEnded += (s, e) => { player.Position = TimeSpan.Zero; player.Play(); };
                    }
                    player.Play();
                }
            }
            catch { /* Ignore sound errors */ }
        }

        private static void Beep(int frequency, int duration)
        {
            try
            {
                Console.Beep(frequency, duration);
            }
            catch { /* Console.Beep może nie działać na niektórych systemach */ }
        }

        public static void StopAll()
        {
            _bgPlayer.Stop();
            _sfxPlayer.Stop();
        }
    }
}