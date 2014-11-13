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
        Timer timer;
        Work work;
        FileService fileSerivce;

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
            
            timer = new Timer();
            fileSerivce = new FileService();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            work = await fileSerivce.ReadJsonAsync();
            work = new Work()
            {
                ElapsedHours = 0,
                ElapsedMinutes = 0,
                HoursDone = 0,
                InProgress = false,
                LongestStreak = 0,
                StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0)
            };
            this.SetStatistics();
        }

        private async void actionButton_Click(object sender, RoutedEventArgs e)
        {
            if (!work.InProgress)
            {
                actionButton.Icon = new SymbolIcon(Symbol.Pause);
                timer.Start(work);
                this.SetStatistics();
                await fileSerivce.WriteJsonAsync(work);
            }                
            else
            {
                actionButton.Icon = new SymbolIcon(Symbol.Play);
                /* see if an hour has passed */
                this.OneHourPassed();
                work.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);                
                timer.Stop(work);
                this.SetStatistics();
                /* write it to "memory" */
                await fileSerivce.WriteJsonAsync(work);                
            }                
        }

        private void OneHourPassed()
        {
            int hoursPassed = DateTime.Now.Hour - work.StartTime.Hour;
            int minutesPassed = DateTime.Now.Minute - work.StartTime.Minute;

            if (hoursPassed > 1 || hoursPassed == 1 && minutesPassed >= 0)
                work.HoursDone += hoursPassed;
            /* see if the person broke the record hours */
            if (hoursPassed > work.LongestStreak)
                if (minutesPassed < 0)
                    work.LongestStreak = --hoursPassed;
                else
                    work.LongestStreak = hoursPassed;

            /*if (Math.Abs(minutesPassed) > work.LongestStreak)
                if (minutesPassed < 0)
                    work.LongestStreak = --minutesPassed;
                else
                    work.LongestStreak = minutesPassed;

            if (Math.Abs(minutesPassed) >= 1)
                work.HoursDone += Math.Abs(minutesPassed); */

        }

        /* update the frame */
        private void SetStatistics()
        {
            FormattedTime formattedTime;

            textBlockHoursDone.Text = work.HoursDone.ToString() + " out of 20 hours";
            textBlockLongestStreak.Text = work.LongestStreak.ToString() + " hours";

            formattedTime = FormatTime();

            if (!work.InProgress)
                textBlockStartTime.Text = "not started yet";
            else
                textBlockStartTime.Text = formattedTime.Hours + ":" + formattedTime.Minutes + ":" + formattedTime.Seconds;           
        }

        /* format time so that it will be displayed correctly*/
        private FormattedTime FormatTime()
        {
            FormattedTime formattedTime = new FormattedTime();

            if (work.StartTime.Hour < 10)
                formattedTime.Hours = "0" + work.StartTime.Hour.ToString();
            else
                formattedTime.Hours = work.StartTime.Hour.ToString();

            if(work.StartTime.Minute < 10)
                formattedTime.Minutes = "0" + work.StartTime.Minute.ToString();
            else
                formattedTime.Minutes = work.StartTime.Minute.ToString();

            if(work.StartTime.Second < 10)
                formattedTime.Seconds = "0" + work.StartTime.Second.ToString();
            else
                formattedTime.Seconds = work.StartTime.Second.ToString();

            return formattedTime;
        }
    }
}
