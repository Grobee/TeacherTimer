using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherTimer
{
    public class Timer
    {
        /* start the timer */
        public void Start(Work work)
        {
            work.InProgress = true;
            work.StartTime = DateTime.Now;
        }
        /* stop the timer */
        public void Stop(Work work)
        {
            work.InProgress = false;
        }
        /* reset the timer */
        public void Reset(Work work)
        {
            work.InProgress = false;
        }        
    }
}
