using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
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
            //a method which checks whether a number of days is correct based on the provided year
            if (daysNum == 29)
            {
                if((year / 4 % 1) != 0) { return false; }
                if((year / 100 % 1) == 0 && (year / 400 % 1) != 0) { return false; } //if the year is dividable by 100 but not by 400, then it is not a leap year
            }
            return true;
        }
        public string normalToAmericanDate(string normal)
        {
            //a method that changes the date notation from dd/mm/yyyy to mm/dd/yyyy
            string american = normal.Substring(3, 2) + "/" + normal.Substring(0, 2) + "/" + normal.Substring(6, 4);
            return american;
        }
        public string twoDigitDate(int date)
        {
            //if a date provided is lesser than 10, then a zero is added in front of it eg. 7 -> 07
            if(0 < date && date < 10) { return "0" + date.ToString(); }
            return date.ToString();
        }
        public string dateChange(string date, TextChangedEventArgs e, EditText dateText, TextView badDateText)
        {
            //this method is called every time the user makes a change in the date and will provide an auto fill of "/" sign, and auto deletion of it when needed
            //it will also check for errors in date
            string americanDate = "";
            int dayNum = 0;
            int monthNum = 0;
            bool properDate = true;

            //Firts if and else if statements are checking if the user had pressed the backspace
            if (e.AfterCount != 0) //if user did not pressed backspace
            {
                if ((e.Text.Count() == 2 && !date.Contains('/')) || (e.Text.Count() == 5 && date[2] == '/')) //add a "/" sign when user fully provided days or months in a date
                {
                    dateText.Text += "/";
                    dateText.SetSelection(dateText.Text.Length);  //move selection to the end of the text
                }
                else if (e.Text.Count() == 10) //after the user typed in full date, close the keyboard
                {
                    dateText.Enabled = false; //this line closes the keyboard
                    dateText.Enabled = true;  //this line makes it so that the user can still interact with datetext to edit it
                }
                else if (e.Text.Count() > 10) //if the user tries to add more than 10 characters
                {
                    dateText.Text = dateText.Text.Substring(0, 10);
                }
            }
            else if (e.Text.Count() == 2 || e.Text.Count() == 5) //if backspace deleted 3rd or 6th character(the "/" character), delete one additional character
            { 
                dateText.Text = dateText.Text.Substring(0, dateText.Text.Length - 1);
                dateText.SetSelection(dateText.Text.Length);
            }

            //Regardless of the previous 2 statements all the following statements will be checked:
            if (date.Length > 1)
            { 
                Int32.TryParse(date.Substring(0, 2), out int j); 
                if (1 > j || j > 31) 
                { 
                    badDateText.Text = "Wrong Day";
                    properDate = false; 
                } 
                else 
                {
                    dayNum = j; 
                } 
            }
            if (date.Length > 4) 
            { 
                Int32.TryParse(date.Substring(3, 2), out int k); if (1 > k || k > 12) 
                {
                    badDateText.Text = "Wrong Month";
                    properDate = false; 
                } 
                else if (!correctDaysNum(dayNum, k)) 
                { 
                    badDateText.Text = "Wrong Day";
                    properDate = false; 
                } 
                else 
                {
                    monthNum = k; 
                } 
            }
            if (date.Length == 10) 
            { 
                Int32.TryParse(date.Substring(6, 4), out int l); 
                if (monthNum == 2) 
                {
                    if (!correctFebruary(dayNum, l)) 
                    { 
                        badDateText.Text = "Wrong February";
                        properDate = false; 
                    } 
                } 
                if (properDate)
                {
                    americanDate = normalToAmericanDate(date);
                }
            }
            if (properDate) 
            {
                badDateText.Text = ""; //if an issue with date was resolved, delete the error message
            }
            return americanDate;
        }
        public void timeChange(string time, TextChangedEventArgs e, EditText timeText, TextView badDateText)
        {
            bool properTime = true;
            if (e.AfterCount != 0) //if user did not pressed the backspace
            {
                if (e.Text.Count() == 2 && !time.Contains(':')) //if the user fully typed in number of hours
                { 
                    timeText.Text += ":"; 
                    timeText.SetSelection(timeText.Text.Length); 
                }
                else if (e.Text.Count() == 5) //after the user typed in full time, close the keyboard
                { 
                    timeText.Enabled = false; //this line closes the keyboard
                    timeText.Enabled = true;  //this line makes it so that the user can still interact with datetext to edit it
                }
                else if (e.Text.Count() > 5) //if the user tries to add more characters than 5
                { 
                    timeText.Text = timeText.Text.Substring(0, 5); 
                }
            }
            else if (e.Text.Count() == 2) //if the backspace deleted the 3rd character(the ":" sign)
            {
                timeText.Text = timeText.Text.Substring(0, timeText.Text.Length - 1); //delete one additional character
                timeText.SetSelection(timeText.Text.Length); 
            }


            if (time.Length > 1) 
            { 
                Int32.TryParse(time.Substring(0, 2), out int j); 
                if (0 > j || j > 23) 
                { 
                    badDateText.Text = "Wrong Hours"; properTime = false; 
                } 
            };
            if (time.Length == 5) 
            { 
                Int32.TryParse(time.Substring(3, 2), out int k); 
                if (0 > k || k > 59) 
                { 
                    badDateText.Text = "Wrong Minutes"; 
                    properTime = false; 
                } 
            }
            if (properTime) 
            { 
                badDateText.Text = ""; //if an issue with time was resolved, delete the error message
            }
        }
    }
}