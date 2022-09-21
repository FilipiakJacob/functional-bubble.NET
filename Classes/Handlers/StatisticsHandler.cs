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
        public void AddOneTotalTask()
        {
            User user = GetUser();

            user.TotalTasks += 1;

            Update(user);
        }

        public void AddOneCompletedTask()
        {
            User user = GetUser();

            user.CompletedTasks += 1;

            Update(user);
        }

        public void AddOneAbandonedTask()
        {
            User user = GetUser();

            user.AbandonedTasks += 1;

            Update(user);
        }

        public int GetTotalTask()
        {
            User user = GetUser();

            return user.TotalTasks;
        }
        public int GetCompletedTask()
        {
            User user = GetUser();

            return user.CompletedTasks;
        }
        public int GetAbandonedTask()
        {
            User user = GetUser();

            return user.AbandonedTasks;
        }
    }
}