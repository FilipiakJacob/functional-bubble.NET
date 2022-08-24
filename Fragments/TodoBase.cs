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
using AndroidX.Navigation;
using functional_bubble.NET.Classes;


namespace functional_bubble.NET.Fragments
{
    public class TodoBase : Fragment
    {
        private Button mBtnNewTask;
        private Button mTestButton;
        private List<Task> mItems;
        private ListView mainListView;
        private TaskHandler mdatabaseHandler;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //Inflating view pretty much creates it in memory, without showing it on screen.
            View view = inflater.Inflate(Resource.Layout.todo_base, container, false);
            return view;
        }

        
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        // OnViewCreated is called after OnCreateView and can access the inflated View to findById.
        {
            mainListView = view.FindViewById<ListView>(Resource.Id.MainView);
            mdatabaseHandler = new TaskHandler();
            mItems = mdatabaseHandler.GetAllTasks();


            ListViewAdapter adapter = new ListViewAdapter(Android.App.Application.Context, mItems, view);
            mainListView.Adapter = adapter;

            mBtnNewTask = view.FindViewById<Button>(Resource.Id.activity_main_buttonNewTask);
            mBtnNewTask.Click += (object sender, EventArgs e) =>
            {
                //Method for creating DialogFragment from Dialog_NewTask Class
                Dialog_NewTask newTaskDialog = new Dialog_NewTask(); //Create a new Dialog Fragment
                newTaskDialog.Show(ChildFragmentManager, "Dialog"); //Show on screen the Dialog Fragment
                newTaskDialog.mNewTaskComplete += (object sender, onNewTaskEventArgs e) =>
                {
                    //Method executed when onNewTaskEventArgs in Dialog_newTask is Invoked
                    adapter.Add(e.mNewTaskInEvent); //Add Task from onNewTaskEventArgs class as a new list row in Task UI 
                    
                };
            };
            
            adapter.mDeleteClicked += (object sender, onDeleteClicked e) =>
            {
                //This function will be needed to show comfirmation window before task deletion. Yet to be developed
                    
            };

        }
    }
}