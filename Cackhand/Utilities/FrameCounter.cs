using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cackhand.Utilities
{
    public class FrameCounter
    {
        private int lastTick;
        private int lastFrameRate;
        private int frameRate;

        public int CalculateFrameRate()
        {
            if(System.Environment.TickCount - lastTick >= 1000)
            {
                lastFrameRate = frameRate;
                frameRate = 0;
                lastTick = System.Environment.TickCount;
            }
            frameRate++;
            return lastFrameRate;
        }
    }
}
