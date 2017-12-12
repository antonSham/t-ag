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
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static string connectonString = @"Data Source=LOCALHOST\SQLEXPRESS;" + @"Initial Catalog=t-agDatabase;" + @"Integrated Security=True;" + @"Pooling=False;" + @"MultipleActiveResultSets=True";
        public static List<Order> getAllOrders()
        {
            logger.Info("Get all orders");
            List<Order> orders = new List<Order>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM [Order] WHERE [Finished]=1", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Order el = new Order();
                        el.id = (int)reader["Id"];
                        el.tour = TourDAO.getTourById((int)reader["TourId"]);
                        el.price = (int)reader["Price"];
                        el.customer = UserDAO.getUserById((int)reader["CustomerId"]);
                        var employeeId = reader["EmployeeId"];
                        if (!(employeeId is DBNull))
                        {
                            el.employee = UserDAO.getUserById((int)employeeId);
                        }
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
                string message = "Cannot get all orders: " + ex.Message;
                logger.Error(message);
                throw new DOAException(message, ex);
            }
            return orders;
        }

        public static int addOrder(Order order)
        {
            logger.Info("Add order");
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO [Order] ([TourId], [Price], [CustomerId]) VALUES (@tour, @price, @customer); SELECT SCOPE_IDENTITY()", connection);
                    command.Parameters.Add("@tour", SqlDbType.Int);
                    command.Parameters.Add("@price", SqlDbType.Int);
                    command.Parameters.Add("@customer", SqlDbType.Int);

                    command.Parameters["@tour"].Value = order.tour.id;
                    command.Parameters["@price"].Value = order.price;
                    command.Parameters["@customer"].Value = order.customer.id;

                    int orderId = Convert.ToInt32(command.ExecuteScalar());

                    order.participants.ForEach(el => addParticipant(orderId, el.id));

                    return orderId;
                }
            }
            catch (SqlException ex)
            {
                string message = "Cannot add order" + ex.Message;
                logger.Error(message);
                throw new DOAException(message, ex);
            }
        }

        public static void addParticipant(int orderId, int participantId)
        {
            logger.Info("Add participant");
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO [OrderParticipant] (OrderId, ParticipantID) VALUES (@order, @participant)", connection);
                    command.Parameters.Add("@order", SqlDbType.Int);
                    command.Parameters.Add("@participant", SqlDbType.Int);

                    command.Parameters["@order"].Value = orderId;
                    command.Parameters["@participant"].Value = participantId;

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                string message = "Cannot add participant" + ex.Message;
                logger.Error(message);
                throw new DOAException(message, ex);
            }
        }

        public static void commitOrder(int orderId)
        {
            logger.Info("Commit order");
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("UPDATE [Order] SET Finished=1 WHERE Id=@id", connection);
                    command.Parameters.Add("@id", SqlDbType.Int);

                    command.Parameters["@id"].Value = orderId;

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                string message = "Cannot commit order" + ex.Message;
                logger.Error(message);
                throw new DOAException(message, ex);
            }
        }

        public static void cancelOrder(int orderId)
        {
            logger.Info("Cancel order");
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("UPDATE [Order] SET Finished=0 WHERE Id=@id", connection);
                    command.Parameters.Add("@id", SqlDbType.Int);

                    command.Parameters["@id"].Value = orderId;

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                string message = "Cannot cancel order" + ex.Message;
                logger.Error(message);
                throw new DOAException(message, ex);
            }
        }

        public static void commitOrderAmount(int orderId)
        {
            logger.Info("Cancel order");
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("UPDATE [Order] SET Amount=[Price] WHERE Id=@id", connection);
                    command.Parameters.Add("@id", SqlDbType.Int);

                    command.Parameters["@id"].Value = orderId;

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                string message = "Cannot cancel order" + ex.Message;
                logger.Error(message);
                throw new DOAException(message, ex);
            }
        }
        public static Order getOrderById(int id)
        {
            logger.Info("Get order by id: " + id);
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
                    el.price = (int)reader["Price"];
                    el.customer = UserDAO.getUserById((int)reader["CustomerId"]);
                    var employeeId = reader["EmployeeId"];
                    if (!(employeeId is DBNull))
                    {
                        el.employee = UserDAO.getUserById((int)employeeId);
                    }
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
                string message = "Cannot get order by id (Id: " + id + "): " + ex.Message;
                logger.Error(message);
                throw new DOAException(message, ex);
            }
            catch (InvalidOperationException ex)
            {
                // No such id
                string message = "Cannot get order by id (Wrong id: " + id + "): " + ex.Message;
                logger.Error(message);
                throw new DOAException(message, ex);
            }
        }

    }
}
