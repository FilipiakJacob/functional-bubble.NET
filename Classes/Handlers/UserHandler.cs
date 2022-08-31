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
    public class UserHandler : DatabaseHandler
    {
        public UserHandler() // constructor
        {
            CheckTableIfOK();
        }
        public void CheckTableIfOK() // this method checks if table is up with assummed conditions 
        {
            List<User> table = this.GetAll();
            if (table == null)
            {
                User user = new User();
                _db.Insert(user);
                return;
            }
            if(table.Count > 1)
            {
                for (int i = 1; i < table.Count; i++) {
                    _db.Query<User>("DELETE FROM User WHERE id={0}", i);
                }
                return;
            }
            return;
        }

        public List<User> GetAll()
        {
            List<User> userTable = _db.Query<User>("SELECT * FROM User");
            return userTable;
        }

        public User GetUser()
        {
            User user = _db.Get<User>(0);
            return user;
        }

        public void CheckStreak() // checks if user has a streak 
        {
            User user = GetUser();
            //condition for situation when user completes task day after day
            if (user.LastCompletedTaskDate.Day == (DateTime.Now.Day - 1) &&
                user.LastCompletedTaskDate.Month == DateTime.Now.Month &&
                user.LastCompletedTaskDate.Year == DateTime.Now.Year) 
            {
                user.LastCompletedTaskDate = DateTime.Now;
                user.StreakIsActive = true;
                user.StreakCount++;

                return;
            }
            //condition for situation when user completes multiple tasks in one day
            else if (user.LastCompletedTaskDate.Day == DateTime.Now.Day &&
                user.LastCompletedTaskDate.Month == DateTime.Now.Month &&
                user.LastCompletedTaskDate.Year == DateTime.Now.Year)
            {
                return;
            }
            //condition for situation when user completes task, but didn't complete task the day before
            else
            {
                user.StreakCount = 0;
                user.StreakIsActive= false;
                return;
            }
        }
    }
}