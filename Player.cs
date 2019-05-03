using System;
namespace adventuretimerough
{
    public class Player
    {
        /* Player objects have the following attributes:
         * Name - the Name that the Player enters for their character.
         * Location - the ID of the Room that the player is currently in.
         * TorchLit - identifies whether the 'torch' Loot is lit or not.
         * MoveCount - tracks how many times the player has moved to a new Room.
         *      At a certain point in the game, there is a time limit imposed that
         *      limits the number of additional moves a player can make.
         * MoveLimit - when the player triggers the start of the move countdown
         *      the maximum number of moves is stored here.
         * MoveLimitActive - defaults to false, toggles to true when the move limit
         *      is activated.
         */
        public string Name { get; set; }
        //public string Inv1 { get; set; }
        //public string Inv2 { get; set; }
        private int Location { get; set; }
        public bool TorchLit;
        public int MoveCount;
        public int MoveLimit;
        bool MoveLimitActive;
        public bool tooDarkToSee;
        public bool isDead;

        // The constructor for the Player class. Calls SetPlayerName to request
        // user input for the player name and display a greeting. Also sets the
        // starting Room, move count, isDead, tooDarkToSee, and TorchLit status.
        public Player()
        {
            SetPlayerName();
            TorchLit = false;
            Location = 0;
            MoveCount = 0;
            MoveLimit = 0;
            MoveLimitActive = false;
            tooDarkToSee = false;
            isDead = false;
        }

        // Displays a brief greeting and requests input which is then stored as
        // the Player.Name attribute when a new player starts the game.
        public void SetPlayerName()
        {
            Output.WriteMessage("Hello bold adventurer! What is your name?");
            // Output.WriteMessage("");
            Output.ClearInputPrompt();
            Name = Console.ReadLine();
            Output.WriteMessage("Ho! Well met!");
            Output.WriteLineBreak();
            Output.WriteMessage("{0}: ", Name);

        }

        // Provides a means for other classes to access and update the player
        // Location private variable.
        public int CurrentLocation
        {
            get { return Location; }
            set
            {
                Location = value;
            }
        }
        // Called as part of moving to a new Room, increments the number of moves
        // taken during the game and updates the Location of the Player. Displays
        // a short message indicating the player has moved, then displays the short
        // room description unless it is too dark, the starting room, or the torch
        // is lit.
        public void MoveTo(int newRoomID, string newRoomShortDesc)
        {
            Location = newRoomID;
            MoveCount++;
            if(Location == 10 && MoveLimitActive == false)
            {
                MoveLimit = MoveCount + 8;
                MoveLimitActive = true;
            }
            if(Location != 0) { Output.WriteMessage("You head into the next room..."); }
            
            Output.WriteLineBreak();
            if (tooDarkToSee == false || Location == 0 || TorchLit == true)
            {
                Output.WriteMessage(newRoomShortDesc);
            }
            else
            {
                Output.WriteMessage("You can't see anything! If only you had a light source...");
            }

        }

    }
}
