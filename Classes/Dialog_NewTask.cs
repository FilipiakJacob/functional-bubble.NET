using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using functional_bubble.NET.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        private TimePicker mTimePicker;
        private Button mBtnCreateTask; //Confirmation Button When All data about a new task had been written
        public Task mNewTask = new Task();
        public Calendar mCalendar = new Calendar();
        public string[] mSpinnerEntries = Application.Context.Resources.GetStringArray(Resource.Array.priorities_array);

        public event EventHandler<onNewTaskEventArgs> mNewTaskComplete; //Instance of onNewTaskEventArgs Event Handler
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.dialog_new_task, container, false);

            mNewTaskTitle = view.FindViewById<EditText>(Resource.Id.new_task_title);
            mNewTaskDescription = view.FindViewById<EditText>(Resource.Id.new_task_description);

            mNewTaskPriority = view.FindViewById<Spinner>(Resource.Id.new_task_priority);
            //mNewTaskPriority.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (MNewTaskPriority_ItemSelected); //method called when an item from priority spinner is chosen


            mBtnCreateTask = view.FindViewById<Button>(Resource.Id.new_task_button);            
            mBtnCreateTask.Click += MBtnCreateTask_Click; //Method executed when Confirmation Button is clicked

            mTimePicker = view.FindViewById<TimePicker>(Resource.Id.timePicker1);
            mTimePicker.SetIs24HourView(Java.Lang.Boolean.True);
            /*
            EditText mNewTaskDeadline = view.FindViewById<EditText>(Resource.Id.new_task_deadline);
            TextView mNewTaskWrongDate = view.FindViewById<TextView>(Resource.Id.new_task_wrongDateMessage);
            mNewTaskDeadline.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                string txt = mNewTaskDeadline.Text;
                int dayNum = 0;
                int monthNum = 0;
                bool properDate = true;
                if (e.AfterCount != 0)
                {
                    if ((e.Text.Count() == 2 && !txt.Contains('/')) || (e.Text.Count() == 5 && txt[2] == '/')) { mNewTaskDeadline.Text += "/"; mNewTaskDeadline.SetSelection(mNewTaskDeadline.Text.Length); }
                    else if (e.Text.Count() == 10) { mNewTaskDeadline.Enabled = false; mNewTaskDeadline.Enabled = true; }
                    else if (e.Text.Count() > 10) { mNewTaskDeadline.Text = mNewTaskDeadline.Text.Substring(0, 10); }
                }
                else if( e.Text.Count() == 2 || e.Text.Count() == 5) { mNewTaskDeadline.Text = mNewTaskDeadline.Text.Substring(0, mNewTaskDeadline.Text.Length -1); mNewTaskDeadline.SetSelection(mNewTaskDeadline.Text.Length); }
                if (txt.Length > 1) { Int32.TryParse(txt.Substring(0, 2), out int j); if (1 > j || j > 31) { mNewTaskWrongDate.Text = "Go Fuck Yourself Daily"; properDate = false; } else { dayNum = j; } }
                if (txt.Length > 4) { Console.WriteLine("hi"); Console.WriteLine(txt.Substring(3, 2)); Int32.TryParse(txt.Substring(3, 2), out int k); if (1 > k || k > 12) { mNewTaskWrongDate.Text = "Go Fuck Yourself Monthly"; properDate = false; } else if (!mCalendar.correctDaysNum(dayNum, k)) { mNewTaskWrongDate.Text = "Go Fuck Yourself Monthly wrong day"; properDate = false; } else{ monthNum = k; } }
                if (txt.Length == 10) { Int32.TryParse(txt.Substring(7,2), out int l); if (monthNum == 2) { if (!mCalendar.correctFebruary(dayNum, l)) { mNewTaskWrongDate.Text = "Go Fuck Yourself Yearly Feb"; properDate = false; } } }
                if(properDate) { mNewTaskWrongDate.Text = ""; }
            };
            */






                return view;
        }

        /*
        private void MNewTaskPriority_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            /// <summary>
            /// A method called when user chose an item from priority spinner
            /// </summary>
            Spinner MNewTaskPriority = (Spinner)sender;
            
        }
        */


        private void MBtnCreateTask_Click(object sender, EventArgs e)
        {
            //User clicked the Confirmation Button
            mNewTask.Title = mNewTaskTitle.Text;
            mNewTask.Description = mNewTaskDescription.Text;
            mNewTask.Priority = Array.IndexOf(mSpinnerEntries, mNewTaskPriority.SelectedItem.ToString()); //mNewTaskPriority.SelectedItem.ToString();
            Console.WriteLine(mNewTask.Priority);
            mNewTaskComplete.Invoke(this, new onNewTaskEventArgs(mNewTask)); //Invoke the Event Handler
            this.Dismiss(); //Dialog Fragment deletes itself
        }
    }
}