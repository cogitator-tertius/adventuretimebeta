using System;
using System.Collections.Generic;

namespace adventuretimerough
{
    class MainClass
    {

        public static void Main(string[] args)
        {
            //User Input that the game will accept:
            List<string> validInputs = new List<string>()
            {
                "look", "pick up", "north", "south", "east", "west", "down",
                    "inv", "help", "exit"
            };
            List<string[]> LongDescList = new List<string[]>();
            List<Room> RoomList = new List<Room>();
            List<string> Inventory = new List<string>();
            for (int i = 0; i < 19; i++)
            {
                LongDescList.Add(new string[6]);
                RoomList.Add(new Room(i, LongDescList[i]));
            }

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





            LongDescList.Add(new string[6]);



            Room ActiveRoom = new Room();

            Output.InitializeInterface();

            ActiveRoom.SetIdentity(RoomList[0]);

            string input;
            Player p = new Player();
            Output.WriteLongMessage(introText);
            p.MoveTo(ActiveRoom.ID, ActiveRoom.ShortDesc);
            Output.WriteMessage("Have a look around? (type look)");

            //Inventory.Add("towel");

            Output.WriteInventory(Inventory);

            bool again = true;
            //bool playerDied = false;
            while (again)
            {
                bool allowEntry = true;
                Output.ClearInputPrompt();
                input = Console.ReadLine();
                string tempLower = input.ToLower();

                if (p.isDead == true)
                {
                    Output.WriteMessage("The mine has claimed another victim. Press any key to continue...");
                        Console.ReadKey();
                    break;
                }

                if(ActiveRoom.ID == 18)
                {
                    Output.WriteMessage("You are lucky - you managed to escape the mine with your life! Good job, and thanks");
                    Output.WriteMessage("for playing our game.Press any key to continue...");
                    Console.ReadKey();
                    break;
                }
                if (Inventory.Contains("torch") && p.tooDarkToSee == true && p.TorchLit ==false)
                {
                    p.TorchLit = true;
                    Output.WriteMessage("You fumble around for a moment in the dark before you manage to light the torch you");
                    Output.WriteMessage("picked up. Thanks to the illumination it provides you can see again!");

                    Output.WriteLineBreak();
                }
                if (!validInputs.Contains(tempLower))
                {
                    Output.WriteMessage("Do what now? Type (help) for a list of valid commands");
                }

                else
                {

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

                        if(ActiveRoom.ID == 9)
                        {
                            p.isDead = true;
                        }
                        if (ActiveRoom.Loot != null)
                        {
                            Output.WriteMessage("There is a {0} here.", ActiveRoom.Loot);
                            Output.WriteMessage("Do you want to (pick up) the {0}?", ActiveRoom.Loot);
                        }
                        /*if (ActiveRoom.RoomToNorth > 0)
                        {
                            Output.WriteMessage("to the (north), a faint light");
                        }
                        if (ActiveRoom.RoomToSouth > 0)
                        {
                            Output.WriteMessage("to the (south), is that something moving?");
                        }
                        if (ActiveRoom.RoomToEast > 0)
                        {
                            Output.WriteMessage("to the (east), a low grinding rumble");
                        }
                        if (ActiveRoom.RoomToWest > 0)
                        {
                            Output.WriteMessage("to the (west), somethings smells... good?");
                        }*/
                    }

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
                                        //Console.Write(aRoom.ID);
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
                            //bool allowEntry = true;
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
                            //bool allowEntry = true;

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
                            //bool allowEntry = true;
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

                    if (tempLower == "pick up")
                    {
                        if (ActiveRoom.Loot != null)
                        {
                            Output.WriteMessage("You pick up the {0}", ActiveRoom.Loot);
                            Inventory.Add(ActiveRoom.Loot);
                            ActiveRoom.Loot = null;
                            Output.WriteMessage("Cool!");
                            Output.WriteInventory(Inventory);
                        }
                        else
                        {
                            Output.WriteMessage("Nothing to pick up here!");
                        }
                        Output.WriteLineBreak();
                    }

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
                                    Output.WriteMessage("You can't climb down safely without                a some rope");
                                }
                                break;


                            default:
                                Output.WriteMessage("You can't go that way!");
                                break;
                        }
                    }

                    if (tempLower == "inv")
                    {
                        Output.WriteMessage("you have: ");
                        foreach (string item in Inventory)
                        {
                            Output.WriteMessage(item);
                        }
                    }


                    if (ActiveRoom.ID == 7 && p.TorchLit == false)
                    {
                        Output.WriteMessage("A huge crash startles you as rubble fills the passage you came");
                        Output.WriteMessage("in through. What little light there was filtering in is blocked now.");
                        Output.WriteMessage("You are in pitch black darkness....");
                        Output.WriteLineBreak();
                        p.tooDarkToSee = true;
                    }
                    if (tempLower == "help")
                    {
                        Output.WriteMessage("Valid commands:");
                        // Output.WriteMessage("For now try typing:\n north,\n south,\n east,\n" +
                        //    "west,\n pick up,\n exit,\n inv,\n help");
                        foreach (string command in validInputs)
                        {
                            Output.WriteMessage(" - {0}", command);
                        }
                        Output.WriteLineBreak();
                    }
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




