using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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
            var builder = new Notification.Builder(Application.Context, CHANNEL_ID).SetAutoCancel(true)
                .SetContentTitle("Task Created")
                .SetSmallIcon(Resource.Drawable.ic_clock_black_24dp)
                .SetContentText($"TEST TEST TEST TEST");

            var nmc = NotificationManager.FromContext(Application.Context);
            nmc.Notify(NOTIFICATION_ID, builder.Build());
        }

    }
}