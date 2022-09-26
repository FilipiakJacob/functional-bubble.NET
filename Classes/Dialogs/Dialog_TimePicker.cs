using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace functional_bubble.NET.Classes
{

    public class Dialog_TimePicker : AndroidX.Fragment.App.DialogFragment, TimePickerDialog.IOnTimeSetListener
    {

        public EventHandler<TimePickerDialog.TimeSetEventArgs> mOnTimePicked;

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            mOnTimePicked.Invoke(this,new TimePickerDialog.TimeSetEventArgs(hourOfDay, minute));
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currentDate = DateTime.Now;

            TimePickerDialog dialog = new TimePickerDialog(Activity, this, currentDate.Hour,currentDate.Minute,true);
            return dialog;
        }

    }
}