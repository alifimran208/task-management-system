using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Models;

namespace TaskManagement.Repository.Interfaces
{
    public interface IDeviceRepository
    {
        List<Device> GetAll();
        Device GetById(int id);
        bool Insert(Device device);
        void Update(Device device);
        void Delete(int TaskID);
    }
}
