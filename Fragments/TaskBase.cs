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
            //This is a test only:
            string testText = "The ID of this task is " + ID.ToString();
            view.FindViewById<TextView>(Resource.Id.prop_title).Text = testText; 
            //end of test
            return view;
        }
       
    }
}