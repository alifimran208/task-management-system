using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManagement.Models;
using TaskManagement.Repository.Interfaces;

namespace TaskManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository repository;

        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public UserController(IUserRepository repository)
        {
            this.repository = repository;
        }

        // GET: User
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(User user)
        {
            //if (user == null)
            //{
            //    RedirectToAction("CreateUser", "User");
            //}
            bool flag = false;
            flag = repository.Insert(user);
            return RedirectToAction("Homepage", "Task");
        }

        public ActionResult ShowUser()
        {
            List<User> user_list = new List<User>();
            user_list = repository.GetAll();
            return View(user_list);
        }

        public ActionResult Edit(int id)
        {
            User user = null;
            user = repository.GetById(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            repository.Update(user);
            return RedirectToAction("ShowUser", "User");
        }

        public ActionResult Delete(int id)
        {
            repository.Delete(id);
            return RedirectToAction("ShowUser", "User");
        }



    }
}