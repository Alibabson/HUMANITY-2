using System;
using System.Threading.Tasks;
using System.Windows;
using HumanityWPF.Model;
using HumanityWPF.ViewModel;

namespace HumanityWPF.Controller
{
    public class WPFGameController
    {
        private readonly GameViewModel _viewModel;
        private readonly GameModel _model;
        private readonly ItemModel _itemModel;
        private bool _running = true;
        private int idx;
        private int nextRoomIdx;

        public WPFGameController(GameViewModel viewModel)
        {
            _viewModel = viewModel;
            _model = viewModel.GameModel;
            _itemModel = viewModel.ItemModel;
        }

        public async Task Run()
        {
            _viewModel.ClearOutput();

            if (!_model.IntroPlayed)
            {
                // Intro - uproszczone dla WPF
                await ShowIntro();
                _model.IntroPlayed = true;
            }

            // Pokaż help
            _viewModel.AppendOutput(Help());
            _viewModel.AppendOutput("");

            // Pokaż pierwszy pokój
            LookFunction("");
        }

        private async Task ShowIntro()
        {
            // Animowane intro (uproszczone)
            foreach (var line in _model.Prologue)
            {
                _viewModel.AppendOutput(line.Text);
                await Task.Delay(100); // Szybsze dla WPF
            }

            _viewModel.AppendOutput("\n[Press ENTER to continue...]");
            await Task.Delay(2000);
            _viewModel.ClearOutput();
        }

        public void HandleInput(string input)
        {
            var command = "";
            var argument = "";
            var inputLower = (input ?? "").Trim().ToLowerInvariant();

            // Obsługa "GO TO"
            if (inputLower.StartsWith("go to "))
            {
                string room = inputLower.Substring("go to ".Length).Trim();
                nextRoomIdx = _model.NextRoomIdx(room);

                if (nextRoomIdx == -1)
                {
                    _viewModel.AppendOutput($"❌ Error: Unknown room '{room}'. Try again.\n");
                    return;
                }

                CheckPossible(nextRoomIdx);
                return;
            }

            // Rozdzielenie komendy i argumentu
            var parts = inputLower.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            command = parts.Length > 0 ? parts[0] : "";
            argument = parts.Length > 1 ? parts[1] : "";

            // Przetwarzanie komend
            switch (command)
            {
                case "help":
                    _viewModel.ClearOutput(); // Czyścimy przed pokazaniem HELP
                    _viewModel.AppendOutput(Help());
                    break;

                case "look":
                    _model.LookedRoom[_model.room_idx] = true;
                    LookFunction(argument);
                    break;

                case "check":
                    HandleCheckCommand(argument);
                    break;

                case "use":
                    if (_model.hasDevice && argument == "device" && _model.room_idx == 0)
                    {
                        DestroyDevice();
                    }
                    else
                    {
                        _viewModel.AppendOutput("❌ You can't use that here.\n");
                    }
                    break;

                case "exit":
                case "quit":
                    _viewModel.AppendOutput(_model.Quit());
                    Application.Current.Shutdown();
                    break;

                default:
                    _viewModel.AppendOutput("❌ Unknown command. Type HELP for a list of commands.\n");
                    break;
            }
        }

        private void HandleCheckCommand(string item)
        {
            if (string.IsNullOrEmpty(item))
            {
                _viewModel.AppendOutput("❌ Error: CHECK command requires an item name.\n");
                return;
            }

            idx = _model.room_idx;
            var desc = _model.checkItem(idx, item);

            if (desc.Count == 0)
            {
                _viewModel.AppendOutput($"❌ Error: There is no item named '{item}' in this room.\n");
                return;
            }

            // Zmień obraz na przedmiot
            _viewModel.UpdateItemDisplay(item);

            // Wyświetl opis przedmiotu
            foreach (var line in desc)
            {
                // Usunięcie Spectre markup tags (np. [lime], [/])
                string cleanLine = System.Text.RegularExpressions.Regex.Replace(line, @"\[.*?\]", "");
                _viewModel.AppendOutput(cleanLine);
            }

            // Specjalna obsługa niektórych przedmiotów
            HandleSpecialItems(item);
        }

        private void HandleSpecialItems(string item)
        {
            switch (item.ToLower())
            {
                case "floor":
                    if (idx == 1 && !_model.hasKey)
                    {
                        _model.hasKey = true;
                        _viewModel.AppendOutput("\n🔑 You found a KEY!\n");
                    }
                    break;

                case "monitor":
                case "monitors":
                case "terminal":
                    if (idx == 0)
                    {
                        ShowTerminalMenu();
                    }
                    break;
            }
        }

        private void ShowTerminalMenu()
        {
            _viewModel.AppendOutput("\n=== TERMINAL MENU ===");
            _viewModel.AppendOutput("1. CHECK CURRENT PATIENT STATUS");
            _viewModel.AppendOutput("2. OPEN TEST LOGS");
            if (_model.Reason && _model.Emotion && _model.Morality)
            {
                _viewModel.AppendOutput("3. RESTORE HUMANITY");
            }
            _viewModel.AppendOutput("\nType the number or command name.\n");
        }

        private void DestroyDevice()
        {
            if (!_model.DEVICE)
            {
                _viewModel.AppendOutput("💥 Device activated! Data erasing...");
                _model.DEVICE = true;
            }
            else
            {
                _viewModel.AppendOutput("You already used the device.");
            }

            LookFunction("");
        }

        private void CheckPossible(int nextRoomIdx)
        {
            int idx = _model.room_idx;
            bool canGo = false;

            switch (idx)
            {
                case 0: canGo = nextRoomIdx == 1; break;
                case 1: canGo = nextRoomIdx == 0 || nextRoomIdx == 2 || nextRoomIdx == 5; break;
                case 2: canGo = nextRoomIdx == 4 || nextRoomIdx == 1 || nextRoomIdx == 5 || nextRoomIdx == 3; break;
                case 3: canGo = nextRoomIdx == 2; break;
                case 4: canGo = nextRoomIdx == 2; break;
                case 5:
                    if (nextRoomIdx == 8)
                    {
                        if (_model.hasKey)
                        {
                            canGo = true;
                            _viewModel.AppendOutput("🔑 You used the key.\n");
                        }
                        else
                        {
                            _viewModel.AppendOutput("❌ The room is locked. You need a key.\n");
                            return;
                        }
                    }
                    else
                    {
                        canGo = nextRoomIdx == 6 || nextRoomIdx == 7 || nextRoomIdx == 2 || nextRoomIdx == 1;
                    }
                    break;
                case 6: canGo = nextRoomIdx == 5 || nextRoomIdx == 7; break;
                case 7: canGo = nextRoomIdx == 5 || nextRoomIdx == 6; break;
                case 8: canGo = nextRoomIdx == 5; break;
            }

            if (canGo)
            {
                NextRoomProcess(nextRoomIdx);
            }
            else
            {
                _viewModel.AppendOutput("❌ You can't go to that room from here.\n");
            }
        }

        private void NextRoomProcess(int nextRoomIdx)
        {
            _model.GoTo_Possible(nextRoomIdx);
            _viewModel.AppendOutput($"➡️  You move to the {_model.RoomName(nextRoomIdx)}.\n");

            // Losowanie ducha
            RandomGhost();

            // Aktualizacja wyświetlania
            _viewModel.UpdateRoomDisplay(nextRoomIdx);
            LookFunction("");
        }

        private void RandomGhost()
        {
            var rng = new Random();
            int procent = _model.sanity >= 75 ? 5 : _model.sanity >= 50 ? 15 : 25;
            int chance = rng.Next(1, 101);

            if (chance <= procent)
            {
                int ghostIndex = rng.Next(_model.Ghosts.Count);
                string ghostMessage = _model.Ghosts[ghostIndex];

                _viewModel.AppendOutput($"\n👻 {ghostMessage.Replace("\n", " ")}\n");
                _model.sanity = Math.Max(0, _model.sanity - 10);
            }
            else
            {
                _model.sanity = Math.Max(0, _model.sanity - 5);
            }

            _viewModel.SanityValue = _model.sanity;
        }

        private void LookFunction(string argument)
        {
            if (!string.IsNullOrEmpty(argument))
            {
                _viewModel.AppendOutput("❌ LOOK command does not take arguments.\n");
                return;
            }

            // HUD usunięty - mamy go na górze ekranu!
            idx = _model.room_idx;

            if (_model.LookedRoom[idx])
            {
                _model.pickLook(idx, 0);

                _viewModel.AppendOutput(""); // Pusta linia
                foreach (string x in _model.look)
                {
                    // Usunięcie Spectre markup
                    string clean = System.Text.RegularExpressions.Regex.Replace(x, @"\[.*?\]", "");
                    _viewModel.AppendOutput(clean);
                }
                _viewModel.AppendOutput(""); // Pusta linia na końcu
            }
        }

        private string Help()
        {
            return @"
╔═══════════════════════════════════════════╗
║         AVAILABLE COMMANDS:               ║
╠═══════════════════════════════════════════╣
║ HELP       - Show this help message       ║
║ LOOK       - Observe surroundings         ║
║ CHECK [item] - Examine an item            ║
║ GO TO [room] - Move to another room       ║
║ USE [item] - Use an item                  ║
║ QUIT/EXIT  - End the game                 ║
╚═══════════════════════════════════════════╝
";
        }
    }
}