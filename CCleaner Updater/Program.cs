using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCleaner_Updater
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // This app takes either 0 arguments or exactly 1 argument (/s or /S) which indicates a silent update from the command line. '
            // If the user has indicated that they want to be prompted before installing an update (though the main GUI), a dialog will still appear asking them to install the update
            if (args.Length != 0 && args.Length != 1 || (args.Length == 1 && !args[0].Equals("/S", StringComparison.OrdinalIgnoreCase))) {
                MessageBox.Show("Invalid command line parameters");
                System.Environment.Exit(1);
            } else if (args.Length == 1) {
                Application.Run(new CCleanerUpdater(true));
            } else if (args.Length == 0) {
                Application.Run(new CCleanerUpdater(false));
            }            
        }
    }
}