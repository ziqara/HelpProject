using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpProject.Models;
using Npgsql;

namespace HelpProject.Repository
{
    public class NpgRepositoryUser
    {
        const string connStr = "Host=localhost;Port=5432;Database=DemoEx;Username=postgres;Password=228Egor1337;";

        public List<User> GetAllUsers()
        {
            try
            {
                List<User> users = new List<User>();
                var conn = new NpgsqlConnection(connStr);
                conn.Open();
                string sqlUser = "SELECT * FROM users";
                using (var cmd = new NpgsqlCommand(sqlUser, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User();
                        user.Id = reader.GetInt32(0);
                        user.role = reader.GetString(1);
                        user.name = reader.GetString(2);
                        user.login = reader.GetString(3);
                        user.password = reader.GetString(4);
                        users.Add(user);
                    }
                }
                conn.Close();
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
