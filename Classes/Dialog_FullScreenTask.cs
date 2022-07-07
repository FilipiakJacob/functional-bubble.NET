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
    class Dialog_FullScreenTask : AndroidX.Fragment.App.DialogFragment
    {
        /// <summary>
        /// Started a class that will display a fullscreen task, yet to be finished
        /// </summary>
        private TextView mPropTitle;

        /* test code
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.dialog_new_task, container, false);

            mPropTitle = view.FindViewById<TextView>(Resource.Id.prop_title);



        }
        */
    }
}