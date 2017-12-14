using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using NUnit.Framework;

using t_ag.Models;
using t_ag.DAO;

namespace t_ag.UnitTest
{
    [TestFixture]
    public class DAOTests
    {
        private static string connectonString = @"Data Source=LOCALHOST\SQLEXPRESS;" + @"Initial Catalog=t-agDatabase;" + @"Integrated Security=True;" + @"Pooling=False;";

        private void deleteUserById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectonString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM [User] WHERE Id=@id", connection);
                command.Parameters.Add("@id", SqlDbType.Int);

                command.Parameters["@id"].Value = id;

                command.ExecuteNonQuery();
            }
            
        }

        private List<User> getDiff(List<User> A, List<User> B)
        {
            List<User> diff = new List<User>();

            A.ForEach((User elA) =>
            {
                B.ForEach((User elB) =>
                {
                    if (elA.id == elB.id)
                    {
                        if (elA.role != elB.role || elA.login != elB.login || elA.password != elB.password)
                        {
                            diff.Add(elB);
                        }
                    }
                });
            });

            A.ForEach((User elA) =>
            {
                bool found = false;
                B.ForEach((User elB) =>
                {
                    if (elA.id == elB.id)
                    {
                        found = true;
                    }
                });

                if (!found)
                {
                    diff.Add(elA);
                }
            });


            B.ForEach((User elB) =>
            {
                bool found = false;
                A.ForEach((User elA) =>
                {
                    if (elB.id == elA.id)
                    {
                        found = true;
                    }
                });

                if (!found)
                {
                    diff.Add(elB);
                }
            });

            return diff;
        }
        private void checkUsersAreSame(User A, User B)
        {
            Assert.AreEqual(A.id, A.id);
            Assert.AreEqual(A.role, A.role);
            Assert.AreEqual(A.login, A.login);
            Assert.AreEqual(A.password, A.password);
        }
        private void checkById()
        {
            UserDAO.getAllUsers().ForEach((User element) => {
                checkUsersAreSame(element, UserDAO.getUserById(element.id));
            });

        }

        [Test]
        public void Test()
        {
            User firstUser = new User();
            firstUser.role = "customer";
            firstUser.login = "testUser1";
            firstUser.password = "12345";

            User secondUser = new User();
            secondUser.role = "customer";
            secondUser.login = "testUser2";
            secondUser.password = "asdzxc";

            List<User> Before = UserDAO.getAllUsers();

            firstUser.id = UserDAO.addUser(firstUser);

            List<User> AfterFirstAdd = UserDAO.getAllUsers();
            List<User> FirstDiff = getDiff(Before, AfterFirstAdd);

            Assert.AreEqual(FirstDiff.Count, 1);
            checkUsersAreSame(firstUser, FirstDiff[0]);

            secondUser.id = UserDAO.addUser(secondUser);

            Assert.AreNotEqual(firstUser.id, secondUser.id);

            List<User> AfterSecondAdd = UserDAO.getAllUsers();
            List<User> SecondDiff = getDiff(AfterFirstAdd, AfterSecondAdd);

            Assert.AreEqual(SecondDiff.Count, 1);
            checkUsersAreSame(secondUser, SecondDiff[0]);

            checkById();

            secondUser.role = "employee";

            UserDAO.updateRole(secondUser.id, "employee");

            List<User> AfterUpdate = UserDAO.getAllUsers();
            List<User> UpdateDiff = getDiff(AfterSecondAdd, AfterUpdate);

            Assert.AreEqual(UpdateDiff.Count, 1);
            checkUsersAreSame(secondUser, UpdateDiff[0]);

            deleteUserById(firstUser.id);
            deleteUserById(secondUser.id);
        }
    }
}
