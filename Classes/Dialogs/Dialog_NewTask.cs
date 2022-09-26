using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using functional_bubble.NET.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        private TextView mNewTaskWrongDate;
        private TimePicker mTimePicker;
        private ViewStub mViewStub;
        private Button mBtnCreateTask; //Confirmation Button When All data about a new task had been written
        public Task mNewTask = new Task();
        public Calendar mCalendar = new Calendar();
        public string[] mSpinnerEntries = Application.Context.Resources.GetStringArray(Resource.Array.priorities_array);
        public string dateForTheseAmericans;
        
        public EventHandler<TimePickerDialog.TimeSetEventArgs> OnTimeSet { get; private set; }
        public EventHandler<DatePickerDialog.DateSetEventArgs> OnDateSet;

        public event EventHandler<onNewTaskEventArgs> mNewTaskComplete; //Instance of onNewTaskEventArgs Event Handler
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {

            LayoutInflater inflater = Activity.LayoutInflater;

            View view = inflater.Inflate(Resource.Layout.dialog_new_task, null);
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity,Resource.Style.WrapContentDialog);
            if (view != null)
            {
                builder.SetView(view);
            }
            AlertDialog dialog = builder.Create();
            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Transparent));
            
            mViewStub = view.FindViewById<ViewStub>(Resource.Id.viewStub1);
            mViewStub.LayoutInflater = this.LayoutInflater;
            mViewStub.LayoutResource = Resource.Layout.date_and_time_layout;
            mViewStub.Inflate();
            

            //Task Title:
            mNewTaskTitle = view.FindViewById<EditText>(Resource.Id.new_task_title);

            //Task Description:
            mNewTaskDescription = view.FindViewById<EditText>(Resource.Id.new_task_description);

            //Priotiry Spinner:
            mNewTaskPriority = view.FindViewById<Spinner>(Resource.Id.new_task_priority);

            //Add Task button:
            mBtnCreateTask = view.FindViewById<Button>(Resource.Id.new_task_button);            
            mBtnCreateTask.Click += MBtnCreateTask_Click; //Method executed when Confirmation Button is clicked

            //Wrong date TextView:
            mNewTaskWrongDate = view.FindViewById<TextView>(Resource.Id.new_task_wrongDateMessage);

            //Date Text:
            mNewTaskDeadlineDate = view.FindViewById<EditText>(Resource.Id.new_task_deadline_date);
            mNewTaskDeadlineDate.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                dateForTheseAmericans = mCalendar.dateChange(mNewTaskDeadlineDate.Text, e, mNewTaskDeadlineDate, mNewTaskWrongDate);
            };

            //DatePicker button:
            Button mDateButton = view.FindViewById<Button>(Resource.Id.new_task_calendar_button);
            mDateButton.Click += (object sender, EventArgs e) =>
            {
                Dialog_DatePicker datePicker = new Dialog_DatePicker();
                datePicker.Show(ChildFragmentManager,"Date");
                datePicker.mOnDatePicked += (object sender, DatePickerDialog.DateSetEventArgs e) => 
                {
                    mNewTaskDeadlineDate.Text = mCalendar.twoDigitDate(e.DayOfMonth) + "/" + mCalendar.twoDigitDate(e.Month + 1) + "/" + (e.Year).ToString();
                    dateForTheseAmericans = mCalendar.twoDigitDate(e.Month + 1) + "/" + mCalendar.twoDigitDate(e.DayOfMonth) + "/" + (e.Year).ToString();

                };
            };

            //Time Text:
            mNewTaskDeadlineTime = view.FindViewById<EditText>(Resource.Id.new_task_deadline_time);
            mNewTaskDeadlineTime.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                mCalendar.timeChange(mNewTaskDeadlineTime.Text,e,mNewTaskDeadlineTime,mNewTaskWrongDate);
            };

            //TimePicker button:
            Button mTimeButton = view.FindViewById<Button>(Resource.Id.new_task_clock_button);
            mTimeButton.Click += (object sender, EventArgs e) =>
            {
                Dialog_TimePicker timePicker = new Dialog_TimePicker();
                timePicker.Show(ChildFragmentManager, "Time");
                timePicker.mOnTimePicked += (object sender, TimePickerDialog.TimeSetEventArgs e) =>
                {
                    mNewTaskDeadlineTime.Text = mCalendar.twoDigitDate(e.HourOfDay) + ":" + mCalendar.twoDigitDate(e.Minute);
                };
            };

            return dialog;
        }

        private void MBtnCreateTask_Click(object sender, EventArgs e)
        {
            //User clicked the Confirmation Button
            if (!DateTime.TryParseExact(mNewTaskDeadlineDate.Text,"dd/MM/yyyy",null, System.Globalization.DateTimeStyles.None, out _))
            { 
                //try to parse the date from string to DateTime, if it is wrong, execute this if statement
                mNewTaskWrongDate.Text = "Wrong Date";
                return;
            }
            if (!DateTime.TryParseExact(mNewTaskDeadlineTime.Text,"HH:mm",null, System.Globalization.DateTimeStyles.None, out _))
            { 
                //try to parse the time from string to DateTime, if it is wrong, execute this if statement
                mNewTaskWrongDate.Text = "Wrong Time";
                return; 
            }
            mNewTask.Title = mNewTaskTitle.Text;
            mNewTask.Description = mNewTaskDescription.Text;
            mNewTask.Priority = Array.IndexOf(mSpinnerEntries, mNewTaskPriority.SelectedItem.ToString()); //mNewTaskPriority.SelectedItem.ToString();
            mNewTask.Deadline = DateTime.Parse(dateForTheseAmericans + " " + mNewTaskDeadlineTime.Text);
            if (mNewTask.Deadline < DateTime.Now) //If the date provided already happened
            {
                mNewTaskWrongDate.Text = "Date in past";
                return;
            }
            mNewTaskComplete.Invoke(this, new onNewTaskEventArgs(mNewTask)); //Invoke the Event Handler
            this.Dismiss(); //Dialog Fragment deletes itself
        }
    }
}