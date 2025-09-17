using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManagement.Models;
using TaskManagement.Repository.Interfaces;
using System.Web.DynamicData;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace TaskManagement.Repository
{
    public class UserRepository : IUserRepository 
    {
        private readonly string connectionString;
        public UserRepository() 
        {
            connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        public User GetById(int id)
        {
            User user = new User();
            List<User> users_list = new List<User>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string sql = "SELECT * FROM USERS WHERE UserID = @UserID";
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        user.Id = reader.GetInt32("UserID");
                        user.name = reader.GetString("Name");
                        user.username = reader.GetString("UserName");
                        user.email = reader.GetString("Email");
                        user.password = reader.GetString("PasswordHash");
                    }
                }
                con.Close();
            }
            return user;
            
        }

        public User GetLogin(User users)
        {
            bool flag = false;
            User user1 = null;
            List<User> users_list = new List<User>();
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
                            name = reader.GetString("Name"),
                            username = reader.GetString("UserName"),
                            password = reader.GetString("PasswordHash")

                        };
                        users_list.Add(user);
                    }
                    foreach (User user in users_list)
                    {
                        if (user.username == users.username && user.password == users.password)
                        {
                            flag = true;
                            user1 = user;
                        }
                    }
                }
            }
            return user1;

        }
        public bool Insert(User user)
        {
           
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO USERS (Name, UserName, Email, PasswordHash) VALUES (@Name, @UserName, @Email, @Password)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", user.name);
                        cmd.Parameters.AddWithValue("@UserName", user.username);
                        cmd.Parameters.AddWithValue("@Email", user.email);
                        cmd.Parameters.AddWithValue("@Password", user.password); // Consider hashing this
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                    return true;
                    
                }
                catch (Exception ex)
                {

                }
            }
            return false;
        }

        public List<User> GetAll()
        {
            List<User> user_list = new List<User>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string sql = "SELECT UserId, Name, UserName, Email, PasswordHash FROM Users";
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        User user = new User()
                        {
                            Id = reader.GetInt32("UserId"),
                            name = reader.GetString("Name"),
                            username = reader.GetString("UserName"),
                            password = reader.GetString("PasswordHash"),
                            email = reader.GetString("Email")
                        };
                        user_list.Add(user);
                    }
                    con.Close();
                }
            }
            return user_list;
        }

        public void Update(User user)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string sql = "UPDATE USERS SET Name = @Name, UserName = @UserName, Email = @Email, PasswordHash =  @PasswordHash WHERE UserID = @UserID";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserID", user.Id);
                cmd.Parameters.AddWithValue("@Name", user.name);
                cmd.Parameters.AddWithValue("@UserName", user.username);
                cmd.Parameters.AddWithValue("@Email", user.email);
                cmd.Parameters.AddWithValue("@PasswordHash", user.password);
                cmd.ExecuteNonQuery();

                con.Close();
            };
        }

        public void Delete(int UserID)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string sql = "DELETE FROM USERS WHERE UserID = @UserID";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
    }
}