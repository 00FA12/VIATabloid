using Application.DaoInterfaces;
using Domain.DTOs;
using Domain.Model;
using System;
using Npgsql;

namespace DBConnection;

public class StoryDAO : IStoryDAO
{
    string connectionString = "Host=localhost;Port:5432;Database=postgres;Username=postgres;Password=password";

    private NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(connectionString);
    }

    public Task<Story> CreateStoryAsync(StoryDTO storyDTO)
    {
        try
        {
            NpgsqlConnection connection = GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand("INSERT INTO story (title, body) VALUES (?, ?) RETURNING id", connection);
            cmd.Parameters.AddWithValue("title", storyDTO.title);
            cmd.Parameters.AddWithValue("body", storyDTO.body);
            using var reader = cmd.ExecuteReader();
            var returnedValue = reader["id"];
            Story created = new()
            {
                title = storyDTO.title,
                body = storyDTO.body,
                id = (int)returnedValue
            };
            connection.Close();
            return Task.FromResult(created);
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Task<Story> DeleteStoryAsync(int storyId)
    {
        try
        {
            NpgsqlConnection connection = GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM story WHERE id=?", connection);
            cmd.Parameters.AddWithValue("id", storyId);
            using var reader = cmd.ExecuteReader();
            var stTitle = reader["title"];
            var stBody = reader["body"];
            using var cmd2 = new NpgsqlCommand("DELETE FROM story WHERE id=?", connection);
            cmd2.Parameters.AddWithValue("id", storyId);
            cmd2.ExecuteNonQuery();
            Story created = new Story{
                title = (string)stTitle,
                body = (string)stBody,
                id = storyId
            };
            connection.Close();
            return Task.FromResult(created);
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Task<IEnumerable<Story>> GetAllStoriesAsync()
    {
        try
        {
            var stories = new List<Story>();
            NpgsqlConnection connection = GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM story", connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var title = reader.GetString(reader.GetOrdinal("title"));
                var body = reader.GetString(reader.GetOrdinal("body"));
                var id = reader.GetInt32(reader.GetOrdinal("id"));
                stories.Add(new Story{
                    title = title,
                    body = title,
                    id = id
                });
            }
            connection.Close();
            return Task.FromResult(stories.AsEnumerable());
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Task<Story> GetStoryByIdAsync(int storyId)
    {
        try
        {
            NpgsqlConnection connection = GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM story WHERE id = ?", connection);
            cmd.Parameters.AddWithValue("id", storyId);
            Story story = new();

            using var reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                if(reader.GetInt32(reader.GetOrdinal("id")) == storyId)
                {
                    var title = reader.GetString(reader.GetOrdinal("title"));
                    var body = reader.GetString(reader.GetOrdinal("body"));
                    var id = reader.GetInt32(reader.GetOrdinal("id"));
                    story = new Story{
                    title = title,
                    body = title,
                    id = id
                    };
                }
            }
            connection.Close();
            return Task.FromResult(story);
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
