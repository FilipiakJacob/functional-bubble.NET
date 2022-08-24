using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace functional_bubble.NET
{

    /*
     * @author Mikołaj Petri
     * @date 10.8.2022
     * 
     * Class used as template for "Priority" table in database
     * and for adding new Priorities to database
     */

    [Table("Priorities")] //name of the table that stores Labels
    public class Priority
    {
        [PrimaryKey, AutoIncrement, Unique, Column("id")]
        public int Id { get; set; }

        [Column("Description")]
        public string Description { get; set; }

    }
}