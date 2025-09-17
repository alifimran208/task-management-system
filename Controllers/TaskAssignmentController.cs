using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskManagement.Models;
using TaskManagement.Repository.Interfaces;

namespace TaskManagement.Controllers
{
    public class TaskAssignmentController : Controller
    {
        // GET: TaskAssignment
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly ITaskAssignmentRepository _taskAssignmentRepository;

        public TaskAssignmentController(ITaskRepository taskRepo, IDeviceRepository deviceRepo, IUserRepository userRepo, ITaskAssignmentRepository taskAssignmentRepository)
        {
            _taskRepository = taskRepo;
            _userRepository = userRepo;
            _deviceRepository = deviceRepo;
            _taskAssignmentRepository = taskAssignmentRepository;
        }
        public ActionResult Create()
        {
            TaskAssignment assignment = new TaskAssignment
            {
                Tasks = GetTasks(),
                Users = GetUsers(),
                Devices = GetDevices()
            };


            return View(assignment);
        }

        [HttpPost]
        public ActionResult Create(TaskAssignment tAssign)
        {

            bool flag = _taskAssignmentRepository.Insert(tAssign);
            return RedirectToAction("Homepage", "Task");
        }

        private List<TaskItem> GetTasks()
        {
            List<TaskItem> taskList = new List<TaskItem>();
            taskList = _taskRepository.GetAll();
            return taskList;
        }

        private List<User> GetUsers()
        {
            List<User> userList = new List<User>();
            userList = _userRepository.GetAll();
            return userList;
        }

        private List<Device> GetDevices()
        {
            // Get all devices
            var deviceList = _deviceRepository.GetAll();

            // Get devices that are already assigned
            var assignedDevices = _taskAssignmentRepository.CheckDevice();

            // Filter out inactive + already assigned
            var availableDevices = deviceList
                .Where(d => d.status == "A" && !assignedDevices.Any(a => a.Id == d.Id))
                .ToList();

            return availableDevices;
        }



    }
}