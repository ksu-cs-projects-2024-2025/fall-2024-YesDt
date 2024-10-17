using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackersDayOut.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Drawing;


namespace HackersDayOut.StateManagement
{
    public class RoomTransfer: GameScreen
    {

            public RoomTransfer(ScreenManager sm, GameScreen source, GameScreen destination, PlayerIndex? ControllingPlayer)
        {

            source.ExitScreen();
            sm.RemoveScreen(source);
            sm.AddScreen(destination, ControllingPlayer);

        }
    }
}
