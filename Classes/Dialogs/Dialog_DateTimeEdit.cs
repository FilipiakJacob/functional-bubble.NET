using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace functional_bubble.NET.Classes.Dialogs
{
    public class onDateChangedEventArgs : EventArgs
    {
        //An Event class which when Invoked will pass its data into all methods subscribed to this event
        public DateTime mDateTimeChangedInEvent; //Class argument which is a Task Instance

        public onDateChangedEventArgs(DateTime newDate)
        {
            //Class Constructor which will requires Task Instance as argument and will set the class Task attribute to be that passed instance  
            mDateTimeChangedInEvent = newDate;
        }


    }
    public class Dialog_DateTimeEdit : AndroidX.Fragment.App.DialogFragment
    {
        //a class containing a dialogFragment which lets the user change the date
        public event EventHandler<onDateChangedEventArgs> mDateTimeChanged;

        public Dialog_DateTimeEdit(DateTime unchagedTime)
        {

        }
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            LayoutInflater inflater = Activity.LayoutInflater;

            View view = inflater.Inflate(Resource.Layout.date_and_time_layout, null);
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity, Resource.Style.WrapContentDialog);
            if (view != null)
            {
                builder.SetView(view);
            }
            AlertDialog dialog = builder.Create();
            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Transparent));
            dialog.Window.SetDimAmount(0);


            return dialog;
        }
    }
}