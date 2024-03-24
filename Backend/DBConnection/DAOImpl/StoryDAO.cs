using Application.DaoInterfaces;
using Domain.DTOs;
using Domain.Model;
using System;
using Npgsql;
using NpgsqlTypes;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace DBConnection;

public class StoryDAO : IStoryDAO
{
    string connectionString = "Username=postgres;Password=password;Server=localhost;Port=5432;Database=postgres;SearchPath=viatabloid";

    private NpgsqlDataSource GetConnection()
    {
        return NpgsqlDataSource.Create(connectionString);
    }

    public Task<Story> CreateStoryAsync(StoryDTO storyDTO)
    {
        try
        {
            int id = 0;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO story (title, body) VALUES (@title, @body) RETURNING id", conn))
                {
                    cmd.Parameters.AddWithValue("@title", storyDTO.title);
                    cmd.Parameters.AddWithValue("@body", storyDTO.body);
                    id = (int)cmd.ExecuteScalar()!;

                }
                conn.Close();
            }
            Story created = new()
            {
                title = storyDTO.title,
                body = storyDTO.body,
                id = id
            };
            return Task.FromResult(created);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Task<Story> DeleteStoryAsync(int storyId)
    {
        string title;
        string body;
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM story WHERE id=@value1", conn))
                {
                    cmd.Parameters.AddWithValue("@value1", storyId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        title = reader.GetString(reader.GetOrdinal("title"));
                        body = reader.GetString(reader.GetOrdinal("body"));
                        conn.Close();
                    }
                }
                conn.Open();
                using (var cmd2 = new NpgsqlCommand("DELETE FROM story WHERE id=@value1", conn))
                {
                    cmd2.Parameters.AddWithValue("@value1", storyId);
                    cmd2.ExecuteNonQuery();
                    Story created = new Story
                    {
                        title = title,
                        body = body,
                        id = storyId
                    };
                    conn.Close();
                    return Task.FromResult(created);
                }

            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<IEnumerable<Story>> GetAllStoriesAsync()
    {
        try
        {
            NpgsqlDataSource dataSource = GetConnection();
            await using var command = dataSource.CreateCommand("SELECT * FROM story");
            await using var reader = await command.ExecuteReaderAsync();
            IList<Story> created = new List<Story>();
            while (reader.Read())
            {
                created.Add(new Story
                {
                    id = reader.GetInt32(0),
                    title = reader.GetString(1),
                    body = reader.GetString(2)
                });
            }
            return created.AsEnumerable();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Task<Story?>? GetStoryByIdAsync(int storyId)
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM story WHERE id = @value1", conn))
                {
                    cmd.Parameters.AddWithValue("@value1", storyId);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        try
                        {
                            var id = reader.GetInt32(reader.GetOrdinal("id"));
                            var title = reader.GetString(reader.GetOrdinal("title"));
                            var body = reader.GetString(reader.GetOrdinal("body"));
                            Story? created = new()
                            {
                                title = title,
                                body = body,
                                id = id
                            };
                            conn.Close();
                            return Task.FromResult(created)!;
                        }
                        catch
                        {
                            return null;
                        }

                    }
                }
            }


        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
