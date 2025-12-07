using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using HumanityWPF.Model;
using HumanityWPF.ViewModel;

namespace HumanityWPF.Controller
{
    public class WPFItemController
    {
        private readonly GameViewModel _viewModel;
        private readonly GameModel _model;
        private readonly ItemModel _itemModel;

        // Stany przedmiotów
        private bool whiteboardPassed = false;
        private int whiteboardGuesses = 0;
        private bool whiteboardHint = false;

        private bool poemOpened = false;
        private bool diaryOpened = false;

        private int currentLogPage = 1;
        private int currentDiaryPage = 1;

        public WPFItemController(GameViewModel viewModel, GameModel model, ItemModel itemModel)
        {
            _viewModel = viewModel;
            _model = model;
            _itemModel = itemModel;
        }

        // Helper do czyszczenia Spectre markup
        private string CleanMarkup(string text)
        {
            return System.Text.RegularExpressions.Regex.Replace(text, @"\[.*?\]", "");
        }

        // ===== FLOOR/KEY (STAIRS - 1) =====
        public async Task KeyAsync(List<string> text)
        {
            foreach (var line in text)
            {
                _viewModel.AppendOutput(CleanMarkup(line));
            }

            if (!_model.hasKey)
            {
                _model.hasKey = true;
                _viewModel.AppendOutput("\n🔑 You found a KEY!\n");
            }

            await _viewModel.WaitForKeyPress();
            _viewModel.ClearOutput();
        }

        // ===== TERMINAL (LAB - 0) =====
        public async Task Monitor(List<string> text)
        {
            _viewModel.ClearOutput();
            _viewModel.AppendOutput("=== TERMINAL MENU ===\n");
            _viewModel.AppendOutput("1. CHECK CURRENT PATIENT STATUS");
            _viewModel.AppendOutput("2. OPEN TEST LOGS");

            if (_model.Reason && _model.Emotion && _model.Morality)
            {
                _viewModel.AppendOutput("3. RESTORE HUMANITY");
            }

            _viewModel.AppendOutput("4. QUIT TERMINAL");
            _viewModel.AppendOutput("\nType the number (1-4) to select option.\n");
        }

        public void MonitorOption1() // Status
        {
            _viewModel.ClearOutput();
            _model.Status();
            foreach (var line in _model.statusLines)
            {
                _viewModel.AppendOutput(CleanMarkup(line));
            }
            _viewModel.AppendOutput("\nType 'back' to return to terminal menu.");
        }

        public void MonitorOption2() // Logs
        {
            if (_model.DEVICE)
            {
                _viewModel.ClearOutput();
                _viewModel.AppendOutput("❌ DATA CORRUPTED.");
                return;
            }

            _viewModel.ClearOutput();
            _viewModel.AppendOutput("=== TEST LOGS ===");
            _viewModel.AppendOutput("Use commands: 'next page', 'prev page', 'back'");
            _viewModel.AppendOutput("\nShowing page 1/5...\n");
            ShowLogPage(1);
        }

        public void ShowLogPage(int page)
        {
            currentLogPage = page;
            _itemModel.Logs(page);

            _viewModel.ClearOutput();
            foreach (var line in _itemModel.logLines)
            {
                _viewModel.AppendOutput(CleanMarkup(line));
            }
            _viewModel.AppendOutput($"\nPage {page}/5 | Commands: 'next page', 'prev page', 'back'");
        }

        public int GetCurrentLogPage() => currentLogPage;

        public void MonitorOption3() // Restore Humanity - ENDING
        {
            _viewModel.ClearOutput();
            _viewModel.AppendOutput("⚠️  WARNING: This action is IRREVERSIBLE.");
            _viewModel.AppendOutput("\nDo you wish to restore your HUMANITY?");
            _viewModel.AppendOutput("This will erase everything.\n");
            _viewModel.AppendOutput("Type 'yes' to proceed or 'no' to cancel.\n");
        }

        public async Task ShowGoodEnding()
        {
            _viewModel.ClearOutput();
            _viewModel.CurrentImage = null; // Pełny ekran tekstu

            foreach (var line in _model.EpilogueGood)
            {
                _viewModel.AppendOutput(CleanMarkup(line.Text));
                await Task.Delay(200);
            }
            await Task.Delay(3000);
            _viewModel.AppendOutput("\n" + CleanMarkup(_model.goodbye));
            await Task.Delay(5000);
            Application.Current.Shutdown();
        }

        public async Task ShowBadEnding()
        {
            _viewModel.ClearOutput();
            _viewModel.CurrentImage = null; // Pełny ekran tekstu

            foreach (var line in _model.EpilogueBad)
            {
                _viewModel.AppendOutput(CleanMarkup(line.Text));
                await Task.Delay(200);
            }
            await Task.Delay(3000);
            _viewModel.AppendOutput("\n" + CleanMarkup(_model.goodbye));
            await Task.Delay(5000);
            Application.Current.Shutdown();
        }

        // ===== WHITEBOARD (LAB - 0) =====
        public void Whiteboard(List<string> text)
        {
            if (whiteboardPassed)
            {
                _viewModel.AppendOutput(CleanMarkup(_itemModel.whiteboardList[6]));
                _viewModel.WaitForKeyPress();
                _viewModel.UpdateRoomDisplay(0);
                
                _viewModel.ClearOutput();
                return;
            }

            _viewModel.ClearOutput();
            _viewModel.AppendOutput(CleanMarkup(text[0]));
            _viewModel.AppendOutput(CleanMarkup(text[1]));
            _viewModel.AppendOutput("\nType 'solve' to attempt solving, or 'back' to leave.");
        }

        public void WhiteboardSolve()
        {
            _viewModel.ClearOutput();
            _viewModel.AppendOutput(CleanMarkup(_itemModel.whiteboardList[0]));

            if (whiteboardHint)
            {
                _viewModel.AppendOutput(CleanMarkup(_itemModel.whiteboardList[1]));
            }

            _viewModel.AppendOutput(CleanMarkup(_itemModel.whiteboardList[2]));

            if (whiteboardGuesses >= 3)
            {
                _viewModel.AppendOutput(CleanMarkup(_itemModel.whiteboardList[3]));
            }

            _viewModel.AppendOutput("\nEnter your answer (or type 'hint' if available):");
        }

        public bool WhiteboardCheckAnswer(string answer)
        {
            if (whiteboardGuesses >= 3 && answer.ToLower() == "hint")
            {
                whiteboardHint = true;
                WhiteboardSolve();
                return false;
            }

            if (answer == "1")
            {
                whiteboardPassed = true;
                _viewModel.ClearOutput();
                _viewModel.AppendOutput(CleanMarkup(_itemModel.whiteboardList[7]));
                //_viewModel.AppendOutput("\n✅ REASON fragment obtained!");
                //_model.Reason = true;
                //ShowStatus("r");
                //_viewModel.WaitForKeyPress();
                return true;
            }
            else
            {
                whiteboardGuesses++;
                _viewModel.AppendOutput("❌ Incorrect! Try again.");
                WhiteboardSolve();
                return false;
            }
        }

        // ===== NEWSPAPER (KITCHEN - 4) =====
        public async Task Newspaper(List<string> text)
        {
            _viewModel.ClearOutput();
            _itemModel.Newspaper();
            var lines = _itemModel.GetNewspaper;


            await _viewModel.WaitForKeyPress();
            _viewModel.ClearOutput();
            _viewModel.UpdateRoomDisplay(_model.room_idx);
            
        }

        // ===== BOOKSHELF (LIBRARY - 3) =====
        public void Bookshelf(List<string> text)
        {
            _viewModel.ClearOutput();
            _viewModel.AppendOutput(CleanMarkup(_itemModel.bookshelf[0]));
            _viewModel.AppendOutput("\nType coordinates as 'row column' (e.g., '9 5')");
            //_viewModel.AppendOutput("\nOr type 'back' to leave.\n");
            //_viewModel.AppendOutput("Hint: Row 9, Column 5 has something interesting...");
            //_viewModel.AppendOutput("Hint: Row 4, Column 20 might help you...");
        }

        public void BookshelfCoordinates(int row, int col, List<string> text)
        {
            if (row == 9 && col == 5)
            {
                _viewModel.ClearOutput();
                _viewModel.AppendOutput(CleanMarkup(text[0]));
                _viewModel.AppendOutput("\nType 'read' to read the poem, or 'back' to return.");
            }
            else if (row == 4 && col == 20)
            {
                _viewModel.ClearOutput();
                _viewModel.AppendOutput(CleanMarkup(_itemModel.ShowNotes()));
            }
            else
            {
                _viewModel.ClearOutput();
                _viewModel.AppendOutput(CleanMarkup(_itemModel.bookshelf[3]));
            }
        }

        public void Poem(List<string> text)
        {
            if (poemOpened)
            {
                _viewModel.AppendOutput(CleanMarkup(_itemModel.List_replay[0]));
                return;
            }

            _viewModel.ClearOutput();
            foreach (string s in _itemModel.Poem)
            {
                _viewModel.AppendOutput(CleanMarkup(s));
            }

            _viewModel.AppendOutput("\n\nType the missing word to obtain the fragment:");
        }

        public bool PoemCheckAnswer(string answer)
        {
            if (answer.ToLower() == "reason")
            {
                poemOpened = true;
                _model.Reason = true;
                _viewModel.ClearOutput();
                _viewModel.AppendOutput("✅ Correct! REASON fragment obtained!");
                ShowStatus("r");
                return true;
            }
            else
            {
                _viewModel.AppendOutput(CleanMarkup(_model.wrong));
                return false;
            }
        }

        // ===== TABLE (LIBRARY - 3) =====
        public async Task Table(List<string> text)
        {
            _viewModel.ClearOutput();
            foreach (string s in text)
            {
                _viewModel.AppendOutput(CleanMarkup(s));
            }

            await _viewModel.WaitForKeyPress();
            _viewModel.ClearOutput();
            _viewModel.UpdateRoomDisplay(_model.room_idx);
        }

        // ===== CLOCK (LIVING ROOM - 2) =====
        public async Task Clock(List<string> text)
        {
            _viewModel.ClearOutput();
            _viewModel.AppendOutput(CleanMarkup(text[0]));
            // ASCII usuniemy - nie wyświetlamy zegara

            await _viewModel.WaitForKeyPress();
            _viewModel.ClearOutput();
            _viewModel.UpdateRoomDisplay(_model.room_idx);
            
        }

        // ===== PIANO (LIVING ROOM - 2) =====
        public void Piano(List<string> text)
        {
            _viewModel.ClearOutput();
            if (!_model.hasDiaryKey)
            {
                _viewModel.AppendOutput(CleanMarkup(text[0]));
                _viewModel.AppendOutput(CleanMarkup(text[1]));
                //_viewModel.AppendOutput(CleanMarkup(_itemModel.ShowPiano()));
                //_viewModel.AppendOutput("\n🎹 Play the correct sequence: D-A-F-A");
                _viewModel.AppendOutput("\nType the sequence (e.g., 'cdec') to try:");
            }
            else
            {
                _viewModel.AppendOutput(CleanMarkup(text[2]));
                _viewModel.UpdateRoomDisplay(_model.room_idx);
                //while (!PianoCheckSequence("dafa"));
                // _viewModel.AppendOutput(CleanMarkup(_itemModel.ShowPiano()));
            }
        }

        public bool PianoCheckSequence(string sequence)
        {
            if (sequence.ToLower().Replace(" ", "").Replace("-", "") == "dafa")
            {
                _model.hasDiaryKey = true;
                _viewModel.ClearOutput();
                _viewModel.AppendOutput("\n🎵 The piano opens!");
                _viewModel.AppendOutput("\n✅ You found the DIARY KEY!\n\n");
                return true;
            }
            if (sequence.ToLower() == "back")
            {
                _viewModel.ClearOutput();
                _viewModel.UpdateRoomDisplay(_model.room_idx);
                return true;
            }
            else
            {
                _viewModel.AppendOutput("\n❌ Wrong sequence. Try again.\n");
                return false;
            }
        }

        // ===== MIRROR & NOTE (BATHROOM - 6) =====
        public void Mirror(List<string> text)
        {
            _viewModel.ClearOutput();
            _viewModel.AppendOutput(CleanMarkup(text[0]));
            _viewModel.AppendOutput(CleanMarkup(text[1]));
            _viewModel.AppendOutput("\nType 'read note' to read it, or 'back' to return.");
        }

        public async Task Note(string text)
        {
            _viewModel.ClearOutput();
            _viewModel.AppendOutput(CleanMarkup(text));

            await _viewModel.WaitForKeyPress();
            _viewModel.ClearOutput();
            _viewModel.UpdateRoomDisplay(_model.room_idx);
        }

        // ===== CABINET (BATHROOM - 6) =====
        public void Cabinet(List<string> text)
        {
            _viewModel.ClearOutput();
            _viewModel.AppendOutput(CleanMarkup(text[0]));

            if (!_model.hasMusicBoxKey)
            {
                _viewModel.AppendOutput("\nType 'search' to search the cabinet, or 'back' to return.");
            }
            else
            {
                _viewModel.AppendOutput(CleanMarkup(text[4]));
            }
        }

        public async Task CabinetSearch(List<string> text)
        {
            if (!_model.hasMusicBoxKey)
            {
                _model.hasMusicBoxKey = true;
                _viewModel.ClearOutput();
                _viewModel.AppendOutput(CleanMarkup(text[3]));

                await _viewModel.WaitForKeyPress();
                _viewModel.ClearOutput();
                _viewModel.UpdateRoomDisplay(_model.room_idx);
            }
        }

        // ===== PHOTO & SAFE (HALLWAY - 5) =====
        public async Task Photo(List<string> text)
        {
            _viewModel.ClearOutput();
            _itemModel.Photo();
            var lines = _itemModel.GetPhoto;

            for (int i = 0; i < 5; i++)
            {
                _viewModel.AppendOutput(CleanMarkup(lines[i]));
            }
            // Bez ASCII photo - będziemy mieć grafikę

            if (_model.KnowsSafeLocation && !_model.SafeOpened)
            {
                _viewModel.AppendOutput("\n" + CleanMarkup(text[0]));
                _viewModel.AppendOutput("\nType 'open safe' to try opening it, or 'back' to return.");
            }
            else
            {
                await _viewModel.WaitForKeyPress();
                _viewModel.ClearOutput();
                _viewModel.UpdateRoomDisplay(_model.room_idx);
            }
        }

        public void Safe(List<string> text)
        {
            _viewModel.ClearOutput();
            _viewModel.AppendOutput(CleanMarkup(text[0]));
            _viewModel.AppendOutput("\nEnter 5-digit password (or type 'back' to cancel):");
        }

        public bool SafeCheckPassword(string password)
        {
            if (password == _model.SafePassword)
            {
                _model.SafeOpened = true;
                _model.hasDevice = true;
                _viewModel.ClearOutput();
                _viewModel.AppendOutput("✅ Safe opened!");
                _viewModel.AppendOutput("🔧 You found the DEVICE!");
                _viewModel.AppendOutput("Use it in the LAB with 'use device'");
                return true;
            }
            else
            {
                _viewModel.AppendOutput("❌ Wrong password. Try again.");
                return false;
            }
        }

        // ===== MUSIC BOX (BEDROOM - 7) =====
        public void MusicBox(List<string> text)
        {
            _viewModel.ClearOutput();

            if (!_model.hasMusicBoxKey)
            {
                _viewModel.AppendOutput(CleanMarkup(text[0]));
                _viewModel.AppendOutput(CleanMarkup(text[1]));
            }
            else if (!_model.hasRing)
            {
                _viewModel.AppendOutput(CleanMarkup(text[0]));
                _viewModel.AppendOutput("\nType 'open' to open the music box, or 'back' to return.");
            }
            else
            {
                _viewModel.AppendOutput("You already took the ring.");
            }
        }

        public async Task MusicBoxOpen(List<string> text)
        {
            if (!_model.hasRing)
            {
                _model.hasRing = true;
                _viewModel.ClearOutput();
                _viewModel.AppendOutput(CleanMarkup(text[3]));

                await _viewModel.WaitForKeyPress();
                _viewModel.ClearOutput();
                _viewModel.UpdateRoomDisplay(_model.room_idx);
            }
        }

        // ===== DIARY (BEDROOM - 7) =====
        public void Diary(List<string> text)
        {
            if (diaryOpened)
            {
                _viewModel.AppendOutput(CleanMarkup(_itemModel.DiaryAddons[0]));
                return;
            }

            if (!_model.hasDiaryKey)
            {
                _viewModel.ClearOutput();
                foreach (var l in text)
                {
                    _viewModel.AppendOutput(CleanMarkup(l));
                }
                _viewModel.AppendOutput(CleanMarkup(_itemModel.DiaryAddons[1]));
                return;
            }

            ShowDiaryPage(1);
        }

        public void ShowDiaryPage(int page)
        {
            currentDiaryPage = page;
            _itemModel.DiaryPages(page);

            _viewModel.ClearOutput();
            foreach (var l in _itemModel.DiaryLines)
            {
                _viewModel.AppendOutput(CleanMarkup(l));
            }
            _viewModel.AppendOutput($"\nPage {page}/5 | Commands: 'next page', 'prev page', 'answer', 'back'");
        }

        public int GetCurrentDiaryPage() => currentDiaryPage;

        public bool IsDiaryOpened() => diaryOpened;

        public void DiaryAnswer()
        {
            _viewModel.AppendOutput("\nType the missing word to obtain the fragment:");
        }

        public bool DiaryCheckAnswer(string answer)
        {
            if (answer.ToLower() == "emotion")
            {
                diaryOpened = true;
                _model.Emotion = true;
                _viewModel.ClearOutput();
                _viewModel.AppendOutput("✅ Correct! EMOTION fragment obtained!");
                ShowStatus("e");
                return true;
            }
            else
            {
                _viewModel.AppendOutput(CleanMarkup(_model.wrong));
                return false;
            }
        }

        // ===== DESK (OFFICE - 8) =====
        public void Desk(List<string> text)
        {
            _viewModel.ClearOutput();
            _viewModel.AppendOutput(CleanMarkup(text[0]));
            _viewModel.AppendOutput("\nType 'desk' to check desk, 'drawer' to check drawer, or 'back' to return.");
        }

        public void DeskCheck()
        {
            _viewModel.ClearOutput();
            _viewModel.AppendOutput(CleanMarkup(_itemModel.Cipher[0]));
        }

        public void DrawerCheck()
        {
            _viewModel.ClearOutput();
            _viewModel.AppendOutput(CleanMarkup(_itemModel.Cipher[1]));
            _model.KnowsSafeLocation = true;
            _viewModel.AppendOutput("\n💡 You now know where the safe is! Check the PHOTO in the HALLWAY.");
        }

        // ===== DEVICE (OFFICE - 8) =====
        public void Destroy()
        {
            if (_model.DEVICE)
            {
                _viewModel.AppendOutput(CleanMarkup(_itemModel.DeviceAddons));
                return;
            }

            _viewModel.ClearOutput();
            List<string> text = _itemModel.DestroyList;
            _viewModel.AppendOutput(CleanMarkup(text[0]));
            _viewModel.AppendOutput("\nType 'activate' to proceed, or 'cancel' to go back.");
        }

        public void DestroyActivate(List<string> text)
        {
            _viewModel.ClearOutput();
            _viewModel.AppendOutput(CleanMarkup(text[3]));
            _viewModel.AppendOutput(CleanMarkup(text[4]));
            _viewModel.AppendOutput("\nEnter the 4-digit code:");
        }

        public bool DestroyCheckCode(string code)
        {
            if (code == "1967")
            {
                _viewModel.AppendOutput("\nType the missing word to obtain the final fragment:");
                return true;
            }
            else
            {
                _viewModel.AppendOutput(CleanMarkup(_itemModel.DestroyList[8]));
                return false;
            }
        }

        public bool DestroyFinalAnswer(string answer)
        {
            if (answer.ToLower() == "morality")
            {
                _model.Morality = true;
                _model.DEVICE = true;
                _viewModel.ClearOutput();
                _viewModel.AppendOutput("✅ MORALITY fragment obtained!");
                _viewModel.AppendOutput(CleanMarkup(_itemModel.DestroyList[5]));
                _viewModel.AppendOutput(CleanMarkup(_itemModel.DestroyList[6]));
                _viewModel.AppendOutput(CleanMarkup(_itemModel.DestroyList[7]));
                ShowStatus("m");
                return true;
            }
            else
            {
                _viewModel.AppendOutput(CleanMarkup(_model.wrong));
                return false;
            }
        }

        // ===== STATUS DISPLAY =====
        private void ShowStatus(string fragment)
        {
            _viewModel.ClearOutput();
            _model.Status();
            foreach (string line in _model.statusLines)
            {
                _viewModel.AppendOutput(CleanMarkup(line));
            }

            if (_model.Reason && _model.Emotion && _model.Morality)
            {
                _viewModel.AppendOutput(CleanMarkup(_itemModel.terminalAddons[3]));
            }
        }
    }
}