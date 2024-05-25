using Application.DaoInterfaces;
using Domain.DTOs;
using Domain.Model;
using Npgsql;

namespace DBConnection;

public class DepartmentDAO : IDepartmentDAO
{

    string _connectionString = Environment.GetEnvironmentVariable("ConnectionString");

    private NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
    public async Task<Department> CreateDepartmentAsync(DepartmentDTO department)
    {
        try
        {
            NpgsqlConnection connection = GetConnection();
            connection.Open();
            using (var cmd = new NpgsqlCommand("INSERT INTO department (name, tabloidid) VALUES (@value1, 1) RETURNING id", connection))
            {
                cmd.Parameters.AddWithValue("@value1", department.name);
                // using var reader = cmd.ExecuteReader();
                var returnedValue = await cmd.ExecuteScalarAsync();
                Department created = new()
                {
                    name = department.name,
                    id = (int)returnedValue,
                    stories = new List<int>()
                };
                // connection.Close();
                return created;
            }
        }
        catch (Exception e)
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
            using var cmd = new NpgsqlCommand("SELECT * FROM department WHERE id=@value1", connection);
            cmd.Parameters.AddWithValue("@value1", depId);
            using var reader = cmd.ExecuteReader();
            reader.Read();
            var name = reader.GetString(reader.GetOrdinal("name"));
            connection.Close();
            connection.Open();
            using var cmd2 = new NpgsqlCommand("DELETE FROM department WHERE id=@value1", connection);
            cmd2.Parameters.AddWithValue("@value1", depId);
            cmd2.ExecuteNonQuery();
            Department created = new Department
            {
                name = (string)name,
                id = depId
            };
            // connection.Close();
            return Task.FromResult(created);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<Department> GetDepartmentByIdAsync(int depId)
    {
        try
        {
            NpgsqlConnection connection = GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM department WHERE id = @value1", connection);
            cmd.Parameters.AddWithValue("@value1", depId);

            Department dep = new();


            using var reader = await cmd.ExecuteReaderAsync();
            await reader.ReadAsync();
            if (reader.GetInt32(reader.GetOrdinal("id")) == depId)
            {
                var title = reader.GetString(reader.GetOrdinal("name"));
                var id = reader.GetInt32(reader.GetOrdinal("id"));
                dep = new Department
                {
                    name = title,
                    id = id
                };
            }
            connection.Close();


            connection.Open();
            List<int> sts = new List<int>();
            using var command = new NpgsqlCommand("SELECT id FROM story WHERE departmentid = @value1", connection);
            command.Parameters.AddWithValue("@value1", depId);
            using var rd = await command.ExecuteReaderAsync();
            while(await rd.ReadAsync())
            {
                var id = rd.GetInt32(rd.GetOrdinal("id"));
                sts.Add(id);
            }
            dep.stories = sts.AsEnumerable();
            return dep;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<IEnumerable<Department>> GetDepartmentsAsync()
    {
        try
        {
            var ids = new List<int>();
            NpgsqlConnection connection = GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT id FROM department", connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int depId = reader.GetInt32(reader.GetOrdinal("id"));
                ids.Add(depId);
            }
            connection.Close();
            var deps = new List<Department>();
            for (int i = 0; i < ids.Count; i++)
            {   
                Department department = await GetDepartmentByIdAsync(ids.ElementAt(i));
                deps.Add(department);
            }

            return deps.AsEnumerable();
        }
        catch (Exception e)
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
            using var cmd = new NpgsqlCommand("UPDATE department SET name = @value1 WHERE id = @value2", connection);
            cmd.Parameters.AddWithValue("@value1", department.name);
            cmd.Parameters.AddWithValue("@value2", department.id);
            cmd.ExecuteNonQuery();
            connection.Close();
            foreach (var st in department.stories)
            {
                connection.Open();
                using var cmd3 = new NpgsqlCommand("UPDATE story SET departmentId = @value1 WHERE id = @value2", connection);
                cmd3.Parameters.AddWithValue("@value1", department.id);
                cmd3.Parameters.AddWithValue("@value2", st);
                cmd3.ExecuteNonQuery();
                connection.Close();
            }
            connection.Open();
            using var cmd2 = new NpgsqlCommand("SELECT * FROM department WHERE id = @value1", connection);
            cmd2.Parameters.AddWithValue("@value1", department.id);

            Department dep = new();


            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetInt32(reader.GetOrdinal("id")) == department.id)
                {
                    var title = reader.GetString(reader.GetOrdinal("name"));
                    var id = reader.GetInt32(reader.GetOrdinal("id"));
                    dep = new Department
                    {
                        name = title,
                        id = id
                    };
                }
            }
            connection.Close();
            return Task.FromResult(dep);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
