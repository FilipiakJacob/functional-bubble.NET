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
    public class TaskHandler : DatabaseHandler
    {
        public void Add(Task task) // inserts task object to Task table
        {
            _db.Insert(task);
        }
        public Task Get(int id) // returns task object with given id
        {
            Task task = _db.Get<Task>(id);
            return task;
        }

        public void Update(Task task) //updates task in Tasks table
        {
            _db.Update(task);
        }

        public void Update(int taskId, char choice, string change)
        {
            switch (choice)
            {
                case 't':
                    _db.Query<Task>("UPDATE Tasks SET Title={0} WHERE id={1}", change, taskId);
                    break;
                case 'd':
                    _db.Query<Task>("UPDATE Tasks SET Description={0} WHERE id={1}", change, taskId);
                    break;
            }
        }

        public List<Task> GetAllTasks() 
        {
            List<Task> allTasks = _db.Query<Task>("SELECT * FROM Tasks");
            return allTasks;
        }

        //@author Mateusz Staszek
        public void DeleteTask(Task task)
        {
            _db.Delete(task);
        }
        
        //@author Mateusz Staszek
        public void DeleteAll()
        {
            //for testing purpouses only
            //deletes all record in the database
            _db.DeleteAll<Task>();
        }
    }
}