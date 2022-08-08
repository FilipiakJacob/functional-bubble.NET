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
    public class ShopBase : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Android.Media.MediaPlayer mPlayer; //Sound player
            View view = inflater.Inflate(Resource.Layout.shop_base, container, false);

            Button quackButton = view.FindViewById<Button>(Resource.Id.QuackButton);//the button is on the penguin and is invisible
            quackButton.SoundEffectsEnabled = false; //disable deafult button clicking sound
            quackButton.Click += (object sender, EventArgs e) =>
            {
                mPlayer = Android.Media.MediaPlayer.Create(Android.App.Application.Context, Resource.Raw.easter_egg);//get the sound to play
                mPlayer.Start();
            };
            return view;
        }
    }
}