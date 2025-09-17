using System;
using System.Collections.Generic;
using TaskManagement.Models;

namespace TaskManagement.Repository.Interfaces
{
    public interface IUserRepository
    {
        User GetById(int id);
        User GetLogin(User users);
        bool Insert(User user);

        List<User> GetAll();
        void Update(User user);
        void Delete(int userID);
    }
}
