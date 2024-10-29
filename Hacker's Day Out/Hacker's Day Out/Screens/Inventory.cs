using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackersDayOut.StateManagement;

namespace HackersDayOut.Screens
{
    public class Inventory: MenuScreen
    {
       
        public Inventory(): base("Paused")
        {
            var PythonEntry = new MenuEntry("Python");
            var exitEntry = new MenuEntry("Exit");

            PythonEntry.Selected += PythonEntrySelected;
            exitEntry.Selected += OnCancel;


            if (ScreenManager.PythonBookCollected) MenuEntries.Add(PythonEntry);
            MenuEntries.Add(exitEntry);
        }

        private void PythonEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new PythonInformation(), e.PlayerIndex);
        }
    }
}
