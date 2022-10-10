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
using Android.Animation;
using AndroidX.Navigation;
using functional_bubble.NET.Classes;

namespace functional_bubble.NET
{
    public class onClickedTask : EventArgs
    {
        //An Event class which when Invoked will pass its data into all methods subscribed to this event
        public Task mClickedTask; //Class argument which is a Task Instance

        public onClickedTask(Task task)
        {
            //Class Constructor which will requires Task Instance as argument and will set the class Task attribute to be that passed instance  
            mClickedTask = task;
        }



    }
    public class onDeleteClicked : EventArgs
    {
        public int mPosition;
        public onDeleteClicked(int position) { mPosition = position; }
    }

    public class ListViewAdapter : BaseAdapter<Task>, View.IOnTouchListener
    {
        //Class that transforms items from a list of Task instances into rows of ListView in Task UI
        private Context mContext; //Enviorment in which adapter will work
        public List<Task> mItems = new List<Task>(); //List of all Tasks which will be displayed in Task UI
        public object mSender;
        public event EventHandler<onNewTaskEventArgs> mClickedTask; //I think this is NOT NEEDED
        public event EventHandler<onDeleteClicked> mDeleteClicked; //An event informing the application that a delete button had been pressed
        private float mLastPosX; //The Last Position of the task row (Propably not needed)
        bool goBack = true; //true if the task row needs to go to its original position, false if not
        public GestureDetector mGestureDetecor; //a class detecting various gestures like double tap or long click
        public GestureListener mGestureListener; //child class of Gesture Detector which handles what to do when a gesture had been made
        int[] task_row_background_color = {Resource.Drawable.task_row_low,Resource.Drawable.task_row_medium, Resource.Drawable.task_row_high, Resource.Drawable.task_row_critical};
        public ListViewAdapter(Context context, List<Task> items, View parentView)
        {
            //Class Constructor which initialises context and Task list of the class
            //Now it also contains parent view in order to switch to Task Fragment
            mItems = items;
            mContext = context;
            mGestureListener = new GestureListener(parentView);
            mGestureDetecor = new GestureDetector(mContext, mGestureListener);
        }
        public override int Count
        {
            //Class method which returns number of items inside the mItmes list
            get { return mItems.Count; }
        }

        public override long GetItemId(int position)
        {
            //Unimplemented class method that is supposed to give ID of an item in a given positionn
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.todo_task_row, null, false); //Create the TextView resource

                Button task_row_delete_button = row.FindViewById<Button>(Resource.Id.task_row_delete_button); //A button for task deletion
                task_row_delete_button.Click += (object sender, EventArgs e) =>
                {
                    //funtion called when delete button had been pressed
                    //!!!!!!!!!Make it instead show a popup window!!!!!!!
                    Delete(position, row); //call Delete class method 
                };
            }
            GridLayout task_row_grid = row.FindViewById<GridLayout>(Resource.Id.task_row_grid);//Get the grid layout from task_row (this is where the task row is displayed in the list)
            task_row_grid.Tag = mItems[position].Id; //set the ID of task grid to be the id of the task
            task_row_grid.SetBackgroundResource(task_row_background_color[mItems[position].Priority]);
            task_row_grid.SetOnTouchListener(this); //Set a listener that will respond when task row had been touched

            TextView task_row_id = row.FindViewById<TextView>(Resource.Id.task_row_title); //Get task_row_title TextView from task_row 
            task_row_id.Text = mItems[position].Title; //Set Text of that task_row_title to be the Title attribute of Task instance

            TextView task_row_task = row.FindViewById<TextView>(Resource.Id.task_row_description); //Get task_row_description TextView from task_row 
            if (mItems[position].Description.Length < 20)
            {
                task_row_task.Text = mItems[position].Description; //Set Text of that task_row_description to be the Description attribute of Task instance
            }
            else
            {
                //Shorten the text in description if it is over 20 characters long
                string shortenedTxt = mItems[position].Description.Substring(0, 17);
                task_row_task.Text = shortenedTxt + "...";
            };

            return row;
        }


        public void Add(Task newTask)
        {
            //A method for adding new Tasks into the Task list
            TaskHandler db = new TaskHandler(); //create a new handler of Task table in the database
            db.Add(newTask); //place the newly created task in the database
            mItems = db.GetSortedTasks(); //update the list so that the added task is sorted in it
            NotifyDataSetChanged();//Refresh Task UI
        }

        public void Delete(int position, View v)
        {
            //A method for deleting Tasks from list
            //Input: a position of a view that we want to delete, and the view itself
            int origHeight = v.Height; //the original height of the v view
            ObjectAnimator objAnim = ObjectAnimator.OfFloat(v, "Alpha", 1, 0); //create an object animation to change the v view opacity from 0 to 1
            ValueAnimator valAnim = ValueAnimator.OfInt(origHeight, 0); //create a value animation to set the height of v view from its original height to 0
            valAnim.Update += delegate
            {
                //method which modifies the v view on every step of animation
                var value = (int)valAnim.AnimatedValue; //each frame, the value will get updated with new heigh value of the v view
                ViewGroup.LayoutParams layoutParams = v.LayoutParameters; //get layout parametes of v view
                layoutParams.Height = value; //set the new height of the v view
                v.LayoutParameters = layoutParams; // set layout paramaters of the v view as the updated parameters
            };
            AnimatorSet animSet = new AnimatorSet(); //create a new animation set
            animSet.PlayTogether(objAnim, valAnim); //set both animation to play at the same time
            animSet.SetDuration(500);
            animSet.Start();
            animSet.AnimationEnd += delegate
            {
                //method which gets called at the end of the animation set
                mItems[position].delete_data();//delete task from database
                mItems.RemoveAt(position);
                //after removing an item from the list, all of its properties are set to the successor of its position, therefore, we have to reset these properties
                v.FindViewById<GridLayout>(Resource.Id.task_row_grid).TranslationX = 0;
                v.LayoutParameters.Height = origHeight;
                v.Alpha = 1;
                NotifyDataSetChanged(); //Refresh Task UI
            };

        }

        public bool OnTouch(View v, MotionEvent e)
        {
            /*
            //Input: a view which in this case is a grid layout for task row.
            //Output: boolean, true if there was a propper response gesture to users' action, false otherwise
            */
            //MOVE IT TO A NEW FIlE
            if (v.TranslationX == 0)
            { //let the user go into the task only if he had not moved the task
                this.mGestureListener.setId(Int32.Parse(v.Tag.ToString()));//Set the ID of Task being touched by taking its tag and converting it first from Java.Lang.Object to string, and then from string to int
                this.mGestureDetecor.OnTouchEvent(e);//Before checking if any motion is being done with the Task, we first check for gestures
            }
            v.Parent.RequestDisallowInterceptTouchEvent(true); //When the user was moving the task, whenever any movement on the Y-axis was made,
                                                               //the motion event was cancelled and Parent View intercepted it, this line stops such behaviour,
                                                               //allowing for Y-movement(which is necessary as the user now does not need perfect X-axis only movement to move the task)
            switch (e.Action)
            {
                case MotionEventActions.Down: //task row was pressed
                    mLastPosX = e.GetX(); //Get X-Position of task row
                    return true;

                case MotionEventActions.Move: //task row is being dragged by the user
                    float CurrentPosition = e.GetX(); //Get X-Position of task row
                    float deltaX = mLastPosX - CurrentPosition; //The difference between beggining position of users finger, minus its current position, it tells us how much we have to move the task row
                    float transX = v.TranslationX; //TranslationX is how much the task row had been moved from its original position on the x-axis
                    transX -= deltaX;
                    if (transX > 0) { transX = 0; } //We do not want the task row to go right, so we do not want the translation to ever be above 0
                    if (transX < -250) { transX = -250; goBack = false; } else { goBack = true; } //The maximum translation to left is 250, we disallow it to go further
                    v.TranslationX = transX; // Set the new translationX
                    return true;

                case MotionEventActions.Up: //task row has stopped being pressed 
                    if (v.TranslationX == 0) { return false; }
                    if (goBack == true)
                    {
                        v.Animate().TranslationXBy(-v.TranslationX).SetDuration(400).Start(); //Move task row to its original position 
                    }
                    return true;
                default:
                    return false;

            }


        }
    }
}