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
using Android.Views.Animations;

namespace functional_bubble.NET.Animations
{
    internal class TaskAnimation : Animation
    {
        //Possibly useless class, but it will be kept for now. The code inside of it is not used and can safetly be deleted
        private View mView;
        private float mOriginalPosition;
        private float mMoveBy;

        public TaskAnimation(View view,float originalSize)
        {
            mView = view;
            mOriginalPosition = originalSize;
            mMoveBy = -originalSize;

        }
        protected override void ApplyTransformation(float interpolatedTime, Transformation t)
        {
            mView.TranslationX = mOriginalPosition + (mMoveBy * interpolatedTime);
        }

        public override bool WillChangeBounds()
        {
            return true;
        }
    }
}