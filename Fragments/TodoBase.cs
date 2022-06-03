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

namespace functional_bubble.NET.Fragments
{
    public class TodoBase : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        private Button mBtnNewTask;
        private List<Task> mItems;
        private ListView mainListView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            /*
            View view = inflater.Inflate(Resource.Layout.todo_base, container, false);
            mainListView = FindViewById<ListView>(Resource.Id.MainView);
            mItems = new List<Task>();

            ListViewAdapter adapter = new ListViewAdapter(this, mItems);
            mainListView.Adapter = adapter;

            mBtnNewTask = FindViewById<Button>(Resource.Id.activity_main_buttonNewTask);
            mBtnNewTask.Click += (object sender, EventArgs e) =>
            {
                //Method for creating DialogFragment from Dialog_NewTask Class
                Dialog_NewTask newTaskDialog = new Dialog_NewTask(); //Create a new Dialog Fragment
                newTaskDialog.Show(SupportFragmentManager, "Dialog"); //Show on screen the Dialog Fragment

                newTaskDialog.mNewTaskComplete += (object sender, onNewTaskEventArgs e) =>
                {
                    //Method executed when onNewTaskEventArgs in Dialog_newTask is Invoked
                    adapter.Add(e.mNewTaskInEvent); //Add Task from onNewTaskEventArgs class as a new list row in Task UI
                    adapter.NotifyDataSetChanged(); //Refresh Task UI

                };
            };
            */

            View view = inflater.Inflate(Resource.Layout.todo_base, container, false);

            return view;
        }
    }
}