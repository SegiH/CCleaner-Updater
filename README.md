CCleaner-Updater is an application that can run silently in the background and will check the CCleaner website for an update.

If an update is found it will be downloaded and silently installed. If you have User Access Control enabled in Windows, UAC may still prompt you before the installation starts.

Installation instructions:

Open the project in Visual Studio
Install these NuGet packages if they are not installed: jint,NSoup,System.Threading.Tasks
Compile and run

The project also includes an installer project if you want to develop an installer for this application although it isn't needed because it can run as a standalone application. If you do want to use it as a standalone application, make sure the necessary dll files (NSoup.dll and jint.dll) are in the same folder as the executable.

Usage:

This app can be used in one of 2 different ways.

1. Manual way: Running the app normally will show you the main CC Updater window where you can specify whether you want to be prompted or not before updating and lets you choose the location of CCleaner.exe if it is not installed in one of the standard locations (C:\Program Files\CCleaner or C:\Program Files (x86)\CCleaner). You can click on the "Check for updates now" button to manually check if there is an update. If there is, you will be prompted to install it if you checked on the setting to prompt you. I recommend to leave this setting unchecked.

2. Automatic way: This app can be run from the command line with the parameter /S or /s to run silently in the background. If there is an update, it will be installed silently without prompting you (Unless you checked on the setting in the application to prompt you before installing an update).

Note: If you have UAC (User Access Control) enabled in Windows 8 or newer (which is enabled by default), you will see a UAC prompt asking you to confirm that you want to install this app every time that an update is installed. That prompt is part of Windows which I can't prevent from happening but there is a workaround for this to make the app run completely silently by running CCleaner Updater as a scheduled task in Windows.

1. Open the Task Scheduler application that comes with Windows.
2. Click on create task on the right hand side under Actions.
3. Give the task a name on the General tab like CCleaner Updater Task.
4. Click on Triggers tab and click on New to add a schedule. I choose to run this once a day.
5. Click on Actions and click on New.
6. For Program/script: enter the full path to CCleaner.exe. If the path has a space in it, make sure to wrap the entire path with quotation marks "C:\Program Files\CCleaner Updater\CCleaner Updater.exe".
7. For Add Arguments: Enter /S.
8. Click on OK.
9. Click on OK again to save this task.