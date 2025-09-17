using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using TaskManagement.Models;
using TaskManagement.Repository.Interfaces;

namespace TaskManagement.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private readonly IUserRepository repository;
        public ActionResult Index()
        {
            return View();
        }

        public AccountController(IUserRepository repository)
        {
            this.repository = repository;
        }


        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(User users)
        {
            if (ModelState.IsValid)
            {
                repository.Insert(users);
            }
            return View(users);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User users)
        {
            User user;
            user = repository.GetLogin(users);
            if(user != null)
            {
                Session["Name"] = user.name;
                Session["UserName"] = user.username; 
                return RedirectToAction("Homepage", "Task");
            }    
            else return RedirectToAction("Login", "Account");
        }

        public ActionResult Logout()
        {
            Session["Name"] = null;
            Session["UserName"] = null;
            return RedirectToAction("Login", "Account");
        }
    }
}