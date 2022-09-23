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
        /// <summary>
        /// LabelHandler calls DatabaseHandler and DefaultRows() which make sure that Label table has all basic rows
        /// </summary>
        public LabelHandler()
        {
            DefaultRows();
        }

        /// <summary>
        /// inserts Label object to Task table
        /// </summary>
        /// <param name="label"></param>
        public void Add(Label label)
        {
            _db.Insert(label);
        }

        public Label Get(int id) // returns task object with given id
        {
            Label label = _db.Get<Label>(id);
            return label;
        }

        public void Update(Label label) // updates label row in Labels table
        {
            _db.Update(label);
        }

        public List<Label> GetAll()
        {
            List<Label> allLabels = _db.Query<Label>("SELECT * FROM Labels");
            return allLabels;
        }

        public void DefaultRows()
        {
            List<Label> LabelTable = GetAll();
            if (LabelTable.Count == 0)
            {
                Label label= new Label();
                label.Description = "Work";
                _db.Insert(label);
                label.Description = "Personal";
                _db.Insert(label);
                label.Description = "Family";
                _db.Insert(label);
                label.Description = "School";
                _db.Insert(label);
            }
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