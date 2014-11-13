using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherTimer
{
    public class Work
    {
        Session session;

        public Work(ref Session session)
        {
            this.session = session;
        }

        /* start the work session */
        public void Start()
        {
            session.InProgress = true;
            session.StartTime = session.FirstTime = DateTime.Now;            
        }
        /* stop the work session */
        public void Stop()
        {
            session.InProgress = false;
            this.CheckTime();
            session.FirstTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            session.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);            
        }
        /* reset the work session */
        public void Reset()
        {           
            session.ElapsedHours = 0;
            session.ElapsedMinutes = 0;
            session.HoursDone = 0;
            session.InProgress = false;
            session.LongestStreak = 0;
            session.FirstTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            session.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);            
        }

        public void CheckTime()
        {
            int hoursPassed = DateTime.Now.Hour - session.StartTime.Hour;
            int minutesPassed = DateTime.Now.Minute - session.StartTime.Minute;

           /* if (hoursPassed > 1 || hoursPassed == 1 && minutesPassed >= 0)
                session.HoursDone += hoursPassed;
            /* see if the person broke the record hours */

            session.StartTime = DateTime.Now;
        }

        public void CheckLongestStreak()
        {
            int hoursPassed = DateTime.Now.Hour - session.FirstTime.Hour;
            int minutesPassed = DateTime.Now.Minute - session.FirstTime.Minute;

            /* if (hoursPassed > session.LongestStreak)
                    if (minutesPassed < 0)
                        session.LongestStreak = --hoursPassed;
                    else
                        session.LongestStreak = hoursPassed;*/

            if (Math.Abs(minutesPassed) > session.LongestStreak)
                if (minutesPassed < 0)
                    session.LongestStreak = --minutesPassed;
                else
                    session.LongestStreak = minutesPassed;

            if (Math.Abs(minutesPassed) >= 1)
                session.HoursDone += Math.Abs(minutesPassed);

        }
    }
}
