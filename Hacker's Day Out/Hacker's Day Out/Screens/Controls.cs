using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackersDayOut.Screens
{
    public class Controls : MenuScreen
    {


        private readonly MenuEntry _controlsEntry;
        private readonly MenuEntry _InteractEntry;


        public Controls() : base("Controls")
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _controlsEntry = new MenuEntry(string.Empty);
            _InteractEntry = new MenuEntry(string.Empty);

            setControlsEntryText();
            var back = new MenuEntry("Back");

            back.Selected += OnCancel;

            MenuEntries.Add(_controlsEntry);
            MenuEntries.Add(_InteractEntry);
            MenuEntries.Add(back);

        }


        private void setControlsEntryText()
        {
            //_controlsEntry.Text = "WASD or the arrow keys to move.";
            //_InteractEntry.Text = "Press E to Interact.";

            _controlsEntry.Text = "Hello";
            _InteractEntry.Text = "World";
        }

    }
}
