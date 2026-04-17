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

namespace HelpForm
{
    public partial class card : UserControl
    {
        public card()
        {
            InitializeComponent();
        }

        public void ShowInfoCard (Product product)
        {
            lblName.Text = product.name;
            lblCategory.Text = product.category;
            lblDescttiption.Text = product.description;
            lblManufacturer.Text = product.manufacturer;
            lblStockQuantity.Text = product.stockQuantity.ToString();
            lblUnit.Text = product.unit;
            lblSupplier.Text = product.supplier;
            lblPrice.Text = product.price.ToString();

            if (string.IsNullOrWhiteSpace(product.picture))
            {
                pictureBox.Load("picture.png");
            }
            else 
            {
                pictureBox.Load(product.picture);
            }
        }
    }
}
