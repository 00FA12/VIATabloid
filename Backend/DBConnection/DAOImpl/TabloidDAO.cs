using Application.DaoInterfaces;
using Domain.Model;
using Npgsql;

namespace DBConnection;

public class TabloidDAO : ITabloidDao
{
    string _connectionString = Environment.GetEnvironmentVariable("ConnectionString");

    private NpgsqlDataSource GetConnection()
    {
        return NpgsqlDataSource.Create(_connectionString);
    }
    public async Task<Tabloid> CreateTabloidAsync()
    {
        try
        {
            NpgsqlDataSource dataSource = GetConnection();
            await using var command = dataSource.CreateCommand("INSERT INTO tabloid");
            await using var reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();
            var returnedValue = reader.GetInt32(0);
            Tabloid created = new();
            return created;
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public Task<Tabloid> UpdateTabloidAsync(Tabloid tabloid)
    {
        throw new NotImplementedException();
    }

    public async Task<Tabloid?> GetTabloidAsync()
    {
        try
        {
            NpgsqlDataSource dataSource = GetConnection();
            await using var command = dataSource.CreateCommand("SELECT id FROM tabloid");
            await using var reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();
            var returnedValue = reader.GetInt32(0);
            if(returnedValue != 1)
            {
                return null;
            }
            Tabloid tabloid = new();
            await using var cmd = dataSource.CreateCommand("SELECT * FROM department WHERE tabloidid = 1");
            await using var rd = await cmd.ExecuteReaderAsync();
            IList<int> departments = new List<int>();
            while (rd.Read())
            {
                var id = rd.GetInt32(rd.GetOrdinal("id"));
                departments.Add(id);
            }
            tabloid.departments = departments.AsEnumerable();

            return tabloid;
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }        
    }

    // public Task<Tabloid> UpdateTabloidAsync(Tabloid tabloid)
    // {
    //     try
    //     {
    //         NpgsqlConnection connection = GetConnection();
    //         connection.Open();
    //         using var cmd2 = new NpgsqlCommand("SELECT id FROM tabloid", connection);
    //         using var reader3 = cmd2.ExecuteReader();
    //         reader3.Read();
    //         int tabloidId = reader3.GetInt32(reader3.GetOrdinal("id"));

    //         using var cmd = new NpgsqlCommand("SELECT * FROM departments", connection);
    //         using var reader = cmd.ExecuteReader();
    //         List<Department> deps = new List<Department>();
    //         while (reader.Read())
    //         {
    //             var name = reader.GetString(reader.GetOrdinal("name"));
    //             var id = reader.GetInt32(reader.GetOrdinal("id"));
    //             deps.Add(new Department{
    //                 name = name,
    //                 id = id
    //             });
    //         }
    //         connection.Close();
    //         foreach (var dp in deps)
    //         {
    //             using var cmd4 = new NpgsqlCommand("UPDATE department SET tabloidId = ? WHERE id = ?", connection);
    //             cmd4.Parameters.AddWithValue("tabloidId", tabloidId);
    //             cmd4.Parameters.AddWithValue("id", dp.id);
    //             cmd4.ExecuteNonQuery();                
    //         }

    //         Tabloid tb = new();

    //         return Task.FromResult(tb);
    //     }
    //     catch(Exception e)
    //     {
    //         throw new Exception(e.Message);
    //     }
    // }
}
