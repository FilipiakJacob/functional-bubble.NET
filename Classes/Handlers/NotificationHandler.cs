using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Android.Util.EventLogTags;

namespace functional_bubble.NET.Classes.Handlers
{
    public class NotificationHandler
    {
        private static readonly int NOTIFICATION_ID = 1000; 
        private static readonly string CHANNEL_ID = "local_notification";

        /// <summary>
        /// sends notification to the user screaming TEST TEST TEST TEST
        /// </summary>
        public void Notification()
        {
            DeeplinkHandler deeplinkHandler = new DeeplinkHandler();
            int taskID = RandomTaskDueNextHour();

            if (taskID == 0)
            {
                return;
            }
            
            var builder = new Notification.Builder(Application.Context, CHANNEL_ID).SetAutoCancel(true)
                .SetContentTitle("Task Created")
                .SetContentIntent
                (deeplinkHandler.CreateDeeplink(Application.Context, Resource.Id.dest_task, "taskID", taskID))
                .SetSmallIcon(Resource.Drawable.ic_clock_black_24dp)
                .SetContentText(RandomMSG());

            var nmc = NotificationManager.FromContext(Application.Context);
            nmc.Notify(NOTIFICATION_ID, builder.Build());
        }

        /// <summary>
        /// Returns a random notification message based on how many tasks
        /// are there to notify about
        /// </summary>
        /// <returns>string message</returns>
        public string RandomMSG()
        {
            TaskHandler taskHandler = new TaskHandler();
            List<Task> taskDueHour = taskHandler.GetTasksDueNextHour();

            if (taskDueHour.Count() > 1) {
                int numTask = taskDueHour.Count();
                string msg = $"You have {numTask} tasks left";
                return msg;
            }

            string[] msgArray = Application.Context.Resources.GetStringArray(Resource.Array.msg_array);
            Random rnd = new Random();
            int num = rnd.Next(msgArray.Length);

            return msgArray[num];
        }
        /// Return id  to random task that is due next hour
        /// </summary>
        /// <returns>int id</returns>
        public int RandomTaskDueNextHour()
        {
            TaskHandler taskHandler = new TaskHandler();
            Random rnd = new Random();
            List<Task> taskDueHour = taskHandler.GetTasksDueNextHour();
            if (taskDueHour.Count() > 0)
            {
                return taskDueHour
                    [rnd.Next(taskDueHour.Count())]
                    .Id;
            }
            return 0;
        }
    }
}