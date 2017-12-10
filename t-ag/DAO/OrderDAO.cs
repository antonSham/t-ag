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
    class OrderDAO
    {
        private static string connectonString = @"Data Source=LOCALHOST\SQLEXPRESS;" + @"Initial Catalog=t-agDatabase;" + @"Integrated Security=True;" + @"Pooling=False;";
        public static List<Order> getAllOrders()
        {
            List<Order> orders = new List<Order>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM [Order]", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Order el = new Order();
                        el.id = (int)reader["Id"];
                        el.tour = TourDAO.getTourById((int)reader["TourId"]);
                        el.customer = UserDAO.getUserById((int)reader["CustomerId"]);
                        el.employee = UserDAO.getUserById((int)reader["EmployeeId"]);
                        el.amount = (int)reader["Amount"];
                        el.participants = new List<Participant>();

                        SqlCommand command2 = new SqlCommand("SELECT * FROM [OrderParticipant] WHERE OrderId=@id", connection);
                        command2.Parameters.Add("@id", SqlDbType.Int);
                        command2.Parameters["@id"].Value = el.id;

                        SqlDataReader reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            el.participants.Add( ParticipantDAO.getParticipantById((int)reader2["ParticipantId"]));
                        }

                        orders.Add(el);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return orders;
        }

        public static int addOrder(Order order)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO [Order] ([TourId], [CustomerIs], [EmployeeId], [Amount]) VALUES (@tour, @customer, @employee, @amount)", connection);
                    command.Parameters.Add("@tour", SqlDbType.Int);
                    command.Parameters.Add("@customer", SqlDbType.Int);
                    command.Parameters.Add("@employee", SqlDbType.Int);
                    command.Parameters.Add("@amount", SqlDbType.Int);

                    command.Parameters["@tour"].Value = order.tour.id;
                    command.Parameters["@customer"].Value = order.customer.id;
                    command.Parameters["@employee"].Value = order.employee.id;
                    command.Parameters["@amount"].Value = order.amount;

                    command.ExecuteNonQuery();

                    SqlCommand command2 = new SqlCommand("SELECT SCOPE_IDENTITY()", connection);
                    SqlDataReader reader = command2.ExecuteReader();

                    reader.Read();
                    int orderId = (int)reader[0];

                    order.participants.ForEach(el => addParticipant(orderId, el.id));

                    return orderId;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        private static void addParticipant(int orderId, int participantId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO [OrderParticipant] (OrderId, ParticipantID) VALUES (@order, @participant)", connection);
                    command.Parameters.Add("@order", SqlDbType.Int);
                    command.Parameters.Add("@participant", SqlDbType.Text);

                    command.Parameters["@order"].Value = orderId;
                    command.Parameters["@participant"].Value = participantId;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static Order getOrderById(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM [Order] WHERE Id=@id", connection);
                    command.Parameters.Add("@id", SqlDbType.Int);

                    command.Parameters["@id"].Value = id;

                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();

                    Order el = new Order();
                    el.id = (int)reader["Id"];
                    el.tour = TourDAO.getTourById((int)reader["TourId"]);
                    el.customer = UserDAO.getUserById((int)reader["CustomerId"]);
                    el.employee = UserDAO.getUserById((int)reader["EmployeeId"]);
                    el.amount = (int)reader["Amount"];
                    el.participants = new List<Participant>();

                    SqlCommand command2 = new SqlCommand("SELECT * FROM [OrderParticipant] WHERE OrderId=@id", connection);
                    command2.Parameters.Add("@id", SqlDbType.Int);
                    command2.Parameters["@id"].Value = el.id;

                    SqlDataReader reader2 = command2.ExecuteReader();
                    while (reader2.Read())
                    {
                        el.participants.Add(ParticipantDAO.getParticipantById((int)reader2["ParticipantId"]));
                    }

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
