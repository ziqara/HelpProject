using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HelpProject.Models;
using HelpProject.Repository;
using HelpProject.Services;

namespace HelpForm
{
    public partial class Form1 : Form
    {
        private List<Product> products_;
        private ServiceProduct service_;
        private User currentUser;

        public Form1(User user)
        {
            InitializeComponent();
            NpgRepositoryProduct npgRepositoryProduct = new NpgRepositoryProduct();
            service_ = new ServiceProduct(npgRepositoryProduct);
            products_ = service_.GetProducts();
            FillList(products_);
            currentUser = user;
            TitleForForm(user);
        }

        public void FillList(List<Product> product)
        {
            listBoxProduct.DataSource = null;
            listBoxProduct.DataSource = product;
            listBoxProduct.DisplayMember = "article";
        }

        private void listBoxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            var product = listBoxProduct.SelectedItem as Product;
            card1.ShowInfoCard(product);
        }

        public void TitleForForm(User user)
        {
            if (user == null)
            {
                this.Text = $"Товары для гостя";
            }
            else
            {
                this.Text = $"Товары - {user.name} - {user.role}";
            }
        }
    }
}
