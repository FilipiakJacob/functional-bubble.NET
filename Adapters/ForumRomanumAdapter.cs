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
    internal class ForumRomanymAdapter : BaseAdapter<Task>
    {
        private Context mContext;
        public Task mTask;

        public ForumRomanymAdapter(Context context, Task task)
        {
            mTask = task;
            mContext = context;
        }
        public override int Count
        {
            get { return 0; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override Task this[int position]
        {
            get { return mTask; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            throw new NotImplementedException();
        }
    }
}