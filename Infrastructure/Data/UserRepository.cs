using Application.Interfaces;
using Domain.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT COUNT(1) FROM Users WHERE Email = @Email";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    int count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return count > 0;
                }
            }
        }

        public async Task<User> AddUserAsync(User user)
        {
            if (user.Id == Guid.Empty)
            {
                user.Id = Guid.NewGuid();
            }

            if (string.IsNullOrWhiteSpace(user.Role))
            {
                user.Role = "User"; // default role at signup
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Users (Id, Name, Gender, Email, Phone, Age, PasswordHash, Role) 
                                 VALUES (@Id, @Name, @Gender, @Email, @Phone, @Age, @PasswordHash, @Role)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.Parameters.AddWithValue("@Name", user.Name ?? string.Empty);
                    command.Parameters.AddWithValue("@Gender", user.Gender ?? string.Empty);
                    command.Parameters.AddWithValue("@Email", user.Email ?? string.Empty);
                    command.Parameters.AddWithValue("@Phone", user.Phone ?? string.Empty);
                    command.Parameters.AddWithValue("@Age", user.Age);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash ?? string.Empty);
                    command.Parameters.AddWithValue("@Role", user.Role);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Update this query to match your actual database column names.
                // I am assuming you have PasswordHash and Email columns.
                string query = @"
            UPDATE Users 
            SET PasswordHash = @PasswordHash
                WHERE Email = @Email";

                using (var command = new SqlCommand(query, connection))
                {
                    // The ?? DBNull.Value handles cases where the property might be null
                    command.Parameters.AddWithValue("@PasswordHash", (object)user.PasswordHash ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", user.Email);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email, string phone)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT Id, Name, Gender, Email, Phone, Age, Role FROM Users WHERE Email = @Email AND Phone = @Phone";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Phone", phone);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                Id = reader.GetGuid(0),
                                Name = reader.GetString(1),
                                Gender = reader.GetString(2),
                                Email = reader.GetString(3),
                                Phone = reader.GetString(4),
                                Age = reader.GetInt32(5),
                                Role = reader.IsDBNull(6) ? "User" : reader.GetString(6)
                            };
                        }
                    }
                }
            }
            return null;
        }



        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT Id, Name, Gender, Email, Phone, Age, PasswordHash, Role FROM Users WHERE Email = @Email";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                Id = reader.GetGuid(0),
                                Name = reader.GetString(1),
                                Gender = reader.GetString(2),
                                Email = reader.GetString(3),
                                Phone = reader.GetString(4),
                                Age = reader.GetInt32(5),
                                PasswordHash = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                Role = reader.IsDBNull(7) ? "User" : reader.GetString(7)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task<bool> UpdateUserNameAsync(Guid id, string Name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE Users SET Name = @Name WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> UpdateUserRoleAsync(Guid id, string role)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE Users SET Role = @Role WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<int> CountAdminsAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT COUNT(1) FROM Users WHERE Role = @Role";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Role", "Admin");
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT Id, Name, Gender, Email, Phone, Age, Role FROM Users WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                Id = reader.GetGuid(0),
                                Name = reader.GetString(1),
                                Gender = reader.GetString(2),
                                Email = reader.GetString(3),
                                Phone = reader.GetString(4),
                                Age = reader.GetInt32(5),
                                Role = reader.IsDBNull(6) ? "User" : reader.GetString(6)
                            };
                        }
                    }
                }
            }
            return null;
        }

    }
}