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
using static Android.Provider.UserDictionary;


namespace functional_bubble.NET.Classes
{
    public class TaskHandler : DatabaseHandler
    {

        #region ADD/UPDATE
        /// <summary>
        /// inserts task object to Task table
        /// </summary>
        /// <param name="task"></param>
        public void Add(Task task) 
        {
            UserHandler userHandler = new UserHandler();

            task.CoinsReward = userHandler.CalculateReward(task);

            _db.Insert(task);    
        }

        /// <summary>
        /// updates task in Tasks table
        /// </summary>
        /// <param name="task"></param>
        public void Update(Task task)
        {
            _db.Update(task);
        }

        /// <summary>
        /// updates task's Title (choice = t) or Description (choice = d) in Tasks table with change string
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="choice"></param>
        /// <param name="change"></param>
        public void Update(int taskId, char choice, string change)
        {
            switch (choice)
            {
                case 't':
                    _db.Query<Task>("UPDATE Tasks SET Title=? WHERE id=?", change, taskId);
                    break;
                case 'd':
                    _db.Query<Task>("UPDATE Tasks SET Description=? WHERE id=?", change, taskId);
                    break;
            }
        }
        #endregion

        #region GETTERS
        /// <summary>
        /// returns task object with given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task object</returns>
        public Task Get(int id)
        {
            Task task = _db.Get<Task>(id);
            return task;
        }

        /// <summary>
        /// Get all Tasks from table
        /// </summary>
        /// <returns>List of tasks object</returns>
        public List<Task> GetAllTasks()
        {
            CheckAndAdaptRepeatableTasks();

            List<Task> allTasks = _db.Table<Task>().ToList();

            return allTasks;
        }

        /// <summary>
        /// sorting tasks by priorities and deadline
        /// </summary>
        /// <returns>List<Task> sortedTasks</returns>
        public List<Task> GetSortedTasks()
        {
            List<Task> allTasksList = GetAllTasks();

            //LINQ expression
            var sortedTasks = allTasksList. //takes allTasks
                OrderByDescending(t => t.Priority). //order them by priorities (descending bc highest priority is 3)
                ThenBy(t => t.Deadline).ToList(); //order them by deadline descending

            return sortedTasks;
        }

        public IEnumerable<Task> GetFilteredTasks(string filter,bool ascend)
        {

            if (!ascend)
            {
                return _db.Query<Task>("SELECT * ORDER BY ? DESC", filter);
            }

            return _db.Query<Task>("SELECT * ORDER BY ?", filter); 
        }

        /// <summary>
        /// Method which returns all tasks that end between 1 and 2 hours from the moment it is triggered.
        /// </summary>
        /// <returns> List<Task> </returns>
        public List<Task> GetTasksDueNextHour()
        {
            List<Task> approachingTasks = new List<Task>();
            IEnumerable<Task> allTasks= GetAllTasks();
            IEnumerable<Task> tasksSameDay = allTasks.
                Where(t => t.Deadline.Equals(DateTime.Today)).
                OrderBy(t => t.Deadline);
            int timeSpan;
            foreach (Task task in tasksSameDay)
            {
                timeSpan = task.Deadline.Hour - DateTime.Now.Hour;
                if (timeSpan == 1)
                {
                    approachingTasks.Add(task);
                }
            }
            return approachingTasks;
        }

        #endregion

        #region DELETE
        //@author Mateusz Staszek
        /// <summary>
        /// Delete task from database
        /// </summary>
        /// <param name="task"></param>
        public void DeleteTask(Task task)
        {
            CheckIfAbandonPenalty(task);
            _db.Delete(task);
        }

        //@author Mateusz Staszek
        /// <summary>
        ///for TESTING purpouses only
        ///deletes all record in the database
        /// </summary>
        public void DeleteAll()
        {
            _db.DeleteAll<Task>();
        }
        #endregion

        #region OTHER_METHODS
        /// <summary>
        /// completing task, 
        /// adding reward to user's account.
        /// deleting completed task
        /// </summary>
        /// <param name="id"></param>
        public void CompleteTask(int id)  
        {
            Task task = _db.Get<Task>(id); //get task
            UserHandler user = new UserHandler(); //get user

            user.AddRewardCoins(task); //adding reward to user's account

            if (task.Repeatable) // if task is repeatable then we dont delete this task
            {
                RepeatableTask(task); // just change its variables :)
                return;
            }

            DeleteTask(task); //deleting completed task from database
        }

        /// <summary>
        /// this method checks if user deleted task 30+ mins after creating it 
        /// and if yes it applies penalty to his account
        /// </summary>
        /// <param name="task"></param>
        public void CheckIfAbandonPenalty(Task task)
        {
            TimeSpan minsFromCreationTask = DateTime.Now.Subtract(task.CreationTime); // minutes that passed from the moment of creating task to NOW

            if (minsFromCreationTask.Minutes > 30) // checks if 30+ minutes passed from the moment of creating task
            // if yes
            {
                UserHandler user = new UserHandler(); // opens var that operates on users data
                user.Penalty(task.CoinsReward); // applying penalty on user based on amount that user was going to earn for completing this task
            }

            // if no does nothing
        }

        /// <summary>
        /// changes var in repeatable task and updates this task
        /// </summary>
        /// <param name="task"></param>
        public void RepeatableTask(Task task)
        {
            task.DoneToday = true;
            task.LastCompleted = DateTime.Now;
            task.Deadline = task.Deadline.AddDays(
                task.RepeatEveryNDays);

            Update(task);
        }

        /// <summary>
        /// Checks if repeatable tasks have correct parameters and if not changes them
        /// </summary>
        public void CheckAndAdaptRepeatableTasks()
        {
            //this query only gets repeatable tasks that wasn't done today,
            //but their parameters imply that them was

            List<Task> repeatableTasks = _db.Table<Task>().
                Where(t => t.DoneToday == true).
                Where(t => t.LastCompleted != DateTime.Today).ToList();

            //in this loop every repeatable task that wasnt correct
            //gets a proper treatment and becomes correct
            foreach (var task in repeatableTasks)
            {
                task.DoneToday = false;
                Update(task);
            }
        }

        #endregion

    }
}