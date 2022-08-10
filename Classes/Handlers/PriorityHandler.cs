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
using SQLite;
using System.IO;

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

        //@author Mateusz Staszek
        public void Delete(Priority priority)
        {
            _db.Delete(priority);
        }

        //@author Mateusz Staszek
        public void DeleteAll()
        {
            //for testing purpouses only
            //deletes all record in the database
            _db.DeleteAll<Priority>();
        }
    }
}