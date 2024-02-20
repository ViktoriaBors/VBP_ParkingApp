﻿using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;

namespace VBP_ParkingApp.Classes
{
    class SqlDataHandler
    {
        private static string connectionStr = "server=mysql.nethely.hu;uid=parkingapp;pwd=prooktatas2024;database=parkingapp";
        private static MySqlConnection connection = new MySqlConnection(connectionStr); //winformnál Mysql.data, itt inkább mysql.connecor

        public static FunctionResult FinishedParkingPeriodSave(ParkingPeriod newParking)
        {
            FunctionResult result = new FunctionResult();
            string query = "INSERT INTO parkolasok_vbp VALUES(@id,@startTime,@endTime,@price)";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@id", 0);
                    cmd.Parameters.AddWithValue("@startTime", newParking.StartTime);
                    cmd.Parameters.AddWithValue("@endTime", newParking.EndTime);
                    cmd.Parameters.AddWithValue("@price", newParking.Price);
                    result.Result = cmd.ExecuteNonQuery() > 0;
                    result.Message = "Finished Parking period is saved to database";
                }
                catch (Exception ex)
                {
                    result.Result=false;
                    result.Message=ex.Message;
                }
                finally { connection.Close(); }
                return result;
            }
        }
    }
}