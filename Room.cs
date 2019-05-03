using System;
using System.Collections.Generic;
using System.IO;


namespace adventuretimerough
{
    public class Room
    {
        /*Room objects have the following attributes:
         * Name - the name that is displayed in the "You are in:" element of the UI.
         * ID - unique identifier used for movement system and attribute generation.
         * ShortDesc - the text that is displayed when the player enters the room.
         * LongDesc - the text that is displayed when the player enters the 'look' command.
         * Loot - a string that identifies any items that appear in the room, may be null.
         * RoomTo(North/South/East/West) - defines the Rooms that are connected to a 
         *      particular Room.
         * RequiredToEnter - defines the Loot that is required to move to a certain room 
         *      (eg "key"); may be null if there is no requirement.
         */
        public string Name;
        public int ID { get; set; }
        public string ShortDesc { get; set; }
        public string[] LongDesc;
        public string Loot { get; set; }
        public int RoomToNorth { get; set; }
        public int RoomToSouth { get; set; }
        public int RoomToEast { get; set; }
        public int RoomToWest { get; set; }
        public string RequiredToEnter { get; set; }
        
        // Constructor for Room objects that sets some basic information, not currently in use.
        public Room()
        {
            Name = null;
            RoomToNorth = -1;
            RoomToSouth = -1;
            RoomToEast = -1;
            RoomToWest = -1;
            LongDesc = new string[6];
        }

        // Overload for the Room constructor that is used for generating the various rooms in the
        // game along with their attributes. Each Room is identified by Room.ID, which is passed as
        // an integer in the main class. This method sets defaults for each room and then uses switch
        // and case to create each room and assign the appropriate attributes to it.
        public Room(int RoomID, string[] LongDescArray)
        {
            Name = null;
            RoomToNorth = -1;
            RoomToSouth = -1;
            RoomToEast = -1;
            RoomToWest = -1;
            Loot = null;
            RequiredToEnter = null;


            switch (RoomID)
            {
                case 0:
                    Name = "The Entryway";
                    ID = RoomID;
                    ShortDesc = "";
                    string[] LongDescText0 = new string[]
                    {
                        "You see a decrepit entryway to the (east) with weatherworn tracks and equipment",
                        "scattered about, worn away by age, covered in rubble, loose rock, and plant overgrowth.",
                        " ",
                        null,
                        null,
                        null
                    };
                    Array.Copy(LongDescText0, LongDescArray, 6);
                    RoomToEast = 1;
                    break;

                case 1:
                    Name = "QuarryMaster's Office";
                    ID = RoomID;
                    ShortDesc = "You see an old wood table with a well-worn journal on it.";
                    string[] LongDescText1 = new string[]
                    {
"The writing on the old paper is hard to make out, though it seems to be a report dated", "July 19th 1886, from the quarrymaster about the daily progress of the mines. There is", "a passage to the (south) that curves out of sight and a straight tunnel to the (east).",
                        "",
                        "",
                        ""
                    };
                    Array.Copy(LongDescText1, LongDescArray, 6);
                    Loot = "tattered journal";
                    RoomToSouth = 6;
                    RoomToEast = 2;                    
                    break;

                case 2:
                    Name = "Mining Depot";
                    ID = RoomID;
                    ShortDesc = "You go east through the tunnel and see a torch on the wall.";
                    string[] LongDescText2 = new string[]
                    {
"The room is full of mining equipment, but it is all rotted away and doesn't seem to be", "of much use or value. You spot a torch on the wall covered in a hardening resin and", "cloth, maybe it could be useful? To the (north) you see more mining equipment, to the", "(east) you see a darkening corridor and stagnant air, to the (south) there seems to be",
"a dug out area, to the (east) is back to the journal room.",
                        ""
                    };
                    Array.Copy(LongDescText2, LongDescArray, 6);
                    Loot = "torch";
                    RoomToNorth = 3;
                    RoomToSouth = 6;
                    RoomToEast = 4;
                    RoomToWest = 1;
                    //RequiredToEnter = "tattered journal pages";
                    break;

                case 3:
                    Name = "Equipment Storage";
                    ID = RoomID;
                    if (DoesRopeSpawn())
                    {
                        ShortDesc = "The room is full of old equipment. You spot a rope hanging on the wall. ";
                        Loot = "rope";
                    }
                    else
                    {
                        ShortDesc = "The room is full of old equipment, but you find nothing of use";
                        Loot = null;
                    }
                    string[] LongDescText3 = new string[]
                    {
"The room is full of dilapidated equipment as you look around you see a passage to the", "(east), where there is a putrid smell coming from the hall. To the (south) is the room", "with some equipment.",
                        "",
                        "",
                        ""
                    };
                    Array.Copy(LongDescText3, LongDescArray, 6);
                    RoomToSouth = 2;
                    RoomToEast = 5;
                    RequiredToEnter = null;
                    break;

                case 4:
                    Name = "Ooo! Shiny!";
                    ID = RoomID;
                    ShortDesc = "You see a vaguely shiny object covered in rubble and rock.";
                    string[] LongDescText4 = new string[]
                    {
                        "You see a hilt sticking out of the rocks, is that a sword? What would miners be doing ",
                        "with a sword? You see a passage leading (down) , a foul smelling room to the ",
                        "(north), and a room with some equipment to the (west).",
                        "",
                        "",
                        ""
                    };
                    Array.Copy(LongDescText4, LongDescArray, 6);
                    Loot = "sword";
                    RoomToNorth = 5;
                    RoomToWest = 2;
                    break;

                case 5:
                    Name = "Smelly Chamber";
                    ID = RoomID;
                    ShortDesc = "The room is lined with crystals letting off a faint glow.";
                    string[] LongDescText5 = new string[] {
                        "You get closer to the awful smell and realize that the faint glow is being emitted from",
                        "a human shaped crystal surrounded by the rest which appears to have been pierced by ",
                        "something large and hexagonal, a look of terror frozen on his expression. What a ",
                        "strange art piece for a mine. To the (west) there is mining equipment, and to the (south)",
                        "there is a faintly shiny object.",
                        ""
                    };
                    Array.Copy(LongDescText5, LongDescArray, 6);
                    RoomToSouth = 4;
                    RoomToWest = 3;
                    break;

                case 6:
                    Name = "Precarious Situation";
                    ID = RoomID;
                    ShortDesc = "You hear falling pebbles and can see a minecart track.";
                    string[] LongDescText6 = new string[]
                    {
                        "You notice there is a dynamite filled minecart halfway down the tracks hung over a deep ",
                        "crevice, the tracks appear unstable with the cart stuck in place by a few lucky rocks. ",
                        "The track seems to lead to a flickering light in a room filled with a black, tarry ",
                        "substance in the far off distance. To the (north) is the room with some equipment, to the ",
                        "(west) is the curving corridor.",
                        ""
                    };
                    Array.Copy(LongDescText6, LongDescArray, 6);
                    RoomToNorth = 2;
                    RoomToWest = 1;
                    break;

                case 7:
                    Name = "Another Clue";
                    ID = RoomID;
                    ShortDesc = "You find more pieces of paper crumpled up on a shelf.";
                    string[] LongDescText7 = new string[] 
                    {
                        "The paper is dated September 8th 1886 - another report from the quarrymaster. It seems ",
                        "the mining crew started finding deposits of crystal and was going to request more ",
                        "miners and resources. You wonder why there was still crystal in the mine after all the ",
                        "time that has passed since the writing of this paper. You finish reading, ",
                        "and glance up to see a tunnel (north) and one to the (east).",
                        "",
                        };
                    Array.Copy(LongDescText7, LongDescArray, 6);
                    Loot = "crumpled, hand-written note";
                    RoomToNorth = 8;
                    RoomToEast = 10;
                    break;

                case 8:
                    Name = "Collapsed Floor";
                    ID = RoomID;
                    ShortDesc = "You see a chasm; it seems the floor has mostly collapsed here.";
                    string[] LongDescText8 = new string[] 
                    {
                        "You kick a palm sized stone down the chasm to gauge it's depth and the rock falls for ",
                        "about 4 seconds. If you had something to climb, you might be able to go (down)...",
                        "The room is nothing but cracked stone walls and floors. You can go (south), where you ",
                        "see something glowing in the distance, or (east) into a tunnel.",
                        "",
                        ""
                    };
                    Array.Copy(LongDescText8, LongDescArray, 6);
                    RoomToSouth = 10;
                    RoomToWest = 7;
                    break;

                case 9:
                    Name = "Danger!";
                    ID = RoomID;
                    ShortDesc = "The crystallized miners here who seem like they were trying to get away from something.";
                    string[] LongDescText9 = new string[]
                    {
                        "You move to get a closer look at the crystallized remains of the miners, when",
                        "suddenly you hear a loud snapping sound and before you can turn around, you",
                        "are turned into pure crystal where you stand. ",
                        "You are DEAD.",
                        "",
                        ""
                    };
                    Array.Copy(LongDescText9, LongDescArray, 6);
                    RoomToSouth = 12;
                    break;

                case 10:
                    Name = "Cave In!";
                    ID = RoomID;
                    ShortDesc = "You hear rumbling far away and the entire mine shakes.";
                    string[] LongDescText10 = new string[]
                    {
                        "The cave shakes and rocks begin to fall as choking dust fills of your vision. ",
                        "Something must have happened! As the shaking calms down and dust settles, you see you ",
                        "are surrounded by paths in every direction, to the (north) you can see nothing but ",
                        "darkness, to the (west) is the room with the quarry master notes, to the (south) you",
                        "see more darkness, and to the (east) you see glimpses of sunlight.",
                        ""
                    };
                    Array.Copy(LongDescText10, LongDescArray, 6);
                    //Loot = "Timer**";
                    RoomToNorth = 8;
                    RoomToSouth = 11;
                    RoomToEast = 12;
                    RoomToWest = 7;
                    break;

                case 11:
                    Name = "Key to Freedom";
                    ID = RoomID;
                    ShortDesc = "You see something metallic and shiny on a rock pedestal.";
                    string[] LongDescText11 = new string[]
                    {
                        "You see a key on the rocky pedestal near some spiral steps leading downwards. The room ",
                        "has a tunnel to the (east) that seems like sunlight might be coming from, a tunnel to",
                        "the (north), and circular steps leading (down).",
                        "",
                        "",
                        ""
                    };
                    Array.Copy(LongDescText11, LongDescArray, 6);
                    Loot = "key";
                    RoomToNorth = 10;
                    RoomToEast = 12;
                    break;

                case 12:
                    Name = "Actually Kind of Pretty";
                    ID = RoomID;
                    ShortDesc = "You see a deep pool of water with glowing crystals inside.";
                    string[] LongDescText12 = new string[]
                    {
                        "You are struck by the beauty of the crystals glowing in the water that fills the small ",
                        "lake in this room. There is a tunnel leading (north) full of crystals, another leading",
                        "(west), and a third curving tunnel snakes away to the (south).",
                        "",
                        "",
                        ""
                    };
                    Array.Copy(LongDescText12, LongDescArray, 6);
                    RoomToNorth = 9;
                    RoomToSouth = 11;
                    RoomToWest = 10;
                    break;

                case 13:
                    Name = "It's... Alive?!";
                    ID = RoomID;                    
                    ShortDesc = "A hulking mass of glowing crystals charges at you!";
                    string[] LongDescText13 = new string[]
                    {
                        "You swiftly dodge to the side and the amalgam smashes into the wall! As it turns to",
                        "charge again you swing the sword with all your might, shattering it into a million",
                        "iridescent shards. Catching your breath, you see a path to the (south) and a sturdy ",
                        "iron gate in the room to the (east).",
                        "",
                        ""
                    };
                    Array.Copy(LongDescText13, LongDescArray, 6);
                    RoomToSouth = 16;
                    RoomToEast = 15;
                    break;

                case 14:
                    Name = "The Way Out?";
                    ID = RoomID;
                    ShortDesc = "You see a primitive dolley system.";
                    string[] LongDescText14 = new string[]
                    {
                        "You climb inside the dolley and pull the lever inside. It shakily begins to ascend an",
                        "old mine shaft choked with cobwebs. Hopefully there is a way out of this mine at the ",
                        "top...",
                        "",
                        "",
                        ""
                    };
                    Array.Copy(LongDescText14, LongDescArray, 6);
                    RoomToNorth = 17;
                    RoomToSouth = 18;
                    break;

                case 15:
                    Name = "Need a Key?";
                    ID = RoomID;
                    ShortDesc = "You see a sturdy metal gate. There is a padlock keeping it shut.";
                    string[] LongDescText15 = new string[]
                    {
                        "The gate is firmly held closed by a rusty padlock. Beyond it, you can see a passage ",
                        "leading to the (east).",
                        "",
                        "",
                        "",
                        ""
                    };
                    Array.Copy(LongDescText15, LongDescArray, 6);
                    RoomToEast = 14;
                    RoomToWest = 13;
                    RequiredToEnter = "key";
                    break;

                case 16:
                    Name = "Shroom Room";
                    ID = RoomID;
                    ShortDesc = "This room is full of strange mushrooms and the air is thick with moisture.";
                    string[] LongDescText16 = new string[]
                    {
                        "The mushrooms here are a dizzying array of different shapes and sizes. This dank ",
                        "",
                        "",
                        "",
                        "",
                        ""
                    };
                    Array.Copy(LongDescText16, LongDescArray, 6);
                    RoomToNorth = 13;
                    break;

                case 17:
                    Name = "Bottom of the Chasm";
                    ID = RoomID;
                    ShortDesc = "You safely climb down the rope into a small room.";
                    string[] LongDescText17 = new string[]
                    {
                        "You find yourself in a small chamber with a dirt floor.",
                        "There is a passage to the (south) you can just squeeze through.",
                        "",
                        "",
                        "",
                        ""
                    };
                    Array.Copy(LongDescText17, LongDescArray, 6);
                    RoomToEast = 14;
                    RequiredToEnter = "rope";
                    break;

                case 18:
                    Name = "Sweet Freedom";
                    ID = RoomID;
                    ShortDesc = "You see daylight and take a deep breath of fresh air.";
                    string[] LongDescText18 = new string[]
                    {
                        "",
                        "for playing our game.",
                        ":)",
                        "",
                        "",
                        ""
                    };
                    Array.Copy(LongDescText18, LongDescArray, 6);
                    break;

                default:
                    ID = RoomID;
                    ShortDesc = "";
                    string[] LongDescTextdefault = new string[]
                    {
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""
                    };
                    break;
            }
        }

        /* SetIdentity is used to update the ActiveRoom attributes to match the destination 
           Room when the player moves to a Room. It sets each attribute to the destination 
           Room's attributes, which are passed as targetRoom. Next, it clears the output 
           window of the old room's text and resets the line counter used to write room text.
           Finally, it calls the UpdateRoomInfo method in this class to update the valid 
           exits that are displayed in the interface. */
        public void SetIdentity(Room targetRoom)
        {
            Name = targetRoom.Name;
            ID = targetRoom.ID;
            ShortDesc = targetRoom.ShortDesc;
            LongDesc = targetRoom.LongDesc;
            Loot = targetRoom.Loot;
            RoomToNorth = targetRoom.RoomToNorth;
            RoomToSouth = targetRoom.RoomToSouth;
            RoomToEast = targetRoom.RoomToEast;
            RoomToWest = targetRoom.RoomToWest;
            RequiredToEnter = targetRoom.RequiredToEnter;
            //Output.WriteLineBreak();
            UpdateRoomInfo(this);
        }

        // Determines whether the "rope" Loot is spawned in the Room where
        // it may appear in each game.
        public bool DoesRopeSpawn()
        {
            //bool result;
            Random random = new Random();
            if(random.Next(1, 10) > 5)
            { return  true; }
            else { return false; }
        }

        // UpdateRoomInfo is called as part of the SetIdentity method.
        // It passes a string type array to Output.WriteValidMoves
        // that indicates which directions are valid movement choices from
        // a given room, and overwrites exits that are not valid anymore.
        // Finally it updates the Room name in the UI element.

        public void UpdateRoomInfo(Room targetRoom)
        {
            string[] nsew = { " ", "  " , "  " , "  " };
            if (targetRoom.RoomToNorth > 0)
            { nsew[0] = "N"; }
            if (targetRoom.RoomToSouth > 0)
            { nsew[1] = " S"; }
            if (targetRoom.RoomToEast > 0)
            { nsew[2] = " E"; }
            if (targetRoom.RoomToWest > 0)
            { nsew[3] = " W"; }
            Output.WriteValidMoves(nsew);
            Output.WriteRoomName(targetRoom.Name);
        }
    }

}

