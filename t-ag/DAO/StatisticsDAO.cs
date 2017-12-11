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
    class StatisticsDAO
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static string connectonString = @"Data Source=LOCALHOST\SQLEXPRESS;" + @"Initial Catalog=t-agDatabase;" + @"Integrated Security=True;" + @"Pooling=False;";

        private static string countryStatisticsRequest = 
            @"SELECT 
                [TCountry].[Country], 
                [TFinished].[Orders], 
                [TFinished].[Total], 
                [TFinished].[AVG],
                [TCanceled].[Canceled]
            FROM (
                SELECT 
                    [Tour].[Country] 
                FROM 
                    [Tour] 
                GROUP BY 
                    [Tour].[Country]
            ) AS [TCountry] 
            LEFT JOIN (
                SELECT 
                    [Tour].[Country], 
                    count(*) AS [Orders], 
                    sum([Tour].[Price]) AS [Total], 
                    avg([Tour].[Price]) AS [AVG] 
                FROM 
                    [Order] 
                INNER JOIN 
                    [Tour] 
                ON 
                    [Order].[TourId] = [Tour].[Id] 
                WHERE 
                    [Order].[Finished]=1 
                GROUP BY 
                    [Tour].[Country] 
            ) AS [TFinished] 
            ON 
                [TCountry].[Country]=[TFinished].[Country] 
            LEFT JOIN (
                SELECT 
                    [Tour].[Country], 
                    count(*) AS [Canceled] 
                FROM 
                    [Order] 
                INNER JOIN 
                    [Tour] 
                ON 
                    [Order].[TourId]=[Tour].[Id] 
                WHERE 
                    [Order].[Finished]=0 
                GROUP BY [Tour].[Country]
            ) AS [TCanceled] 
            ON 
                [TCountry].[Country]=[TCanceled].[Country]";

        private static string typeStatisticsRequest =
            @"SELECT 
                [TType].[Type], 
                [TFinished].[Orders], 
                [TFinished].[Total], 
                [TFinished].[AVG],
                [TCanceled].[Canceled]
            FROM (
                SELECT 
                    [Tour].[Type] 
                FROM 
                    [Tour] 
                GROUP BY 
                    [Tour].[Type]
            ) AS [TType] 
            LEFT JOIN (
                SELECT 
                    [Tour].[Type], 
                    count(*) AS [Orders], 
                    sum([Tour].[Price]) AS [Total], 
                    avg([Tour].[Price]) AS [AVG] 
                FROM 
                    [Order] 
                INNER JOIN 
                    [Tour] 
                ON 
                    [Order].[TourId] = [Tour].[Id] 
                WHERE 
                    [Order].[Finished]=1 
                GROUP BY 
                    [Tour].[Type] 
            ) AS [TFinished] 
            ON 
                [TType].[Type]=[TFinished].[Type] 
            LEFT JOIN (
                SELECT 
                    [Tour].[Type], 
                    count(*) AS [Canceled] 
                FROM 
                    [Order] 
                INNER JOIN 
                    [Tour] 
                ON 
                    [Order].[TourId]=[Tour].[Id] 
                WHERE 
                    [Order].[Finished]=0 
                GROUP BY [Tour].[Type]
            ) AS [TCanceled] 
            ON 
                [TType].[Type]=[TCanceled].[Type]";

        public static List<Statistics> getCountryStatistics()
        {
            logger.Info("Get country statistics");
            List<Statistics> countrys = new List<Statistics>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(countryStatisticsRequest, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Statistics el = new Statistics();

                        el.id = (string)reader["Country"];

                        if (!(reader["Orders"] is DBNull)) {  el.orders = (int)reader["Orders"]; }
                        if (!(reader["Total"] is DBNull)) { el.total = (int)reader["Total"]; }
                        if (!(reader["AVG"] is DBNull)) { el.avg = (int)reader["AVG"]; }
                        if (!(reader["Canceled"] is DBNull)) { el.canceled = (int)reader["Canceled"]; }

                        countrys.Add(el);
                    }
                }
            }
            catch (SqlException ex)
            {
                string message = "Cannot get country statistics: " + ex.Message;
                logger.Error(message);
                throw new DOAException(message, ex);
            }
            return countrys;
        }

        public static List<Statistics> getTypeStatistics()
        {
            logger.Info("Get type statistics");
            List<Statistics> countrys = new List<Statistics>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(typeStatisticsRequest, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Statistics el = new Statistics();

                        el.id = (string)reader["Type"];

                        if (!(reader["Orders"] is DBNull)) { el.orders = (int)reader["Orders"]; }
                        if (!(reader["Total"] is DBNull)) { el.total = (int)reader["Total"]; }
                        if (!(reader["AVG"] is DBNull)) { el.avg = (int)reader["AVG"]; }
                        if (!(reader["Canceled"] is DBNull)) { el.canceled = (int)reader["Canceled"]; }

                        countrys.Add(el);
                    }
                }
            }
            catch (SqlException ex)
            {
                string message = "Cannot get type statistics: " + ex.Message;
                logger.Error(message);
                throw new DOAException(message, ex);
            }
            return countrys;
        }

    }
}
