using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using TaskManagement.Models;
using TaskManagement.Repository.Interfaces;



namespace TaskManagement.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskRepository repository;
        // GET: Task
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public TaskController(ITaskRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult Homepage()
        {
            return View();
        }

        public ActionResult AddTask()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult AddTask(TaskItem task)
        {
            repository.Insert(task);
            return View();
        }

        [HttpGet]
        public ActionResult ShowTask()
        {
            if (Session["UserName"] != null)
            {
                List<TaskItem> TaskList = new List<TaskItem>();
                TaskList = repository.GetAll();
                return View(TaskList);
            }
            return RedirectToAction("Login", "Account");
        }


        public ActionResult Edit(int id)
        {
            TaskItem task = new TaskItem();
            task = repository.GetById(id);
            return View(task);
        }

        [HttpPost]
        public ActionResult Edit(TaskItem task)
        {
            repository.Update(task); 
            return RedirectToAction("ShowTask", "Task");
        }

        public ActionResult Delete(int id)
        {
            repository.Delete(id);
            return RedirectToAction("ShowTask", "Task");
        }


    }
}