using AutoGenerateAPI.Database.Models;
using AutoGenerateAPI.Entities.CustomModels;
using AutoGenerateAPI.Entities.ResponseStanders;
using AutoGenerateAPI.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AutoGenerateAPI.Repository
{
    public class POSTRepository : IPOST
    {
        public readonly TryAutomationContext _context;
        public IConfiguration _configuration;
        public POSTRepository(TryAutomationContext context , IConfiguration configuration) 
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ResponseModel<dynamic>> CreateTable(TableCreation requestModel)
        {
            ResponseModel<dynamic> response = new ResponseModel<dynamic>();
            try
            {
                // Validate requestModel and ensure it is not null
                if (requestModel == null || string.IsNullOrWhiteSpace(requestModel.TableName) || requestModel.Columns == null || requestModel.Columns.Count == 0)
                {

                    response.IsSuccess = false;
                    response.Message = "correct the table name";
                    response.Data = null;
                    return response;
                }
                else
                {
                    bool flagTableName = await executeSQLTableName(requestModel.TableName.ToString());

                    if (!flagTableName)
                    {
                        response.IsSuccess = false;
                        response.Message = "choose a differenttable name";
                        response.Data =null;
                        return response;
                    }
                }

                // Construct the CREATE TABLE query
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append($"CREATE TABLE {requestModel.TableName} (");

                // Append primary key column (standard convention: TableName + "ID")
                queryBuilder.Append($"{requestModel.TableName.ToLower()}ID INT PRIMARY KEY IDENTITY(1,1), ");

                // Append other columns with data types and lengths
                foreach (var column in requestModel.Columns)
                {

                    if (column.Value.DataType?.ToLower() == "choice" || column.Value.DataType?.ToLower() == "multichoice" || column.Value.DataType?.ToLower() == "dropdown")
                    {
                        CreateOptionTable(column.Key, column.Value.choices, requestModel.TableName);
                        column.Value.DataType = "nvarchar";
                    }

                    if (column.Value.DataType == "Yes/No" || column.Value.DataType == "Paragraph" ||
                        column.Value.DataType == "String" || column.Value.DataType == "Number" || column.Value.DataType == "string" || !string.IsNullOrEmpty(column.Value.DataType?.ToLower()))
                    {
                        if (column.Value.DataType == "Yes/No")
                        {
                            column.Value.DataType = "bool";
                        }
                        if (column.Value.DataType == "Number")
                        {
                            column.Value.DataType = "int";
                        }
                        else
                        {
                            column.Value.DataType = "nvarchar";
                        }
                    }

                    queryBuilder.Append($"{column.Key} {column.Value.DataType}");

                    // Append length if applicable
                    if (!string.IsNullOrWhiteSpace(column.Value.MaxLength))
                    {
                        if (column.Value.MaxLength.Equals("max", StringComparison.OrdinalIgnoreCase) || column.Value.MaxLength.Equals("string", StringComparison.OrdinalIgnoreCase))
                        {
                            queryBuilder.Append("(max)");
                        }
                        else
                        {
                            queryBuilder.Append($"({column.Value.MaxLength})");
                        }
                    }

                    queryBuilder.Append(", ");
                }
                queryBuilder.Remove(queryBuilder.Length - 2, 2); // Remove the last comma and space
                queryBuilder.Append(");");


                // Execute the SQL command to create the table
                bool flag = await executeSQL(queryBuilder.ToString());
                if (flag)
                {
                    var model = new JsonResult(new
                    {
                        TableName = requestModel.TableName,
                        Columns = requestModel.Columns.Select(c => new
                        {
                            Key = c.Key,
                            Value = "" // Blank field for data insertion
                        }).ToList()
                    });

                    response.IsSuccess = true;
                    response.Message = "Table creation successfull";
                    response.Data = model;
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Table creation failed";
                    response.Data = null;
                    return response;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                response.IsSuccess = false;
                response.Message = "An error occurred";
                response.Data = null;
                return response;
            }
        }

        public async Task<bool> CreateOptionTable(string columnName, string choices, string tableName)
        {

            if (columnName.Equals("string", StringComparison.OrdinalIgnoreCase))
            {
                // If the data type is string, return true without creating any table
                return true;
            }
            // Split choices string by comma to get individual options
            var optionNames = choices.Split(',');

            // Construct the CREATE TABLE query for the option table
            StringBuilder optionTableQueryBuilder = new StringBuilder();
            optionTableQueryBuilder.Append($"CREATE TABLE {tableName.ToLower()}Option (");

            // Append primary key column
            optionTableQueryBuilder.Append($"optionID INT PRIMARY KEY IDENTITY(1,1), ");

            // Append option key column
            optionTableQueryBuilder.Append($"OptionKey INT, ");

            // Append option value column
            optionTableQueryBuilder.Append($"optionValue NVARCHAR(MAX)");

            optionTableQueryBuilder.Append(");");

            // Execute the SQL command to create the table
            bool tableCreated = await executeSQL(optionTableQueryBuilder.ToString());

            if (tableCreated)
            {
                // Construct the INSERT query to add options as separate rows
                StringBuilder insertQueryBuilder = new StringBuilder();
                insertQueryBuilder.Append($"INSERT INTO {columnName.ToLower()}Option1 (OptionKey, OptionValue) VALUES ");

                // Add each option as a separate row with a manually incremented OptionKey
                for (int i = 0; i < optionNames.Length; i++)
                {
                    insertQueryBuilder.Append($"({i + 1}, '{optionNames[i].Trim()}'), ");
                }

                // Remove the last comma and space
                insertQueryBuilder.Remove(insertQueryBuilder.Length - 2, 2);

                // Execute the INSERT query
                bool optionsInserted = await executeSQL(insertQueryBuilder.ToString());

                return optionsInserted;
            }

            return false;
        }

        private async Task<bool> executeSQL(string tableName)
        {
            try
            {
                var connectionString = _configuration["ConnectionStrings:DefaultConnection"];

                using (var connection = new SqlConnection(connectionString)) // Replace "YourConnectionString" with your actual database connection string
                {
                    await connection.OpenAsync();

                    // Query to check if the table exists in the database
                    string query = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TableName", tableName);

                        // Execute the query
                        int count = (int)await command.ExecuteScalarAsync();

                        // If count is greater than 0, the table exists; otherwise, it doesn't exist
                        if (count > 0)
                        {
                            return false;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private async Task<bool> executeSQLTableName(string query)
        {
            try
            {
                var connectionString = _configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
