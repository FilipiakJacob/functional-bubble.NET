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
    internal class TaskHandler : DatabaseHandler
    {
        public void Add(Task task) // inserts task object to Task table
        {
            _db.Insert(task);
        }
        public Task Get(int id) // returns task object with given id
        {
            var task = _db.Get<Task>(id);
            return task;
        }
        public List<Task> GetAllTasks() 
        {
            var allTasks = _db.Query<Task>("SELECT * FROM Tasks");
            return allTasks;
        }
    }
}