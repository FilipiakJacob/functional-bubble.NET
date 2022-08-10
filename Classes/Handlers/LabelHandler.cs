using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace functional_bubble.NET.Classes
{
    public class LabelHandler : DatabaseHandler
    {
        public void Add(Label label) // inserts task object to Task table
        {
            _db.Insert(label);
        }

        public Label Get(int id) // returns task object with given id
        {
            Label label = _db.Get<Label>(id);
            return label;
        }

        public List<Label> GetAll()
        {
            List<Label> allLabels = _db.Query<Label>("SELECT * FROM Labels");
            return allLabels;
        }

        //@author Mateusz Staszek
        public void Delete(Label label)
        {
            _db.Delete(label);
        }

        public void DeleteAll()
        {
            //for testing purpouses only
            //deletes all record in the database
            _db.DeleteAll<Label>();
        }
    }
}