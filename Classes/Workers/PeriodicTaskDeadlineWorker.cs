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
using Java.Security;

namespace functional_bubble.NET.Classes.Workers
{
    /// <summary>
    /// This worker will be triggered periodically every hour.
    /// It will check if there are any tasks that end between 1h to 2h from the moment it is triggered.
    /// If there are such tasks, it will send a notification.
    /// Then, it will update the widget.
    /// </summary>
    public class PeriodicTaskDeadlineWorker : Worker
    {
        public PeriodicTaskDeadlineWorker(Context context, WorkerParameters workerParameters) : base(context, workerParameters)
        {
        }

        public override Result DoWork()
        {
            //Send out notifications about tasks due between 1h 0 minutes 0 seconds and 1h 59 minutes 59 seconds.
            sendNotifications();

            //Update the widget.
            WidgetTodo.UpdateWidget();

            return Result.InvokeSuccess();
        }

        public void sendNotifications()
        {
            TaskHandler taskHandler = new TaskHandler();
            List<Task> tasksDue = taskHandler.GetTasksDueNextHour();

            if (tasksDue.Count == 1)
            {
                foreach (Task task in tasksDue)
                {
                    //TODO: Send notification containing task title
                    Console.WriteLine(" HHHHHHHHHHHHHHHHHH " + task.Title);
                }
            }
            if (tasksDue.Count > 1)
            {
                //TODO: Send notification contaning the taskDue.Count number
            }

        }
    }
}