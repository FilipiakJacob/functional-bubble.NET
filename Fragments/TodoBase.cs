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
using static AndroidX.RecyclerView.Widget.RecyclerView;


namespace functional_bubble.NET.Fragments
{
    public class TodoBase : Fragment
    {
        private Button mBtnNewTask;
        private Button mTestButton;
        private List<Task> mItems;
        private ListView mainListView;
        private TaskHandler mdatabaseHandler;
        private int openDialog;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            openDialog = Arguments.GetInt("openDialog"); //Reading the bundle in OnCreate means it is only read once, when the fragment is first created.

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
            if (openDialog == 1) //The value will be 1 when the fragment is openend by the WidgetTodo new task button.
            {
                open_dialog(this, adapter);
                openDialog=0; //Set the value to 0 afterwards so the dialog does not reopen whenever the fragment is opened.
            }

            mBtnNewTask = view.FindViewById<Button>(Resource.Id.activity_main_buttonNewTask);
            mBtnNewTask.Click += (Object sender, EventArgs e) =>
            {
                open_dialog(sender,adapter);
            };

            
            adapter.mDeleteClicked += (object sender, onDeleteClicked e) =>
            {
                //This function will be needed to show comfirmation window before task deletion. Yet to be developed
                    
            };

        }

        private void open_dialog(object sender, ListViewAdapter adapter)
        {
            //Method for creating DialogFragment from Dialog_NewTask Class
            Dialog_NewTask newTaskDialog = new Dialog_NewTask(); //Create a new Dialog Fragment
            newTaskDialog.Show(ChildFragmentManager, "Dialog"); //Show on screen the Dialog Fragment
            newTaskDialog.mNewTaskComplete += (object sender, onNewTaskEventArgs e) =>
            {
                //Method executed when onNewTaskEventArgs in Dialog_newTask is Invoked
                adapter.Add(e.mNewTaskInEvent); //Add Task from onNewTaskEventArgs class as a new list row in Task UI 

            };
        }


    }
}