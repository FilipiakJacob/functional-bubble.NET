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
    public class StatisticsHandler : UserHandler
    {

        #region ADD_STATISTICS
        /// <summary>
        /// +1 to total tasks statistic
        /// </summary>
        public void AddOneTotalTask()
        {
            User user = GetUser();

            user.TotalTasks += 1;

            Update(user);
        }

        /// <summary>
        /// +1 to total completed tasks statistic
        /// </summary>
        public void AddOneCompletedTask()
        {
            User user = GetUser();

            user.CompletedTasks += 1;

            Update(user);
        }

        /// <summary>
        /// +1 to total tasks abandoned statistic
        /// </summary>
        public void AddOneAbandonedTask()
        {
            User user = GetUser();

            user.AbandonedTasks += 1;

            Update(user);
        }

        /// <summary>
        /// Get exact number of total tasks statistic
        /// </summary>
        /// <returns>int</returns>
        #endregion

        #region GET_STATISTICS
        public int GetTotalTask()
        {
            User user = GetUser();

            return user.TotalTasks;
        }

        /// <summary>
        /// Get exact number of total completed tasks statistic
        /// </summary>
        /// <returns>int</returns>
        public int GetCompletedTask()
        {
            User user = GetUser();

            return user.CompletedTasks;
        }

        /// <summary>
        /// Get exact number of total abandoned tasks statistic
        /// </summary>
        /// <returns>int</returns>
        public int GetAbandonedTask()
        {
            User user = GetUser();

            return user.AbandonedTasks;
        }
        #endregion
    }
}