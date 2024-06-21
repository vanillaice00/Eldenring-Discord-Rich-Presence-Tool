using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingDiscordPresence
{
    public record Configuration
    {

        public bool StartWithWindows {  get; set; }

        public bool ShowGraceLocationName { get; set; }

        public bool ShowElapsedTime { get; set; }

        public bool UseCloudLocationRegister { get; set; }

        public bool ShowAreaImages { get; set; }
        public int UpdateDelay {  get; set; }

        public Configuration(bool startWithWindows, int updateDelay,bool showElapsedTime, bool showAreaImages, bool showGraceLocationName, bool useCloudLocationRegister)
        {
            this.StartWithWindows = startWithWindows;
            this.ShowElapsedTime = showElapsedTime;
            this.ShowAreaImages = showAreaImages;
            this.ShowGraceLocationName = showGraceLocationName;
            this.UpdateDelay = updateDelay;
            this.UseCloudLocationRegister = useCloudLocationRegister;
        }

    }
}
