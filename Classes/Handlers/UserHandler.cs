using Android.App;
using Android.Content;
using Android.Icu.Text;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Emoji2.Text.FlatBuffer;
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
            if (table == null) // if table is empty this method adds new user
            {
                InitialUserTable();
                return;
            }
            if(table.Count > 1) // if table has more then one user it deletes additional users
            {
                DeleteAdditionalUsers(table);
                return;
            }
            return;
        }
        public List<User> GetAll() //method used for checking if user table is ok
        {
            List<User> userTable = _db.Query<User>("SELECT * FROM User");
            return userTable;
        }

        public User GetUser() // method used for all operations on user
        {
            User user = _db.Get<User>(0);
            return user;
        }

        public void Update(User user) //updates user in database
        {
            _db.Update(user);
        }

        public bool CheckStreak() // checks if user has a streak 
        {
            User user = GetUser();

            TimeSpan result = DateTime.Now.Subtract(user.LastCompletedTaskDate);


            if (result.Days == -1) // condition when user's last completed task isn't with todays date
            {
                user.LastCompletedTaskDate = DateTime.Now;
                user.StreakIsActive = true;
                user.StreakCount++;

                Update(user);
                return true;
            }
            //condition for situation when user completes task, but didn't complete task the day before
            else if (result.Days < -1)
            {
                user.StreakCount = 0;
                user.StreakIsActive = false;
                Update(user);
                return false;
            }
            //condition for situation when user completes multiple tasks in one day
            else if (result.Days == 0)
            {
                return true;
            }
            return true;
         
        }

        public void InitialUserTable() //Initial state of the user table
        {
            User user = new User();
            _db.Insert(user);
        }

        public void DeleteAdditionalUsers(List<User> userTable) //deletes all additional users in table
        {
            for (int i = 1; i < userTable.Count; i++)
            {
                _db.Query<User>("DELETE FROM User WHERE id={0}", i);
            }
        }

        public float CalculateReward(Task task)
        {
            float reward;
            float streakMultiplier = CalculateStreakMultiplier();

            TimeSpan substract = DateTime.Now.Subtract(task.Deadline);

            reward = streakMultiplier * substract.Days;
           
            return reward;
        }

        public float CalculateStreakMultiplier()
        {
            User user = GetUser();
            if (CheckStreak())
            {
                if (user.StreakCount <= 5 && user.StreakCount > 0)
                {
                    return 0.155f;
                }
                else if (user.StreakCount > 5 && user.StreakCount <= 10)
                {
                    return 0.255f;
                }
                else if (user.StreakCount > 10 && user.StreakCount <= 20)
                {
                    return 0.255f;
                }
                else if (user.StreakCount > 20)
                {
                    return 0.355f;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}