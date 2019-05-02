using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adventuretimerough
{

    public static class Output
    {
        private static int Origin_X;
        private static int Origin_Y;
        private static readonly int[] MessageWindowSize = new int[2];
        private static readonly string blankline;
        private static readonly string blankLineRoom;
        private static string[] OutPutBuffer;
        private static int[] CurrentCoords = new int[2];
        private static int[] SavedLineNumber = new int[2];
        private static readonly char save;
        private static readonly char recall;
        //private static readonly string newLineMarker;
        private static readonly int[] SplashPromptCoords = new int[2];
        private static readonly int[] RoomNameCoords = new int[2];
        private static readonly int[] InventoryCoords = new int[2];
        private static readonly int[] ExitListCoords = new int[2];
        private static readonly int[] MessageOutputCoords = new int[2];
        private static readonly int[] CmdLineCoords = new int[2];
        private static readonly int CmdLineWidth;


        private const string splashScreen = @"


         +--------------------------------------------------------------------------------------------------+
         |                                                                                                  |
         |                                                                                                  |
         |                 __  ____                          ____   ____                                    |
         |                /  \/  (_)___  ___  _____   ____  / __/  / __ \____  ____  ____ ___               |
         |               / /\_/ / / __ \/ _ \/ ___/  / __ \/ /_   / / / / __ \/ __ \/ __ `__ \              |
         |              / /  / / / / / /  __(__  )  / /_/ / __/  / /_/ / /_/ / /_/ / / / / / /              |
         |             /_/  /_/_/_/ /_/\___/____/   \____/_/    /_____/\____/\____/_/ /_/ /_/               |
         |                                                                                                  |
         |                                                                                                  |
         |                                                                                                  |
         |                                                                                                  |
         |                                                                                                  |
         |                  ** Please note that re-sizing the console will bork the interface...**          |
         |                                     Press any key to continue...                                 |
         |                                                                                                  |
         |                                                                                                  |
         |                                                                                                  |
         |                                                                                                  |
         +--------------------------------------------------------------------------------------------------+

";


        private const string GameInterface = @"+-------------------------------------------------------------------------------------------+------------------------+
| You are in:                                                                                |        Exits:         |
|                                                                                            |                       |
| ask for (help) if you are stuck                                                            |                       |
+--------------------------------------------------------------------------------------------+-----------------------+
|                                                                                            |You are carrying:      |
|                                                                                            |                       |
|                                                                                            |                       |
|                                                                                            |                       |
|                                                                                            |                       |
|                                                                                            |                       |
|                                                                                            |                       |
|                                                                                            |                       |
|                                                                                            |                       |
|                                                                                            |                       |
|                                                                                            |                       |
|                                                                                            |                       |
|                                                                                            |                       |
|                                                                                            |                       |
|                                                                                            |                       |
|                                                                                            |                       |
|                                                                                            |                       |
|                                                                                            |                       |
|                                                                                            |                       |
+--------------------------------------------------------------------------------------------+-----------------------+
| Command:                                                                                                           |
|                                                                                                                    |
|                                                                                                                    |
+--------------------------------------------------------------------------------------------------------------------+


";
        static Output()
        {
            Console.SetWindowSize(120, 33);
            MessageWindowSize[0] = 90;
            MessageWindowSize[1] = 18;
            blankline = new String(' ', MessageWindowSize[0]);
            blankLineRoom = new string(' ', 65);
            OutPutBuffer = new string[MessageWindowSize[1]+1];
            SplashPromptCoords[0] = 75;
            SplashPromptCoords[1] = 17;
            RoomNameCoords[0] = 14;
            RoomNameCoords[1] = 1;
            ExitListCoords[0] = 100;
            ExitListCoords[1] = 2;
            InventoryCoords[0] = 95;
            InventoryCoords[1] = 7;
            MessageOutputCoords[0] = 3;
            MessageOutputCoords[1] = 5;
            CmdLineCoords[0] = 10;
            CmdLineCoords[1] = 25;
            CmdLineWidth = MessageWindowSize[0] - 40;
            save = 's';
            recall = 'r';
            //newLineMarker = ">";
        }
        
        // InitializeInterface sets the origin coordinates used for all Output methods.
        // The columan and row origins are saved as originX and Origin_Y, respectively.
        // It then draws the user interface for the game.
        public static void InitializeInterface()
        {
            Console.Title = "Mines of DOOM";
            Console.Write(splashScreen);
            WriteAt(SplashPromptCoords, null);
            Console.ReadKey();
            Console.Clear();
            Origin_Y = Console.CursorTop;
            Origin_X = Console.CursorLeft;
            Console.Write(GameInterface);
            Array.Copy(MessageOutputCoords, CurrentCoords, 2);
            Array.Copy(MessageOutputCoords, SavedLineNumber, 2);
        }

        // To use WriteAt, pass it an array with two integers representing X & Y coordinates
        // and a text string to write.
        // For example, if test = [4, 2] 
        // Writeat(test, "testing");
        // Will move the cursor to Column 4, Row 2 and print "testing"
        public static void WriteAt(int[] X_Y_Coords, string text)
        {
            try
            {
                Console.SetCursorPosition(Origin_X + X_Y_Coords[0], Origin_Y + X_Y_Coords[1]);
                Console.Write(text);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.Clear();
                Console.WriteLine("WARNING: INVALID COORDINATES");
            }
        }

        //public static void WriteAt(int[] X_Y_Coords, string text, string ref1)
        //{
        //    X_Y_Coords[0] = -1;
        //    text = ref1 + text;
        //    WriteAt(X_Y_Coords, text);
        //}

        // The WriteValidMoves method is called as part of the movement tracking system,
        // and updates the exits displayed when the player moves to a new room. It is called
        // during Room. SetIdentity method and accepts a string array that contains text
        // identifying which exits are valid. The cursor position is saved, then moved to saved 
        // coordinates for the interface element that shows which moves are valid before
        // writing the moves which are legal from a given room before returning the cursor to
        // the previous position.
        public static void WriteValidMoves(string [] nsew)
        {
            //CursorLine('s');
            string validExits = nsew[0] + nsew[1] + nsew[2] + nsew[3]; 
            WriteAt(ExitListCoords, validExits);
            ClearInputPrompt();
        }

        // CursorLine and ClearInputPrompt are used to save/retrieve the current cursor line
        // number and return to the player command line after writing text to the screen.
        public static void CursorLine(char choice)
        {
            if (choice == 's')
            {
               //SavedLineNumber[0] = Console.CursorLeft;
               SavedLineNumber[1] = CurrentCoords[1];
            }
            else if (choice == 'r')
            {
                //Console.CursorLeft = SavedLineNumber[0];
                Console.CursorTop = SavedLineNumber[1];
            }
        }

        public static void ClearInputPrompt()
        {
            WriteAt(CmdLineCoords, new String(' ', CmdLineWidth));
            WriteAt(CmdLineCoords, "");
        }

        public static void ClearRoomText()
        {
            Array.Copy(MessageOutputCoords, CurrentCoords, 2);
            for (int i = 0; i < MessageWindowSize[1]; i++)
            {
                WriteAt(CurrentCoords, blankline);
                ++CurrentCoords[1];
            }
            Array.Copy(MessageOutputCoords, CurrentCoords, 2);
        }

        public static void WriteMessage(string outputText)
        {
            CursorLine(recall);
            int lineNumber = CurrentCoords[1] - MessageOutputCoords[1];
            if (lineNumber <= MessageWindowSize[1])
                {
                WriteAt(CurrentCoords, outputText);
                OutPutBuffer[lineNumber] = outputText;
                CurrentCoords[1] += 1;
                }
            else
            {
                for(int i = 0; (i + 1) < OutPutBuffer.Length; i++)
                {
                    OutPutBuffer[i] = OutPutBuffer[(i + 1)];
                    CurrentCoords[1] = MessageOutputCoords[1] + i;
                    WriteAt(CurrentCoords, blankline);
                    WriteAt(CurrentCoords, OutPutBuffer[i]);
                }
                CurrentCoords[1] += 1;
                WriteAt(CurrentCoords, blankline);
                WriteMessage(outputText);
            }
            CursorLine(save);
        }

        public static void WriteMessage(string outputText, string ref1)
        {
            string format = String.Format(outputText, ref1);
            WriteMessage(format);
        }

        public static void WriteMessage(string outputText, string ref1, string ref2)
        {
            string format = String.Format(outputText, ref1, ref2);
            WriteMessage(format);
        }

        public static void WriteLongMessage(string[] outputText)
        {
            foreach (string line in outputText)
            {
                if (line?.Length != null)
                {
                    WriteMessage(line);
                }
                else
                { continue; }

            }
        }

        public static void WriteLineBreak()
        {
            WriteMessage(blankline);
        }

        public static void NewRoomText(int newRoomID, string outputText)
        {
            Array.Copy(MessageOutputCoords, CurrentCoords, 2);
            WriteAt(CurrentCoords, outputText);
            OutPutBuffer[0] = outputText;
            CursorLine(save);
        }

        public static void WriteInventory(List<string> Inventory)
        {
            int i = 2;
            string itemListEntry;
            foreach (string item in Inventory)
            {
                itemListEntry = "- " + item; 
                WriteAt(InventoryCoords, itemListEntry);
                InventoryCoords[1] += i;
            }
            InventoryCoords[1] = 7;
        }

        public static void WriteRoomName(string roomName)
        {
            WriteAt(RoomNameCoords, blankLineRoom);
            WriteAt(RoomNameCoords, roomName);
        }
    }
}
