using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WGestures.Common.OsSpecific.Windows
{
    public static class AutoStarter
    {
        private const string RunLocation = @"Software\Microsoft\Windows\CurrentVersion\Run";


        public static void Register(string identifier, string appPath)
        {

            var key = Registry.CurrentUser.CreateSubKey(RunLocation);
            key.SetValue(identifier, appPath);
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

            RegistryKey key = Registry.CurrentUser.CreateSubKey(RunLocation);
            
            key.DeleteValue(identifier,throwOnMissingValue: false);
           /* using (var ts = new TaskService())
            {
                ts.RootFolder.DeleteTask(identifier);
            }*/
        }

        public static bool IsRegistered(string identifier,string appPath)
        {
            var key = Registry.CurrentUser.OpenSubKey(RunLocation);
            if (key == null)
                return false;

            var value = (string)key.GetValue(identifier);
            if (value == null)
                return false;

            return (value == appPath);
            /*
            using (var ts = new TaskService())
            {
                return ts.RootFolder.Tasks.Exists(identifier);
            }*/
        }
    }
}
