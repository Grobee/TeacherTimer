using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherTimer
{
    public class Work
    {
        /* start the work session */
        public void Start(ref Session session)
        {
            session.InProgress = true;
            session.StartTime = DateTime.Now;
        }
        /* stop the work session */
        public void Stop(ref Session session)
        {
            session.InProgress = false;
            session.PreviousElapsedHours = 0;
        }
        /* reset the work session */
        public void Reset(ref Session session)
        {
            session = new Session()
            {
                HoursDone = 0,
                ElapsedHours = 0,
                ElapsedMinutes = 0,
                PreviousElapsedHours = 0,
                InProgress = false,
                LongestStreak = 0,
                StartTime = DateTime.MinValue                
            };
        }        
    }
}
