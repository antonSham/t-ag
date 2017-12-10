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
    class ParticipantDAO
    {
        private static string connectonString = @"Data Source=LOCALHOST\SQLEXPRESS;" + @"Initial Catalog=t-agDatabase;" + @"Integrated Security=True;" + @"Pooling=False;";
        public static List<Participant> getAllParticipants()
        {
            List<Participant> participants = new List<Participant>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM [Participant]", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Participant el = new Participant();
                        el.id = (int)reader["Id"];
                        el.fullName = (string)reader["Name"];
                        el.age = (int)reader["Age"];
                        el.passport = (string)reader["Passport"];
                        participants.Add(el);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return participants;
        }

        public static int addParticipant(Participant participant)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO [Participant] ([Name], [Age], [Password]) VALUES (@fullName, @age, @passport)", connection);
                    command.Parameters.Add("@fullName", SqlDbType.VarChar);
                    command.Parameters.Add("@age", SqlDbType.Int);
                    command.Parameters.Add("@passport", SqlDbType.VarChar);

                    command.Parameters["@fullName"].Value = participant.fullName;
                    command.Parameters["@age"].Value = participant.age;
                    command.Parameters["@passport"].Value = participant.passport;

                    command.ExecuteNonQuery();

                    SqlCommand command2 = new SqlCommand("SELECT SCOPE_IDENTITY()", connection);
                    SqlDataReader reader = command2.ExecuteReader();

                    reader.Read();
                    int participantId = (int)reader[0];

                    return participantId;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static Participant getParticipantById(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM [Participant] WHERE Id=@id", connection);
                    command.Parameters.Add("@id", SqlDbType.Int);

                    command.Parameters["@id"].Value = id;

                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();

                    Participant el = new Participant();
                    el.id = (int)reader["Id"];
                    el.fullName = (string)reader["Name"];
                    el.age = (int)reader["Age"];
                    el.passport = (string)reader["Passport"];


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
