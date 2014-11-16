using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace TeacherTimer
{
    public sealed partial class MainPage : Page
    {
        FileService fileService;
        Work work;
        Session session;
        DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;             
            
            /* initialize the attributes */            
            fileService = new FileService();
            work = new Work();

            /* event handlers */
            actionButton.Click += (s, e) =>
            {
                if (!session.InProgress)
                    this.StartWork();
                else
                    this.StopWork();
            };

            resetButton.Click += (s, e) => this.ResetWork();

            /* initialize the timer */
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 1, 0);
            timer.Tick += (s, e) => CountAndSetText();            
        }

        async protected override void OnNavigatedTo(NavigationEventArgs e) 
        {
            session = await fileService.ReadJsonAsync();

            if (session.InProgress)
                this.ContinueWork();
            else
                /* set the text of the text controllers */
                this.CountAndSetText();            
        }

        /* do some work */
        async private void StartWork()
        {
            work.Start(ref session);
            timer.Start();
            actionButton.Icon = new SymbolIcon(Symbol.Stop);
            this.CountAndSetText();
            await fileService.WriteJsonAsync(session);
        }

        private void ContinueWork()
        {
            timer.Start();
            actionButton.Icon = new SymbolIcon(Symbol.Stop);
            this.CountAndSetText();
        }

        private void StopWork()
        {
            work.Stop(ref session);
            timer.Stop();
            actionButton.Icon = new SymbolIcon(Symbol.Play);
        }

        async private void ResetWork()
        {
            work.Reset(ref session);
            actionButton.Icon = new SymbolIcon(Symbol.Play);
            this.CountAndSetText();
            await fileService.WriteJsonAsync(session);
        }

        /* set the text controlls' text */
        private void CountAndSetText()
        {
            /* count */
            session.ElapsedHours = session.StartTime.Equals(DateTime.MinValue) ? 0 : DateTime.Now.Hour - session.StartTime.Hour;
            session.ElapsedMinutes = session.StartTime.Equals(DateTime.MinValue) ? 0 : DateTime.Now.Minute - session.StartTime.Minute;            

            /* complex algorithm to count the hours done*/
            if (session.ElapsedHours - session.PreviousElapsedHours > 1)
                session.HoursDone += session.ElapsedHours - session.PreviousElapsedHours;
            else if (session.ElapsedHours - session.PreviousElapsedHours == 0)
                session.HoursDone = session.HoursDone;
            else if (session.ElapsedHours - session.PreviousElapsedHours == 1)
                ++session.HoursDone;

            session.PreviousElapsedHours = session.ElapsedHours;

            /* set the longest streak */
            session.LongestStreak = session.ElapsedHours.CompareTo(session.LongestStreak) > 0 ? session.ElapsedHours : session.LongestStreak;

            /* set text */
            textBlockHoursDone.Text = session.HoursDone + " out of 20 hours";
            textBlockLongestStreak.Text = session.LongestStreak + " hours";
            textBlockStartTime.Text = session.StartTime.Hour + ":" + session.StartTime.Minute + ":" + session.StartTime.Second;
        }        
    }
}
