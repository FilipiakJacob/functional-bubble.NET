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
    public class ListViewAdapter : BaseAdapter<Task>
    {
        //Class that transforms items from a list of Task instances into rows of ListView in Task UI
        private Context mContext; //Enviorment in which adapter will work
        public List<Task> mItems; //List of all Tasks which will be displayed in Task UI

        public ListViewAdapter(Context context,List<Task> items)
        {
            //Class Constructor which initialises context and Task list of the class
            mItems = items;
            mContext = context;
        }
        public override int Count {
            //Class method which returns number of items inside the mItmes list
            get { return mItems.Count;}
        }

        public override long GetItemId(int position)
        {
            //Unimplemented class method that is supposed to give ID of an item in a given position
            return position;
        }
        public override Task this[int position]
        {
            //Class method that returns an item in a given position
            get { return mItems[position]; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //Main class method that transforms the items in class into rows in Task UI
            View row = convertView; //The ListView where items will be placed
            if (row == null) //If the TextView resource does not exist
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.task_row, null, false); //Create the TextView resource
            }
            TextView task_row_id = row.FindViewById<TextView>(Resource.Id.task_row_title); //Get task_row_title TextView from task_row 
            task_row_id.Text = mItems[position].Title; //Set Text of that task_row_title to be the Title attribute of Task instance

            TextView task_row_task = row.FindViewById<TextView>(Resource.Id.task_row_description); //Get task_row_description TextView from task_row 
            task_row_task.Text = mItems[position].Description; //Set Text of that task_row_description to be the Description attribute of Task instance

            TextView task_row_priority = row.FindViewById<TextView>(Resource.Id.task_row_priority);//Get task_row_priority TextView from task_row
            task_row_priority.Text = mItems[position].Priority; //Set Text of that task_row_priority to be the Priority attribute of Task instance

            return row;
        }
        public void Add(Task newTask)
        {
            //A method for adding new Tasks into the Task list
            mItems.Add(newTask);
        }
    }
}