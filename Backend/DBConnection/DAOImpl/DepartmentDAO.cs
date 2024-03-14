using Application.DaoInterfaces;
using Domain.DTOs;
using Domain.Model;
using Npgsql;

namespace DBConnection;

public class DepartmentDAO : IDepartmentDAO
{
    string connectionString = "Host=localhost;Port:5432;Database=postgres;Username=postgres;Password=password";

    private NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(connectionString);
    }

    public Task<Department> CreateDepartmentAsync(DepartmentDTO department)
    {
        try
        {
            NpgsqlConnection connection = GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand("INSERT INTO department (name) VALUES (?) RETURNING id", connection);
            cmd.Parameters.AddWithValue("name", department.name);
            using var reader = cmd.ExecuteReader();
            var returnedValue = reader["id"];
            Department created = new()
            {
                name = department.name,
                id = (int)returnedValue,
                stories = new List<int>()
            };
            connection.Close();
            return Task.FromResult(created);
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Task<Department> DeleteDepartmentAsync(int depId)
    {
        try
        {
            NpgsqlConnection connection = GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM department WHERE id=?", connection);
            cmd.Parameters.AddWithValue("id", depId);
            using var reader = cmd.ExecuteReader();
            var name = reader["name"];
            using var cmd2 = new NpgsqlCommand("DELETE FROM story WHERE id=?", connection);
            cmd2.Parameters.AddWithValue("id", depId);
            cmd2.ExecuteNonQuery();
            Department created = new Department{
                name = (string)name,
                id = depId
            };
            connection.Close();
            return Task.FromResult(created);
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Task<Department> GetDepartmentByIdAsync(int depId)
    {
        try
        {
            NpgsqlConnection connection = GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM department WHERE id = ?", connection);
            cmd.Parameters.AddWithValue("id", depId);
            
            Department dep = new();
            

            using var reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                if(reader.GetInt32(reader.GetOrdinal("id")) == depId)
                {
                    var title = reader.GetString(reader.GetOrdinal("name"));
                    var id = reader.GetInt32(reader.GetOrdinal("id"));
                    dep = new Department{
                    name = title,
                    id = id
                    };
                }
            }
            connection.Close();
            return Task.FromResult(dep);
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }    
    }

    public Task<IEnumerable<Department>> GetDepartmentsAsync()
    {
        try
        {
            var deps = new List<Department>();
            NpgsqlConnection connection = GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM department", connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var name = reader.GetString(reader.GetOrdinal("name"));
                var id = reader.GetInt32(reader.GetOrdinal("id"));
                deps.Add(new Department{
                    name = name,
                    id = id
                });
            }
            connection.Close();
            return Task.FromResult(deps.AsEnumerable());
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Task<Department> UpdateDepartmentAsync(Department department)
    {
        try
        {
            NpgsqlConnection connection = GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand("UPDATE department SET name = ? WHERE id = ?", connection);
            cmd.Parameters.AddWithValue("name", department.name);
            cmd.Parameters.AddWithValue("id", department.id);
            cmd.ExecuteNonQuery();
            foreach (var st in department.stories)
            {
                using var cmd3 = new NpgsqlCommand("UPDATE story SET departmentId = ? WHERE id = ?", connection);
                cmd3.Parameters.AddWithValue("departmentId", department.id);
                cmd3.Parameters.AddWithValue("id", st);
                cmd3.ExecuteNonQuery();                
            }
            using var cmd2 = new NpgsqlCommand("SELECT * FROM department WHERE id = ?", connection);
            cmd2.Parameters.AddWithValue("id", department.id);
            
            Department dep = new();
            

            using var reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                if(reader.GetInt32(reader.GetOrdinal("id")) == department.id)
                {
                    var title = reader.GetString(reader.GetOrdinal("name"));
                    var id = reader.GetInt32(reader.GetOrdinal("id"));
                    dep = new Department{
                    name = title,
                    id = id
                    };
                }
            }
            return Task.FromResult(dep);
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
