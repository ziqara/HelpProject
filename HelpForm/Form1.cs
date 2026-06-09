using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

            currentUser = user;
            TitleForForm(user);
        }

        // Вызываем инициализацию списков при загрузке формы
        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshData();

            // Инициализация ComboBox для сортировки
            cbxSort.Items.Clear();
            cbxSort.Items.Add("Без сортировки");
            cbxSort.Items.Add("Цена (по возрастанию)");
            cbxSort.Items.Add("Цена (по убыванию)");
            cbxSort.Items.Add("Количество (по возрастанию)");
            cbxSort.Items.Add("Количество (по убыванию)");
            cbxSort.SelectedIndex = 0;

            // Заполнение фильтра поставщиков уникальными значениями из базы
            List<string> suppliers = new List<string>();
            suppliers.Add("Все поставщики");
            foreach (Product p in products_)
            {
                if (!string.IsNullOrEmpty(p.supplier) && !suppliers.Contains(p.supplier))
                {
                    suppliers.Add(p.supplier);
                }
            }
            cbxSupplier.DataSource = suppliers;

            // Проверяем роль пользователя после загрузки данных
            CheckRole();
        }

        private void RefreshData()
        {
            products_ = service_.GetProducts();
            SearchAndFilter(); // Вместо прямой заливки вызываем объединенный метод поиска/фильтрации
        }

        public void FillList(List<Product> product)
        {
            listBoxProduct.DataSource = null;
            listBoxProduct.DataSource = product;
            listBoxProduct.DisplayMember = "article";
        }

        private void listBoxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxProduct.SelectedItem is Product product)
            {
                card1.ShowInfoCard(product);
            }
        }

        // ОБЪЕДИНЕННЫЙ МЕТОД: ПОИСК, ФИЛЬТРАЦИЯ И СОРТИРОВКА
        private void SearchAndFilter()
        {
            if (products_ == null) return;

            // 1. ПОИСК ПО НАЗВАНИЮ (из текстового поля txtSearch)
            string search = txtSearch.Text.Trim().ToLower();
            List<Product> searchedProducts = new List<Product>();

            if (!string.IsNullOrEmpty(search))
            {
                foreach (Product p in products_)
                {
                    // Ищем по названию, как ты просил (при желании сюда можно дописать p.article или p.description)
                    if (p.name != null && p.name.ToLower().Contains(search))
                    {
                        searchedProducts.Add(p);
                    }
                }
            }
            else
            {
                searchedProducts = products_;
            }

            // 2. ФИЛЬТРАЦИЯ ПО ПОСТАВЩИКУ (из cbxSupplier)
            List<Product> filteredProducts = new List<Product>();
            if (cbxSupplier.SelectedItem == null || cbxSupplier.SelectedItem.ToString() == "Все поставщики")
            {
                filteredProducts = searchedProducts;
            }
            else
            {
                string selectedSupplier = cbxSupplier.SelectedItem.ToString();
                foreach (Product p in searchedProducts)
                {
                    if (p.supplier == selectedSupplier)
                    {
                        filteredProducts.Add(p);
                    }
                }
            }

            // 3. СОРТИРОВКА ПО ЦЕНЕ И КОЛИЧЕСТВУ (из cbxSort)
            if (cbxSort.SelectedItem != null)
            {
                string sortOption = cbxSort.SelectedItem.ToString();

                if (sortOption == "Цена (по возрастанию)")
                {
                    filteredProducts = filteredProducts.OrderBy(p => p.price).ToList();
                }
                else if (sortOption == "Цена (по убыванию)")
                {
                    filteredProducts = filteredProducts.OrderByDescending(p => p.price).ToList();
                }
                else if (sortOption == "Количество (по возрастанию)")
                {
                    filteredProducts = filteredProducts.OrderBy(p => p.stockQuantity).ToList();
                }
                else if (sortOption == "Количество (по убыванию)")
                {
                    filteredProducts = filteredProducts.OrderByDescending(p => p.stockQuantity).ToList();
                }
            }

            // Выводим итоговый отсортированный и отфильтрованный список в ListBox
            FillList(filteredProducts);
        }

        // СОБЫТИЯ ИЗМЕНЕНИЯ ЭЛЕМЕНТОВ ИНТЕРФЕЙСА
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchAndFilter();
        }

        private void cbxSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchAndFilter();
        }

        private void cbxSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchAndFilter();
        }

        // ПРОВЕРКА РОЛЕЙ ПОЛЬЗОВАТЕЛЕЙ (как в твоем примере)
        private void CheckRole()
        {
            if (currentUser != null)
            {
                if (currentUser.role == "Администратор")
                {
                    btnAddProduct.Enabled = true;
                    btnDeleteProduct.Enabled = true;
                    btnEditProduct.Enabled = true;
                }
                else
                {
                    btnAddProduct.Enabled = false;
                    btnDeleteProduct.Enabled = false;
                    btnEditProduct.Enabled = false;
                }

                if (currentUser.role == "Менеджер" || currentUser.role == "Администратор")
                {
                    txtSearch.Enabled = true;
                    cbxSupplier.Enabled = true;
                    cbxSort.Enabled = true;
                }
                else
                {
                    txtSearch.Enabled = false;
                    cbxSupplier.Enabled = false;
                    cbxSort.Enabled = false;
                }
            }
            else
            {
                // Если зашел гость
                txtSearch.Enabled = false;
                cbxSupplier.Enabled = false;
                cbxSort.Enabled = false;
                btnAddProduct.Enabled = false;
                btnEditProduct.Enabled = false;
                btnDeleteProduct.Enabled = false;
            }
        }

        public void TitleForForm(User user)
        {
            if (user == null) this.Text = "Товары для гостя";
            else this.Text = $"Товары - {user.name} - {user.role}";
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            AddEditForm form = new AddEditForm(service_, 0, new Product());

            if (form.ShowDialog() == DialogResult.OK)
            {
                Product newProduct = form.GetNewTovar();
                try
                {
                    service_.AddProduct(newProduct);
                    MessageBox.Show("Товар успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            if (listBoxProduct.SelectedItem is Product selectedProduct)
            {
                AddEditForm form = new AddEditForm(service_, 1, selectedProduct);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    Product updatedProduct = form.GetNewTovar();
                    try
                    {
                        service_.EditProduct(updatedProduct);
                        MessageBox.Show("Товар успешно обновлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (listBoxProduct.SelectedItem is Product selectedProduct)
            {
                if (service_.HasOrder(selectedProduct.article))
                {
                    MessageBox.Show("Невозможно удалить товар, так как он находится в существующих заказах!",
                        "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var result = MessageBox.Show($"Вы уверены, что хотите удалить товар {selectedProduct.name} (Артикул: {selectedProduct.article})?",
                    "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        service_.RemoveProduct(selectedProduct);
                        MessageBox.Show("Товар успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}