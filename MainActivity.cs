using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Google.Android.Material.BottomNavigation;
using System;
using System.Collections.Generic;
using functional_bubble.NET.Fragments;


namespace functional_bubble.NET

{
    [Android.App.Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener //AppCompatActivity extends FragmentActivity, which we need for fragments
    {

        //Declare the fragment manager. We're using fragments from AndroidX.Fragment.App
        //rather than the default ones, so the manager is from it too.
        FragmentTransaction fragmentManager;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            /*
            mainListView = FindViewById<ListView>(Resource.Id.MainView);
            mItems = new List<Task>();

            ListViewAdapter adapter = new ListViewAdapter(this, mItems);
            mainListView.Adapter = adapter;

            mBtnNewTask = FindViewById<Button>(Resource.Id.activity_main_buttonNewTask);
            mBtnNewTask.Click += (object sender, EventArgs e) =>
            {
                //Method for creating DialogFragment from Dialog_NewTask Class
                Dialog_NewTask newTaskDialog = new Dialog_NewTask(); //Create a new Dialog Fragment
                newTaskDialog.Show(SupportFragmentManager,"Dialog"); //Show on screen the Dialog Fragment

                newTaskDialog.mNewTaskComplete += (object sender, onNewTaskEventArgs e) =>
                {
                    //Method executed when onNewTaskEventArgs in Dialog_newTask is Invoked
                    adapter.Add(e.mNewTaskInEvent); //Add Task from onNewTaskEventArgs class as a new list row in Task UI
                    adapter.NotifyDataSetChanged(); //Refresh Task UI

                };
            };
            */

            //Fragment manager is used to dynamically replace fragments that are currently part of the activity.
            fragmentManager = SupportFragmentManager.BeginTransaction();        //Calling this method returns an instance of FragmentTransaction
            fragmentManager.Add(Resource.Id.replacableContainer, new TodoBase());  //Create a new instance of TodoBase (our default screen when app is opened) and
                                                                                //add a fragment to the container (replacableLayout)
            fragmentManager.Commit();

            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    fragmentManager = SupportFragmentManager.BeginTransaction();
                    fragmentManager.Replace(Resource.Id.replacableContainer, new TodoBase()); //There already is a fragment in the container. Replace() forces all
                                                                                              //fragments in the container to destroy themselves and adds a new fragment.
                    fragmentManager.Commit();
                    return true;

                case Resource.Id.navigation_dashboard:
                    fragmentManager = SupportFragmentManager.BeginTransaction();
                    fragmentManager.Replace(Resource.Id.replacableContainer, new ShopBase());
                    fragmentManager.Commit();
                    return true;

                case Resource.Id.navigation_notifications:
                    fragmentManager = SupportFragmentManager.BeginTransaction();
                    fragmentManager.Replace(Resource.Id.replacableContainer, new PenguinBase());
                    fragmentManager.Commit();
                    return true;
            }
            return false;
        }
    }

}
