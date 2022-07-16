using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace functional_bubble.NET.Classes
{
    public class GestureListener : GestureDetector.SimpleOnGestureListener
    {
        //This is a GestureListener for ListViewAdapter, it stores response methods for gestures preformed on a Task in ToDo list
        int mId;
        View mview;
        public GestureListener(View parentView)
        {
            mview = parentView; //Set the parent of the ListViewAdapter(ToDo Fragment) as one of the class attributes
        }
        public void setId(int id) { mId = id; }

        public override bool OnSingleTapConfirmed(MotionEvent e)
        {
            var bundle = new Bundle(); 
            bundle.PutInt("taskID", mId);
            Navigation.FindNavController(mview).Navigate(Resource.Id.GoToTask, bundle); //Navigate to the Task Fragment
            return base.OnSingleTapConfirmed(e);
        }
        public override void OnLongPress(MotionEvent e)
        {
            Console.WriteLine("LongPress works");
            base.OnLongPress(e);
        }
    }
}