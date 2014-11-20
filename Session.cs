using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherTimer
{
    public class Session
    {        
        public DateTime StartTime { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public TimeSpan TotalTime { get; set; }
        public TimeSpan LongestStreak { get; set; }
        public bool InProgress { get; set; }
    }
}
