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
            var JavaEntry = new MenuEntry("Java");
            var CEntry = new MenuEntry("C");
            var CSharpEntry = new MenuEntry("CSharp");
            var exitEntry = new MenuEntry("Exit");

            PythonEntry.Selected += PythonEntrySelected;
            JavaEntry.Selected += JavaEntrySelected;
            CEntry.Selected += CEntrySelected;
            CSharpEntry.Selected += CSharpEntrySelected;
            exitEntry.Selected += OnCancel;


            if (ScreenManager.PythonBookCollected) MenuEntries.Add(PythonEntry);
            if (ScreenManager.JavaBookCollected) MenuEntries.Add(JavaEntry);
            if (ScreenManager.CBookCollected) MenuEntries.Add(CEntry);
            if (ScreenManager.CSharpBookCollected) MenuEntries.Add(CSharpEntry);
            MenuEntries.Add(exitEntry);
        }

        private void PythonEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new PythonInformation(), e.PlayerIndex);
        }
        private void JavaEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new JavaInformation(), e.PlayerIndex);
        }
        private void CEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new CInformation(), e.PlayerIndex);
        }
        private void CSharpEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new CSharpInformation(), e.PlayerIndex);
        }
    }
}
