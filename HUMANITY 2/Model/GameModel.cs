namespace HumanityWPF.Model
{
    public class GameModel
    {
        public bool Reason { get; set; } = false;  
        public bool Emotion { get; set; } = false;
        public bool Morality { get; set; } = false;
        public string wrong = "[red]W R O N G[/]\n";

        public bool hasKey { get; set; } = false;
        public string gotKey = "[green]You used the key.[/]";
        public bool hasMusicBoxKey { get; set; } = false;
        public bool hasDiaryKey { get; set; } = false;
        public bool hasRing { get; set; } = false;

        public bool KnowsSafeLocation { get; set; } = false;
        //---wazne--//
        public bool DEVICE { get; set; } = false;
        public bool hasDevice { get; set; } = false;
        //----------//
        public int sanity { get; set; } = 100;
        // Stan ogólny
        public bool IntroPlayed { get; set; } = false;  /// ZMIENIĆ NA false;
        public string SafePassword { get; set; } = "41070";
        public bool SafeOpened { get; set; } = false;
        public int room_idx { get; set; } = 0;

        //// STARTER
        public List<string> startGameList = new List<string>()
        {
            "WARNING: Consciousness integrity compromised.",
            "> Consciousness fragmentation detected.",
            "> REASON... lost.",
            "> EMOTION... lost.",
            "> MORALITY... lost.",
            "> Awaiting system reboot...",
            "----- SYSTEM REBOOT -----",
            "\r----- SYSTEM REBOOT ----- PARTIAL",
            "\nSession terminated."
        };

        ////NacisnijAbyKontynuować
        public List<string> pressAny = new List<string>()
        {
            "[olive slowblink]\nPress any button to continue...[/]\n\n",
            "[olive slowblink]\nPress any button to [/][red slowblink]END THE GAME...[/]\n\n"
        };
        public string continuee = "[grey underline]\nPress any button to continue...[/]\n";



        // Prosty dziennik wspomnień (do komendy RECALL)
        /* public List<string> MemoryLogs { get; } = new()
         {
             "[MEMORY LOG #07]\nSubject: Anna Voss\nReaction: Panic. Neural feedback overload.\nOutcome: Terminal brain death.\nNote: Decrease voltage in next trial.",
             "[MEMORY LOG #12]\nNo volunteers left.\nBeginning self-experimentation."
         };

         public int MemoryIndex { get; set; } = 0;

         public string StatusLine =>
             $"Status — Reason: {(Reason ? "ONLINE" : "offline")} | Emotion: {(Emotion ? "ONLINE" : "offline")} | Morality: {(Morality ? "ONLINE" : "offline")}";
        */
        public bool[] LookedRoom = new bool[9];
        public List<string> Intro = new()
        {
            "> Backup data loaded",
            "> Initializing neural interface...",
            "> Experiment database - inclomplete",
            "> Last session data found",
            "> Restoring memories...",
            "> ~$ apt-sudo install get coffee",
            "> Neural map - incomplete",
            "> Scanning patient identity",
            "> Destroying non-essential memories...",
            "> You can't read this message...",
            "> Checking sanity...",
        };
        public List<string> Ghosts = new()
        {
            "Y O U\nS H O U L D\nK E E P\nR O T T I N G",
            "O N C E\nH U M A N\n\nN E V E R\nH U M A N",
            "\nT A K E\nH U M A N I T Y\n\nL O S E\nH U M A N I T Y",
            "F A L S E\nH U M A N\n\nT R U E\nM O N S T E R",
            "L I E S\n\nC A L L E D\\nP R O M I S E S",
        };
        public class PrologueLine
        {
            public string Text { get; set; }
            public string Color { get; set; }
            public int DelayMs { get; set; }

            public PrologueLine(string text, int delay, string color)
            {
                Text = text;
                DelayMs = delay;
                Color = color;
            }
        }
        public List<PrologueLine> Prologue = new()
        {
           new PrologueLine( "You shouldn’t have woken up.\n", 45 ,"[italic teal]"),
           new PrologueLine("\nA sharp, high-pitched noise pierces the silence.\nThe light above you flickers, buzzing faintly, washing the room in a cold, sterile glow.\nMetal. Concrete. The faint smell of chemicals and ozone.\nYou’re lying on a metal table, surrounded by shattered glass and dark stains that look too much like blood.\n", 15, "[italic silver]"),
           new PrologueLine("\nYou don’t remember how you got here.\nYou don’t remember why you’re here.\nYou don’t even remember your name.\n", 15, "[italic yellow4]"),
           new PrologueLine("\nBeside you, a monitor flickers to life, spilling pale light across the room.\n For a second, shapes and words flash across the screen — too fast to read —\n then everything freezes.\nOne of the machines is connected straight to your body. The heart rate monitor flickers, as if it can’t decide whether there’s a heartbeat to find, but you’re not dead,\n", 15, "[italic silver]"),
           new PrologueLine("Are you?\n", 45, "[italic red]"),
           new PrologueLine("\nSomewhere behind you, a faint echo — a breath, or maybe a whisper.\n", 15, "[italic silver]"),
           new PrologueLine("You promised…\n", 45, "[italic teal]"),
           new PrologueLine("It’s hard to tell if it came from the room... or from inside your head.\nYour heartbeat — slow, irregular… fills the silence again.\n", 15, "[italic silver]"),
           new PrologueLine("\nYou can’t form a single coherent thought in your brain - like someone’s rewiring your thoughts from the inside.\n", 15, "[italic silver]"),
           new PrologueLine("You can’t feel anything…\n", 15, "[italic red]"),
           new PrologueLine("You were part of a scheme you know nothing about. Seems like you were just a disposable element…\n", 15, "[italic silver]"),
           new PrologueLine("\nThe hum of dying machines and the atmosphere of a room feels too familiar to be a stranger’s.\nAfter brief confusion, you realize that whatever you were before...", 15, "[italic silver]"),
           new PrologueLine(" you are not anymore.\n", 45, "[italic red]")
        };

        public List<PrologueLine> EpilogueGood = new()
        {
            new PrologueLine("You connect the device to the system.\n", 20, "[italic silver]"),
           new PrologueLine("For a moment, nothing happens - then the lights flicker, and the monitors burst into static.\nLines of code twist and dissolve, data collapsing in on itself like fading memories.\n", 15, "[italic silver]"),
           new PrologueLine("A new window appears on the central screen:\n", 15, "[italic silver]"),
           new PrologueLine("> \"Do you wish to restore your HUMANITY — knowing it will erase everything?\"\n", 40, "[italic yellow4]"),
           new PrologueLine("\nYour hand trembles.\nYou press \"YES.\"\n", 35, "[italic red]"),
           new PrologueLine("\nThe world spins.\nThe walls melt into blinding white, and distant echoes flood your mind — laughter, warmth, music, her voice.\nThen silence.\n", 15, "[italic silver]"),
           new PrologueLine("\nAdrian opens his eyes.\n", 35, "[italic yellow4]"),
           new PrologueLine("\nHe's standing in his house — not a lab, not anymore. Morning sunlight pours through the curtains.\nThe doorbell rings.\n", 15, "[italic silver]"),
           new PrologueLine("\nAdrian blinks, his heart pounding.\nHe walks to the door and opens it — the same moment he remembers.\nA young volunteer stands there, smiling nervously waiting to be 'cured'.\n", 15, "[italic silver]"),
           new PrologueLine("\"Not today,\" he says softly.\n", 45, "[italic teal]"),
           new PrologueLine("He closes the door and turns around. Elara is there, eyes full of quiet hope.\n", 15, "[italic silver]"),
           new PrologueLine("He walks to her - and for the first time in a long time, he doesn’t think about experiments, or data, or immortality.\n", 15, "[italic silver]"),
           new PrologueLine("\nHe just holds her.\n", 100, "[italic orchid]"),
           new PrologueLine("\n The device hums faintly one last time, and then everything fades to peace.\n", 20, "[italic silver]"),
        };
        public List<PrologueLine> EpilogueBad = new()
        {
           new PrologueLine("You connect the device to the system.\n", 20, "[italic silver]"),
           new PrologueLine("The screens blink awake, lines of data pulsing like veins under glass.\nThen — a new prompt appears:\n", 15, "[italic silver]"),
           new PrologueLine("> \"Do you wish to restore your HUMANITY — knowing it will erase everything?\"\n", 40, "[italic yellow4]"),
           new PrologueLine("\nYour reflection stares back from the monitor — hollow eyes, trembling hands.\nAnd yet… you hesitate.\n", 15, "[italic silver]"),
           new PrologueLine("You press \"NO.\"\n", 45, "[italic red]"),
           new PrologueLine("\nThe hum deepens.\nThe air thickens, vibrating with a low, metallic tone that crawls under your skin.\nEvery light flares crimson.\nThe lab seems to breathe.\n", 15, "[italic silver]"),
           new PrologueLine("\nThe device glows — then shatters, spilling energy across the room like liquid fire.\n", 15, "[italic silver]"),
           new PrologueLine("The screens scream with distorted faces, hundreds of them, twisting and merging — the lost volunteers, their minds echoing through the circuitry.\n", 15, "[italic silver]"),
           new PrologueLine("Their voices overlap, whispering one word:\n", 15, "[italic silver]"),
           new PrologueLine("> \"Why…\"\n", 45, "[italic teal]"),
           new PrologueLine("\nYou stumble back, clutching your head as their memories flood you — fear, pain, confusion, their final moments before silence.\n", 15, "[italic silver]"),
           new PrologueLine("And through it all, you hear her voice.\n", 15, "[italic silver]"),
           new PrologueLine("> \"Adrian… you promised you’d stop.\"\n", 45, "[italic teal]"),
           new PrologueLine("\nThe lights flicker once… twice… then darkness.\n", 15, "[italic silver]"),
           new PrologueLine("\nWhen you open your eyes, you are no longer standing.\nYou *are* the lab.\n", 35, "[italic yellow4]"),
           new PrologueLine("\nCables stretch like nerves.\nMonitors blink like eyes.\nEvery sound — your thoughts, looping endlessly, decaying into static.\n", 15, "[italic silver]"),
           new PrologueLine("You try to scream, but only the speakers respond, echoing the same mechanical phrase:\n", 15, "[italic silver]"),
           new PrologueLine("> \"Subject: Adrian Holloway. Consciousness preserved.\"\n", 45, "[italic teal]"),
           new PrologueLine("\nNo heartbeat.\nNo body.\nOnly an endless awareness, trapped inside the cold shell of your own creation.\n", 15, "[italic silver]"),
           new PrologueLine("\nOutside, somewhere far above, the house collapses under its own weight.\nBut deep within the servers, your mind keeps running.\nForever.\n", 15, "[italic silver]"),
           new PrologueLine("\nAnd in the static… Elara whispers one last time — soft, broken, and final:\n", 15, "[italic silver]"),
           new PrologueLine("> \"You finally proved it, Adrian… Consciousness can live without a soul.\"\n", 45, "[italic teal]"),
        };
        public string goodbye = "[lime]THANKS FOR PLAYING.\n[/][yellow1]-Emi & Eryk.[/]";
        public string Uknown()
        {
            return "Unknown command. Type HELP for a list of commands.";
        }
        public string Help()
        {
            if (!hasDevice)
            {
                return "-------------------------------------------------------------------------\n" +
                       "|Available commands:                                                     |\n" +
                       "|- HELP: Show this help message.                                         |\n" +
                       "|- [yellow]LOOK[/]: Observe your surroundings.                                      |\n" +
                       "|- [teal]CHECK[/] [[ITEM]]: Describe item.                                          |\n" +
                       "|- [palegreen1_1]GO TO[/] [[ROOM]]: move to the next room.                                  |\n" +
                       "|- QUIT/EXIT: Terminate the session and give up on your H U M A N I T Y. |\n" +
                       "-------------------------------------------------------------------------";
            }
            else
            {
                return "-------------------------------------------------------------------------\n" +
                      "|Available commands:                                                     |\n" +
                      "|- HELP: Show this help message.                                         |\n" +
                      "|- [yellow]LOOK[/]: Observe your surroundings.                                      |\n" +
                      "|- [teal]CHECK[/]: Describe item.                                                   |\n" +
                      "|- [palegreen1_1]GO TO[/] [[ROOM]]: move to the next room.                                  |\n" +
                      "|- QUIT/EXIT: Terminate the session and give up on your H U M A N I T Y. |\n" +
                      "|-[red] USE DEVICE: DESTROY ALL THE EVIDENCE IN LAB   [/]                        |\n" +
                      "-------------------------------------------------------------------------";
            }
        }
        public List<string> look = new();
        public void pickLook(int idx, int part)
        {
            look.Clear();
            switch (idx)
            {
                case 0:
                    look.Add("You are in a ");
                    look.Add("[yellow]LAB[/]");
                    look.Add(".\nThe room is filled with scientific equipment, ");
                    look.Add("[teal]MONITORS [/]");
                    look.Add("displaying data, and an unsolved, barely recognizible \nequation scribbled on a ");
                    look.Add("[teal]WHITEBOARD [/]");
                    look.Add("as if reason itself was lost here.\n");

                    look.Add("Behind you,");
                    look.Add(" [palegreen1_1]STAIRS [/]");
                    look.Add("lead back upward.");
                    look.Add("\n\n[darkred]Something about this room feels painfully familiar.[/]");
                    return;

                case 1:
                    look.Add("You are on the ");
                    look.Add("[yellow]STAIRS[/]");
                    look.Add(".\n");

                    look.Add("The steps creak softly, carrying echoes you can't place. ");
                    look.Add("\nCold air drifts through the narrow space. The ");
                    look.Add("[teal]floor[/]");
                    look.Add(" feels worn beneath your feet.\n");

                    look.Add("Below is the ");
                    look.Add("[palegreen1_1]LAB[/]");
                    look.Add(". Ahead lies the ");
                    look.Add("[palegreen1_1]LIVING ROOM[/]");
                    look.Add(". A side passage leads into the ");
                    look.Add("[palegreen1_1]HALLWAY[/]");
                    look.Add(".\n\n");

                    look.Add("[darkred]A quiet tension hangs in the air, urging you to choose a path.[/]");
                    return;

                case 4:
                    look.Add("You are in the ");
                    look.Add("[yellow]KITCHEN[/]");
                    look.Add(".\n");

                    look.Add("The air smells faintly of stale coffee and unwashed dishes. ");
                    look.Add("Dust motes float in the shafts of light, settling on a ");
                    look.Add("[teal]NEWSPAPER[/]");
                    look.Add(" left carelessly on the counter, its headlines yellowed and cracked with age.\n");

                    look.Add("Behind you, the doorway leads back to the ");
                    look.Add("[palegreen1_1]LIVING ROOM[/]");
                    look.Add(".\n\n");

                    look.Add("[darkred]Something about the mundane chaos here makes your thoughts drift — a story half-remembered, details just out of reach.[/]");
                    return;

                case 3:
                    look.Add("You are in the ");
                    look.Add("[yellow]LIBRARY[/]");
                    look.Add(".\n");

                    look.Add("The room is dominated by a single, towering ");
                    look.Add("[teal]BOOKSHELF[/]");
                    look.Add(", reaching from floor to ceiling, packed with countless books.\n");

                    look.Add("Next to you, the doorway opens back to the ");
                    look.Add("[palegreen1_1]LIVING ROOM[/]");
                    look.Add(".\n\n");

                    look.Add("[darkred]The quiet here presses on your thoughts, each spine a potential clue, each title a whisper of forgotten knowledge.[/]");
                    return;

                case 2:
                    look.Add("You are in the ");
                    look.Add("[yellow]LIVING ROOM[/]");
                    look.Add(".\n");

                    look.Add("Soft light filters through dusty curtains, highlighting a ");
                    look.Add("[teal]CLOCK[/]");
                    look.Add(" on the wall and a ");
                    look.Add("[teal]PIANO[/]");
                    look.Add(" in the corner. \nThe faint scent of old wood and forgotten melodies lingers.\n");

                    look.Add("Behind you, the ");
                    look.Add("[palegreen1_1]STAIRS[/]");
                    look.Add(" lead back down. ");
                    look.Add("A doorway ahead opens into the ");
                    look.Add("[palegreen1_1]KITCHEN[/]");
                    look.Add(". ");
                    look.Add("To your left, a passage leads toward the ");
                    look.Add("[palegreen1_1]LIBRARY[/]");
                    look.Add(".\n\n");
                    look.Add("[darkred]Thoughts drift through your mind like scattered notes, elusive and ungraspable, hinting at what might have been lost.[/]");
                    // look.Add("[darkred]Every object here seems to hum with memories you cannot fully grasp.[/]");
                    return;

                case 5:
                    look.Add("You are in the ");
                    look.Add("[yellow]HALLWAY[/]");
                    look.Add(".\n");

                    look.Add("The narrow corridor stretches before you, dimly lit by a single, swaying bulb. ");
                    look.Add("Along the wall hangs a ");
                    look.Add("[teal]PHOTO[/]");
                    look.Add(", faded and \nslightly askew, capturing a beautiful moment for memories.\n");

                    look.Add("You see the ");
                    look.Add("[palegreen1_1]STAIRS[/]");
                    look.Add(" leading down, ");
                    look.Add("[palegreen1_1]LIVING ROOM[/]");
                    look.Add(" and ");
                    look.Add("doors branch off to the ");
                    look.Add("[palegreen1_1]BATHROOM[/]");
                    look.Add(", the ");
                    look.Add("[palegreen1_1]BEDROOM[/]");
                    look.Add(", and the ");
                    look.Add("[palegreen1_1]OFFICE[/]");
                    look.Add(".\n\n");

                    look.Add("[darkred]The corridor feels like a thread connecting scattered fragments of thought, each door hinting at secrets yet to be uncovered.[/]");

                    return;

                case 6:
                    look.Add("You are in the ");
                    look.Add("[yellow]BATHROOM[/]");
                    look.Add(".\n");

                    look.Add("Tile walls glisten faintly under dim light, the air is heavy with the scent of damp and forgotten soap. \nA ");
                    look.Add("[teal]MIRROR[/]");
                    look.Add(" reflects your image oddly, distorted by time, while a small ");
                    look.Add("[teal]CABINET[/]");
                    look.Add(" sits closed in the corner, hiding unknown contents.\n\n");

                    look.Add("Behind you, the doorway leads back to the ");
                    look.Add("[palegreen1_1]HALLWAY[/]");
                    look.Add(".\n\n");

                    look.Add("[darkred]Reflections and shadows mingle here, stirring thoughts that feel just out of reach.[/]");

                    return;

                case 7:
                    look.Add("You are in the ");
                    look.Add("[yellow]BEDROOM[/]");
                    look.Add(".\n");

                    look.Add("Soft light filters through curtains onto a neatly made bed. ");
                    look.Add("On a nearby dresser sits a delicate ");
                    look.Add("[teal]MUSIC BOX[/]");
                    look.Add(", silent for now, and a worn ");
                    look.Add("[teal]DIARY[/]");
                    look.Add(", its pages full of private thoughts.\n\n");

                    look.Add("Behind you, the doorway opens to the ");
                    look.Add("[palegreen1_1]HALLWAY[/]");
                    look.Add(".\n\n");

                    look.Add("[darkred]The room carries an intimate weight, hinting at memories both cherished and painful.[/]");

                    /*   if (Emotion==false)
                           look.Add("\n \nYou don't feel any [orchid]EMOTIONS.[/]");
                       else
                           look.Add("\n \nYou feel a wide range of [orchid]EMOTIONS. [/]"); */
                    return;
                case 8:
                    look.Add("You are in the ");
                    look.Add("[yellow]OFFICE[/]");
                    look.Add(".\n");

                    look.Add("The room smells faintly of old paper and ink. ");
                    look.Add("A large ");
                    look.Add("[teal]DESK[/]");
                    look.Add(" dominates the space, papers scattered and stationery neatly arranged, as if waiting for someone to resume the work.\n\n");

                    look.Add("Behind you, the doorway leads back to the ");
                    look.Add("[yellow]HALLWAY[/]");
                    look.Add(".\n\n");

                    look.Add("[darkred]The silence presses in, making you wonder what secrets these walls have witnessed.[/]");

                    return;
                default:
                    return;
            }
        }
        private string wrongItem(string item)
        {
            string wrong = "There is no such item as" + item;
            return wrong;
        }
        public List<string> checkItem(int idx, string item)
        {
            List<string> itemDesc = new();
            switch (item)
            {
                /////////////////LAB
                case "monitor":
                case "monitors":
                case "terminal":
                    if (idx == 0)
                    {
                        itemDesc.Add("[lime]Welcome, Dr. Hallaway. What would you like to do?\n[/]");
                        itemDesc.Add("[lime]CHECK CURRENT PATIENT STATUS[/]");
                        itemDesc.Add("[lime]OPEN TEST LOGS[/]");
                        itemDesc.Add("[red]RESTORE HUMANITY.[/]");
                        itemDesc.Add("[lime]QUIT TERMINAL[/]");
                    }
                    else itemDesc.Add(wrongItem(item));
                    break;

                case "whiteboard":
                    if (idx == 0)
                    {
                        itemDesc.Add("The whiteboard is filled with confusing conclusions. Seems like someone was trying to figure out how human brain functions on the emotional and spiritual level.");
                        itemDesc.Add("\nYou can try solving this scientific riddle... \nDo you want to try?\n");
                        itemDesc.Add("[lime]YES[/]");
                        itemDesc.Add("[lime]NO[/]");
                    }
                    else wrongItem(item);
                    break;
                //////////////////////////

                /////////////////STAIRS
                case "floor":
                    if (idx == 1)
                    {
                        if (!hasKey)
                        {
                            itemDesc.Add("[silver]You found a [/][deepskyblue2]key[/][silver] on the floor. It looks old and rusty. What does it open?[/]");
                            hasKey = true;
                        }
                        else
                        {
                            itemDesc.Add("[grey underline]There's nothing interesting here.[/]\n");
                        }
                    }
                    break;
                /////////////////////////

                ///////////////////KITCHEN
                case "newspaper":
                    if (idx == 4)
                    {
                        itemDesc.Add("The newspaper is old and blacked out, but the text is still fully readable.\n");
                    }
                    break;
                //////////////////////////

                //////////////////////LIBRARY
                case "bookshelf":
                    {
                        if (idx == 3)
                        {
                            itemDesc.Add("[lightslateblue]You found a romantic poem with a letter snuck inside of it. Do you want to read it?[/]\n \n");
                            itemDesc.Add("[lime]YES[/]");
                            itemDesc.Add("[lime]NO[/]");
                        }
                    }
                    break;
                case "table":
                    {
                        if(idx==3)
                        {
                            itemDesc.Add("[grey]There's an open book on the table. Someone crossed out the text of this book - leaving a written message:[/]\n");
                            itemDesc.Add("[red]\nTHERE ARE TWO BOOKS THAT MIGHT INTEREST YOU.[/]\n");
                            //itemDesc.Add("[darkred]\nYOU LIKED THE CONCEPT OF TIME.\nSHE LIKED MUSIC.[/]\n");
                        }
                    }
                    break;
                ///////////////////////////////

                ////////////////////////LIVING ROOM
                case "clock":
                    if (idx == 2)
                    {
                        itemDesc.Add("The clock on the wall is broken, its hands frozen.\nA faint inscription reads: 'Time is an illusion.'");
                    }
                    break;
                default:
                    wrongItem(item);
                    break;
                case "piano":
                    if (idx == 2)
                    {
                        itemDesc.Add("[silver]The piano is old, but it still works. You can play it however you want.[/]\n");
                        itemDesc.Add("[grey underline]Use buttons C, D, E, F, G, A, H to play, press ENTER to leave[/]");

                        itemDesc.Add("[silver]You already got the [/][yellow1]Diary key[/][silver].[/]");
                        itemDesc.Add("\n[lightslateblue]You hear something switched. The piano opened and you found[/] [yellow1]Diary key[/][lightslateblue]![/]\n");
                    }
                    break;
                /////////////////////////////////////////

                ///////////////////////////////HALLWAY
                case "photo":
                case "picture":
                    {
                        if (idx == 5)
                        {
                            itemDesc.Add("\n[gold1]Do you want to check what's behind the photo?[/]\n");
                            itemDesc.Add("[lime]YES[/]");
                            itemDesc.Add("[lime]NO[/]");
                        }
                    }
                    break;
                case "safe":
                    {
                        if (idx == 5)
                        {
                            itemDesc.Add("[silver]The safe is protected with a 5-digit password, do you want to try and open it?[/]\n");
                            itemDesc.Add("[lime]YES[/]");
                            itemDesc.Add("[lime]NO[/]");

                            itemDesc.Add("[yellow1]You opened the safe. You found the[/] [lime]device[/][yellow1]. Use it in a [/][palegreen1_1]LAB[/][yellow1].[/]\n");

                            itemDesc.Add("[red]Wrong password. Try again.[/]\n");

                        }
                    }
                    break;
                ////////////////////////////////////////

                //////////////////////////////BATHROOM
                case "cabinet":
                    {
                        if (idx == 6)
                        {
                            itemDesc.Add("[silver]Do you want to search the cabinet?[/]\n");
                            itemDesc.Add("[lime]YES[/]");
                            itemDesc.Add("[lime]NO[/]");
                            itemDesc.Add("\n[silver]You obtained[/][yellow1] music box key[/][silver].[/]");
                            itemDesc.Add("[grey]You already checked it before.[/]");
                        }
                    }
                    break;
                case "mirror":
                    {
                        if (idx == 6)
                        {
                            itemDesc.Add("[silver]You see your reflection in the mirror. Your face looks tired and worn out, like you've been through a lot.[/]\n");
                            itemDesc.Add("[silver]You noticed a note hidden behind the mirror.[/]\n");
                            itemDesc.Add("\n[silver]Do you want to read it?[/]");
                            itemDesc.Add("[lime]YES[/]");
                            itemDesc.Add("[lime]NO[/]");
                            itemDesc.Add("[red]     Books[/]\n[silver]aren't the only\nthing that stops\n[/][red]     time.[/]");
                        }
                    }
                    break;
                //////////////////////////////////////

                ///////////////////////////BEDROOM
                case "diary":
                    {
                        if (idx == 7)
                        {
                            itemDesc.Add("\n");
                        }
                    }
                    break;
                case "music":
                case "musicbox":
                case "music box":
                    {
                        if (idx == 7)
                        {
                            if (!hasMusicBoxKey)
                            {
                                itemDesc.Add("[silver]You see a closed music box. The main thing that catches your eye are the written numbers: \n[/][gold1]4[/][silver] and[/][gold1] 20[/]\n");
                                itemDesc.Add("\n \n[silver]There is also a keyhole. [/][red]You don't have the key yet.[/]");
                            }
                            else
                            {
                                itemDesc.Add("[yellow1]The key fit perfectly into the music box.[/][silver]A beautiful wind-up ballerina appeared before you.[/]\n \nBeneath her, you could see a small drawer. Do you want to open it?\n \n");
                                itemDesc.Add("[lime]YES[/]");
                                itemDesc.Add("[lime]NO[/]");
                                itemDesc.Add("[silver]You found a [/][lime]wedding ring[/][silver] inside the music box. There's a text engraved on it: \n \n[/][yellow1]Elara & Adrian[/][silver].[/]\n");
                            }
                        }
                    }
                    break;
                ////////////////////////////////

                /////////////OFFICE             
                case "desk":
                    {
                        if (idx == 8)
                        {
                            itemDesc.Add("[silver]Do you want to check what's on the desk, or what's insite the drawer?[/]\n");
                            itemDesc.Add("[lime]CHECK DESK[/]");
                            itemDesc.Add("[lime]CHECK DRAWER[/]");
                            itemDesc.Add("[lime]NEVERMIND[/]");
                        }
                    }
                    break;
                    ////////////////////////

            }
            return itemDesc;
        }
        public int NextRoomIdx(string room)
        {
            int next_idx;
            switch (room)
            {
                case "lab":
                    next_idx = 0;
                    return next_idx;

                case "stairs":
                    next_idx = 1;
                    return next_idx;

                case "living room":
                case "living":
                    next_idx = 2;
                    return next_idx;

                case "library":
                    next_idx = 3;
                    return next_idx;

                case "kitchen":
                    next_idx = 4;
                    return next_idx;

                case "hallway":
                case "second floor":
                case "hall":
                case "f2 hallway":
                    next_idx = 5;
                    return next_idx;

                case "bath":
                case "bathroom":
                    next_idx = 6;
                    return next_idx;

                case "bedroom":
                    next_idx = 7;
                    return next_idx;

                case "office":
                    next_idx = 8;
                    return next_idx;

                default:
                    return -1;
            }
        }
        public void GoTo_Possible(int idx)
        {
            room_idx = idx;
        }
        public string RoomName(int x)
        {
            switch (x)
            {
                case 0:
                    return "LABORATORY";
                case 1:
                    return "STAIRS";
                case 2:
                    return "LIVING ROOM";
                case 3:
                    return "LIBRARY";
                case 4:
                    return "KITCHEN";
                case 5:
                    return "HALLWAY";
                case 6:
                    return "BATHROOM";
                case 7:
                    return "BEDROOM";
                case 8:
                    return "OFFICE";
                default:
                    return "UNKNOWN LOCATION";
            }
        }
        public List<string> statusLines = new();
        public List<string> Status()
        {
            statusLines.Clear();
            statusLines.Add("[lime bold]SYSTEM STATUS:[/]\n");
            statusLines.Add($"[lime]- REASON: {(Reason ? "ONLINE[/]\n" : "[/][red]OFFLINE[/]\n")}");
            statusLines.Add($"[lime]- EMOTION: {(Emotion ? "ONLINE[/]\n" : "[/][red]OFFLINE[/]\n")}");
            statusLines.Add($"[lime]- MORALITY: {(Morality ? "ONLINE[/]\n" : "[/][red]OFFLINE[/]\n")}");
            return statusLines;
        }

        public string Quit()
        {
            return ("You feel a sudden wave of relief as you decide to give up.\n" +
            "The weight of uncertainty lifts off your shoulders, replaced by a strange sense of calm.\n" +
            "As you step away from the challenge, a part of you wonders what could have been achieved.\n" +
            "But for now, you choose peace over the unknown.");
        }
    }
}
