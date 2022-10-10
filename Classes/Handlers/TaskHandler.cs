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
            List<Task> allTasks = _db.Table<Task>().ToList();

            return allTasks;
        }

        /// <summary>
        /// You get all tasks as IEnumerable
        /// </summary>
        /// <returns>IEnumerable allTasks</returns>
        public IEnumerable<Task> GetAllTasksAsIEnumerable()
        {
            IEnumerable<Task> allTasks = GetAllTasks();

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


        /// <summary>
        /// Get n amount of SortedTasks in form of IEnumerable right now!
        /// </summary>
        /// <param name="n"></param>
        /// <returns>IEnumerable nTasks</returns>
        public IEnumerable<Task> GetSortedTasks(int n)
        {
            IEnumerable<Task> nTasks = GetSortedTasks();
            return nTasks.Take(n);
        }

        /// <summary>
        /// You get sorted tasks by priorities and deadline as IEnumerable 
        /// </summary>
        /// <returns>IEnumerable sortedTasks</returns>
        public IEnumerable<Task> GetSortedTasksAsIEnumerable()
        {
            IEnumerable<Task> sortedTasks = GetSortedTasks();

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
        #endregion

    }
}