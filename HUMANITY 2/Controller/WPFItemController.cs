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

        private void Clear()
        {
            _viewModel.ClearOutput();
        }

        private async Task AwaitKey()
        {
            await _viewModel.WaitForKeyPress();
        }

        // ===== TERMINAL (LAB - 0) =====
        public async Task Monitor(List<string> text)
        {
            Clear();
            _viewModel.AppendOutput("=== TERMINAL MENU ===\n");
            _viewModel.AppendOutput("1. CHECK CURRENT PATIENT STATUS");
            _viewModel.AppendOutput("2. OPEN TEST LOGS");

            if (_model.Reason && _model.Emotion && _model.Morality)
            {
                _viewModel.AppendOutput("3. RESTORE HUMANITY");
            }

            _viewModel.AppendOutput("4. QUIT TERMINAL");
            _viewModel.AppendOutput("\nType the number (1-4) to select option.\n");

            // To będzie obsłużone przez normalny input gracza
        }

        public void MonitorOption1() // Status
        {
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
                _viewModel.AppendOutput("❌ DATA CORRUPTED.");
                return;
            }

            _viewModel.AppendOutput("\n=== TEST LOGS ===");
            _viewModel.AppendOutput("Use commands: 'next page', 'prev page', 'back'");
            _viewModel.AppendOutput("\nShowing page 1/5...\n");
            ShowLogPage(1);
        }

        private int currentLogPage = 1;
        public void ShowLogPage(int page)
        {
            currentLogPage = page;
            _itemModel.Logs(page);

            foreach (var line in _itemModel.logLines)
            {
                _viewModel.AppendOutput(CleanMarkup(line));
            }
            _viewModel.AppendOutput($"\nPage {page}/5 | Commands: 'next page', 'prev page', 'back'");
        }

        public void MonitorOption3() // Restore Humanity - ENDING
        {
            Clear();
            _viewModel.AppendOutput("⚠️  WARNING: This action is IRREVERSIBLE.");
            _viewModel.AppendOutput("\nDo you wish to restore your HUMANITY?");
            _viewModel.AppendOutput("This will erase everything.\n");
            _viewModel.AppendOutput("Type 'yes' to proceed or 'no' to cancel.\n");
        }

        public async Task ShowGoodEnding()
        {
            Clear();
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
            Clear();
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
        private bool whiteboardPassed = false;
        private int whiteboardGuesses = 0;
        private bool whiteboardHint = false;

        public void Whiteboard(List<string> text)
        {
            if (whiteboardPassed)
            {
                _viewModel.AppendOutput(CleanMarkup(_itemModel.whiteboardList[6]));
                return;
            }

            Clear();
            _viewModel.AppendOutput(CleanMarkup(text[0]));
            _viewModel.AppendOutput(CleanMarkup(text[1]));
            _viewModel.AppendOutput("\nType 'solve' to attempt solving, or 'leave' to go back.");
        }

        public void WhiteboardSolve()
        {
            Clear();
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
                Clear();
                _viewModel.AppendOutput(CleanMarkup(_itemModel.whiteboardList[7]));
                _viewModel.AppendOutput("\n✅ REASON fragment obtained!");
                _model.Reason = true;
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

        // ===== KEY (STAIRS - 1) =====
        public void Key(List<string> text)
        {
            foreach (var line in text)
            {
                _viewModel.AppendOutput(CleanMarkup(line));
            }
        }

        // ===== NEWSPAPER (KITCHEN - 4) =====
        public void Newspaper(List<string> text)
        {
            Clear();
            _itemModel.Newspaper();
            var lines = _itemModel.GetNewspaper;

            foreach (var x in lines)
            {
                _viewModel.AppendOutput(CleanMarkup(x));
            }
        }

        // ===== BOOKSHELF (LIBRARY - 3) =====
        public void Bookshelf(List<string> text)
        {
            Clear();
            _viewModel.AppendOutput(CleanMarkup(_itemModel.bookshelf[0]));
            _viewModel.AppendOutput("\nType coordinates as 'row column' (e.g., '9 5')");
            _viewModel.AppendOutput("Or type 'back' to leave.\n");
            _viewModel.AppendOutput("Try: Row 9, Column 5 for something interesting...");
        }

        private bool poemOpened = false;
        public void BookshelfCoordinates(int row, int col, List<string> text)
        {
            if (row == 9 && col == 5)
            {
                Clear();
                _viewModel.AppendOutput(CleanMarkup(text[0]));
                _viewModel.AppendOutput("\nType 'read' to read the poem, or 'back' to return.");
            }
            else if (row == 4 && col == 20)
            {
                Clear();
                _viewModel.AppendOutput(CleanMarkup(_itemModel.ShowNotes()));
            }
            else
            {
                Clear();
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

            Clear();
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
                Clear();
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
        public void Table(List<string> text)
        {
            Clear();
            foreach (string s in text)
            {
                _viewModel.AppendOutput(CleanMarkup(s));
            }
        }

        // ===== CLOCK (LIVING ROOM - 2) =====
        public void Clock(List<string> text)
        {
            Clear();
            _viewModel.AppendOutput(CleanMarkup(text[0]));
            _viewModel.AppendOutput(CleanMarkup(_itemModel.ShowClock()));
        }

        // ===== PIANO (LIVING ROOM - 2) =====
        public void Piano(List<string> text)
        {
            Clear();
            if (!_model.hasDiaryKey)
            {
                _viewModel.AppendOutput(CleanMarkup(text[0]));
                _viewModel.AppendOutput(CleanMarkup(text[1]));
                _viewModel.AppendOutput(CleanMarkup(_itemModel.ShowPiano()));
                _viewModel.AppendOutput("\n🎹 Play the correct sequence: D-A-F-A");
                _viewModel.AppendOutput("Type the sequence (e.g., 'dafa') to try:");
            }
            else
            {
                _viewModel.AppendOutput(CleanMarkup(text[2]));
                _viewModel.AppendOutput(CleanMarkup(_itemModel.ShowPiano()));
            }
        }

        public bool PianoCheckSequence(string sequence)
        {
            if (sequence.ToLower().Replace(" ", "") == "dafa")
            {
                _model.hasDiaryKey = true;
                Clear();
                _viewModel.AppendOutput("🎵 The piano opens!");
                _viewModel.AppendOutput("✅ You found the DIARY KEY!");
                return true;
            }
            else
            {
                _viewModel.AppendOutput("❌ Wrong sequence. Try again.");
                return false;
            }
        }

        // ===== MIRROR & NOTE (BATHROOM - 6) =====
        public void Mirror(List<string> text)
        {
            Clear();
            _viewModel.AppendOutput(CleanMarkup(text[0]));
            _viewModel.AppendOutput(CleanMarkup(text[1]));
            _viewModel.AppendOutput("\nType 'read note' to read it, or 'back' to return.");
        }

        public void Note(string text)
        {
            Clear();
            _viewModel.AppendOutput(CleanMarkup(text));
        }

        // ===== CABINET (BATHROOM - 6) =====
        public void Cabinet(List<string> text)
        {
            Clear();
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

        public void CabinetSearch(List<string> text)
        {
            if (!_model.hasMusicBoxKey)
            {
                _model.hasMusicBoxKey = true;
                Clear();
                _viewModel.AppendOutput(CleanMarkup(text[3]));
            }
        }

        // ===== PHOTO & SAFE (HALLWAY - 5) =====
        public void Photo(List<string> text)
        {
            Clear();
            _itemModel.Photo();
            var lines = _itemModel.GetPhoto;

            for (int i = 0; i < 5; i++)
            {
                _viewModel.AppendOutput(CleanMarkup(lines[i]));
            }
            _viewModel.AppendOutput(CleanMarkup(_itemModel.ShowPhoto()));

            if (_model.KnowsSafeLocation && !_model.SafeOpened)
            {
                _viewModel.AppendOutput("\n" + CleanMarkup(text[0]));
                _viewModel.AppendOutput("\nType 'open safe' to try opening it, or 'back' to return.");
            }
        }

        public void Safe(List<string> text)
        {
            Clear();
            _viewModel.AppendOutput(CleanMarkup(text[0]));
            _viewModel.AppendOutput("\nEnter 5-digit password (or type 'back' to cancel):");
        }

        public bool SafeCheckPassword(string password)
        {
            if (password == _model.SafePassword)
            {
                _model.SafeOpened = true;
                _model.hasDevice = true;
                Clear();
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
            Clear();

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

        public void MusicBoxOpen(List<string> text)
        {
            if (!_model.hasRing)
            {
                _model.hasRing = true;
                Clear();
                _viewModel.AppendOutput(CleanMarkup(text[3]));
            }
        }

        // ===== DIARY (BEDROOM - 7) =====
        private bool diaryOpened = false;
        private int currentDiaryPage = 1;

        public void Diary(List<string> text)
        {
            if (diaryOpened)
            {
                _viewModel.AppendOutput(CleanMarkup(_itemModel.DiaryAddons[0]));
                return;
            }

            if (!_model.hasDiaryKey)
            {
                Clear();
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

            Clear();
            foreach (var l in _itemModel.DiaryLines)
            {
                _viewModel.AppendOutput(CleanMarkup(l));
            }
            _viewModel.AppendOutput($"\nPage {page}/5 | Commands: 'next page', 'prev page', 'answer'");
        }

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
                Clear();
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
            Clear();
            _viewModel.AppendOutput(CleanMarkup(text[0]));
            _viewModel.AppendOutput("\nType 'desk' to check desk, 'drawer' to check drawer, or 'back' to return.");
        }

        public void DeskCheck()
        {
            Clear();
            _viewModel.AppendOutput(CleanMarkup(_itemModel.Cipher[0]));
        }

        public void DrawerCheck()
        {
            Clear();
            _viewModel.AppendOutput(CleanMarkup(_itemModel.Cipher[1]));
            _model.KnowsSafeLocation = true;
            _viewModel.AppendOutput("\n💡 You now know where the safe is!");
        }

        // ===== DEVICE (OFFICE - 8) =====
        public void Destroy()
        {
            if (_model.DEVICE)
            {
                _viewModel.AppendOutput(CleanMarkup(_itemModel.DeviceAddons));
                return;
            }

            Clear();
            List<string> text = _itemModel.DestroyList;
            _viewModel.AppendOutput(CleanMarkup(text[0]));
            _viewModel.AppendOutput("\nType 'activate' to proceed, or 'cancel' to go back.");
        }

        public void DestroyActivate(List<string> text)
        {
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
                Clear();
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
            Clear();
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