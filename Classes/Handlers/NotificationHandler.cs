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
        public int taskID;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="taskID">int taskID - id of the task that is destination of notification</param>
        public NotificationHandler(int taskID)
        {
            this.taskID = taskID;
        }

        /// <summary>
        /// sends notification to the user screaming TEST TEST TEST TEST
        /// </summary>
        public void Notification()
        {
            DeeplinkHandler deeplinkHandler = new DeeplinkHandler();

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
        /// Returns a random notification message 
        /// </summary>
        /// <returns>string message</returns>
        public string RandomMSG()
        {
            string[] msgArray = Application.Context.Resources.GetStringArray(Resource.Array.msg_array);
            Random rnd = new Random();
            int num = rnd.Next(msgArray.Length);

            return msgArray[num];
        }
    }
}