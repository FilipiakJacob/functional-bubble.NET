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

namespace functional_bubble.NET.Classes
{

    /*
     * @author Mikołaj Petri
     * 
     * class that is template for the table User in database
     */

    [Table("User")]
    public class User
    {
        [PrimaryKey, Unique, Column("id")]
        public int Id { get; set; }
        [Column("Username")]
        public string Username { get; set; } // var with not any particular usage rn but maybe in the future will be used to idk personalised encouragement messages?
        [Column("LastCompletedTaskDate")]
        public DateTime LastCompletedTaskDate { get; set; } // variable that is used for checking streak
        [Column("Streak")]
        public bool StreakIsActive { get; set; } = false; // variable that tells if user is on hot streak or not
        [Column("StreakCount")]
        public int StreakCount { get; set; } // variable that tells how many days in the row user did a task
        [Column("Money")]
        public int Money { get; set; } // variable that shows how much money user has
    }
}