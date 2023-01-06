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

        #region GET/ADD/UPDATE

        /// <summary>
        /// returns Label object with given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Label object</returns>
        public Label Get(int id)
        {
            Label label = _db.Get<Label>(id);
            return label;
        }

        /// <summary>
        /// Gets all Labels
        /// </summary>
        /// <returns>List of Label objects</returns>
        public List<Label> GetAll()
        {
            List<Label> allLabels = _db.Table<Label>().ToList();
            return allLabels;
        }

        /// <summary>
        /// inserts Label object to Task table
        /// </summary>
        /// <param name="label"></param>
        public void Add(Label label)
        {
            _db.Insert(label);
        }

        /// <summary>
        /// updates label row in Labels table
        /// </summary>
        /// <param name="label"></param>
        public void Update(Label label)
        {
            _db.Update(label);
        }

        #endregion

        #region DELETE

        /// @author Mateusz Staszek
        /// <summary>
        /// Deletes row which is equal to given Label object
        /// </summary>
        /// <param name="label"></param>
        public void Delete(Label label)
        {
            _db.Delete(label);
        }

        /// <summary>
        /// for TESTING purpouses only deletes all record in the database
        /// </summary>
        public void DeleteAll()
        {
            _db.DeleteAll<Label>();
        }

        #endregion

        #region OTHER_METHODS

        /// <summary>
        /// If Label table has zero rows than initials default rows
        /// </summary>
        public void DefaultRows()
        {
            List<Label> LabelTable = GetAll();
            if (LabelTable.Count == 0)
            {
                Label label= new Label() { Description = "Work" };
                _db.Insert(label);
                label.Description = "Personal";
                _db.Insert(label);
                label.Description = "Family";
                _db.Insert(label);
                label.Description = "School";
                _db.Insert(label);
            }
        }
        #endregion
    }
}