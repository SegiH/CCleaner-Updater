using NSoup;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using System.Text;
using System.ComponentModel;

namespace CCleaner_Updater
{
    public partial class CCleanerUpdater : Form
    {
        FileVersionInfo f; // Object to get version from EXE
        bool silentRun;
        const String downloadURL = "http://www.piriform.com/ccleaner/download/standard"; // This URL contains the download link to download the latest version of CCleaner which doesn't have a toolbar installed
        const String latestVersionURL = "https://www.ccleaner.com/ccleaner/version-history"; // This page contains the version history which is parsed for the latest version

        // Constructor that is called when app is run the normal way
        /*public CCleanerUpdater() {
            InitializeComponent();
        }*/

        // Constructor called when the app is run from the command line
        public CCleanerUpdater(Boolean silentRun) {
            // Make sure that a network connection is available
            if (isNetworkAvailable() == false) {
                MessageBox.Show("Internet connection not available. Exiting");
                System.Environment.Exit(1);
            }

            // Set flag to indicate that this is a silent run as provided by the call from Main() in the Program class
            this.silentRun = silentRun;

            // Required by UI - Don't remove
            InitializeComponent();

            if (silentRun == true) {
                // Check for an update
                checkforUpdates();
            }            
        }

        // Event when this app is run normally using the UI.
        private void CCleanerUpdater_Load(object sender, EventArgs e) {
            // Make sure that network is available
            if (isNetworkAvailable() == false) {
                MessageBox.Show("Internet connection not available. Exiting");
                System.Environment.Exit(1);
            }

            // If the path is not set, set it to the location of CCleaner.exe if is in one of the 2 default locations (C:\Program Files\CCleaner\ccleaner.exe or C:\Program Files (x86)\CCleaner\ccleaner.exe)
            if (Properties.Settings.Default.CCleanerPath.Equals("")) {
                if (File.Exists("C:\\Program Files\\CCleaner\\ccleaner.exe")) {
                    txtCCleanerPath.Text = "C:\\Program Files\\CCleaner\\ccleaner.exe";
                    saveProperty("CCleanerPath", "C:\\Program Files\\CCleaner\\ccleaner.exe");
                }
                else if (File.Exists("C:\\Program Files ((x86)\\CCleaner\\ccleaner.exe")) {
                    txtCCleanerPath.Text = "C:\\Program Files ((x86)\\CCleaner\\ccleaner.exe";
                    saveProperty("CCleanerPath", "C:\\Program Files ((x86)\\CCleaner\\ccleaner.exe");
                }
            } else { // Use the path that the user previously provided
                txtCCleanerPath.Text = Properties.Settings.Default.CCleanerPath;
            }
            
            // Update prompt me checkbox based on users' saved setting
            chkUpdatePrompt.Checked = (Properties.Settings.Default.PromptUser == true ? true : false);

            // Get the version of CCleaner that is currently installed (if its installed)
            string installedCCleanerVersion = getInstalledCCleanerVersion();

            // Update the text that displays the currently installed version of CCleaner on this computer
            if (!installedCCleanerVersion.Equals("")) {
                lblCurrentVersion.Text = installedCCleanerVersion;
            }

            //this.components = new System.ComponentModel.Container();
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        }

        // Check for updates now button click event
        private void btnCheckUpdatesNow_Click(object sender, EventArgs e) {
            checkforUpdates();
        }

        // Event when the user clicks on the button to select the path to CCleaner.exe
        private void btnSelectCCleanerPath_Click(object sender, EventArgs e) {
            // Show select file dialog with filter to only allow the user to select ccleaner.exe
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "CCleaner|CCleaner.exe";

            // If CCleaner is found in one of the default locations (C:\Program Files\CCleaner\ccleaner.exe or C:\Program Files (x86)\CCleaner\ccleaner.exe) 
            // select that directory as the default location
            if (File.Exists("C:\\Program Files\\CCleaner\\ccleaner.exe")) {
                dialog.InitialDirectory = @"C:\\Program Files\\CCleaner\\";
            }
            else if (File.Exists("C:\\Program Files ((x86)\\CCleaner\\ccleaner.exe")) {
                dialog.InitialDirectory = @"C:\\Program Files ((x86)\\CCleaner\\";
            }
            else {
                dialog.InitialDirectory = @"C:\";
            }

            dialog.Title = "Please select CCleaner.exe";

            // When the user clicks on OK, fill in text field and save pref. Otherwise erase pref
            if (dialog.ShowDialog() == DialogResult.OK) {
                txtCCleanerPath.Text = dialog.FileName;

                // Save preference
                saveProperty("CCleanerPath", dialog.FileName);
            }
            else {
                txtCCleanerPath.Text = "";

                // Save preference
                saveProperty("CCleanerPath", "");
            }
        }

        // Checks the CCleaner site for an update by scraping the download page for the currently released version
        private void checkforUpdates() {
            // Get the HTML code
            string installedCCleanerVersion = "";
            string downloadLink = "";

            // Get the installed version of the app
            installedCCleanerVersion = getInstalledCCleanerVersion();

            // Get the latest version from the CCleaner website
            string latestVersion = getLatestCCleanerVersion();

            // Compare installed and latest versions.
            if (!installedCCleanerVersion.Equals(latestVersion)) {
                // Scrape the CCleaner sites' HTML for the latest version
                downloadLink = getDownloadLink();

                if (downloadLink.Equals("")) {
                    if (silentRun == false) {
                        MessageBox.Show("Unable to download the latest version of CCleaner. Unable to get the download link.");
                    } else {
                        System.Environment.Exit(1);
                    }
                }

                // Download the installer
                using (var client = new WebClient()) {
                    try {
                        client.DownloadFile(downloadLink, "ccsetup.exe");
                    } catch (Exception e) {
                        if (silentRun == false) {
                            MessageBox.Show("Unable to download the latest version of CCleaner. An error occurred getting the download link with the error " + e.ToString());
                        } else {
                            System.Environment.Exit(1);
                        }
                    }

                    // Execute it and wait for it to finish
                    if (File.Exists("ccsetup.exe")) {
                        
                        //Seems to be broken and always determines the file size to be < 1 MB

                        // Get the size of the file. Sometimes the file will not successfully download but leave a 1k file. This verifies the file size
                        long length = new System.IO.FileInfo("ccsetup.exe").Length;

                        // The file is less than 1MB, it is probably corrupt
                        if (length < 1000000) {
                            // Delete the file
                            File.Delete("ccsetup.exe");

                            if (silentRun == false) {
                                MessageBox.Show("Unable to download the latest version of CCleaner. The downloaded file appears to be corrupt");
                            } else {
                                System.Environment.Exit(1);
                            }
                        }

                        // If the user specified that they want to be prompted
                        if (Properties.Settings.Default.PromptUser == true) {
                            // The prompt only shows the currently installed version if CCleaner is actually installed
                            DialogResult dialogResult = MessageBox.Show("There is a CCleaner update available. " + (!installedCCleanerVersion.Equals("") ? "You have version " + installedCCleanerVersion + " and the" : "The") + " latest version is " + latestVersion + ". Do you want to update it now ?", "CCleaner Update is available", MessageBoxButtons.YesNo);

                            if (dialogResult == DialogResult.No) {
                                // Delete the file
                                File.Delete("ccsetup.exe");

                                // Exit the app when this is a silent run
                                if (silentRun == true) {
                                    System.Environment.Exit(1);
                                } else {
                                    return;
                                }
                            }

                        }

                        // Start the CCleaner installation in silent mode
                        try {
                            ProcessStartInfo cmdsi = new ProcessStartInfo("ccsetup.exe");
                            cmdsi.Arguments = "/S";

                            Process cmd = Process.Start(cmdsi);
                            cmd.WaitForExit();

                            // Delete the file
                            File.Delete("ccsetup.exe");
                        } catch (Win32Exception) { // Don't show error if the user canceled the installation or clicked on No on the UAC prompt
                                 MessageBox.Show("Canceled");
                        } catch (Exception e) {
                            if (silentRun == false) {
                                 MessageBox.Show("An error occurred while installing the latest version of CCleaner. Error: " + e.ToString());
                            }                            
                        } finally {
                            // Delete the file
                            File.Delete("ccsetup.exe");
                        }
                    } else {
                        if (silentRun == true) {
                            MessageBox.Show("CCleaner download failed. Please check your network connection");
                        } else {
                            System.Environment.Exit(1);
                        }
                    }
                }
            } else if (silentRun == false) {
                MessageBox.Show("CCleaner is up to date");
            }

            if (silentRun == true) {
                System.Environment.Exit(1);
            }
        }

        // Ask me before updating CCleaner checkbox state change
        private void chkUpdatePrompt_CheckedChanged(object sender, EventArgs e) {
            saveProperty("PromptUser", (chkUpdatePrompt.Checked == true ? true : false));
        }

        // Get the download URL from the CCleaner site
        string getDownloadLink() {            
            string downloadLink="";

           // Retrieve all of the HTML code from the download page
            string downLoadLinkData = readURL(downloadURL);

            // Use NSoup to parse the HTML and find the download URL
            NSoup.Nodes.Document doc = NSoupClient.Parse(downLoadLinkData);
            NSoup.Nodes.Element el = doc.GetElementsByClass("btn--download").First;

            string html = el.OuterHtml().ToString();

            if (html.IndexOf("href=\"") != -1) {
                downloadLink = html.Substring(html.IndexOf(("href=\"")) + "href=\"".Length);
                downloadLink = downloadLink.Substring(0, downloadLink.IndexOf(("\">")));

                if (downloadLink.Equals("")) {
                    MessageBox.Show("Unable to get the download link from the CCleaner web site");
                    System.Environment.Exit(1);
                }
           }

            return downloadLink;
        }

        // Get the latest version of CCleaner that is installed on this machine
        string getInstalledCCleanerVersion() {
            string installedCCleanerVersion = "";
            string ccleanerpath = "";

            // Use user specified path if it is set
            if (!Properties.Settings.Default.CCleanerPath.Equals("")) {
                // Validate that the executable exists
                if (!File.Exists(Properties.Settings.Default.CCleanerPath)) {
                    return ""; // This indicates that  either CCleaner isn't installed or can't be found in the user specified location. Returns an empty string so CCleaner will be initially installed
                } else {
                    ccleanerpath = Properties.Settings.Default.CCleanerPath;
                }
            } else if (File.Exists("C:\\Program Files\\CCleaner\\ccleaner.exe")) {
                ccleanerpath = "C:\\Program Files\\CCleaner\\ccleaner.exe";
            } else if (File.Exists("C:\\Program Files ((x86)\\CCleaner\\ccleaner.exe")) {
                ccleanerpath = "C:\\Program Files ((x86)\\CCleaner\\ccleaner.exe";
            }

            // Locate CCleaner executable and get the version info
            if (File.Exists(ccleanerpath)) {
                string filePath = ccleanerpath;

                // Parse the executable to get the installed version of CCleaner
                f = FileVersionInfo.GetVersionInfo(filePath);

                installedCCleanerVersion = f.ProductVersion;

                installedCCleanerVersion = installedCCleanerVersion.Split('.')[0] + "." + installedCCleanerVersion.Split('.')[1] + "." + installedCCleanerVersion.Split('.')[3];

                // Remove v from beginning of the version if its there
                if (installedCCleanerVersion != null && installedCCleanerVersion.StartsWith("v")) {
                    installedCCleanerVersion = installedCCleanerVersion.Substring(1);
                }

                // Convert version which is stored as 5,0,30,00 , 1234 to match the same version format as the CCleaner site
                installedCCleanerVersion = installedCCleanerVersion.Replace(" 00,", "");
                installedCCleanerVersion = installedCCleanerVersion.Replace(" ", "");
                installedCCleanerVersion = installedCCleanerVersion.Replace(",", ".");

                return installedCCleanerVersion;
            } else { // Either CCleaner is not installed or it is installed but the exe can't be located because its installed in a custom location
                return ""; // This indicates that  either CCleaner isn't installed or can't be found
                
                //MessageBox.Show("Unable to locate the CCleaner executable" + (!ccleanerpath.Equals("") ? " in " + ccleanerpath : ""));
                //System.Environment.Exit(1);
            }
        }

        // Get the latest version of CCleaner from the CClaner site
        string getLatestCCleanerVersion() {
            String latestVersion;

            // Read the current version from the CCleaner site
            string currentVersionData = this.readURL(latestVersionURL);

            if (currentVersionData == null) {
                MessageBox.Show("Unable to connect to the CCleaner web site");
                System.Environment.Exit(1);
            }

            // Use NSoup to parse the HTML and find the latest version number
            NSoup.Nodes.Document doc = NSoupClient.Parse(currentVersionData);

            NSoup.Nodes.Element el = doc.GetElementsByClass("main-content-column")[0].Children.Eq(1).First;
            /*
            NSoup.Nodes.Element el = doc.GetElementsByClass("icon_square")[1].NextElementSibling;
            //el = el.GetElementsByTag("indent").First;
            el = el.GetElementsByTag("strong").First;
            */
            latestVersion = el.Html().ToString();

            latestVersion = latestVersion.Replace("<h6>", "");
            latestVersion = latestVersion.Replace("</h6>", "");

            String []tmp = latestVersion.Split(' ');
            latestVersion = tmp[0];

            if (latestVersion != null && latestVersion.StartsWith("v")) {
                latestVersion = latestVersion.Substring(1);
            }

            return latestVersion;
        }

        // Check network status to make sure internet connection is available
        public static bool isNetworkAvailable() {
            return isNetworkAvailable(0);
        }

        // Check network status to make sure internet connection is available
        public static bool isNetworkAvailable(long minimumSpeed) {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return false;

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces()) {
                // discard because of standard reasons
                if ((ni.OperationalStatus != OperationalStatus.Up) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel))
                    continue;

                // this allow to filter modems, serial, etc.
                // I use 10000000 as a minimum speed for most cases
                if (ni.Speed < minimumSpeed)
                    continue;

                // discard virtual cards (virtual box, virtual pc, etc.)
                if ((ni.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (ni.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0))
                    continue;

                // discard "Microsoft Loopback Adapter", it will not show as NetworkInterfaceType.Loopback but as Ethernet Card.
                if (ni.Description.Equals("Microsoft Loopback Adapter", StringComparison.OrdinalIgnoreCase))
                    continue;

                return true;
            }

            return false;
        }

        // Read the specified URL and return the HTML code
        string readURL(string urlAddress) {
            string data = "";

            // Check the website for the latest version
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Only continue if the return code indicates success
            if (response.StatusCode == HttpStatusCode.OK) {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null || response.CharacterSet.Equals("")) {
                    readStream = new StreamReader(receiveStream);
                } else {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }

            return data.ToString();
        }

        // Save property
        private void saveProperty(string propertyName,object propertyValue) {
            Properties.Settings.Default[propertyName] = propertyValue;
            Properties.Settings.Default.Save();
        }   
    }
}