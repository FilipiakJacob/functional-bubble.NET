﻿using Android.App;
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
        public const int LOWEST_PRIORITY = 0;
        public const int HIGHEST_PRIORITY = 3;

        public void Add(Task task) // inserts task object to Task table
        {
            UserHandler userHandler = new UserHandler();

            //task.CoinsReward = userHandler.CalculateReward(task); 

            _db.Insert(task);    
        }
        public Task Get(int id) // returns task object with given id
        {
            Task task = _db.Get<Task>(id);
            return task;
        }

        /*
         * @author Mikołaj Petri
         * 
         * completing task, 
         * adding reward to user's account, 
         * deleting completed task
         * 
         * no @return
         */
        public void CompleteTask(int id)  
        {
            Task task = _db.Get<Task>(id); //get task
            UserHandler user = new UserHandler(); //get user

            user.AddRewardCoins(task); //adding reward to user's account

            DeleteTask(task); //deleting completed task from database
        }

        public void Update(Task task) //updates task in Tasks table
        {
            _db.Update(task);
        }

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

        public List<Task> GetAllTasks() 
        {
            List<Task> allTasks = _db.Query<Task>("SELECT * FROM Tasks");
            return allTasks;
        }

        public List<Task> GetSortedTasks() // return sorted tasks by priorities and deadline
        {
            List<Task> allTasksList = GetAllTasks();

            //LINQ expression
            var sortedTasks = allTasksList. //takes allTasks
                OrderByDescending(t => t.Priority). //order them by priorities (descending bc highest priority is 3)
                ThenBy(t => t.Deadline).ToList(); //order them by deadline descending

            return sortedTasks;
        }

        public List<Task> SortedByDeadlineTasks(List<Task> tasks) // sort list of tasks by deadline
        {
            tasks.Sort((a, b) => a.Deadline.CompareTo(b.Deadline));
            return tasks;
        }

        /*
         * @param task - to identify what task is set to be checked
         * 
         * this method checks if user deleted task 30+ mins after creating it 
         * and if yes it applies penalty to his account 
         * 
         * no @return
         */
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


        //@author Mateusz Staszek
        public void DeleteTask(Task task)
        {
            CheckIfAbandonPenalty(task);  // line 102
            _db.Delete(task);
        }
        
        //@author Mateusz Staszek
        public void DeleteAll()
        {
            //for testing purpouses only
            //deletes all record in the database
            _db.DeleteAll<Task>();
        }

    }
}