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
        public int Id { get; set; } = 0;
        /// <summary>
        /// var with not any particular usage rn but maybe in the future will be used to idk personalised encouragement messages?
        /// </summary>
        [Column("Username")]
        public string Username { get; set; }
        /// <summary>
        /// variable that is used for checking streak
        /// </summary>
        [Column("LastCompletedTaskDate")]
        public DateTime LastCompletedTaskDate { get; set; }
        /// <summary>
        /// variable that tells if user is on hot streak or not
        /// </summary>
        [Column("Streak")]
        public bool StreakIsActive { get; set; } = false;
        /// <summary>
        /// variable that tells how many days in the row user did a task
        /// </summary>
        [Column("StreakCount")]
        public int StreakCount { get; set; } = 0;
        /// <summary>
        /// variable that shows how much money user has
        /// </summary>
        [Column("Money")]
        public int Money { get; set; } = 0;
        /// <summary>
        /// var that shows all created tasks ever number
        /// </summary>
        [Column("TotalTasks")]
        public int TotalTasks { get; set; } = 0;
        /// <summary>
        /// var that shows all completed tasks number
        /// </summary>
        [Column("CompletedTasks")]
        public int CompletedTasks { get; set;} = 0;
        /// <summary>
        /// var that shows all abandoned tasks number
        /// </summary>
        [Column("AbandonedTasks")]
        public int AbandonedTasks { get; set ; } = 0;
    }
}