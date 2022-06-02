using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.BottomNavigation;
using System;
using System.Collections.Generic;



namespace functional_bubble.NET
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        TextView textMessage;
        private Button mBtnNewTask;
        private List<Task> mItems;
        private ListView mainListView;

       
        

        //Declare objects to reference the 3 main fragments.
        AndroidX.Fragment.App.Fragment fragmentShop;
        AndroidX.Fragment.App.Fragment fragmentPenguin;
        //Declare the fragment manager. We're using fragments from AndroidX.Fragment.App
        //rather than the default ones, so the manager is from it too.
        AndroidX.Fragment.App.FragmentTransaction fragmentManager;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.shop_base);
            /*
            SetContentView(Resource.Layout.activity_main);

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
            fragmentShop = new ShopBase();
            fragmentPenguin = new PenguinBase();

            fragmentManager = SupportFragmentManager.BeginTransaction();
            fragmentManager.Add(Resource.Id.shop_layout, fragmentShop);
            fragmentManager.Commit();


            textMessage = FindViewById<TextView>(Resource.Id.message);
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
                    textMessage.SetText(Resource.String.title_home);
                    return true;
                case Resource.Id.navigation_dashboard:
                    fragmentManager.Replace(Resource.Id.shop_layout, fragmentShop);
                    fragmentManager.Commit();

                    return true;
                case Resource.Id.navigation_notifications:
                    fragmentManager.Replace(Resource.Id.penguin_layout, fragmentPenguin);
                    fragmentManager.Commit();
                    return true;
            }
            return false;
        }
    }

}
