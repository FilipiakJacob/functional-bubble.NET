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
    public class Calendar
    {
        int[] ThirtyDayMonths = new int[] { 4, 6, 9, 11 }; 
        public bool correctDaysNum(int daysNum,int month)
        {

            if((month == 2 && daysNum > 29) ||(ThirtyDayMonths.Contains(month) && daysNum > 30)) { return false; }
            return true;
        }
        public bool correctFebruary(int daysNum,int year)
        {
            if (daysNum == 29)
            {
                if((year / 4 % 1) != 0) { return false; }
                if((year / 100 % 1) == 0 && (year / 400 % 1) != 0) { return false; }
            }
            return true;
        }
        public void DateInPast(int day, int month, int year)
        {

        }
    }
}