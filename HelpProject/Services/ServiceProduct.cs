using HelpProject.Models;
using HelpProject.Repository;
using System.Collections.Generic;

namespace HelpProject.Services
{
    public class ServiceProduct
    {
        private readonly NpgRepositoryProduct repository_;

        public ServiceProduct(NpgRepositoryProduct repository)
        {
            repository_ = repository;
        }

        public List<Product> GetProducts() { return repository_.GetProductFromPostgre(); }

        public void AddProduct(Product product) { repository_.AddProduct(product); }

        public void EditProduct(Product product) { repository_.EditProduct(product); }

        public void RemoveProduct(Product product) { repository_.RemoveProduct(product); }

        public bool HasOrder(string article) { return repository_.HasOrder(article); }
    }
}