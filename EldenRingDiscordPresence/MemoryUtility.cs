using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingDiscordPresence
{

    internal class MemoryUtility
    {

        private long mainAddress;
        private long graceOffset = 0xB3C;
        private MemoryX.Memory? memoryProcess;
   
        public void InitializeMemoryProcess()
        {
           memoryProcess = new MemoryX.Memory();
           memoryProcess.GetProcessHandle(Program.CurrentGameExecutable);

            switch (Program.CurrentGameExecutable)
            {
                case "start_protected_game":
                    mainAddress = 0x7FF646220708;
                    break;
                case "eldenring":
                    mainAddress = 0x7FF722620708;
                    break;
            }
        }

        public long ReadLastGraceLocationId()
        {
            int lastGraceLocationID = 0;
            if(memoryProcess == null)
                return lastGraceLocationID;
       
            long dereferencedBaseAddress = BitConverter.ToInt64(memoryProcess.ReadMemory(mainAddress, 8), 0);

                lastGraceLocationID = memoryProcess.ReadInt32(dereferencedBaseAddress + graceOffset);
            return lastGraceLocationID;
        }

        public void CloseMemoryProcess()
        {
            if (memoryProcess != null)
            {
                memoryProcess.CloseProcessHandle();
                memoryProcess = null;
            }
        }

    }
}
