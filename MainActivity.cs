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
using AndroidX.Navigation;
using AndroidX.Navigation.Fragment;
using AndroidX.Navigation.UI;


namespace functional_bubble.NET

{
    [Android.App.Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        //Declare the fragment manager. We're using fragments from AndroidX.Fragment.App
        //rather than the default ones, so the manager is from it too.
        //FragmentTransaction fragmentManager;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            NavController navController = Navigation.FindNavController(this, Resource.Id.main_nav_host_fragment);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            NavigationUI.SetupWithNavController(navigation, navController);
        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            //save current state of the application so that it can be restored when the app returns from the background
            base.OnSaveInstanceState(outState);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        /*
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    //fragmentManager = SupportFragmentManager.BeginTransaction();
                    //fragmentManager.Replace(Resource.Id.replacableContainer, new TodoBase()); //There already is a fragment in the container. Replace() forces all
                                                                                              //fragments in the container to destroy themselves and adds a new fragment.
                    //fragmentManager.Commit();

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
        */
        public void OnDestinationChanged(NavController controller, NavDestination destination, Bundle arguments)
        {
            NavController navController = (NavController)controller;
        }
    }

}
