using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using TaskManagement.Models;


namespace TaskManagement.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task

        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

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
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "INSERT INTO users (Name, Email, PasswordHash) VALUES (@Name, @Email, @Password)";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Name", users.username);
                            cmd.Parameters.AddWithValue("@Email", users.email);
                            cmd.Parameters.AddWithValue("@Password", users.password); // Consider hashing this
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "Error: " + ex.Message;
                    }
                }
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
            string u_name = users.username;
            List<User> users_list = new List<User>();
            bool flag = false;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string sql = "SELECT * FROM USERS";
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        User user = new User()
                        {
                            username = reader.GetString("Name"),
                            password = reader.GetString("PasswordHash")

                        };
                        users_list.Add(user);
                    }
                    foreach (User user in users_list)
                    {
                        if (user.username == users.username && user.password == users.password)
                        {
                            flag = true;
                        }
                    }
                }
            }
            if (flag) return RedirectToAction("Homepage", "Task");
            return View();
        }




        public ActionResult Homepage()
        {
            return View();
        }

        public ActionResult AddTask()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddTask(Task task)
        {

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO TASKS (Title, Description, Duedate, Status, UserId) VALUES (@Title, @Description, @Duedate, @Status, @UserId)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Title", task.TaskName);
                        cmd.Parameters.AddWithValue("@Description", task.Description);
                        cmd.Parameters.AddWithValue("@DueDate", task.DueDate);
                        cmd.Parameters.AddWithValue("@Status", task.Status);
                        cmd.Parameters.AddWithValue("@UserId", task.AssignedTo);

                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                catch (Exception Ex)
                {
                    ViewBag.ErrorMessage = "Error: " + Ex.Message;
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult ShowTask()
        {
            List<Task> TaskList = new List<Task>();

            using (MySqlConnection conn = new MySqlConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM TASKS";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Task task = new Task()
                            {
                                TaskName = reader.GetString("Title"),
                                Description = reader.GetString("Description"),
                            };
                        }
                    }
                }

                catch
                {

                }
                return View();
            }
        }



    }
}