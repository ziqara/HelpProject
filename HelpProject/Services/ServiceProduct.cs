using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpProject.Models;
using HelpProject.Repository;

namespace HelpProject.Services
{
    public class ServiceProduct
    {
        NpgRepositoryProduct repository_ = new NpgRepositoryProduct();
        public ServiceProduct(NpgRepositoryProduct repository) 
        { 
            repository_ = repository;
        }

        public List<Product> GetProducts() { return repository_.GetProductFromPostgre(); }
    }
}
