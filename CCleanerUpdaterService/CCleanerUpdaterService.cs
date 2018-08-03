using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CCleanerUpdaterService {
    public partial class CCleanerUpdaterService : ServiceBase {
        System.Timers.Timer timer;

        public CCleanerUpdaterService() {
            InitializeComponent();

            /*
            eventLog1 = new System.Diagnostics.EventLog();

            if (!System.Diagnostics.EventLog.SourceExists("MySource")) {
                System.Diagnostics.EventLog.CreateEventSource(
                    "MySource", "MyNewLog");
            }

            eventLog1.Source = "MySource";
            eventLog1.Log = "MyNewLog";
            */
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args) {
            //eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }

        // Service start evebt
        protected override void OnStart(string[] args) {
            //eventLog1.WriteEntry("The CCleanerUpdater Service was started");

            // Set up a timer to trigger every minute.  
            this.timer = new System.Timers.Timer();
            
            timer.Interval = 30000; 
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);

            timer.Start();
        }

        // Service Stop event
        protected override void OnStop() {
            eventLog1.WriteEntry("The CCleanerUpdater Service was stopped");
        }        
    }
}
