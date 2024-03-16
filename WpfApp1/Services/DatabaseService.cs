using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Utils;

namespace WpfApp1.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT T.Product_Id, T.seller_id, S.seller_name, T.product_name, T.Quantity, T.Price, CT.category_name, PT.category_Id " +
                               "FROM Товар T " +
                               "JOIN [Продавцы] S ON T.seller_id = S.seller_id " +
                               "JOIN [Категория_товара] PT ON T.category_id = PT.category_id " +
                               "JOIN [Категория_товара] CT ON PT.category_Id = CT.category_Id";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = reader.GetInt32(0),
                        SellerId = reader.GetInt32(1),
                        SellerName = reader.GetString(2),
                        ProductName = reader.GetString(3),
                        Quantity = reader.IsDBNull(4) ? null : (int?)reader.GetInt32(4),
                        Price = reader.IsDBNull(5) ? null : (int?)reader.GetInt32(5),
                        CategoryName = reader.GetString(6),
                        CategoryId = reader.GetInt32(7)
                    });
                }

                reader.Close();
            }

            return products;
        }

        public void SaveProducts(IEnumerable<Product> products)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                foreach (var product in products)
                {
                    string query = "UPDATE Товар SET product_name = @ProductName, Price = @Price, Quantity = @Quantity WHERE Product_Id = @ProductId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@Price", (object)product.Price ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Quantity", (object)product.Quantity ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ProductId", product.ProductId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteProducts(IEnumerable<Product> products)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                foreach (var product in products)
                {
                    string query = "DELETE FROM Товар WHERE Product_Id = @ProductId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductId", product.ProductId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<PickupPoint> GetPickupPoints()
        {
            List<PickupPoint> pickupPoints = new List<PickupPoint>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT point_id, pickup_points_location, working_hours FROM Пункты_выдачи";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    pickupPoints.Add(new PickupPoint
                    {
                        PointId = reader.GetInt32(0),
                        Location = reader.GetString(1),
                        WorkingHours = reader.GetString(2)
                    });
                }

                reader.Close();
            }

            return pickupPoints;
        }

        public Employee Authenticate(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT Employee_Id, Employee_Name, Employee_Surname, Employee_Patronomic, Employee_Location, Employee_INN, Employee_Phone_Number, Employee_tag_id, point_id " +
                               "FROM Сотрудники " +
                               "WHERE Employee_Login = @Login AND Employee_Password = @HashedPassword";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@HashedPassword", PasswordHasher.HashPassword(password));

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Employee
                    {
                        EmployeeId = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        MiddleName = reader.GetString(3),
                        Location = reader.GetString(4),
                        Inn = reader.GetInt32(5),
                        PhoneNumber = reader.GetString(6),
                        EmployeeTagId = reader.GetInt32(7),
                        PickupPointId = reader.GetInt32(8)
                    };
                }

                reader.Close();
            }

            return null;
        }
    }
}
