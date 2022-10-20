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
using System.Reflection.Emit;

namespace functional_bubble.NET.Classes
{

    /*
     * @author Mikołaj Petri
     * @date 17.7.2022
     */
    /// <summary>
    /// Class used as template for "Labels" table in database
    /// and for adding new Labels to database ; 
    /// has int id and string Description
    /// </summary>
    [Table("Labels")]
    public class Label
    {
        [PrimaryKey, AutoIncrement, Unique, Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// name of the label
        /// </summary>
        [Column("Description")]
        public string Description { get; set; }

    }
}