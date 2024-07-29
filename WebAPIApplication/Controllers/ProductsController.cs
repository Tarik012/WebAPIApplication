using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using WebAPIApplication.Models;

namespace WebAPIApplication.Controllers
{

    [ApiController]
    public class ProductsController : Controller
    {
        private readonly string connectionString;
        public ProductsController(IConfiguration configuration)
        {
            //récupère la connection dans le constructeur
            connectionString = configuration["ConnectionStrings:SqlServerDb"] ?? "";
        }

        [HttpPost]
        [Route("api/[controller]/[action]")]
        public IActionResult CreateProduct(ProductDto productDto)
        {
            try
            {
                //on instancie une nouvelle connection
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "INSERT INTO Products(Name,Brand,Category,Price,Description) " +
                    "VALUES (@Name,@Brand,@Category,@Price,@Description)";

                    using (var sqlCommand = new SqlCommand(sql, connection))
                    {
                        //on remplace les valeurs dans la requête par les valeurs de l'objet productDto
                        sqlCommand.Parameters.AddWithValue("@Name", productDto.Name);
                        sqlCommand.Parameters.AddWithValue("@Brand", productDto.Brand);
                        sqlCommand.Parameters.AddWithValue("@Category", productDto.Category);
                        sqlCommand.Parameters.AddWithValue("@Price", productDto.Price);
                        sqlCommand.Parameters.AddWithValue("@Description", productDto.Description);

                        //on exécute la requête
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Product", "Sorry, but we have an exception : " + ex.Message);
                return BadRequest(ModelState);
            }

            return Ok(productDto);
        }

        [HttpGet]
        [Route("api/[controller]/[action]")]
        public IActionResult GetProducts()
        {
            List<Product> products = new List<Product>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "select * from Products";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        //on exécute la requête
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product();

                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Brand = reader.GetString(2);
                                product.Category = reader.GetString(3);
                                product.Price = reader.GetDecimal(4);
                                product.Description = reader.GetString(5);
                                product.CreatedAt = reader.GetDateTime(6);

                                products.Add(product);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Product", "Sorry, but we have an exception : " + ex.Message);
                return BadRequest(ModelState);
            }

            return Ok(products);
        }

        [HttpGet]
        [Route("api/[controller]/[action]/{id}")]
        public IActionResult GetProductById(int id)
        {
            Product product = new Product();

            try
            {
                using( var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "select * from products where Id =@id";

                    using(var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())  //s'il y a un produit
                            {
                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Brand = reader.GetString(2);
                                product.Category = reader.GetString(3);
                                product.Price = reader.GetDecimal(4);
                                product.Description = reader.GetString(5);
                                product.CreatedAt = reader.GetDateTime(6);
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Product", "Il y a une erreur " + ex.Message);
                return BadRequest(ModelState);
            }

            return Ok(product);
        }

        [HttpPut]
        [Route("api/[controller]/[action]/{id}")]
        public IActionResult UpdateProduct(int id, ProductDto productDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "update Products set Name=@name, Brand=@brand, Category=@category, Price=@price," +
                        "Description=@description where Id = @id";

                    using(var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", productDto.Name);
                        command.Parameters.AddWithValue("@Brand", productDto.Brand);
                        command.Parameters.AddWithValue("@Category", productDto.Category);
                        command.Parameters.AddWithValue("@Price", productDto.Price);
                        command.Parameters.AddWithValue("@Description", productDto.Description);
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Product", "Il y a une erreur " + ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpDelete]
        [Route("api/[controller]/[action]/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "DELETE Products where Id=@id";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Product", "Il y a une erreur " + ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
