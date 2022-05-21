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
    public class ForumRomanymAdapter : BaseAdapter<Task>
    {
        /*Quod spirat tenera malum mordente puella,
        Quod de Corycio quae venit aura croco; 
        Vinea quod primis floret cum cana racemis,
        Gramina quod redolent quae modo carpsit ovis; 
        Quod myrtis, quod messor Arabs, quod sucina trita, 
        Pallidus Eoo ture quod ignis olet; 
        Gleba quod aestivo leviter cum spargitur imbre,
        Quod madidas nardo passa corona comas: 
        Hoc tua, saeve puer Diadumene, basia fragrant.
        Quid si tota dares illa sine invidia?
         */
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