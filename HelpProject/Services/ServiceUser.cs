using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpProject.Models;
using HelpProject.Repository;

namespace HelpProject.Services
{
    public class ServiceUser
    {
        private NpgRepositoryUser _repository;
        public ServiceUser(NpgRepositoryUser repository)
        {
            _repository = repository;
        }

        public User GetUser(string login)
        {
            List<User> users = _repository.GetAllUsers();
            foreach (User user in users)
            {
                if (login == user.login)
                {
                    return user;
                }
            }
            return null;
        }

        public bool CheckUser(string login, string password)
        {
            List<User> users = _repository.GetAllUsers();
            foreach (User user in users)
            {
                if (login == user.login)
                {
                    if (password == user.password)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
