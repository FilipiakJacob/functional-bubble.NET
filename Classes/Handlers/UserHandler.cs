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
    public class UserHandler : DatabaseHandler
    {
        public UserHandler() // constructor
        {
            CheckTableIfOK();
        }
        public void CheckTableIfOK() // this method checks if table is 
        {
            List<User> table = this.GetAll();
            if (table == null)
            {
                User user = new User();
                _db.Insert(user);
                return;
            }
            if(table.Count > 1)
            {
                for (int i = 1; i < table.Count; i++) {
                    _db.Query<User>("DELETE FROM User WHERE id={0}", i);
                }
                return;
            }
            return;
        }

        public List<User> GetAll()
        {
            List<User> userTable = _db.Query<User>("SELECT * FROM User");
            return userTable;
        }
    }
}