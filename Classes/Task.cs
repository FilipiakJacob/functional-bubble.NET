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

namespace functional_bubble.NET
{
    public class Task
    {
        public int Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Label { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime Deadline { get; set; }
        public int CoinsReward { get; set; }
        public string Priority { get; set; }
        public bool Repeatable { get; set; }
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
