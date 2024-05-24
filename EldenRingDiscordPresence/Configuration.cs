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
        public int UpdateDelay {  get; set; }

        public Configuration(bool startWithWindows, int updateDelay)
        {
            this.StartWithWindows = startWithWindows;
            this.UpdateDelay = updateDelay;
        }

    }
}
