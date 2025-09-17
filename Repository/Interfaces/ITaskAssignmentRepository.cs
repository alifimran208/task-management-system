using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Models;

namespace TaskManagement.Repository.Interfaces
{
    public interface ITaskAssignmentRepository
    {
        List<TaskAssignment> GetAll();
        bool Insert(TaskAssignment task);
        List<Device> CheckDevice();
        /*TaskAssignment GetById(int id);
        void Insert(TaskAssignment task);
        void Update(TaskAssignment task);
        void Delete(int TaskID);*/

    }
}
