using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroidX.Fragment.App;
using functional_bubble.NET.Classes;
using AndroidX.Navigation;
using functional_bubble.NET.Classes.Dialogs;
using AndroidX.ConstraintLayout.Widget;
using Android.App;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace functional_bubble.NET.Fragments
{
    public class TaskBase : AndroidX.Fragment.App.Fragment
    {
        Calendar mCalendar = new Calendar();
        Task mTask;
        TextView taskDeadline;
        public string[] mPrioritiesArray = Application.Context.Resources.GetStringArray(Resource.Array.priorities_array);
        public string[] mLabelsArray = Application.Context.Resources.GetStringArray(Resource.Array.labels_array);

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.task_base, container, false);

            int ID = Arguments.GetInt("taskID");//get id of a task passed from ToDo fragment
            TaskHandler db = new TaskHandler();
            mTask = db.Get(ID);

            //Tasks Title:
            EditText title = view.FindViewById<EditText>(Resource.Id.task_base_title);
            title.Text = mTask.Title;
            title.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {
                mTask.Title = title.Text;
                mTask.update_data(); //update record in database
            };

            //Tasks Description:
            EditText description = view.FindViewById<EditText>(Resource.Id.task_base_description);
            description.Text = mTask.Description;
            description.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {
                mTask.Description = description.Text;
                mTask.update_data(); //update record in database
            };

            //Task Priority
            TextView priority = view.FindViewById<TextView>(Resource.Id.task_base_priority);
            priority.Text = mPrioritiesArray[mTask.Priority];

            //Task Label
            TextView label = view.FindViewById<TextView>(Resource.Id.task_base_label);
            label.Text = mLabelsArray[mTask.Label];

            //Go Back Button:
            view.FindViewById<Button>(Resource.Id.task_base_goBackButton).Click += (object sender, EventArgs e) => 
            {
                Navigation.FindNavController(view).Navigate(Resource.Id.GoBackToTodoList);//go back to Todo list
            };

            //Delete Button:
            ImageButton deleteButton = view.FindViewById<ImageButton>(Resource.Id.task_base_deleteTaskButton);
            deleteButton.Click += (object sender, EventArgs e) => 
            {
                mTask.delete_data();//delete task from database
                Navigation.FindNavController(view).Navigate(Resource.Id.GoBackToTodoList);//go back to Todo list
            };

            //Complete Task Button:
            view.FindViewById<Button>(Resource.Id.task_base_completeTaskButton).Click += (object sender, EventArgs e) =>
            {
                //Economy not implemented yet! Therefore this function has the same capability as delete button for now
                mTask.delete_data();
                Navigation.FindNavController(view).Navigate(Resource.Id.GoBackToTodoList);
            };

            //Repeatable Task Button:
            ImageButton repeatebleButton = view.FindViewById<ImageButton>(Resource.Id.task_base_repeatableButton);
            if(mTask.Repeatable == false) { repeatebleButton.SetImageResource(Resource.Drawable.szczalka_off); }
            else { repeatebleButton.SetImageResource(Resource.Drawable.szczalka_on); }
            view.FindViewById<ImageButton>(Resource.Id.task_base_repeatableButton).Click += (object sender, EventArgs e) =>
            {
                if (mTask.Repeatable == false)
                {
                    mTask.Repeatable = true;
                    repeatebleButton.SetImageResource(Resource.Drawable.szczalka_on);
                    mTask.update_data(); //update record in database
                }
                else
                {
                    mTask.Repeatable = false;
                    repeatebleButton.SetImageResource(Resource.Drawable.szczalka_off);
                    mTask.update_data(); //update record in database
                }
            };

            //Task Deadline:
            taskDeadline = view.FindViewById<TextView>(Resource.Id.task_base_deadline);
            taskDeadline.Text = mTask.Deadline.ToString("dd/MM/yyyy HH:mm");

            //Date Edit Button:
            ImageView editDate = view.FindViewById<ImageView>(Resource.Id.task_base_date_edit_image);
            editDate.Click += (object sender, EventArgs e) =>
            {
                Dialog_DatePicker datePicker = new Dialog_DatePicker();
                datePicker.Show(ChildFragmentManager, "Date");
                datePicker.mOnDatePicked += (object sender, DatePickerDialog.DateSetEventArgs e) =>
                {
                    if (e.Date < DateTime.Now) //if new date is earlier than current date
                    {
                        Toast.MakeText(Context, "Date already passed", ToastLength.Long).Show();//Show a toast informing the user that they cannot change to that date
                    }
                    else
                    {
                        AlertDialog.Builder alertDialog = new AlertDialog.Builder(Context);
                        alertDialog.SetTitle("Warning!");
                        if(e.Date > DateTime.Now.Date) 
                        {
                            //if the user had set a new date to be later than the old one, give a penalty to his reward
                            alertDialog.SetMessage("The chosen date is later than the current date, there will be a penalty in the reward. \nDo you want to change the date?");
                            alertDialog.SetPositiveButton("Yes", delegate { DateChange(true, e.Date); });
                        }
                        else
                        {
                            //otherwise change the date normally
                            alertDialog.SetMessage("Do you want to change the date?");
                            alertDialog.SetPositiveButton("Yes", delegate { DateChange(false, e.Date); });
                        }
                        alertDialog.SetNegativeButton("No", delegate { alertDialog.Dispose(); });
                        alertDialog.Show();
                    }
                };
            };

            //Time Edit Button:
            ImageView editTime = view.FindViewById<ImageView>(Resource.Id.task_base_time_edit_image);
            editTime.Click += (object sender, EventArgs e) =>
            {
                Dialog_TimePicker timePicker = new Dialog_TimePicker();
                timePicker.Show(ChildFragmentManager, "Date");
                timePicker.mOnTimePicked += (object sender, TimePickerDialog.TimeSetEventArgs e) =>
                {
                    if(mTask.Deadline.Date == DateTime.Now.Date) //if the user is setting the time on the same date as the deadline date
                    {
                        if(e.HourOfDay < DateTime.Now.Hour || (e.HourOfDay == DateTime.Now.Hour && e.Minute <= DateTime.Now.Minute)) //if new hour is smaller than the current hour, or (if the new hour and current hour are the same and the new minute is smaller than the current minute)
                        {
                            Toast.MakeText(Context, "Time already passed", ToastLength.Long).Show(); //Show a toast informing the user that they cannot change to that time
                            return; //end the mOnTimePicked function
                        }
                    }
                    //Create the dialog for the user to confirm the date change
                    AlertDialog.Builder alertDialog = new AlertDialog.Builder(Context);
                    alertDialog.SetTitle("Warning!");
                    alertDialog.SetMessage("Do you want to change the time?");
                    alertDialog.SetPositiveButton("Yes", delegate { TimeChange(e.HourOfDay,e.Minute); });
                    alertDialog.SetNegativeButton("No", delegate { alertDialog.Dispose(); });
                    alertDialog.Show();
                };
            };

            return view;
        }
        public void DateChange(bool penalty,DateTime newDate)
        {
            if (penalty) 
            { 
                //Functionality to be added later
                Console.WriteLine("You loose 2000 zł"); 
            }
            mTask.Deadline = newDate + mTask.Deadline.TimeOfDay; //add the new date and old time to create a new time
            mTask.update_data(); //update record in the database
            taskDeadline.Text = mTask.Deadline.ToString("dd/MM/yyyy HH:mm"); //change the date in Task fragemnt
        }
        public void TimeChange(int hours, int minutes)
        {
            TimeSpan newTime = new TimeSpan(hours, minutes,0); //create a new time span equal to the new time
            mTask.Deadline = mTask.Deadline.Date.Add(newTime); //set the new deadline to be the old date set with the time of 00:00 and add a new time to it
            mTask.update_data(); //update record in the database
            taskDeadline.Text = mTask.Deadline.ToString("dd/MM/yyyy HH:mm"); //change the time in Task fragemnt
        }



    }
}