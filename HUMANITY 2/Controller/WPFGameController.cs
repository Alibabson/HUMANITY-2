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
        private readonly WPFItemController _itemController;

        private int idx;
        private int nextRoomIdx;

        // Stan interakcji - co aktualnie robimy
        private string _currentInteractionMode = "";
        private string _currentItem = "";

        public WPFGameController(GameViewModel viewModel)
        {
            _viewModel = viewModel;
            _model = viewModel.GameModel;
            _itemModel = viewModel.ItemModel;
            _itemController = new WPFItemController(viewModel, _model, _itemModel);
        }

        public async Task Run()
        {
            _viewModel.ClearOutput();

            if (!_model.IntroPlayed)
            {
                await ShowIntro();
                _model.IntroPlayed = true;
            }

            _viewModel.AppendOutput(Help());
            _viewModel.AppendOutput("");
            LookFunction("");
        }

        private async Task ShowIntro()
        {
            // Ukryj obraz na czas intro
            _viewModel.CurrentImage = null;

            foreach (var line in _model.Prologue)
            {
                _viewModel.AppendOutput(CleanMarkup(line.Text));
                await Task.Delay(50);
            }

            await _viewModel.WaitForKeyPress();
            _viewModel.ClearOutput();

            // Przywróć obraz pokoju
            _viewModel.UpdateRoomDisplay(0);
        }

        public async void HandleInput(string input)
        {
            var inputLower = (input ?? "").Trim().ToLowerInvariant();

            // Jeśli jesteśmy w trybie interakcji, przekaż do odpowiedniej funkcji
            if (!string.IsNullOrEmpty(_currentInteractionMode))
            {
                await HandleInteractionInput(inputLower);
                return;
            }

            // Normalne komendy
            var command = "";
            var argument = "";

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

            var parts = inputLower.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            command = parts.Length > 0 ? parts[0] : "";
            argument = parts.Length > 1 ? parts[1] : "";

            switch (command)
            {
                case "help":
                    _viewModel.ClearOutput();
                    _viewModel.AppendOutput(Help());
                    break;

                case "look":
                    _viewModel.ClearOutput();
                    _model.LookedRoom[_model.room_idx] = true;
                    LookFunction(argument);
                    break;

                case "check":
                    _viewModel.ClearOutput();
                    
                    await HandleCheckCommand(argument);
                    break;

                case "use":
                    if (_model.hasDevice && argument == "device" && _model.room_idx == 0)
                    {
                        _itemController.Destroy();
                        _currentInteractionMode = "device_destroy";
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

        private async Task HandleCheckCommand(string item)
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


            _currentItem = item;
            _viewModel.UpdateItemDisplay(item.ToUpper());

            // Obsługa specjalnych przedmiotów
            switch (item.ToLower())
            {
                case "floor":
                    if (idx == 1)
                    {
                        _viewModel.UpdateItemDisplay(item.ToUpper());
                        await _itemController.KeyAsync(desc);
                        _viewModel.UpdateRoomDisplay(1);
                        LookFunction("");
                    }
                    break;

                case "monitor":
                case "monitors":
                case "terminal":
                    if (idx == 0)
                    {
                        _viewModel.UpdateItemDisplay(item.ToUpper());
                        await _itemController.Monitor(desc);
                        _currentInteractionMode = "terminal";
                    }
                    break;

                case "whiteboard":
                    if (idx == 0)
                    {
                        _itemController.Whiteboard(desc);
                        _currentInteractionMode = "whiteboard_menu";
                        //LookFunction("");
                    }
                    break;

                case "newspaper":
                    if (idx == 4)
                    {
                        _viewModel.UpdateItemDisplay(item.ToUpper());
                        await _itemController.Newspaper(desc);
                        LookFunction("");

                    }
                    break;

                case "bookshelf":
                    if (idx == 3)
                    {
                        _itemController.Bookshelf(desc);
                        _currentInteractionMode = "bookshelf";
                    }
                    break;

                case "table":
                    if (idx == 3)
                    {
                        await _itemController.Table(desc);
                        LookFunction("");
                    }
                    break;

                case "piano":
                    if (idx == 2)
                    {
                        _viewModel.UpdateItemDisplay(item.ToUpper());
                        _itemController.Piano(desc);
                        if (!_model.hasDiaryKey)
                        {
                            _currentInteractionMode = "piano";
                        }
                    }
                    break;

                case "clock":
                    if (idx == 2)
                    {
                        await _itemController.Clock(desc);
                        LookFunction("");
                    }
                    break;

                case "photo":
                case "picture":
                    if (idx == 5)
                    {
                        await _itemController.Photo(desc);
                        if (_model.KnowsSafeLocation && !_model.SafeOpened)
                        {
                            _currentInteractionMode = "photo_safe_prompt";
                        }
                    }
                    break;

                case "mirror":
                    if (idx == 6)
                    {
                        _itemController.Mirror(desc);
                        _currentInteractionMode = "mirror";
                    }
                    break;

                case "cabinet":
                    if (idx == 6)
                    {
                        _itemController.Cabinet(desc);
                        if (!_model.hasMusicBoxKey)
                        {
                            _currentInteractionMode = "cabinet";
                        }
                    }
                    break;

                case "music":
                case "musicbox":
                case "music box":
                    if (idx == 7)
                    {
                        _itemController.MusicBox(desc);
                        if (_model.hasMusicBoxKey && !_model.hasRing)
                        {
                            _currentInteractionMode = "musicbox";
                        }
                    }
                    break;

                case "diary":
                    if (idx == 7)
                    {
                        _itemController.Diary(desc);
                        if (_model.hasDiaryKey && !_itemController.IsDiaryOpened())
                        {
                            _currentInteractionMode = "diary";
                        }
                    }
                    break;

                case "desk":
                    if (idx == 8)
                    {
                        _itemController.Desk(desc);
                        _currentInteractionMode = "desk";
                    }
                    break;

                default:
                    // Zwykły przedmiot - tylko opis
                    foreach (var line in desc)
                    {
                        _viewModel.AppendOutput(CleanMarkup(line));
                    }
                    break;
            }
        }

        private async Task HandleInteractionInput(string input)
        {
            switch (_currentInteractionMode)
            {
                case "terminal":
                    await HandleTerminalInput(input);
                    break;

                case "terminal_logs":
                    HandleTerminalLogsInput(input);
                    break;

                case "terminal_ending":
                    await HandleTerminalEndingInput(input);
                    break;

                case "whiteboard_menu":
                    HandleWhiteboardMenuInput(input);
                    break;

                case "whiteboard_solving":
                    HandleWhiteboardSolvingInput(input);
                    break;

                case "bookshelf":
                    HandleBookshelfInput(input);
                    break;

                case "bookshelf_poem":
                    HandleBookshelfPoemInput(input);
                    break;

                case "piano":
                    HandlePianoInput(input);
                    break;

                case "photo_safe_prompt":
                    HandlePhotoSafePromptInput(input);
                    break;

                case "safe":
                    HandleSafeInput(input);
                    break;

                case "mirror":
                    HandleMirrorInput(input);
                    break;

                case "cabinet":
                    HandleCabinetInput(input);
                    break;

                case "musicbox":
                    HandleMusicBoxInput(input);
                    break;

                case "diary":
                    HandleDiaryInput(input);
                    break;

                case "desk":
                    HandleDeskInput(input);
                    break;

                case "device_destroy":
                    await HandleDeviceDestroyInput(input);
                    break;

                case "device_code":
                    HandleDeviceCodeInput(input);
                    break;

                case "device_final":
                    HandleDeviceFinalInput(input);
                    break;
            }
        }

        // ===== TERMINAL =====
        private async Task HandleTerminalInput(string input)
        {
            if (input == "1")
            {
                _itemController.MonitorOption1();
                // Pozostajemy w trybie terminal, czekamy na "back"
            }
            else if (input == "2")
            {
                _itemController.MonitorOption2();
                _currentInteractionMode = "terminal_logs";
            }
            else if (input == "3" && _model.Reason && _model.Emotion && _model.Morality)
            {
                _itemController.MonitorOption3();
                _currentInteractionMode = "terminal_ending";
            }
            else if (input == "4" || input == "back" || input == "quit")
            {
                _currentInteractionMode = "";
                _viewModel.ClearOutput();
                _viewModel.UpdateRoomDisplay(_model.room_idx);
                LookFunction("");
            }
        }

        private void HandleTerminalLogsInput(string input)
        {
            if (input == "next page")
            {
                int page = _itemController.GetCurrentLogPage() + 1;
                if (page <= 5) _itemController.ShowLogPage(page);
            }
            else if (input == "prev page")
            {
                int page = _itemController.GetCurrentLogPage() - 1;
                if (page >= 1) _itemController.ShowLogPage(page);
            }
            else if (input == "back")
            {
                _itemController.MonitorOption1(); // Powrót do menu
                _currentInteractionMode = "terminal";
            }
        }

        private async Task HandleTerminalEndingInput(string input)
        {
            if (input == "yes")
            {
                await _itemController.ShowGoodEnding();
            }
            else if (input == "no")
            {
                await _itemController.ShowBadEnding();
            }
        }

        // ===== WHITEBOARD =====
        private void HandleWhiteboardMenuInput(string input)
        {
            if (input == "solve")
            {
                _itemController.WhiteboardSolve();
                _currentInteractionMode = "whiteboard_solving";
            }
            else if (input == "leave" || input == "back")
            {
                _currentInteractionMode = "";
                _viewModel.UpdateRoomDisplay(_model.room_idx);
                LookFunction("");
            }
        }

        private void HandleWhiteboardSolvingInput(string input)
        {
            if (_itemController.WhiteboardCheckAnswer(input))
            {
                _currentInteractionMode = "";
                // Powrót do pokoju
                _viewModel.UpdateRoomDisplay(_model.room_idx);
            }
        }

        // ===== BOOKSHELF =====
        private void HandleBookshelfInput(string input)
        {
            if (input == "back")
            {
                _currentInteractionMode = "";
                _viewModel.UpdateRoomDisplay(_model.room_idx);
                LookFunction("");
                return;
            }

            var parts = input.Split(' ', 2);
            if (parts.Length == 2 && int.TryParse(parts[0], out int row) && int.TryParse(parts[1], out int col))
            {
                var desc = _model.checkItem(3, "bookshelf");
                _itemController.BookshelfCoordinates(row, col, desc);

                if (row == 9 && col == 5)
                {
                    _currentInteractionMode = "bookshelf_poem";
                }
            }
        }

        private void HandleBookshelfPoemInput(string input)
        {
            if (input == "read")
            {
                var desc = _model.checkItem(3, "bookshelf");
                _itemController.Poem(desc);
                // Teraz czekamy na odpowiedź
            }
            else if (input == "back")
            {
                _itemController.Bookshelf(_model.checkItem(3, "bookshelf"));
                _currentInteractionMode = "bookshelf";
                _viewModel.ClearOutput();
            }
            else
            {
                // Odpowiedź na zagadkę
                if (_itemController.PoemCheckAnswer(input))
                {
                    _currentInteractionMode = "";
                    _viewModel.UpdateRoomDisplay(_model.room_idx);
                }
            }
        }

        // ===== PIANO =====
        private void HandlePianoInput(string input)
        {
            if (_itemController.PianoCheckSequence(input))
            {
                _currentInteractionMode = "";
                if(_model.hasDiaryKey) _viewModel.UpdateRoomDisplay(_model.room_idx);
                LookFunction("");
            }
        }

        // ===== PHOTO & SAFE =====
        private void HandlePhotoSafePromptInput(string input)
        {
            if (input == "open safe")
            {
                var desc = _model.checkItem(5, "safe");
                _itemController.Safe(desc);
                _currentInteractionMode = "safe";
            }
            else if (input == "back")
            {
                _currentInteractionMode = "";
                _viewModel.UpdateRoomDisplay(_model.room_idx);
                LookFunction("");
            }
        }

        private void HandleSafeInput(string input)
        {
            if (input == "back")
            {
                _currentInteractionMode = "";
                _viewModel.UpdateRoomDisplay(_model.room_idx);
                LookFunction("");
            }
            else if (_itemController.SafeCheckPassword(input))
            {
                _currentInteractionMode = "";
                _viewModel.UpdateRoomDisplay(_model.room_idx);
            }
        }

        // ===== MIRROR =====
        private async Task HandleMirrorInput(string input)
        {
            if (input == "read note")
            {
                var desc = _model.checkItem(6, "mirror");
                await _itemController.Note(desc[5]);
                _currentInteractionMode = "";
            }
            else if (input == "back")
            {
                _currentInteractionMode = "";
                _viewModel.UpdateRoomDisplay(_model.room_idx);
                LookFunction("");
            }
        }

        // ===== CABINET =====
        private async Task HandleCabinetInput(string input)
        {
            if (input == "search")
            {
                var desc = _model.checkItem(6, "cabinet");
                await _itemController.CabinetSearch(desc);
                _currentInteractionMode = "";
            }
            else if (input == "back")
            {
                _currentInteractionMode = "";
                _viewModel.UpdateRoomDisplay(_model.room_idx);
                LookFunction("");
            }
        }

        // ===== MUSIC BOX =====
        private async Task HandleMusicBoxInput(string input)
        {
            if (input == "open")
            {
                var desc = _model.checkItem(7, "music box");
                await _itemController.MusicBoxOpen(desc);
                _currentInteractionMode = "";
            }
            else if (input == "back")
            {
                _currentInteractionMode = "";
                _viewModel.UpdateRoomDisplay(_model.room_idx);
                LookFunction("");
            }
        }

        // ===== DIARY =====
        private void HandleDiaryInput(string input)
        {
            if (input == "next page")
            {
                int page = _itemController.GetCurrentDiaryPage() + 1;
                if (page <= 5) _itemController.ShowDiaryPage(page);
            }
            else if (input == "prev page")
            {
                int page = _itemController.GetCurrentDiaryPage() - 1;
                if (page >= 1) _itemController.ShowDiaryPage(page);
            }
            else if (input == "answer")
            {
                _itemController.DiaryAnswer();
            }
            else if (input == "back")
            {
                _currentInteractionMode = "";
                _viewModel.UpdateRoomDisplay(_model.room_idx);
                LookFunction("");
            }
            else
            {
                // Odpowiedź
                if (_itemController.DiaryCheckAnswer(input))
                {
                    _currentInteractionMode = "";
                    _viewModel.UpdateRoomDisplay(_model.room_idx);
                }
            }
        }

        // ===== DESK =====
        private void HandleDeskInput(string input)
        {
            if (input == "desk")
            {
                _itemController.DeskCheck();
            }
            else if (input == "drawer")
            {
                _itemController.DrawerCheck();
            }
            else if (input == "back")
            {
                _currentInteractionMode = "";
                _viewModel.UpdateRoomDisplay(_model.room_idx);
                LookFunction("");
            }
        }

        // ===== DEVICE =====
        private async Task HandleDeviceDestroyInput(string input)
        {
            if (input == "activate")
            {
                _itemController.DestroyActivate(_itemModel.DestroyList);
                _currentInteractionMode = "device_code";
            }
            else if (input == "cancel")
            {
                _currentInteractionMode = "";
                _viewModel.UpdateRoomDisplay(_model.room_idx);
                LookFunction("");
            }
        }

        private void HandleDeviceCodeInput(string input)
        {
            if (_itemController.DestroyCheckCode(input))
            {
                _currentInteractionMode = "device_final";
            }
        }

        private void HandleDeviceFinalInput(string input)
        {
            if (_itemController.DestroyFinalAnswer(input))
            {
                _currentInteractionMode = "";
                _viewModel.UpdateRoomDisplay(_model.room_idx);
            }
        }

        // ===== POMOCNICZE =====
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
                _viewModel.AppendOutput("\n❌ You can't go to that room from here.\n");
            }
        }

        private void NextRoomProcess(int nextRoomIdx)
        {
            _model.GoTo_Possible(nextRoomIdx);
            _viewModel.ClearOutput();
            _viewModel.AppendOutput($"➡️  You move to the {_model.RoomName(nextRoomIdx)}.\n");
            RandomGhost();
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

            idx = _model.room_idx;

            if (_model.LookedRoom[idx])
            {
                _model.pickLook(idx, 0);
                _viewModel.AppendOutput("");
                foreach (string x in _model.look)
                {
                    _viewModel.AppendOutput(CleanMarkup(x));
                }
                _viewModel.AppendOutput("");
            }
        }

        private string CleanMarkup(string text)
        {
            return System.Text.RegularExpressions.Regex.Replace(text, @"\[.*?\]", "");
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