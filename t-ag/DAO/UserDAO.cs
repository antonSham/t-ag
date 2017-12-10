using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using t_ag.Models;

namespace t_ag.DAO
{
    public static class UserDAO
    {
        private static string connectonString = @"Data Source=LOCALHOST\SQLEXPRESS;" + @"Initial Catalog=t-agDatabase;" + @"Integrated Security=True;" + @"Pooling=False;";
        public static List<User> getAllUsers()
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM [User]", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        User u = new User();
                        u.id = (int)reader["Id"];
                        u.role = (string)reader["type"];
                        u.login = (string)reader["loging"];
                        u.password = (string)reader["password"];
                        users.Add(u);
                    }
                } 
            } catch (SqlException ex)
            {
                throw ex;
            }
            return users;
        }

        public static int addUser(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO [User] ([type], [loging], [password]) VALUES (@role, @login, @password)", connection);
                    command.Parameters.Add("@role", SqlDbType.VarChar);
                    command.Parameters.Add("@login", SqlDbType.VarChar);
                    command.Parameters.Add("@password", SqlDbType.VarChar);

                    command.Parameters["@role"].Value = user.role;
                    command.Parameters["@login"].Value = user.login;
                    command.Parameters["@password"].Value = user.password;

                    command.ExecuteNonQuery();

                    SqlCommand command2 = new SqlCommand("SELECT SCOPE_IDENTITY()", connection);
                    SqlDataReader reader = command2.ExecuteReader();

                    reader.Read();
                    int userId = (int)reader[0];

                    return userId;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static User getUserById(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE Id=@id", connection);
                    command.Parameters.Add("@id", SqlDbType.Int);

                    command.Parameters["@id"].Value = id;

                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    
                    User el = new User();
                    el.id = (int)reader["Id"];
                    el.role = (string)reader["type"];
                    el.login = (string)reader["loging"];
                    el.password = (string)reader["password"];


                    return el;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                // No such id
                throw ex;
            }
        }
    }
}
