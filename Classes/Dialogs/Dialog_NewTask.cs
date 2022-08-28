using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
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
    public class OnDateSet : EventArgs
    {
        private DateTime mDate;
        public OnDateSet()
        {
            Console.WriteLine("pojumbo");
        }
    }
    class Dialog_NewTask : AndroidX.Fragment.App.DialogFragment
    {
        //A class for creating A dialog fragment popup inside Taks UI
        private EditText mNewTaskTitle;
        private EditText mNewTaskDescription;
        private EditText mNewTaskDeadlineDate;
        private EditText mNewTaskDeadlineTime;
        private Spinner mNewTaskPriority;
        private TimePicker mTimePicker;
        private Button mBtnCreateTask; //Confirmation Button When All data about a new task had been written
        public Task mNewTask = new Task();
        public Calendar mCalendar = new Calendar();
        public string[] mSpinnerEntries = Application.Context.Resources.GetStringArray(Resource.Array.priorities_array);

        
        public EventHandler<TimePickerDialog.TimeSetEventArgs> OnTimeSet { get; private set; }
        public EventHandler<DatePickerDialog.DateSetEventArgs> OnDateSet;

        public event EventHandler<onNewTaskEventArgs> mNewTaskComplete; //Instance of onNewTaskEventArgs Event Handler
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {

            var inflater = Activity.LayoutInflater;

            var view = inflater.Inflate(Resource.Layout.dialog_new_task, null);
            var builder = new AlertDialog.Builder(Activity,Resource.Style.WrapContentDialog);
            if (view != null)
            {
                builder.SetView(view);
            }
            var dialog = builder.Create();
            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Transparent));
            mNewTaskTitle = view.FindViewById<EditText>(Resource.Id.new_task_title);
            mNewTaskDescription = view.FindViewById<EditText>(Resource.Id.new_task_description);

            mNewTaskPriority = view.FindViewById<Spinner>(Resource.Id.new_task_priority);
            //mNewTaskPriority.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (MNewTaskPriority_ItemSelected); //method called when an item from priority spinner is chosen


            mBtnCreateTask = view.FindViewById<Button>(Resource.Id.new_task_button);            
            mBtnCreateTask.Click += MBtnCreateTask_Click; //Method executed when Confirmation Button is clicked

            //mTimePicker = view.FindViewById<TimePicker>(Resource.Id.timePicker1);
            //mTimePicker.SetIs24HourView(Java.Lang.Boolean.True);
            
            mNewTaskDeadlineDate = view.FindViewById<EditText>(Resource.Id.new_task_deadline_date);
            mNewTaskDeadlineTime = view.FindViewById<EditText>(Resource.Id.new_task_deadline_time);

            TextView mNewTaskWrongDate = view.FindViewById<TextView>(Resource.Id.new_task_wrongDateMessage);
            mNewTaskDeadlineDate.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                string txt = mNewTaskDeadlineDate.Text;
                int dayNum = 0;
                int monthNum = 0;
                bool properDate = true;
                if (e.AfterCount != 0)
                {
                    if ((e.Text.Count() == 2 && !txt.Contains('/')) || (e.Text.Count() == 5 && txt[2] == '/')) { mNewTaskDeadlineDate.Text += "/"; mNewTaskDeadlineDate.SetSelection(mNewTaskDeadlineDate.Text.Length); }
                    else if (e.Text.Count() == 10) { mNewTaskDeadlineDate.Enabled = false; mNewTaskDeadlineDate.Enabled = true; }
                    else if (e.Text.Count() > 10) { mNewTaskDeadlineDate.Text = mNewTaskDeadlineDate.Text.Substring(0, 10); }
                }
                else if( e.Text.Count() == 2 || e.Text.Count() == 5) { mNewTaskDeadlineDate.Text = mNewTaskDeadlineDate.Text.Substring(0, mNewTaskDeadlineDate.Text.Length -1); mNewTaskDeadlineDate.SetSelection(mNewTaskDeadlineDate.Text.Length); }
                if (txt.Length > 1) { Int32.TryParse(txt.Substring(0, 2), out int j); if (1 > j || j > 31) { mNewTaskWrongDate.Text = "Go Fuck Yourself Daily"; properDate = false; } else { dayNum = j; } }
                if (txt.Length > 4) { Console.WriteLine("hi"); Console.WriteLine(txt.Substring(3, 2)); Int32.TryParse(txt.Substring(3, 2), out int k); if (1 > k || k > 12) { mNewTaskWrongDate.Text = "Go Fuck Yourself Monthly"; properDate = false; } else if (!mCalendar.correctDaysNum(dayNum, k)) { mNewTaskWrongDate.Text = "Go Fuck Yourself Monthly wrong day"; properDate = false; } else{ monthNum = k; } }
                if (txt.Length == 10) { Int32.TryParse(txt.Substring(7,2), out int l); if (monthNum == 2) { if (!mCalendar.correctFebruary(dayNum, l)) { mNewTaskWrongDate.Text = "Go Fuck Yourself Yearly Feb"; properDate = false; } } }
                if(properDate) { mNewTaskWrongDate.Text = ""; }
            };

            Button mDateButton = view.FindViewById<Button>(Resource.Id.new_task_calendar_button);
            mDateButton.Click += (object sender, EventArgs e) =>
            {
                Dialog_DatePicker datePicker = new Dialog_DatePicker();
                datePicker.Show(ChildFragmentManager,"Date");
                datePicker.mOnDatePicked += (object sender, DatePickerDialog.DateSetEventArgs e) => 
                {
                    mNewTaskDeadlineDate.Text = (e.Month + 1).ToString() + "/" + e.DayOfMonth.ToString() + "/" + (e.Year).ToString();
                };
            };

            Button mTimeButton = view.FindViewById<Button>(Resource.Id.new_task_clock_button);
            mTimeButton.Click += (object sender, EventArgs e) =>
            {
                Dialog_TimePicker timePicker = new Dialog_TimePicker();
                timePicker.Show(ChildFragmentManager, "Time");
                timePicker.mOnTimePicked += (object sender, TimePickerDialog.TimeSetEventArgs e) =>
                {
                    mNewTaskDeadlineTime.Text = e.HourOfDay.ToString() + ":" + e.Minute.ToString();
                };
            };

            return dialog;
        }

        private void MBtnCreateTask_Click(object sender, EventArgs e)
        {
            //User clicked the Confirmation Button
            mNewTask.Title = mNewTaskTitle.Text;
            mNewTask.Description = mNewTaskDescription.Text;
            mNewTask.Priority = Array.IndexOf(mSpinnerEntries, mNewTaskPriority.SelectedItem.ToString()); //mNewTaskPriority.SelectedItem.ToString();
            mNewTask.Deadline = DateTime.Parse(mNewTaskDeadlineDate.Text + " " + mNewTaskDeadlineTime.Text);
            Console.WriteLine(mNewTask.Deadline);
            mNewTaskComplete.Invoke(this, new onNewTaskEventArgs(mNewTask)); //Invoke the Event Handler
            this.Dismiss(); //Dialog Fragment deletes itself
        }
    }
}