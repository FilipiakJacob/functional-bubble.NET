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
using functional_bubble.NET.Classes;

namespace functional_bubble.NET
{
    public class onNewTaskEventArgs : EventArgs
    {
        //An Event class which when Invoked will pass its data into all methods subscribed to this event
        public Task mNewTaskInEvent; //Class argument which is a Task Instance

        public onNewTaskEventArgs(Task newTask)
        {
            //Class Constructor which will requires Task Instance as argument and will set the class Task attribute to be that passed instance  
            mNewTaskInEvent = newTask;
        }



    }
    class Dialog_NewTask : AndroidX.Fragment.App.DialogFragment
    {
        //A class for creating A dialog fragment popup inside Taks UI
        private EditText mNewTaskTitle;
        private EditText mNewTaskDescription;
        private Spinner mNewTaskPriority;
        private Button mBtnCreateTask; //Confirmation Button When All data about a new task had been written
        public Task mNewTask = new Task();

        public event EventHandler<onNewTaskEventArgs> mNewTaskComplete; //Instance of onNewTaskEventArgs Event Handler
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.dialog_new_task, container, false);

            mNewTaskTitle = view.FindViewById<EditText>(Resource.Id.new_task_title);
            mNewTaskDescription = view.FindViewById<EditText>(Resource.Id.new_task_description);

            mNewTaskPriority = view.FindViewById<Spinner>(Resource.Id.new_task_priority);
            mNewTaskPriority.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (MNewTaskPriority_ItemSelected); //method called when an item from priority spinner is chosen


            mBtnCreateTask = view.FindViewById<Button>(Resource.Id.new_task_button);            
            mBtnCreateTask.Click += MBtnCreateTask_Click; //Method executed when Confirmation Button is clicked
            return view;
        }

        private void MNewTaskPriority_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            /// <summary>
            /// A method called when user chose an item from priority spinner
            /// </summary>
            Spinner MNewTaskPriority = (Spinner)sender;
            mNewTask.Priority = string.Format((string)MNewTaskPriority.GetItemAtPosition(e.Position));
        }

        private void MBtnCreateTask_Click(object sender, EventArgs e)
        {
            //User clicked the Confirmation Button
            mNewTask.Title = mNewTaskTitle.Text;
            mNewTask.Description = mNewTaskDescription.Text;
            mNewTaskComplete.Invoke(this, new onNewTaskEventArgs(mNewTask)); //Invoke the Event Handler
            this.Dismiss(); //Dialog Fragment deletes itself
        }
    }
}