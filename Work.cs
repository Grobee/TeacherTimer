using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherTimer
{
    public class Work
    {
        public int HoursDone { get; set; }
        public int LongestStreak { get; set; }
        public DateTime StartTime { get; set; }
        public int ElapsedHours { get; set; }
        public int ElapsedMinutes { get; set; }
        public bool InProgress { get; set; }
    }
}
