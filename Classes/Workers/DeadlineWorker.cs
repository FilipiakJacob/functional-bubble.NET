using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Google.Android.Material.BottomNavigation;
using System;
using System.Collections.Generic;
using functional_bubble.NET.Classes;
using functional_bubble.NET.Fragments;
using AndroidX.Navigation;
using AndroidX.Navigation.Fragment;
using AndroidX.Navigation.UI;
using AndroidX.Work;
using Android.Content;


namespace functional_bubble.NET.Classes.Workers
{
    /// <summary>
    /// This worker's purpose is to periodically update the deadlines of tasks in the background.
    /// </summary>
    public class DeadlineWorker: Worker 
    {
        public DeadlineWorker( Context context, WorkerParameters workerParameters):base(context, workerParameters)
        {

        }

        public override Result DoWork()
        {
            UpdateDeadlines();

            return Result.InvokeSuccess();
        }

        private void UpdateDeadlines()
        {
            
        }
    }
}