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
    public class PriorityHandler : DatabaseHandler
    {
        public void Add(Priority priority) // inserts task object to Task table
        {
            _db.Insert(priority);
        }

        public Priority Get(int id) // returns task object with given id
        {
            Priority priority = _db.Get<Priority>(id);
            return priority;
        }

        public List<Priority> GetAll()
        {
            List<Priority> allPriorities = _db.Query<Priority>("SELECT * FROM Priorities");
            return allPriorities;
        }
    }
}