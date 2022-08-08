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
    internal class LabelHandler : DatabaseHandler
    {
        public void Add(Label label) // inserts task object to Task table
        {
            _db.Insert(label);
        }

        public Label Get(int id) // returns task object with given id
        {
            var label = _db.Get<Label>(id);
            return label;
        }

        public List<Label> GetAll()
        {
            var allLabels = _db.Query<Label>("SELECT * FROM Labels");
            return allLabels;
        }
    }
}