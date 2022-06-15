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

namespace functional_bubble.NET
{
    [Table("Tasks")] //name of the table that stores task objects
    public class Task
    {

        /* 
         * @author Mikołaj Petri
         * 
         * Explanation to the used parameters
         * 
         * PrimaryKey - This attribute can be applied to an integer property to force it to be the underlying table's primary key
         * AutoIncrement - This attribute will cause an integer property's value to be auto-increment for each new object inserted into the database
         * Unique - Ensures that the values in the underlying database column are unique
         * Column( string name ) - The name parameter sets the underlying database column's name
         */

        [PrimaryKey, AutoIncrement, Unique, Column("id")] 
        public int Id { get; private set; }
        [Column("Title")]
        public string Title { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("LabelInt")]
        public int Label { get; set; }
        [Column("CreationTime")]
        public DateTime CreationTime { get; set; }
        [Column("Deadline")]
        public DateTime Deadline { get; set; }
        [Column("CoinsReward")]
        public int CoinsReward { get; set; }
        [Column("Priority")]
        public int Priority { get; set; }
        [Column("Repeatable")]
        public bool Repeatable { get; set; }
        [Column("Pinned")]
        public bool Pinned { get; set; }
        
        public Task()
        {
            gen_Id(); //Sets the Id
            //Container :)
            input_data();
        }
        
        public int input_data()
        ///This method fills in the data from the form and saves it to the database, as well as the task object.
        {
            //Create form
            return 0;
        }
        
        public void gen_Id()
        ///Search the database for a free ID number and return it
        {
            int id = 0; //TODO: Search the database, find a free Id number.
            Id = id; 
        }

        public int delete_data()
        ///This method deletes the task from the database and returns 0 if it was succesfull
        {
            return 0;
        }
    }
}
