﻿using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService()
    {
        _connectionString = BuildConnectionString();
    }

    /// <summary>
    /// Method for building a connection string.
    /// </summary>
    /// <returns>Returns a connection string.</returns>
    private string BuildConnectionString()
    {
        var userId = ConfigurationManager.AppSettings["DbUserId"];
        var password = ConfigurationManager.AppSettings["DbPassword"];
        var dataSource = ConfigurationManager.AppSettings["DbDataSource"];

        return $"User Id={userId};Password={password};Data Source={dataSource}";
    }

    public IDbConnection GetConnection()
    {
        return new OracleConnection(_connectionString);
    }

    /// <summary>
    /// Executes a stored procedure.
    /// </summary>
    public int ExecuteProcedure(string procedureName, Action<OracleCommand> configureCommand)
    {
        using (var connection = GetConnection() as OracleConnection)
        {
            connection.Open();
            using (var command = new OracleCommand(procedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                configureCommand?.Invoke(command);
                return command.ExecuteNonQuery();
            }
        }
    }
    public List<T> ExecuteSelect<T>(string query, Func<IDataReader, T> map, Action<OracleCommand> configureCommand = null)
    {
        using (var connection = GetConnection() as OracleConnection)
        {
            connection.Open();
            using (var command = new OracleCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                configureCommand?.Invoke(command);

                using (var reader = command.ExecuteReader())
                {
                    var results = new List<T>();
                    while (reader.Read())
                    {
                        try
                        {
                            var item = map(reader);
                            results.Add(item);
                        }
                        catch (Exception ex)
                        {
                            // Log the error with details from IDataReader if needed
                            Console.WriteLine($"Error processing row: {ex.Message}");
                            // Optionally log the content of the reader
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write($"{reader.GetName(i)}: {(reader.IsDBNull(i) ? "NULL" : reader.GetValue(i).ToString())} | ");
                            }
                            Console.WriteLine();
                        }
                    }
                    return results;
                }
            }
        }
    }

    /// <summary>
    /// Executes a non-query SQL command (INSERT, UPDATE, DELETE).
    /// </summary>
    public int ExecuteNonQuery(string query, Action<OracleCommand> configureCommand = null)
    {
        using (var connection = GetConnection() as OracleConnection)
        {
            connection.Open();
            using (var command = new OracleCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                configureCommand?.Invoke(command);
                return command.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Executes a scalar query and returns the result.
    /// </summary>
    public object ExecuteScalar(string query, Action<OracleCommand> configureCommand = null)
    {
        using (var connection = GetConnection() as OracleConnection)
        {
            connection.Open();
            using (var command = new OracleCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                configureCommand?.Invoke(command);
                return command.ExecuteScalar();
            }
        }
    }
}