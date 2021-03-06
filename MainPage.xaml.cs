﻿using System;
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
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += (s, e) => CountAndSetText();
        }

        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            session = await fileService.ReadJsonAsync();

            if (session.InProgress)
                this.ContinueWork();
            else                
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

        async private void StopWork()
        {
            timer.Stop();
            work.Stop(ref session);
            this.SetDefaultText();
            actionButton.Icon = new SymbolIcon(Symbol.Play);
            await fileService.WriteJsonAsync(session);
        }

        async private void ResetWork()
        {
            timer.Stop();
            work.Reset(ref session);
            actionButton.Icon = new SymbolIcon(Symbol.Play);
            this.CountAndSetText();            
            await fileService.WriteJsonAsync(session);
        }

        /* set the text controlls' text */
        private void CountAndSetText()
        {
            /* count */
            if(session.InProgress) session.ElapsedTime = session.StartTime.Equals(DateTime.MinValue) ? TimeSpan.Zero : DateTime.Now.Subtract(session.StartTime);   

            /* set the longest streak */
            session.LongestStreak = session.ElapsedTime.CompareTo(session.LongestStreak) > 0 ? session.ElapsedTime : session.LongestStreak;

            /* set text */
            textBlockHoursDone.Text = this.FormatTime(session.ElapsedTime.Add(session.TotalTime).Hours) + ":" + this.FormatTime(session.ElapsedTime.Add(session.TotalTime).Minutes) + ":" + this.FormatTime(session.ElapsedTime.Add(session.TotalTime).Seconds);
            textBlockLongestStreak.Text = this.FormatTime(session.LongestStreak.Hours) + ":" + this.FormatTime(session.LongestStreak.Minutes) + ":" + this.FormatTime(session.LongestStreak.Seconds);
            textBlockStartTime.Text = !session.InProgress ? "not started yet" : this.FormatTime(session.StartTime.Hour) + ":" + this.FormatTime(session.StartTime.Minute) + ":" + this.FormatTime(session.StartTime.Second);
            textBlockElapsedTime.Text = !session.InProgress ? "not started yet" : this.FormatTime(session.ElapsedTime.Hours) + ":" + this.FormatTime(session.ElapsedTime.Minutes) + ":" + this.FormatTime(session.ElapsedTime.Seconds);
        }

        private void SetDefaultText()
        {
            textBlockStartTime.Text = "not started yet";
            textBlockElapsedTime.Text = "not started yet";
        }

        private string FormatTime(int time)
        {
            string formattedTime;

            if (time < 10)
                formattedTime = "0" + time;
            else
                formattedTime = time.ToString();

            return formattedTime;
        }
    }
}
