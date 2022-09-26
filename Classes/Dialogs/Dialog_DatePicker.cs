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


    public class Dialog_DatePicker : AndroidX.Fragment.App.DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        public EventHandler<DatePickerDialog.DateSetEventArgs> mOnDatePicked;

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            mOnDatePicked.Invoke(this, new DatePickerDialog.DateSetEventArgs(year,month,dayOfMonth)); //Invoke the Event Handler
        }
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currentDate = DateTime.Now;

            DatePickerDialog dialog = new DatePickerDialog(Activity,this,currentDate.Year,currentDate.Month - 1,currentDate.Day);
            return dialog;
        }
    }

}
