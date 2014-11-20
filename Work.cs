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
            session.TotalTime = session.TotalTime.Add(session.ElapsedTime);
            session.StartTime = DateTime.MinValue;
            session.ElapsedTime = TimeSpan.Zero;
            session.InProgress = false;
        }
        /* reset the work session */
        public void Reset(ref Session session)
        {
            session = new Session()
            {   
                StartTime = DateTime.MinValue,
                ElapsedTime = TimeSpan.Zero,
                TotalTime = TimeSpan.Zero,
                LongestStreak = TimeSpan.Zero,
                InProgress = false                            
            };
        }        
    }
}
