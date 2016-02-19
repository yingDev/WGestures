using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using IWshRuntimeLibrary;

namespace WGestures.Common.OsSpecific.Windows
{
    public static class AutoStarter
    {
        //private const string RunLocation = @"Software\Microsoft\Windows\CurrentVersion\Run";

        static string MakeShortcutPath(string identifier)
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\" + identifier + ".lnk";
        }

        public static void Register(string identifier, string appPath)
        {
            Unregister(identifier);
            CreateShortcut(MakeShortcutPath(identifier), appPath);

            //var key = Registry.CurrentUser.CreateSubKey(RunLocation);
            //key.SetValue(identifier, appPath);
            /*using (var ts = new Microsoft.Win32.TaskScheduler.TaskService())
            {
                var userId = WindowsIdentity.GetCurrent().Name;

                var task = ts.NewTask();
                task.RegistrationInfo.Description = identifier;
                task.Settings.DisallowStartIfOnBatteries = false;
                task.Settings.ExecutionTimeLimit = TimeSpan.Zero;
                task.Settings.Hidden = false;

                task.Principal.LogonType = TaskLogonType.InteractiveToken;
                task.Principal.UserId = userId;

                task.Principal.RunLevel = TaskRunLevel.Highest;
                task.Settings.Priority = ProcessPriorityClass.High;

                task.Triggers.Add(new LogonTrigger());
                task.Actions.Add(new ExecAction(appPath, "",workingDirectory));

                ts.RootFolder.RegisterTaskDefinition(identifier, task, 
                    TaskCreation.CreateOrUpdate, userId, 
                    LogonType: TaskLogonType.InteractiveToken);
            }*/
        }

        public static void Unregister(string identifier)
        {
            System.IO.File.Delete(MakeShortcutPath(identifier));

            //ensure removing registry item added in older versions
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");

            key.DeleteValue(identifier,throwOnMissingValue: false);
            /* using (var ts = new TaskService())
             {
                 ts.RootFolder.DeleteTask(identifier);
             }*/
        }

        public static bool IsRegistered(string identifier,string appPath)
        {
            return System.IO.File.Exists(MakeShortcutPath(identifier));
            /*var key = Registry.CurrentUser.OpenSubKey(RunLocation);
            if (key == null)
                return false;

            var value = (string)key.GetValue(identifier);
            if (value == null)
                return false;

            return (value == appPath);*/
            /*
            using (var ts = new TaskService())
            {
                return ts.RootFolder.Tasks.Exists(identifier);
            }*/
        }

        public static void CreateShortcut(string shortcutPath, string targetFileLocation)
        {
            try
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = shell.CreateShortcut(shortcutPath);

                shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
                shortcut.Save();
            }catch(Exception e)
            {
                Debug.WriteLine(e);
                //may be intercepted by 360 etc. ignore...
            }

                                
        }
    }
}
