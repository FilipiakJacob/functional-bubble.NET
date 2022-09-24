using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Appwidget;
using Android.Util;
using AndroidX.Navigation;

namespace functional_bubble.NET.Classes
{
    [BroadcastReceiver(Label = "3PenguinsWidget", Exported = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/appwidgetprovider")]
    public class WidgetTodo : AppWidgetProvider
    {

        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            //Example: create widget, resize, meet update time, manually updated
            ComponentName me = new ComponentName(context, Java.Lang.Class.FromType(typeof(WidgetTodo)).Name);
            appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context, appWidgetIds));


        }
        
        private RemoteViews BuildRemoteViews(Context context, int[] appWidgetIds)
        {
            //Build view
            RemoteViews widgetView = new RemoteViews(context.PackageName, Resource.Layout.widget_todo);

            //Change text in widget
            SetTextViewText(widgetView);

            //Handle buttons in widget
            RegisterClicks(context, appWidgetIds, widgetView);

            return widgetView;
        }


        /// <summary>
        /// Sets the content of the widget's view.
        /// </summary>
        /// <param name="widgetView"></param>
        private void SetTextViewText(RemoteViews widgetView)
        {
            //widgetView.SetTextViewText(Resource.Id.todo_widget_text, "PENGUIN");
            IEnumerable<Task> sortedTasks = RetrieveTasks(10); //Retrieve 4 tasks
            int i = 0;
            foreach (Task task in sortedTasks)
            {
                Console.WriteLine(task.Id);
                i++;
                Console.WriteLine("i = " + i);
            }
            Console.WriteLine("That's All Folks");
        }

        /// <summary>
        /// Retrieves a certain amount of highest priority tasks from database.
        /// </summary>
        /// <param name="numTask"></param>
        /// <returns></returns>
        private IEnumerable<Task> RetrieveTasks(int numTask) //TODO: Once database handler is updated,
                                                             //there should be a method that only returns a certain amount of tasks.
        {
            TaskHandler taskHandler = new TaskHandler();
            Console.WriteLine("Num of Tasks = " + taskHandler.GetSortedTasks().Count);
            IEnumerable<Task> sortedTasks = taskHandler.GetSortedTasks().Take(numTask); //Take() returns an IEnumerable.
            return sortedTasks;
        }

        private void RegisterClicks(Context context, int[] appWidgetIds, RemoteViews widgetView)
        {
            //Use NavDeepLinkBuilder to create a PendingIntent that deeplinks to the New Task dialog.
            Bundle bundle = new Bundle();
            bundle.PutInt("openDialog", 1);
            PendingIntent pendingIntent = new NavDeepLinkBuilder(context)
                .SetGraph(Resource.Navigation.nav_graph)
                .SetDestination(Resource.Id.dest_todo)
                .SetArguments(bundle)
                .CreatePendingIntent();

            widgetView.SetOnClickPendingIntent(Resource.Id.widget_new_task_button, pendingIntent);

            //var intent = new Intent(context, typeof(WidgetTodo));
            //intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);
            //intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, appWidgetIds);

            // Register click event for button1
            //var widgetButton1 = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);
            //widgetView.SetOnClickPendingIntent(Resource.Id.button1, widgetButton1);
        }


        public override void OnReceive(Context context, Intent intent)
            //Method is called when the widget receives an intent broadcast.
        {
            base.OnReceive(context, intent); //Since OnReceive is overrridden, need to call
                                             //base class to make sure OnUpdate and similar methods ale triggered by this method.
            Toast.MakeText(context, "Received intent!", ToastLength.Short).Show();
        }
    }
}