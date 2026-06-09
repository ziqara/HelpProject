using System;
using System.Collections.Generic;
using HelpProject.Models;
using Npgsql;

namespace HelpProject.Repository
{
    public class NpgRepositoryProduct
    {
        const string connStr = "Host=localhost;Port=5433;Database=DemoEx2;Username=postgres;Password=2509Egor1337;";

        // Ваш существующий метод чтения
        public List<Product> GetProductFromPostgre()
        {
            List<Product> products = new List<Product>();
            string sql = "SELECT article, name, unit, price, supplier, manufacturer, category, discount_percent, stock_quantity, description, picture FROM products";

            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            article = reader.GetString(0),
                            name = reader.GetString(1),
                            unit = reader.GetString(2),
                            price = reader.GetInt32(3),
                            supplier = reader.GetString(4),
                            manufacturer = reader.GetString(5),
                            category = reader.GetString(6),
                            discountPercent = reader.GetInt32(7),
                            stockQuantity = reader.GetInt32(8),
                            description = reader.GetString(9),
                            picture = reader.IsDBNull(10) ? "" : reader.GetString(10)
                        };
                        products.Add(product);
                    }
                }
            }
            return products;
        }

        // МЕТОД ДОБАВЛЕНИЯ
        public void AddProduct(Product product)
        {
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string sql = @"INSERT INTO products (article, name, unit, price, supplier, manufacturer, category, discount_percent, stock_quantity, description, picture) 
                               VALUES (@article, @name, @unit, @price, @supplier, @manufacturer, @category, @discount, @stock, @desc, @pic)";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@article", product.article);
                    cmd.Parameters.AddWithValue("@name", product.name);
                    cmd.Parameters.AddWithValue("@unit", product.unit);
                    cmd.Parameters.AddWithValue("@price", product.price);
                    cmd.Parameters.AddWithValue("@supplier", product.supplier);
                    cmd.Parameters.AddWithValue("@manufacturer", product.manufacturer);
                    cmd.Parameters.AddWithValue("@category", product.category);
                    cmd.Parameters.AddWithValue("@discount", product.discountPercent);
                    cmd.Parameters.AddWithValue("@stock", product.stockQuantity);
                    cmd.Parameters.AddWithValue("@desc", product.description);
                    cmd.Parameters.AddWithValue("@pic", (object)product.picture ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // МЕТОД РЕДАКТИРОВАНИЯ
        public void EditProduct(Product product)
        {
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string sql = @"UPDATE products SET name = @name, unit = @unit, price = @price, supplier = @supplier, 
                               manufacturer = @manufacturer, category = @category, discount_percent = @discount, 
                               stock_quantity = @stock, description = @desc, picture = @pic 
                               WHERE article = @article";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@article", product.article);
                    cmd.Parameters.AddWithValue("@name", product.name);
                    cmd.Parameters.AddWithValue("@unit", product.unit);
                    cmd.Parameters.AddWithValue("@price", product.price);
                    cmd.Parameters.AddWithValue("@supplier", product.supplier);
                    cmd.Parameters.AddWithValue("@manufacturer", product.manufacturer);
                    cmd.Parameters.AddWithValue("@category", product.category);
                    cmd.Parameters.AddWithValue("@discount", product.discountPercent);
                    cmd.Parameters.AddWithValue("@stock", product.stockQuantity);
                    cmd.Parameters.AddWithValue("@desc", product.description);
                    cmd.Parameters.AddWithValue("@pic", (object)product.picture ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // МЕТОД УДАЛЕНИЯ
        public void RemoveProduct(Product product)
        {
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string sql = "DELETE FROM products WHERE article = @article";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@article", product.article);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ПРОВЕРКА НАЛИЧИЯ В ЗАКАЗАХ (чтобы база не выдавала ошибку Foreign Key при удалении)
        public bool HasOrder(string article)
        {
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM order_products WHERE product_article = @article";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@article", article);
                    long count = (long)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}