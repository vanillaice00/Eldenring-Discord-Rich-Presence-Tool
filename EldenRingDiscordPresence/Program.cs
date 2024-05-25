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
        private static Timestamps startGameTimestamp;
        public static string CurrentGameExecutable;

        [STAThread]
        static void Main()
        {

            ApplicationConfiguration.Initialize();
            ConfigurationManager = new ConfigurationManager();
            if(!File.Exists("LocationRegister.json"))
            {
                MessageBox.Show("Could not find LocationRegister.json\nPlease make sure to put the LocationRegister.json file in the same directory as this executable.", "Failed to load LocationRegister", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
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
                MemoryUtility.CloseMemoryProcess();
                startGameTimestamp = null;
                return;
            }

            if (IsEacRunning())
            {
                MainForm.SetStatus("EAC RUNNING. DISABLE FIRST.", Color.DarkRed);
                MemoryUtility.CloseMemoryProcess();
                return;
            }

            if (startGameTimestamp == null)
                startGameTimestamp = Timestamps.Now;



            MemoryUtility.OpenMemoryProcess();
            string location = "The Lands Between";
            string imageKey = "none";

            long locationId = MemoryUtility.ReadLastGraceLocationId();
            MainForm.SetGraceID(locationId);
            if (LocationRegister != null && LocationRegister.ContainsKey(locationId))
            {
                location = LocationRegister[locationId];
            }

            imageKey = GetImageKey(location);

            RichPresence rich = new RichPresence();

            rich.WithDetails(location.Contains(" - ") ? location.Split(" - ")[0] : location);

            if (ConfigurationManager.CurrentConfiguration.ShowGraceLocationName)
            {
               
                rich.WithState(location.Contains(" - ") ? location.Split(" - ")[1] : "");
            }


            if (!ConfigurationManager.CurrentConfiguration.ShowAreaImages)
            {
                imageKey = "none";
            }
            rich.WithAssets(new Assets
                {
                    LargeImageKey = imageKey,
                    LargeImageText = location.Contains(" - ") ? location.Split(" - ")[0] : location
                });
          

            if (ConfigurationManager.CurrentConfiguration.ShowElapsedTime)
            {
                rich.WithTimestamps(startGameTimestamp);
            }
            
            RpcClient.SetPresence(rich);
            MainForm.SetStatus("RUNNING", Color.DarkGreen);
            MemoryUtility.CloseMemoryProcess();
        }

        private static string GetImageKey(string location)
        {
            return location switch
            {
                var loc when loc.Contains("Liurnia") => "liurnia",
                var loc when loc.Contains("Limgrave") => "limgrave",
                var loc when loc.Contains("Caelid") => "caelid",
                var loc when loc.Contains("Altus Plateau") => "altus",
                var loc when loc.Contains("Volcano Manor") => "vulcan",
                var loc when loc.Contains("Crumbling Farum Azula") => "azula",
                var loc when loc.Contains("Academy") => "academy",
                var loc when loc.Contains("Mountaintops") => "mountaintops",
                var loc when loc.Contains("Leyndell") => "leyndell",
                var loc when loc.Contains("Ainsel") => "ainsel",
                var loc when loc.Contains("Deeproot") => "deeproot",
                var loc when loc.Contains("Flame Peak") => "flame_peak",
                var loc when loc.Contains("Mt. Gelmir") => "gelmir",
                var loc when loc.Contains("Haligtree") => "haligtree",
                var loc when loc.Contains("Lake of Rot") => "lake-of-rot",
                var loc when loc.Contains("Nokron") => "nokron",
                var loc when loc.Contains("Nokstella") => "nokstella",
                var loc when loc.Contains("Roundtable") => "roundtable",
                var loc when loc.Contains("Stormveil") => "stormveil",
                var loc when loc.Contains("Weeping") => "weeping",
                var loc when loc.Contains("Siofra") => "siofra",
                var loc when loc.Contains("Aeonia") => "aeonia",
                var loc when loc.Contains("Moonlight") => "moonlight",
                var loc when loc.Contains("Consecrated Snowfield") => "snowfield",
                var loc when loc.Contains("?") => "start",
                var loc when loc.Contains("Subterranean") => "underground",
                var loc when loc.Contains("Forbidden") => "forbidden",

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
