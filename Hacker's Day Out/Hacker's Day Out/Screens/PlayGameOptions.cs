using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.TimeZoneInfo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

using HackersDayOut.Screens;
using HackersDayOut.StateManagement;

namespace HackersDayOut.Screens;
{
    public class PlayGameOptions : MenuScreen
    {




        public PlayGameOptions() : base("PlayGameOptions")
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            var continueEntry = new MenuEntry("Continue");
            var newGameEntry = new MenuEntry("Start New Game");

            continueEntry.Selected += ContinueMenuEntrySelected;
            newGameEntry.Selected += NewGameMenuEntrySelected;


            var back = new MenuEntry("Back");

            back.Selected += OnCancel;

            MenuEntries.Add(continueEntry);
            MenuEntries.Add(newGameEntry);
            MenuEntries.Add(back);

        }


        private void ContinueMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {

            string text = File.ReadAllText("progress.txt");

            if (text.Contains("Door1Unlocked")) Hallway1.ObjDoor.State = doorState.Open;
            if (text.Contains("Door2Unlocked")) Hallway3.ObjDoor2.State = doorState.Open;
            if (text.Contains("Door3Unlocked")) Hallway3.ObjDoor3.State = doorState.Open;
            if (text.Contains("Door4Unlocked")) Hallway3.ObjDoor4.State = doorState.Open;
            if (text.Contains("Door5Unlocked")) Hallway2.ObjDoor5.State = doorState.Open;
            if (text.Contains("Door6Unlocked")) Gym.ObjDoor6.State = doorState.Open;
            if (text.Contains("Door7Unlocked")) Classroom3.ObjDoor7.State = doorState.Open;


            if (text.Contains("PybookAcquired")) ScreenManager.PythonBookCollected = true;
            if (text.Contains("PycodeAcquired")) ScreenManager.PythonCodeCollected = true;
            if (text.Contains("JavabookAcquired")) ScreenManager.JavaBookCollected = true;
            if (text.Contains("JavacodeAcquired")) ScreenManager.JavaCodeCollected = true;
            if (text.Contains("CbookAcquired")) ScreenManager.CBookCollected = true;
            if (text.Contains("CcodeAcquired")) ScreenManager.CCodeCollected = true;
            if (text.Contains("CSharpbookAcquired")) ScreenManager.CSharpBookCollected = true;
            if (text.Contains("CSharpcodeAcquired")) ScreenManager.CSharpCodeCollected = true;

            if (text.Contains("RoomHallway1")) LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new Hallway1(new Vector2(200, 280)));
            else if (text.Contains("RoomCompLab")) LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new ComputerRoom(new Vector2(985, 210)));
            else if (text.Contains("RoomHallway2")) LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new Hallway2(new Vector2(1035, 215)));
            else if (text.Contains("RoomGym")) LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new Gym(new Vector2(150, 250)));
            else if (text.Contains("RoomHallway3")) LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new Hallway3(new Vector2(1035, 255)));
            else if (text.Contains("RoomClassroom1")) LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new Classroom1(new Vector2(165, 253)));
            else if (text.Contains("RoomClassroom2")) LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new Classroom2(new Vector2(955, 178)));
            else if (text.Contains("RoomClassroom3")) LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new Classroom3(new Vector2(165, 178)));
            else
            {
                LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new Hallway1(new Vector2(200, 280)));
            }
        }

        private void NewGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.PythonBookCollected = false;
            ScreenManager.PythonCodeCollected = false;
            ScreenManager.JavaBookCollected = false;
            ScreenManager.JavaCodeCollected = false;
            ScreenManager.CBookCollected = false;
            ScreenManager.CCodeCollected = false;
            ScreenManager.CSharpBookCollected = false;
            ScreenManager.CSharpCodeCollected = false;

            File.WriteAllText("progress.txt", "Door1: Locked, Door2: Locked, Door3: Locked, Door4: Locked, Door5: Locked, Door6: Locked, Door7: Locked \n" +
                "Pybook: Missing, Pycode: Missing, Javabook: Missing, Javacode: Missing, Cbook: Missing, Ccode: Missing, CSharpbook: Missing, CSharpcode: Missing \n"
                + "Room: Hallway1");
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new Hallway1(new Vector2(200, 280)));

        }

    }
}