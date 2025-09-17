using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TaskManagement.Models;
using TaskManagement.Repository.Interfaces;


namespace TaskManagement.Repository
{
    public class TaskAssignmentRepository : ITaskAssignmentRepository
    {

        private readonly string connectionString;

        private readonly IDeviceRepository _deviceRepository;
        public TaskAssignmentRepository(IDeviceRepository deviceRepository)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            _deviceRepository = deviceRepository;
        }

        public List<TaskAssignment> GetAll()
        {
            List<TaskAssignment> TaskList = new List<TaskAssignment>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM taskassignment";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            TaskAssignment task = new TaskAssignment()
                            {
                                TaskID = reader.GetInt32("TaskID"),
                                UserID = reader.GetInt32("Title"),
                                DeviceId = reader.GetInt32("")
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

        public bool Insert(TaskAssignment tAssign)
        {
            Device device = new Device();
            device = _deviceRepository.GetById(tAssign.DeviceId);
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO taskassignment (TaskID, UserID, DeviceID) VALUES (@TaskID, @UserID, @DeviceID)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TaskID", tAssign.TaskID);
                        cmd.Parameters.AddWithValue("@UserID", tAssign.UserID);
                        cmd.Parameters.AddWithValue("@DeviceID", tAssign.DeviceId);
                        if(device.status == "A")
                        {
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            Console.WriteLine("Device is not active. Query skipped.");
                        }
                    }
                    conn.Close();
                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public List<Device> CheckDevice()
        {
            List<Device> DeviceList = new List<Device>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string sql = "SELECT D.DeviceName, D.DeviceID FROM TASKASSIGNMENT TA, DEVICE D WHERE TA.DeviceID = D.DeviceID";
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while(rdr.Read())
                    {
                        Device device = new Device();
                        device.Id = Convert.ToInt32(rdr["DeviceID"]);
                        device.deviceName = rdr["DeviceName"].ToString();
                        DeviceList.Add(device);
                    }
                }
            }
            return DeviceList;
        }
    }
}