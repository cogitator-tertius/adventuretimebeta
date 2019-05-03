using System;
using System.Collections.Generic;

namespace adventuretimerough
{
    class MainClass
    {

        public static void Main(string[] args)
        {

            // Declare and assign lists for storing room info and the long description of each room.
            // See the Room class for more information.

            List<string[]> LongDescList = new List<string[]>();
            List<Room> RoomList = new List<Room>();
            
            for (int i = 0; i < 19; i++)
            {
                LongDescList.Add(new string[6]);
                RoomList.Add(new Room(i, LongDescList[i]));
            }

            // Introductory text blurb for the start of a new game.
            // Likely to be migrated out of main program once there is time for more cleanup.
            string[] introText = new string[]
            {
                "You have spent the day climbing a rocky mountain to find an old mine you heard about in",
                "a nearby village. The villagers cautioned you with tales of a dangerous curse upon",
                "the mine, but they are by and large an uneducated, superstitious lot. You set out with",
                "the hope of finding that the miners have left behind some valuables inside. As the sun",
                "begins to set, you find yourself at the entrance to the mine.",
                "Who knows what dangers may lurk within the depths?",
                " "
            };

            // Initializes the system for writing text to various parts of the UI and storing messages in
            // a buffer as they are displayed to the player. Also responsible for throwing up the initial
            // splash screen and drawing the game interface. See OutputHandler class for more information.
            Output.InitializeInterface();

            // We use a dummy room called Activeroom that inherits the attributes of whichever room the
            // the player is in through the SetIdentity() method. As the player moves, the ActiveRoom is
            // updated with data based on the ID attribute of whichever room the player moves to.
            Room ActiveRoom = new Room();
            ActiveRoom.SetIdentity(RoomList[0]);

            // Creates a new Player object, displays some brief greetings, and gets starting conditions
            // for the player.
            Player p = new Player();

            // Declares the variable we use for inventory tracking and display throughout the course of
            // normal gameplay.
            List<string> Inventory = new List<string>();


            // Writes the introductory text blurb, sets the ActiveRoom to the first room of the game,
            // displays a short description and prompts the player to use their first command.
            Output.WriteLongMessage(introText);
            p.MoveTo(ActiveRoom.ID, ActiveRoom.ShortDesc);
            Output.WriteMessage("Have a look around? (type look)");


           // Declare variable used for player input, and the list of accepted input it will be checked
           // against.
            string input;
            List<string> validInputs = new List<string>()
            {
                "look", "pick up", "north", "south", "east", "west", "down",
                     "help", "exit"
            };

            // The main loop responsible for the game will continue to accept input and display text
            // as long as the again variable is true; it is set to true until the player decides to exit
            // or the main loop is broken by another condition.
            // Main loop needs cleanup, bad!
            bool again = true;
            
            while (again)
            {
                // These are things that need to be reset after each command entry:
                //      - allowEntry checks the requiredToEnter attribute of each room & bounces player if they don't
                //          have what they need to enter the room.
                //      - ClearInputPrompt clears the last entry from the input prompt by overwriting it with " "
                //          and resets the cursor position to the command prompt
                //      - input takes user input, and is then parsed to lowercase and stored in tempLower
                bool allowEntry = true;
                Output.ClearInputPrompt();
                input = Console.ReadLine();
                string tempLower = input.ToLower();

                // If the player gets killed by something, it is GAME OVER MAN!
                if (p.isDead == true)
                {
                    Output.WriteMessage("The mine has claimed another victim. Press any key to continue...");
                        Console.ReadKey();
                    break;
                }

                // If the player makes it out alive, they get a little congratulatory message and the game quits.
                // **TO DO: make winning a little more exciting.
                if(ActiveRoom.ID == 18)
                {
                    Output.WriteMessage("You are lucky - you managed to escape the mine with your life! Good job, and thanks");
                    Output.WriteMessage("for playing our game.Press any key to continue...");
                    Console.ReadKey();
                    break;
                }

                // If it is too dark for the player to see inside the mine, and the player has not lit the torch yet, they do!
                // ** consider moving this to another command that the player can enter.
                if (Inventory.Contains("torch") && p.tooDarkToSee == true && p.TorchLit ==false)
                {
                    p.TorchLit = true;
                    Output.WriteMessage("You fumble around for a moment in the dark before you manage to light the torch you");
                    Output.WriteMessage("picked up. Thanks to the illumination it provides you can see again!");

                    Output.WriteLineBreak();
                }

                // If the player enters an invalid command, return an error and suggest looking at the valid commands.
                if (!validInputs.Contains(tempLower))
                {
                    Output.WriteMessage("Do what now? Type (help) for a list of valid commands");
                }

                else
                {
                    // The player can get killed in a few different ways; this happens if they don't have the sword when they need it.
                    // **implement combat minigame?
                    if (ActiveRoom.ID == 13)
                    {
                        if (!Inventory.Contains("sword"))
                        {                            
                            Output.WriteMessage("You desperately dodge out of the way just in time to avoid the charge!");
                            Output.WriteMessage("As you scramble for something to defend yourself the creature slams");
                            Output.WriteMessage("into you and everything goes dark. You are DEAD!");
                            p.isDead = true;
                            Output.WriteLineBreak();
                            continue;
                        }
                    }

                    // the "look" command will usually provide the long description of a room, unless it is too dark to see and
                    // they player does not have a lit torch.
                    if (tempLower == "look")
                    {

                        if (p.tooDarkToSee == true && p.TorchLit == false)
                        {
                            Output.WriteMessage("You can't see anything! If only you had a light source...");
                            continue;
                        }
                        else
                        {
                            Output.WriteLongMessage(LongDescList[ActiveRoom.ID]);
                        }

                        Output.WriteLineBreak();


                        // Another way the player can meet an unpleasant end...
                        if(ActiveRoom.ID == 9)
                        {
                            p.isDead = true;
                        }

                        // If the current room has loot the player can grab, give them a little prod to pick it up.
                        if (ActiveRoom.Loot != null)
                        {
                            Output.WriteMessage("There is a {0} here.", ActiveRoom.Loot);
                            Output.WriteMessage("Do you want to (pick up) the {0}?", ActiveRoom.Loot);
                        }
                    }

                    // "north", "south, "east" and "west" all move the player after checking to see that they meet
                    // the entry requirements and that there is actually a room to move to. If not, an error is displayed.
                    // ** update failure to meet entry requirements text.
                    if (tempLower == "north")
                    {

                        if (ActiveRoom.RoomToNorth > 0)
                        {
                            if (RoomList[ActiveRoom.RoomToNorth].RequiredToEnter != null)
                            {
                                if (!Inventory.Contains(RoomList[ActiveRoom.RoomToNorth].RequiredToEnter))
                                {
                                    Output.WriteMessage("error-- item missing: " + ActiveRoom.RequiredToEnter);
                                    allowEntry = false;
                                }
                            }

                            if (allowEntry == true)
                            {
                                foreach (Room aRoom in RoomList)
                                {
                                    if (ActiveRoom.RoomToNorth == aRoom.ID)
                                    {
                                        //Console.Write(aRoom.ID); --debug usage
                                        ActiveRoom.SetIdentity(aRoom);
                                        break;
                                    }
                                }
                            p.MoveTo(ActiveRoom.ID, ActiveRoom.ShortDesc);
                            }
                        }
                        else
                        {
                            Output.WriteMessage("You can't go that way...");
                            Output.WriteLineBreak();
                        }
                    }

                    if (tempLower == "south")
                    {
                        if (ActiveRoom.RoomToSouth > 0)
                        {
                            if (RoomList[ActiveRoom.RoomToSouth].RequiredToEnter != null)
                            {
                                if (!Inventory.Contains(RoomList[ActiveRoom.RoomToSouth].RequiredToEnter))
                                {
                                    Output.WriteMessage("error -- item missing " + ActiveRoom.ID);
                                    allowEntry = false;
                                }
                            }
                            if (allowEntry == true)
                            {
                                foreach (Room aRoom in RoomList)
                                {
                                    if (aRoom.ID == ActiveRoom.RoomToSouth)
                                    {
                                        ActiveRoom.SetIdentity(aRoom);
                                        break;
                                    }
                                }
                                p.MoveTo(ActiveRoom.ID, ActiveRoom.ShortDesc);
                            }
                        }
                        else
                        {
                            Output.WriteMessage("You can't go that way...");
                            Output.WriteLineBreak();
                        }
                    }

                    if (tempLower == "east")
                    {
                        if (ActiveRoom.RoomToEast > 0)
                        {
                          

                            if (RoomList[ActiveRoom.RoomToEast].RequiredToEnter != null)
                            {
                                if (!Inventory.Contains(RoomList[ActiveRoom.RoomToEast].RequiredToEnter))
                                {
                                    Output.WriteMessage("error-- item missing " + ActiveRoom.ID);
                                    allowEntry = false;
                                }
                            }

                            if (allowEntry == true)
                            {
                                foreach (Room aRoom in RoomList)
                                {
                                    if (aRoom.ID == ActiveRoom.RoomToEast)
                                    {
                                        ActiveRoom.SetIdentity(aRoom);
                                        break;
                                    }
                                }
                                p.MoveTo(ActiveRoom.ID, ActiveRoom.ShortDesc);
                            }
                        }
                        else
                        {
                            Output.WriteMessage("You can't go that way...");
                            Output.WriteLineBreak();
                        }
                    }

                    if (tempLower == "west")
                    {
                        if (ActiveRoom.RoomToWest > 0)
                        {
                            if (RoomList[ActiveRoom.RoomToWest].RequiredToEnter != null)
                            {
                                if (!Inventory.Contains(RoomList[ActiveRoom.RoomToWest].RequiredToEnter))
                                {
                                    Output.WriteMessage("error -- item missing " + ActiveRoom.ID);
                                    allowEntry = false;
                                }
                            }

                            if (allowEntry == true)
                            {
                                foreach (Room aRoom in RoomList)
                                {
                                    if (aRoom.ID == ActiveRoom.RoomToWest)
                                    {
                                        ActiveRoom.SetIdentity(aRoom);
                                        break;
                                    }
                                }
                                p.MoveTo(ActiveRoom.ID, ActiveRoom.ShortDesc);
                            }
                        }
                        else
                        {
                            Output.WriteMessage("You can't go that way...");
                            Output.WriteLineBreak();
                        }
                    }

                    // "pick up" adds the loot in the current room to the player's inventory and removes it from
                    // ActiveRoom as well as the associated Room object so it does not respawn later.
                    // **TO DO - update the description text to reflect that the object is gone.
                    if (tempLower == "pick up")
                    {
                        if (ActiveRoom.Loot != null)
                        {
                            Output.WriteMessage("You pick up the {0}", ActiveRoom.Loot);
                            Inventory.Add(ActiveRoom.Loot);
                            ActiveRoom.Loot = null;
                            RoomList[ActiveRoom.ID].Loot = null;
                            Output.WriteMessage("Cool!");
                            Output.WriteInventory(Inventory);
                        }
                        else
                        {
                            Output.WriteMessage("Nothing to pick up here!");
                        }
                        Output.WriteLineBreak();
                    }

                    // There are 3 levels to the dungeon; if the player is in the correct room, they can use
                    // "down" to progress to the next floor. It is a one way trip...
                    if (tempLower == "down")
                    {
                        switch (ActiveRoom.ID)
                        {
                            case 4:
                                foreach (Room aRoom in RoomList)
                                {
                                    if (7 == aRoom.ID)
                                    {
                                        //Console.Write(aRoom.ID);
                                        ActiveRoom.SetIdentity(aRoom);
                                        break;
                                    }
                                }
                                p.MoveTo(ActiveRoom.ID, ActiveRoom.ShortDesc);
                                break;

                            case 11:
                                foreach (Room aRoom in RoomList)
                                {
                                    if (13 == aRoom.ID)
                                    {
                                        //Console.Write(aRoom.ID);
                                        ActiveRoom.SetIdentity(aRoom);
                                        break;
                                    }
                                }
                                p.MoveTo(ActiveRoom.ID, ActiveRoom.ShortDesc);
                                break;

                            case 8:
                                if (Inventory.Contains("rope"))
                                {
                                    foreach (Room aRoom in RoomList)
                                    {
                                        if (17 == aRoom.ID)
                                        {
                                            //Console.Write(aRoom.ID);
                                            ActiveRoom.SetIdentity(aRoom);
                                            break;
                                        }
                                    }

                                    p.MoveTo(ActiveRoom.ID, ActiveRoom.ShortDesc);
                                }
                                else
                                {
                                    Output.WriteMessage("You can't climb down safely without some rope!");
                                }
                                break;


                            default:
                                Output.WriteMessage("You can't go that way!");
                                break;
                        }
                    }

                    // "inv" displays your inventory. deprecated since it has been added to UI.
                    if (tempLower == "inv")
                    {
                        Output.WriteMessage("you have: ");
                        foreach (string item in Inventory)
                        {
                            Output.WriteMessage(item);
                        }
                    }

                    // It gets dark sometimes in the mines... hopefully the player is ready.
                    if (ActiveRoom.ID == 7 && p.TorchLit == false)
                    {
                        Output.WriteMessage("A huge crash startles you as rubble fills the passage you came");
                        Output.WriteMessage("in through. What little light there was filtering in is blocked now.");
                        Output.WriteMessage("You are in pitch black darkness....");
                        Output.WriteLineBreak();
                        p.tooDarkToSee = true;
                    }

                    // "help" writes a list of valid commands if the player is stuck
                    if (tempLower == "help")
                    {
                        Output.WriteMessage("Valid commands:");
                        foreach (string command in validInputs)
                        {
                            Output.WriteMessage(" - {0}", command);
                        }
                        Output.WriteLineBreak();
                    }

                    //"exit" quits the game
                    if (tempLower == "exit")
                    {
                        again = false;
                        Output.WriteMessage("Ah, the terror must be too much for a coward such as yourself!");
                        Output.WriteMessage("Press any key to quit...");
                        Console.ReadKey();
                        break;
                    }
                } 
                }
            }

        }

    }




