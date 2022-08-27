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
     * @date 17.7.2022
     * 
     * Class used as template for "Labels" table in database
     * and for adding new Labels to database
     */

    [Table("Labels")] //name of the table that stores Labels
    public class Label
    {
        [PrimaryKey, AutoIncrement, Unique, Column("id")]
        public int Id { get; set; }

        [Column("Description")]
        public string Description { get; set; }

    }
}