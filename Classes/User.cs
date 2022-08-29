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
    [Table("User")]
    public class User
    {
        [PrimaryKey, Unique, Column("id")]
        public int Id { get; set; }
        [Column("Username")]
        public string Username { get; set; }
        [Column("LastCompletedTaskDate")]
        public DateTime LastCompletedTaskDate { get; set; }
        [Column("Streak")]
        public bool StreakIsActive { get; set; }
        [Column("Money")]
        public int Money { get; set; }
    }
}