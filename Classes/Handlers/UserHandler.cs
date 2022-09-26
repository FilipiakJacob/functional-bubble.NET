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
        /// <summary>
        /// Constructor, calls CheckTableIfOK()
        /// </summary>
        public UserHandler()
        {
            CheckTableIfOK();
        }

        /// <summary>
        /// this method checks if table is up with assummed conditions 
        /// </summary>
        public void CheckTableIfOK()
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

        /// <summary>
        /// Get all users from table, used in CheckTableIfOk()
        /// </summary>
        /// <returns>List of user objects</returns>
        public List<User> GetAll()
        {
            List<User> userTable = _db.Query<User>("SELECT * FROM User");
            return userTable;
        }

        /// <summary>
        /// method used for all operations on user
        /// </summary>
        /// <returns>user</returns>
        public User GetUser()
        {
            User user = _db.Get<User>(0);
            return user;
        }

        /// <summary>
        /// updates user in database
        /// </summary>
        /// <param name="user"></param>
        public void Update(User user)
        {
            _db.Update(user);
        }

        /// <summary>
        /// checks if user has a streak 
        /// </summary>
        /// <returns>bool</returns>
        public bool CheckStreak()
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

        /// <summary>
        /// Initial state of the user table
        /// </summary>
        public void InitialUserTable()
        {
            User user = new User();
            _db.Insert(user);
        }

        /// <summary>
        /// deletes all additional users in table
        /// </summary>
        /// <param name="userTable"></param>
        public void DeleteAdditionalUsers(List<User> userTable) 
        {
            for (int i = 1; i < userTable.Count; i++)
            {
                _db.Query<User>("DELETE FROM User WHERE id={0}", i);
            }
        }

        /// <summary>
        /// calculates penalty based on reward and casts RemoveCoins method with penalty param
        /// </summary>
        /// <param name="reward"></param>
        public void Penalty(int reward) 
        {
            int penalty = (int)((-1 * reward) * 0.15f);

            RemoveCoins(penalty);
        }

        /// <summary>
        /// calculates reward based on various variables
        /// </summary>
        /// <param name="task"></param>
        /// <returns>int reward</returns>
        public int CalculateReward(Task task)
        {
            int reward;
            TimeSpan daysToDeadline = task.Deadline.Subtract(DateTime.Now); // days to deadline 
            Random rnd = new Random(); // random that will generate base variable that will be multiplied by multipliers
            int baseCoin = rnd.Next(1, daysToDeadline.Days); // random base variable that has range from 1 to substract (line 104)

            float streakMultiplier = CalculateStreakMultiplier(); 

            float streakMultipliedPart = streakMultiplier * baseCoin; // calculated var based on streak multiplier
            float priorityMultipliedPart = task.Priority * baseCoin; // calculated var based on priority multiplier

            reward = (int)(streakMultipliedPart + priorityMultipliedPart); // final reward calculation

            return reward;
        }

        /// <summary>
        /// adding reward coins to user
        /// </summary>
        /// <param name="task"></param>
        public void AddRewardCoins(Task task)
        {
            User user = GetUser();

            user.Money += task.CoinsReward; //adding reward to user's account 

            Update(user); //update user in database 
        }

        /// <summary>
        /// removing coins from user's account
        /// </summary>
        /// <param name="coinsRemove"></param>
        public void RemoveCoins(int coinsRemove)
        {
            User user = GetUser();

            user.Money -= coinsRemove; //removing reward from user's account 

            Update(user); //update user in database 
        }

        /// <summary>
        /// Calculates Streak Multiplier based on streak's duration
        /// </summary>
        /// <returns>flaot streakMultiplier</returns>
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