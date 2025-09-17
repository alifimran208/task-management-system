using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.Mvc;
using TaskManagement.Models;
using TaskManagement.Repository.Interfaces;
using System.Web.DynamicData;


namespace TaskManagement.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string connectionString;
        public TaskRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }
        public List<TaskItem> GetAll()
        {
            List<TaskItem> TaskList = new List<TaskItem>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
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
                            TaskItem task = new TaskItem()
                            {
                                TaskID = reader.GetInt32("TaskID"),
                                TaskName = reader.GetString("Title"),
                                Description = reader.GetString("Description"),
                                DueDate = reader.GetDateTime("DueDate"),
                                Status = reader.GetString("Status")
                            };
                            TaskList.Add(task);
                        }
                    }
                }

                catch
                {

                }
            }
            return TaskList;
        }

        public TaskItem GetById(int id)
        {
            TaskItem task = new TaskItem();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM Tasks WHERE TaskID = @TaskID";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@TaskID", id);
                con.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    task.TaskID = Convert.ToInt32(rdr["TaskID"]);
                    task.TaskName = rdr["Title"].ToString();
                    task.Description = rdr["Description"].ToString();
                    task.Status = rdr["Status"].ToString();
                }

                con.Close();
            }

            return task;
        }


        public void Insert(TaskItem task)
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
           
                }
            }
        }
        public void Update(TaskItem task)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string sql = "UPDATE TASKS SET TaskID = @TaskID, Title = @Title, Description = @Description, Status =  @Status WHERE TaskID = @TaskID";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@TaskID", task.TaskID);
                cmd.Parameters.AddWithValue("@Title", task.TaskName);
                cmd.Parameters.AddWithValue("@Description", task.Description);
                cmd.Parameters.AddWithValue("@Status", task.Status);
                cmd.ExecuteNonQuery();

                con.Close();
            };
            
        }
        public void Delete(int TaskID)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string sql = "DELETE FROM TASKS WHERE TaskID = @TaskID";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@TaskID", TaskID);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
    }
}