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
using Android.Content.PM;
using System.Drawing;
using Java.Lang;
using System.Runtime.Remoting.Contexts;
using Context = Android.Content.Context;

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

            //Set the top bar
            SetTopBar(context, widgetView);

            //Set the tasks 
            UpdateTasks(context, widgetView, 4);

            return widgetView;
        }


        /// <summary>
        /// Inflates the tasks and replaces the ViewStubs in the widget's layout, if tasks are present.
        /// </summary>
        /// <param name="context">Context in which the receiver is running</param>
        /// <param name="widgetView"></param>
        /// <param name="numTasks">Number of tasks to display (currently only 4 or less works) </param>
        private void UpdateTasks(Context context, RemoteViews widgetView, int numTasks)
        {
            //widgetView.SetTextViewText(Resource.Id.todo_widget_text, "PENGUIN");
            IEnumerable<Task> sortedTasks = RetrieveTasks(numTasks); //Retrieve a number of tasks
            int[] rowViewIds = new int[numTasks]; //Initialize an empty array of size "numTasks". It will hold the IDs of "row" views.
            int[] titleViewIds = new int[numTasks]; //The same but for row title views.
            int[] deadlineViewIds = new int[numTasks]; //The same but for views that show deadlines.
            for (int i = 0; i < numTasks; i++)
                //Loop iterates through task_rows and task_row_title and task_row_time_left views in the layout, adding their IDs to the arrays.
            {
                rowViewIds[i] = context.Resources.GetIdentifier("task_row_" + (i+1), "id", context.PackageName);
                titleViewIds[i] = context.Resources.GetIdentifier("task_row_title_" + (i+1), "id", context.PackageName);
                deadlineViewIds[i] = context.Resources.GetIdentifier("task_row_time_left_" + (i+1), "id", context.PackageName);
            }
            int j = 0;
            foreach (Task task in sortedTasks)
            {
                string untilDeadlineStr;
                TimeSpan untilDeadline = task.Deadline.Subtract(DateTime.Now);
                if (untilDeadline.Days >= 1)
                {
                    untilDeadlineStr = (untilDeadline.Days.ToString() + "d left");
                }
                else
                {
                    untilDeadlineStr = (untilDeadline.Hours.ToString() + "h left");
                }
                widgetView.SetViewVisibility(rowViewIds[j], ViewStates.Visible); //SetViewVisibility should inflate the layout.
                widgetView.SetTextViewText(titleViewIds[j], task.Title);
                widgetView.SetTextViewText(deadlineViewIds[j], untilDeadlineStr);
                PendingIntent pendingIntent = CreateDeepLink(context,Resource.Id.dest_task,"taskID",task.Id);
                widgetView.SetOnClickPendingIntent(rowViewIds[j],pendingIntent); //When the row is clicked, the user should be deeplinked to the task.
                j++;
            }
        }

        /// <summary>
        /// Retrieves a certain amount of highest priority tasks from database.
        /// </summary>
        /// <param name="numTask">Number of tasks</param>
        /// <returns></returns>
        private IEnumerable<Task> RetrieveTasks(int numTask) //TODO: Once database handler is updated,
                                                             //there should be a method that only returns a certain amount of tasks.
        {
            TaskHandler taskHandler = new TaskHandler();
            Console.WriteLine("Num of Tasks = " + taskHandler.GetSortedTasks().Count);
            IEnumerable<Task> sortedTasks = taskHandler.GetSortedTasks().Take(numTask); //Take() returns an IEnumerable.
            return sortedTasks;
        }

        /// <summary>
        /// Sets the contents of the top bar of the widget
        /// </summary>
        /// <param name="context">Context in which the receiver is running</param>
        /// <param name="widgetView"></param>
        private void SetTopBar(Context context, RemoteViews widgetView)
        {
            //Use NavDeepLinkBuilder to create a PendingIntent that deeplinks to the New Task dialog.
            PendingIntent pendingIntent = CreateDeepLink(context, Resource.Id.dest_todo, "openDialog", 1);
            widgetView.SetOnClickPendingIntent(Resource.Id.widget_new_task_button, pendingIntent);
        }

        /// <summary>
        /// Use NavDeepLinkBuilder to create a PendingIntent that deeplinks to a a destination.
        /// </summary>
        /// <param name="destination">Usually just Resource.Id.rourcename</param>
        /// <param name="context">Context in which the receiver is running</param>
        /// <param name="bundleVarType">The pending intent sends a bundle of arguments to the target destination of the deeplink</param>
        /// <param name="bundleVarValue"></param>
        /// <returns>A pending intent</returns>
        private PendingIntent CreateDeepLink(Context context, int destination, string bundleVarType, int bundleVarValue )
        {
            Bundle bundle = new Bundle();
            bundle.PutInt(bundleVarType, bundleVarValue);
            PendingIntent pendingIntent = new NavDeepLinkBuilder(context)
                .SetGraph(Resource.Navigation.nav_graph)
                .SetDestination(destination)
                .SetArguments(bundle)
                .CreatePendingIntent();
            return pendingIntent;
        }


        public override void OnReceive(Context context, Intent intent)
            //Method is called when the widget receives an intent broadcast.
        {
            base.OnReceive(context, intent); //Since OnReceive is overrridden, need to call
                                             //base class to make sure OnUpdate and similar methods ale triggered by this method.
            Toast.MakeText(context, "Received intent!", ToastLength.Short).Show();
        }
        /*
        private void RegisterClicks(Context context, int[] appWidgetIds, RemoteViews widgetView)
        {
            //Use NavDeepLinkBuilder to create a PendingIntent that deeplinks to the New Task dialog.
            PendingIntent pendingIntent = CreateDeepLink(context,Resource.Id.dest_todo,"openDialog",1);

            widgetView.SetOnClickPendingIntent(Resource.Id.widget_new_task_button, pendingIntent);

            //var intent = new Intent(context, typeof(WidgetTodo));
            //intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);
            //intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, appWidgetIds);

            // Register click event for button1
            //var widgetButton1 = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);
            //widgetView.SetOnClickPendingIntent(Resource.Id.button1, widgetButton1);
        }
        */
    }
}