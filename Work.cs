using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherTimer
{
    public class Work
    {
        /* attribute getters and setters */
        public int HoursDone { get; set; }
        public int LongestStreak { get; set; }
        public DateTime StartTime { get; set; }
        public bool InProgress { get; set; }

        public Work()
        {
            HoursDone = 0;
            LongestStreak = 0;
            StartTime = DateTime.Now;
        }
    }
}
