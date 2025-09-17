using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Models;

namespace TaskManagement.Repository.Interfaces
{
    public interface ITaskRepository
    {
        List<TaskItem> GetAll();
        TaskItem GetById(int id);
        void Insert(TaskItem task);
        void Update(TaskItem task);
        void Delete(int TaskID);
    }
}
