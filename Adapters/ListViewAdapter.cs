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
    internal class ListViewAdapter : BaseAdapter<Task>
    {
        private Context mContext;
        public List<Task> mItems;

        public ListViewAdapter(Context context,List<Task> items)
        {
            mItems = items;
            mContext = context;
        }
        public override int Count {
            get { return mItems.Count;}
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override Task this[int position]
        {
            get { return mItems[position]; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.task_row, null, false);
            }
            TextView task_row_task = row.FindViewById<TextView>(Resource.Id.task_row_task);
            task_row_task.Text = mItems[position].Description;

            TextView task_row_id = row.FindViewById<TextView>(Resource.Id.task_row_id);
            task_row_id.Text = mItems[position].Id.ToString();

            return row;
        }
    }
}