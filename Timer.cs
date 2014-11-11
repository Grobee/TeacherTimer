using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherTimer
{
    public class Timer
    {
        Work work;

        /* constructor */
        public Timer(Work work) 
        {
            this.work = work;
        }
        /* start the timer */
        public void Start()
        {
            work.InProgress = true;

            work.StartTime = DateTime.Now;
        }
        /* stop the timer */
        public void Stop()
        {
            work.InProgress = false;
        }
        /* reset the timer */
        public void Reset()
        {
            work.InProgress = false;
        }        
    }
}
