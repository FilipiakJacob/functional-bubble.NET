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

        #region UPDATE

        /// <summary>
        /// updates user in database
        /// </summary>
        /// <param name="user"></param>
        public void Update(User user)
        {
            _db.Update(user);
        }

        #endregion

        #region GETTERS

        /// <summary>
        /// Get all users from table, used in CheckTableIfOk()
        /// </summary>
        /// <returns>List of user objects</returns>
        public List<User> GetAll()
        {
            List<User> userTable = _db.Table<User>().ToList();
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

        #endregion

        #region STREAK_RELATED

        /// <summary>
        /// checks if user has a streak 
        /// </summary>
        /// <returns>bool</returns>
        public bool CheckStreak()
        {
            User user = GetUser();

            TimeSpan result = DateTime.Now.Subtract(user.LastCompletedTaskDate);

            //condition for situation when user completes task, but didn't complete task the day before
            if (result.Days < -1)
            {
                ZeroStreak(user);
                return false;
            }
            // condition when user's last completed task isn't with todays date
            else if (result.Days == -1) 
            {
                AddDayStreak(user);
            }
            //if any of upper if statements wasn't triggered it means
            //that user's LastCompletedTaskDate is today and his streak is active

            return true;
        }

        /// <summary>
        /// Makes Streak active, adds to streakCount and changes LastCompletedTaskDate to now
        /// </summary>
        /// <param name="user">user</param>
        public void AddDayStreak(User user)
        {
            user.LastCompletedTaskDate = DateTime.Now;
            user.StreakIsActive = true;
            user.StreakCount++;

            Update(user);
        }

        /// <summary>
        /// Makes streak unactive, zeros StreakCount
        /// </summary>
        /// <param name="user">user</param>
        public void ZeroStreak(User user)
        {
            user.StreakCount = 0;
            user.StreakIsActive = false;

            Update(user);
        } 

        #endregion

        #region REWARD_RELATED

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
            int baseCoin = rnd.Next(1, (daysToDeadline.Days + 1)); // random base variable that has range from 1 to substract (line 104)

            float streakMultiplier = CalculateStreakMultiplier();

            float streakMultipliedPart = streakMultiplier * baseCoin; // calculated var based on streak multiplier
            float priorityMultipliedPart = task.Priority * baseCoin; // calculated var based on priority multiplier

            reward = (int)(streakMultipliedPart + priorityMultipliedPart); // final reward calculation

            return reward;
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

            #region ADD/REMOVE_COINS

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

        #endregion

        #endregion

        #region TASK_VACATION

        /// <summary>
        /// when vacation is toggled on, we add days to deadline of the tasks that would have its deadline gone during vacation
        /// </summary>
        /// <param name="vacDuration">duration of the vacation / number of days user is absent</param>
        public void Vacation(int vacDuration)
        {
            TaskHandler taskHandler = new TaskHandler();

            List<Task> allTasks = taskHandler.GetAllTasks();

            DateTime endOfVacation = DateTime.Now.AddDays(vacDuration);

            var duringVacTasks = allTasks.Where<Task>(t => (t.Deadline.CompareTo(endOfVacation) > 0) );

            foreach (Task task in duringVacTasks)
            {
                task.Deadline.AddDays(vacDuration);
                task.update_data();
            }
        }

        #endregion

        #region DEFAULT_METHODS

        /// <summary>
        /// this method checks if table is up with assummed conditions 
        /// </summary>
        public void CheckTableIfOK()
        {
            List<User> table = this.GetAll();
            if (table.Count == 0) // if table is empty this method adds new user
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
                _db.Delete<User>(i);
            }
        }

        #endregion
    }
}