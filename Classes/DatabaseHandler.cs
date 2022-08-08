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

/*
 * @author Mikolaj Petri
 * 
 * Class that handles all operations on database ( 4 now only Task table operations hihi )
 */

namespace functional_bubble.NET
{
    public class DatabaseHandler
    {
        public readonly SQLiteConnection _db;
        
        public DatabaseHandler() //constructor
        {
            string dbPath = Path.Combine(
        System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
        "ourAppStorage.db3");

            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<Task>();
            _db.CreateTable<Label>();
        }

        public void AddTask(Task task) // inserts task object to Task table
        {
            _db.Insert(task);
        }

        public Task GetTask(int id) // returns task object with given id
        {
            var task = _db.Get<Task>(id);
            return task;
        }


        //Mateusz Changes:
        public void DeleteTask(Task task)
        {
            _db.Delete(task); 
        }
        public List<Task> GetAllTasks()
        {
            List<Task> tasks = new List<Task>();
            tasks = _db.Table<Task>().ToList();
            return tasks;
        }
        public void DeleteAll()
        {
            //for testing purpouses only
            //deletes all record in the database
            _db.DeleteAll<Task>();

        public void AddLabel(Label label) // inserts task object to Task table
        {
            _db.Insert(label);
        }

        public Label GetLabel(int id) // returns task object with given id
        {
            var label = _db.Get<Label>(id);
            return label;

        }
    }
}