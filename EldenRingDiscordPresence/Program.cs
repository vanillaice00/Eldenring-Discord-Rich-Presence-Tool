using DiscordRPC;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.ServiceProcess;
using System.Timers;
using System.Windows.Forms.VisualStyles;

namespace EldenRingDiscordPresence
{
    internal static class Program
    {
        public static readonly MemoryUtility MemoryUtility = new();
        public static Dictionary<long, string>? LocationRegister;
        public static ConfigurationManager? ConfigurationManager;
        public static MainForm MainForm;
        private static System.Timers.Timer MainTimer;
        private static DiscordRpcClient RpcClient;
        public static string CurrentGameExecutable;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            ConfigurationManager = new ConfigurationManager();
            LocationRegister = JsonConvert.DeserializeObject<Dictionary<long, string>>(File.ReadAllText("LocationRegister.json"));
            RpcClient = new DiscordRpcClient("1243218524554530998");

            RpcClient.Initialize();
            StartTimer();

            Application.Run(MainForm = new MainForm());
        }

        public static void StartTimer()
        {
            if (ConfigurationManager == null)
                return;

            MainTimer = new System.Timers.Timer
            {
                Interval = GetUpdateDelayInSeconds() * 1000,
                AutoReset = true,
                Enabled = true
            };
            MainTimer.Elapsed += OnTimerElapsed;
            MainTimer.Start();
        }

        private static int GetUpdateDelayInSeconds() => ConfigurationManager.CurrentConfiguration.UpdateDelay switch
        {
            0 => 10,
            1 => 30,
            2 => 60,
            _ => 30
        };

        private static void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (!IsGameLoaded())
            {
                MainForm.SetStatus("WAITING FOR GAME...", Color.DarkOrange);
                if (RpcClient.CurrentPresence != null)
                {
                    RpcClient.ClearPresence();
                    RpcClient.SetPresence(null);
                }
                return;
            }

            if (IsEacRunning())
            {
                MainForm.SetStatus("EAC RUNNING. DISABLE FIRST.", Color.DarkRed);
                MemoryUtility.CloseMemoryProcess();
                return;
            }

            MainForm.SetStatus("RUNNING", Color.DarkGreen);

            MemoryUtility.InitializeMemoryProcess();
            string location = "The Lands Between";
            string imageKey = "none";

            long locationId = MemoryUtility.ReadLastGraceLocationId();


          
            if (LocationRegister != null && LocationRegister.ContainsKey(locationId))
            {
                location = LocationRegister[locationId];
            }

            imageKey = GetImageKey(location);

            RpcClient.SetPresence(new RichPresence
            {
                State = location.Contains(" - ") ? location.Split(" - ")[1] : "",
                Details = location.Contains(" - ") ? location.Split(" - ")[0] : location,
                Assets = new Assets
                {
                    LargeImageKey = imageKey,
                    LargeImageText = location.Contains(" - ") ? location.Split(" - ")[0] : location
                }
            });

            MemoryUtility.CloseMemoryProcess();
        }

        private static string GetImageKey(string location)
        {
            return location switch
            {
                var loc when loc.StartsWith("Liurnia") => "liurnia",
                var loc when loc.StartsWith("Limgrave") => "limgrave",
                var loc when loc.StartsWith("Caelid") => "caelid",
                var loc when loc.StartsWith("Altus Plateau") => "altus",
                var loc when loc.StartsWith("Volcano Manor") => "vulcan",
                var loc when loc.StartsWith("Crumbling Farum Azula") => "azula",
                var loc when loc.StartsWith("Academy") => "academy",
                var loc when loc.StartsWith("Mountaintops") => "mountaintops",
                var loc when loc.StartsWith("Leyndell") => "leyndell",
                _ => "none"
            };
        }

        public static void StopTimer()
        {
            MainTimer.Stop();
            MainTimer.Close();
        }

        private static bool IsGameLoaded()
        {
            string[] procLists = new string[] { "start_protected_game", "eldenring" };

            foreach (var item in procLists)
            {
                var procList = Process.GetProcessesByName(item);

                if (procList.Any() && !procList[0].HasExited)
                {
                    int timeout = 0;
                    while (!procList[0].Responding)
                    {
                        if (timeout > 10000)
                            break;
                        procList[0].Refresh();
                        Task.Delay(500).Wait();
                        timeout += 500;
                    }

                    if (procList[0].Responding)
                    {
                        CurrentGameExecutable = item;
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsEacRunning()
        {
            var eacServices = ServiceController.GetServices().Where(service => service.ServiceName.Contains("EasyAntiCheat")).ToArray();
            return eacServices.Any(eacService =>
            {
                using var sc = new ServiceController(eacService.ServiceName);
                return sc.Status == ServiceControllerStatus.Running ||
                       sc.Status == ServiceControllerStatus.ContinuePending ||
                       sc.Status == ServiceControllerStatus.StartPending;
            });
        }
    }
}
