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
    public class DeviceRepository : IDeviceRepository
    {
        private readonly string connectionString;
        public DeviceRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }
        public List<Device> GetAll()
        {
            List<Device> DeviceList = new List<Device>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM Device";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Device task = new Device()
                            {
                                Id = reader.GetInt32("DeviceID"),
                                deviceName = reader.GetString("DeviceName"),
                                status = reader.GetString("Status")
                            };
                            DeviceList.Add(task);
                        }
                    }
                }

                catch
                {

                }
            }
            return DeviceList;
        }

        public Device GetById(int id)
        {
            Device device = new Device();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM Device WHERE DeviceID = @DeviceID";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@DeviceID", id);
                con.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    device.Id = Convert.ToInt32(rdr["DeviceID"]);
                    device.deviceName = rdr["DeviceName"].ToString();
                    device.status = rdr["Status"].ToString();
                }

                con.Close();
            }

            return device;
        }


        public bool Insert(Device device)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Device (DeviceName, Status) VALUES (@DeviceName, @status)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@DeviceName", device.deviceName);
                        cmd.Parameters.AddWithValue("@status", device.status);

                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();

                    return true;
                }
                catch (Exception Ex)
                {
                    return false;
                }
            }
        }
        public void Update(Device device)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string sql = "UPDATE Device SET DeviceName = @DeviceName, status = @status WHERE DeviceID = @DeviceID";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@DeviceID", device.Id);
                cmd.Parameters.AddWithValue("@DeviceName", device.deviceName);
                cmd.Parameters.AddWithValue("@status", device.status);
                cmd.ExecuteNonQuery();

                con.Close();
            };

        }
        public void Delete(int DeviceID)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string sql = "DELETE FROM DEVICE WHERE DeviceID = @DeviceID";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@DeviceID", DeviceID);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
    }
}