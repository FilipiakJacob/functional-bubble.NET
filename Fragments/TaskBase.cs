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

            //Tasks Title and Description:
            EditText title = view.FindViewById<EditText>(Resource.Id.task_base_title);
            title.Text = mTask.Title;
            title.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {
                mTask.Title = title.Text;
                mTask.input_data();
            };

            view.FindViewById<EditText>(Resource.Id.task_base_description).Text = mTask.Description;

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
            view.FindViewById<ImageButton>(Resource.Id.task_base_repeatableButton).Click += (object sender, EventArgs e) =>
            {
                if (mTask.Repeatable == false)
                {
                    mTask.Repeatable = true;
                    repeatebleButton.SetImageResource(Resource.Drawable.szczalka_on);
                }
                else
                {
                    mTask.Repeatable = false;
                    repeatebleButton.SetImageResource(Resource.Drawable.szczalka_off);
                }
            };
            
            return view;
        }

    }
}