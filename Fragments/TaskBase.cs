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

namespace functional_bubble.NET.Fragments
{
    public class TaskBase : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.task_base, container, false);

            int ID = Arguments.GetInt("taskID");//get id of a task passed from ToDo fragment
            TaskHandler db = new TaskHandler();
            Task mTask = db.Get(ID);

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
            TextView deadline = view.FindViewById<TextView>(Resource.Id.task_base_deadline);
            deadline.Click += (object sender, EventArgs e) =>
            {
                //temporary solution! The deadline system is not functioning yet.
                //To be added: a floating window for date change with information and functionality of lowered coin reward when changing the deadline
                Toast.MakeText(Context, "Warning! Changing the deadline will give penalty to tasks reward", ToastLength.Long).Show();
            };

            return view;
        }


    }
}