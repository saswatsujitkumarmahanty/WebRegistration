using Application.Dto;
using Application.Interfaces;
using Domain.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            // Pulls directly from appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            var employees = new List<Employee>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT Id, Name, Gender, Email, Phone, Age, Salary FROM Employees";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        employees.Add(new Employee
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Gender = reader.GetString(2),
                            Email = reader.GetString(3),
                            Phone = reader.GetString(4),
                            Age = reader.GetInt32(5),
                            Salary = reader.GetDecimal(6)
                        });
                    }
                }
            }
            return employees;
        }

        public async Task<Employee?> GetEmployeeByIdAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT Id, Name, Gender, Email, Phone, Age, Salary FROM Employees WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Employee
                            {
                                Id = reader.GetGuid(0),
                                Name = reader.GetString(1),
                                Gender = reader.GetString(2),
                                Email = reader.GetString(3),
                                Phone = reader.GetString(4),
                                Age = reader.GetInt32(5),
                                Salary = reader.GetDecimal(6)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task<Employee> AddEmployeeAsync(AddEmployeeDto addEmployeeDto)
        {
            var newId = Guid.NewGuid();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Employees (Id, Name, Gender, Email, Phone, Age, Salary) 
                                VALUES (@Id, @Name, @Gender, @Email, @Phone, @Age, @Salary)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", newId);
                    command.Parameters.AddWithValue("@Name", addEmployeeDto.Name);
                    command.Parameters.AddWithValue("@Gender", addEmployeeDto.Gender);
                    command.Parameters.AddWithValue("@Email", addEmployeeDto.Email);
                    command.Parameters.AddWithValue("@Phone", addEmployeeDto.Phone);
                    command.Parameters.AddWithValue("@Age", addEmployeeDto.Age);
                    command.Parameters.AddWithValue("@Salary", addEmployeeDto.Salary);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return new Employee
            {
                Id = newId,
                Name = addEmployeeDto.Name,
                Gender = addEmployeeDto.Gender,
                Email = addEmployeeDto.Email,
                Phone = addEmployeeDto.Phone,
                Age = addEmployeeDto.Age,
                Salary = addEmployeeDto.Salary
            };
        }

        public async Task<UpdateEmployeeDto?> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            int rowsAffected;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"UPDATE Employees 
                                SET Name = @Name, Gender = @Gender, Email = @Email, Phone = @Phone, Age = @Age, Salary = @Salary 
                                WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Name", updateEmployeeDto.Name);
                    command.Parameters.AddWithValue("@Gender", updateEmployeeDto.Gender);
                    command.Parameters.AddWithValue("@Email", updateEmployeeDto.Email);
                    command.Parameters.AddWithValue("@Phone", updateEmployeeDto.Phone);
                    command.Parameters.AddWithValue("@Age", updateEmployeeDto.Age);
                    command.Parameters.AddWithValue("@Salary", updateEmployeeDto.Salary);

                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }

            if (rowsAffected == 0) return null;

            updateEmployeeDto.Id = id;
            return updateEmployeeDto;
        }

        public async Task<bool> DeleteEmployeeAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM Employees WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }
}