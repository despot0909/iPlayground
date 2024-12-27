using System.Windows;
using System.Windows.Forms;
using System.Linq;

namespace iPlayground.WPF.Helpers
{
    public static class DisplayHelper
    {
        public static Window ShowOnSecondaryScreen(Window window)
        {
            var screens = Screen.AllScreens;
            var secondaryScreen = screens.FirstOrDefault(s => s.Primary);

            if (secondaryScreen != null)
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Left = secondaryScreen.WorkingArea.Left;
                window.Top = secondaryScreen.WorkingArea.Top;
                window.WindowState = WindowState.Maximized;
                
            }

            return window;
        }
        public static bool HasSecondaryScreen()
        {
            return Screen.AllScreens.Length > 1;
        }
    }
}