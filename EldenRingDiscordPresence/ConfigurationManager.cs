using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingDiscordPresence
{
    public class ConfigurationManager
    {

        private readonly String mainFolderPath;
        private readonly String configurationFilePath;
        public readonly Configuration CurrentConfiguration;

        public ConfigurationManager() {

            mainFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"EldenRingDiscordPresence");
            configurationFilePath = Path.Combine(mainFolderPath, "Configuration.json");
            createFilesIfNotExisting();
            if (new FileInfo(configurationFilePath).Length == 0)
            {
                CurrentConfiguration = new Configuration(false, 1,true,true,true,true);
                File.WriteAllText(configurationFilePath, JsonConvert.SerializeObject(CurrentConfiguration));
            } else
            {
                CurrentConfiguration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configurationFilePath));
            }
        }


        public void updateConfigurationFile()
        {
            File.WriteAllText(configurationFilePath, JsonConvert.SerializeObject(CurrentConfiguration));
        }


        private void createFilesIfNotExisting()
        {
            if (!Directory.Exists(mainFolderPath))
            {
                Directory.CreateDirectory(mainFolderPath);
            }

            if(!File.Exists(configurationFilePath))
            {
                File.Create(configurationFilePath).Close();
            }

        }

    }
}
