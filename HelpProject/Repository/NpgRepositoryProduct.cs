using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpProject.Models;
using Npgsql;

namespace HelpProject.Repository
{
    public class NpgRepositoryProduct
    {
        const string connStr = "Host=localhost;Port=5432;Database=DemoEx;Username=postgres;Password=228Egor1337;";

        public List<Product> GetProductFromPostgre()
        {
            try
            {
                List<Product> products = new List<Product>();
                NpgsqlConnection conn = new NpgsqlConnection(connStr);
                conn.Open();

                string sql = "SELECT * FROM products";
                using(var cmd = new NpgsqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product();
                        product.article = reader.GetString(0);
                        product.name = reader.GetString(1);
                        product.unit = reader.GetString(2);
                        product.price = reader.GetInt32(3);
                        product.supplier = reader.GetString(4);
                        product.manufacturer = reader.GetString(5);
                        product.category = reader.GetString(6);
                        product.discountPercent = reader.GetInt32(7);
                        product.stockQuantity = reader.GetInt32(8);
                        product.description = reader.GetString(9);
                        product.picture = reader.IsDBNull(10) ? "" : reader.GetString(10);
                        products.Add(product);
                    }
                    conn.Close();
                    return products;
                }
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }
    }
}
