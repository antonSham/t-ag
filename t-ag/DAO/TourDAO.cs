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
    class TourDAO
    {
        private static string connectonString = @"Data Source=LOCALHOST\SQLEXPRESS;" + @"Initial Catalog=t-agDatabase;" + @"Integrated Security=True;" + @"Pooling=False;";
        public static List<Tour> getAllTours()
        {
            List<Tour> tours = new List<Tour>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM [Tour]", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Tour el = new Tour();
                        el.id = (int)reader["Id"];
                        el.country = (string)reader["Country"];
                        el.type = (string)reader["Type"];
                        el.price = (int)reader["Price"];
                        el.description = (string)reader["Description"];
                        el.feedbacks = new List<String>();

                        SqlCommand command2 = new SqlCommand("SELECT * FROM [TourFeedback] WHERE TourId=@id", connection);
                        command2.Parameters.Add("@id", SqlDbType.Int);
                        command2.Parameters["@id"].Value = el.id;

                        SqlDataReader reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            el.feedbacks.Add((string)reader2["Feedback"]);
                        }

                        tours.Add(el);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return tours;
        }

        public static int addTour(Tour tour)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO [Tour] ([Country], [Type], [Price], [Description]) VALUES (@country, @type, @price, @description)", connection);
                    command.Parameters.Add("@country", SqlDbType.VarChar);
                    command.Parameters.Add("@type", SqlDbType.VarChar);
                    command.Parameters.Add("@price", SqlDbType.Int);
                    command.Parameters.Add("@description", SqlDbType.Text);

                    command.Parameters["@country"].Value = tour.country;
                    command.Parameters["@type"].Value = tour.type;
                    command.Parameters["@price"].Value = tour.price;
                    command.Parameters["@password"].Value = tour.description;

                    command.ExecuteNonQuery();

                    SqlCommand command2 = new SqlCommand("SELECT SCOPE_IDENTITY()", connection);
                    SqlDataReader reader = command2.ExecuteReader();

                    reader.Read();
                    int tourId = (int)reader[0];

                    tour.feedbacks.ForEach(el => addFeedback(tourId, el));

                    return tourId;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        private static void addFeedback(int tourId, string feedback)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO [TourFeedback] (TourId, Feedback) VALUES (@tourId, @feedback)", connection);
                    command.Parameters.Add("@tourId", SqlDbType.Int);
                    command.Parameters.Add("@feedback", SqlDbType.Text);

                    command.Parameters["@tourId"].Value = tourId;
                    command.Parameters["@feedback"].Value = feedback;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static Tour getTourById(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM [Tour] WHERE Id=@id", connection);
                    command.Parameters.Add("@id", SqlDbType.Int);

                    command.Parameters["@id"].Value = id;

                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();

                    Tour el = new Tour();
                    el.id = (int)reader["Id"];
                    el.country = (string)reader["Country"];
                    el.type = (string)reader["Type"];
                    el.price = (int)reader["Price"];
                    el.description = (string)reader["Description"];
                    el.feedbacks = new List<String>();

                    SqlCommand command2 = new SqlCommand("SELECT * FROM [TourFeedback] WHERE TourId=@id", connection);
                    command2.Parameters.Add("@id", SqlDbType.Int);
                    command2.Parameters["@id"].Value = el.id;

                    SqlDataReader reader2 = command.ExecuteReader();
                    while (reader2.Read())
                    {
                        el.feedbacks.Add((string)reader2["Feedback"]);
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
