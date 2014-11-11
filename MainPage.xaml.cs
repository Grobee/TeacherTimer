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

            work = new Work(); 
            timer = new Timer(work);                    
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            SetStatistics();
        }

        private void actionButton_Click(object sender, RoutedEventArgs e)
        {
            if (!work.InProgress)
            {
                actionButton.Icon = new SymbolIcon(Symbol.Pause);
                timer.Start();
                SetStatistics();
            }                
            else
            {
                actionButton.Icon = new SymbolIcon(Symbol.Play);
                timer.Stop();
            }                
        }

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
