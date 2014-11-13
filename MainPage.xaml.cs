using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Work work;
        Session session;
        FileService fileSerivce;
        DispatcherTimer timer;

        struct FormattedTime
        {
            public string Hours { get; set; }
            public string Minutes { get; set; }
            public string Seconds { get; set; }
        };

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;            
            
            fileSerivce = new FileService();
            timer = new DispatcherTimer();            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            session = await fileSerivce.ReadJsonAsync();
            /*session = new Session()
            {
                ElapsedHours = 0,
                ElapsedMinutes = 0,
                HoursDone = 0,
                InProgress = false,
                LongestStreak = 0,
                StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0)
            };*/
            work = new Work(ref session);
            this.SetStatistics();
            this.SetFirstTime();
        }
 
        private void SetTimer()
        {
            /* count the time that is still remaining from the one hour interval */
            int doneHours = session.ElapsedMinutes == 0 ? 0 : 1;
            int doneMinutes = session.ElapsedMinutes == 0 ? 60 : session.ElapsedMinutes;
            /* set the interval */
            //timer.Interval = new TimeSpan(1 - doneHours, 60 - doneMinutes, 0);
            timer.Interval = new TimeSpan(0, 0, 30);
            //intervalTime.Text = timer.Interval.Hours + " " + timer.Interval.Minutes;
            //intervalTime.Text = session.ElapsedHours + " " + session.ElapsedMinutes;
            intervalTime.Text = timer.Interval.Minutes + " " + timer.Interval.Seconds;
            timer.Start();
            timer.Tick += (o, i) =>
            {
                work.CheckTime();
                this.SetStatistics();
                this.SetTimer();
            };                
        }
    
        private async void actionButton_Click(object sender, RoutedEventArgs e)
        {
            if (!session.InProgress)
            {                
                actionButton.Icon = new SymbolIcon(Symbol.Pause);
                work.Start();
                /* set the various textblock texts */
                this.SetFirstTime();
                this.SetTimer();
                this.SetStatistics();
                /* save the data to a json file */
                await fileSerivce.WriteJsonAsync(session);
            }                
            else
            {
                actionButton.Icon = new SymbolIcon(Symbol.Play);                               
                work.Stop();
                work.CheckLongestStreak();
                /* set the various textblock texts */
                this.SetFirstTime();
                this.SetStatistics();
                /* save the data to a json file */
                await fileSerivce.WriteJsonAsync(session);    
            }
        }
    
        private void SetFirstTime()
        {
            FormattedTime formattedTime;
            formattedTime = this.FormatTime();

            if (!session.InProgress)
                textBlockStartTime.Text = "not started yet";
            else
                textBlockStartTime.Text = formattedTime.Hours + ":" + formattedTime.Minutes + ":" + formattedTime.Seconds;
        }

        /* update the frame */
        private void SetStatistics()
        {
            textBlockHoursDone.Text = session.HoursDone.ToString() + " out of 20 hours";
            textBlockLongestStreak.Text = session.LongestStreak.ToString() + " hours";
        }

        /* format time so that it will be displayed correctly*/
        private FormattedTime FormatTime()
        {
            FormattedTime formattedTime = new FormattedTime();

            if (session.FirstTime.Hour < 10)
                formattedTime.Hours = "0" + session.FirstTime.Hour.ToString();
            else
                formattedTime.Hours = session.FirstTime.Hour.ToString();

            if (session.FirstTime.Minute < 10)
                formattedTime.Minutes = "0" + session.FirstTime.Minute.ToString();
            else
                formattedTime.Minutes = session.FirstTime.Minute.ToString();

            if (session.FirstTime.Second < 10)
                formattedTime.Seconds = "0" + session.FirstTime.Second.ToString();
            else
                formattedTime.Seconds = session.FirstTime.Second.ToString();

            return formattedTime;
        }

        private async void resetButton_Click(object sender, RoutedEventArgs e)
        {
            work.Reset();
            this.SetStatistics();
            this.SetFirstTime();
            actionButton.Icon = new SymbolIcon(Symbol.Play);            
            /* save data into a json file */
            await fileSerivce.WriteJsonAsync(session);
        }
    }
}
