using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManagement.Models;
using TaskManagement.Repository;
using TaskManagement.Repository.Interfaces;

namespace TaskManagement.Controllers
{
    public class DeviceController : Controller
    {
        private readonly IDeviceRepository _repository;

        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public DeviceController(IDeviceRepository repository)
        {
            _repository = repository;
        }

        // GET: User
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Device obj)
        {
            if (obj == null)
            {
                return View();
            }
            var IsAdded = _repository.Insert(obj);

            return RedirectToAction("ShowDevice","Device");
        }

        public ActionResult ShowDevice()
        {
            List<Device> device_list = new List<Device>();
            device_list = _repository.GetAll();
            return View(device_list);
        }

        public ActionResult Edit(int id)
        {
            Device device = null;
            device = _repository.GetById(id);
            return View(device);
        }

        [HttpPost]
        public ActionResult Edit(Device device)
        {
            _repository.Update(device);
            return RedirectToAction("ShowDevice", "Device");
        }

        public ActionResult Delete(int id)
        {
            _repository.Delete(id);
            return RedirectToAction("ShowDevice", "Device");
        }



    }
}