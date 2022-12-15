using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Appwidget;
using AndroidX.Navigation;
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
        private IEnumerable<Task> RetrieveTasks(int numTask) 
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
        /// <param name="bundleVarType">DEFAULT = null. The pending intent sends a bundle of arguments to the target destination of the deeplink. </param>
        /// <param name="bundleVarValue">DEFAULT = 0. This argument is only used if bundleVarType is not null. </param>
        /// <returns>A pending intent</returns>
        private PendingIntent CreateDeepLink(Context context, int destination, string bundleVarType = null, int bundleVarValue = 0 )
        {
            Bundle bundle = new Bundle();
            if (bundleVarType != null)
            {
                bundle.PutInt(bundleVarType, bundleVarValue);
            }
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

        /// <summary>
        ///
        /// Call this method to update the widget 
        /// </summary>
        public static void UpdateWidget()
        {
            //TODO: Check if widget exists before updating it.
            Context mContext = Application.Context;
            //Update the widget
            var intent = new Intent(mContext, typeof(WidgetTodo)); //Create new intent
            intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);
            AppWidgetManager appWidgetManager = AppWidgetManager.GetInstance(mContext); //Get an instance of appwidgetmanager
            //This line of code translates weirdly from Java, I'm not sure if this is optimal but it works.
            ComponentName name = new ComponentName(mContext, Java.Lang.Class.FromType(typeof(WidgetTodo)).Name);
            int[] appWidgetIds = appWidgetManager.GetAppWidgetIds(name);
            intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, appWidgetIds);
            mContext.SendBroadcast(intent); //Send the intent
        }
    }
}