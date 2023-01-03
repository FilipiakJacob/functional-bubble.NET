using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Google.Android.Material.BottomNavigation;
using System;
using System.Collections.Generic;
using functional_bubble.NET.Classes;
using functional_bubble.NET.Fragments;
using AndroidX.Navigation;
using AndroidX.Navigation.Fragment;
using AndroidX.Navigation.UI;
using Android.App;
using Android.Renderscripts;

namespace functional_bubble.NET

{

    [Android.App.Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private static readonly string CHANNEL_ID = "local_notification"; // channel id of the notification channel, needed for notifications

        //Declare the fragment manager. We're using fragments from AndroidX.Fragment.App
        //rather than the default ones, so the manager is from it too.
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main); //Sets layout visible in the activity as "activity_main".
                                                           //This layout contains the NavHostFragment and Bottom Navigation.
                                                           //Think of it as the Background of the app.

            NavController navController = Navigation.FindNavController(this, Resource.Id.main_nav_host_fragment); //navController manages the swapping of destinations in the NavHostFragment.
            BottomNavigationView bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.main_bottom_nav_view);
            NavigationUI.SetupWithNavController(bottomNavigation, navController); //Connects the bottomNavigation with the NavController. 
            CreateNotificationChannel();

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

        public void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channelName = Resource.String.channel_name.ToString();
            var channelDescription = Resource.String.channel_description.ToString();
            var channel = new NotificationChannel(CHANNEL_ID, channelName, NotificationImportance.Default)
            {
                Description = channelDescription
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

    }

}
