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
using SQLite; //you need to add this to use sqlite 
using functional_bubble.NET.Classes;

namespace functional_bubble.NET.Classes
{
    /// <summary>
    /// Task class used as a template to the task table in database ;
    /// </summary>
    [Table("Tasks")] //name of the table that stores task objects
    public class Task
    {

        /* 
         * @author Mikołaj Petri
         * @author Mateusz Staszek
         * 
         * Explanation to the used parameters
         * 
         * PrimaryKey - This attribute can be applied to an integer property to force it to be the underlying table's primary key
         * AutoIncrement - This attribute will cause an integer property's value to be auto-increment for each new object inserted into the database
         * Unique - Ensures that the values in the underlying database column are unique
         * Column( string name ) - The name parameter sets the underlying database column's name
         * 
         * basic documentation - https://docs.microsoft.com/en-us/xamarin/android/data-cloud/data-access/using-sqlite-orm
         */

        [PrimaryKey, AutoIncrement, Unique, Column("id")] 
        public int Id { get; set; }
        [Column("Title")]
        public string Title { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("LabelInt")]
        public int Label { get; set; }
        [Column("CreationTime")]
        public DateTime CreationTime { get; set; } = DateTime.Now;
        [Column("Deadline")]
        public DateTime Deadline { get; set; }
        [Column("CoinsReward")]
        public int CoinsReward { get; set; }
        [Column("Priority")]
        public int Priority { get; set; }
        [Column("Repeatable")]
        public bool Repeatable { get; set; } = false;
        /// <summary>
        /// if task is repeatable then this is int that tell on how it is repeatable
        /// </summary>
        [Column("RepeatEveryNDays")]
        public int RepeatEveryNDays { get; set; }
        /// <summary>
        /// if task is repeatable then this is bool that if task was done today
        /// </summary>
        [Column("DoneToday")]
        public bool DoneToday { get; set; } = false;
        /// <summary>
        /// if task is repeatable then this is dateTime that tell what is the last date that this was last completed
        /// </summary>
        [Column("LastCompleted")]
        public DateTime LastCompleted { get; set; }
        /// <summary>
        /// Pinned to the top of the todo list
        /// </summary>
        [Column("Pinned")]
        public bool Pinned { get; set; }
        
        

        public int update_data()
        {
            //This method will update the Task in the database when called
            TaskHandler db = new TaskHandler();
            db.Update(this);
            return 0;
        }


        public int delete_data()
        ///This method deletes the task from the database and returns 0 if it was succesfull
        {
            TaskHandler db = new TaskHandler();
            db.DeleteTask(this);
            return 0;
        }

        /// <summary>
        /// this method checks if this.Deadline is before compared to Datetime.now (present time) 
        /// and if yes returns true which means task is expired 
        /// </summary>
        /// <returns>bool expired</returns>
        public bool CheckIfDeadlineExpired() 
        {
            return (this.Deadline < DateTime.Now);  
        }
    }
}
