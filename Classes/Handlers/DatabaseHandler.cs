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
using functional_bubble.NET.Classes;
/*
 * @author Mikolaj Petri
 * 
 * Class that handles all operations on database ( 4 now only Task table operations hihi )
 */

namespace functional_bubble.NET.Classes
{
    public abstract class DatabaseHandler
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
            _db.CreateTable<Priority>();
        }
    }
}