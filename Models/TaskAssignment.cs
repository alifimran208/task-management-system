using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskManagement.Models
{
    public class TaskAssignment
    {
        public int TaskID { get; set; }
        public int UserID { get; set; }
        public int DeviceId { get; set; }

        public List<TaskItem> Tasks { get; set; }
        public List<User> Users { get; set; }
        public List<Device> Devices { get; set; }
    }
}