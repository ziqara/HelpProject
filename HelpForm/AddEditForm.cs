using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HelpProject.Models;
using HelpProject.Services;

namespace HelpForm
{
    public partial class AddEditForm : Form
    {
        private Product newTovar_;
        private string selectedImagePath_;
        private ServiceProduct model_;
        private int type_;

        public AddEditForm(ServiceProduct model, int type, Product tovar)
        {
            InitializeComponent();
            newTovar_ = tovar;
            model_ = model;
            type_ = type;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (type_ == 0)
            {
                Product addTovar = new Product();
                addTovar.article = txtArticul.Text;
                addTovar.name = txtName.Text;
                addTovar.unit = txtUnit.Text;
                addTovar.price = (int)numPrice.Value;
                addTovar.supplier = txtSupplier.Text;
                addTovar.manufacturer = txtManufacturer.Text;
                addTovar.category = txtCategory.Text;
                addTovar.discountPercent = (int)numDiscount.Value;
                addTovar.stockQuantity = (int)numStockQuantity.Value;
                addTovar.description = txtDescription.Text;
                addTovar.picture = selectedImagePath_; // Сохраняем только имя файла

                newTovar_ = addTovar;
                DialogResult = DialogResult.OK;
            }

            if (type_ == 1)
            {
                newTovar_.name = txtName.Text;
                newTovar_.unit = txtUnit.Text;
                newTovar_.price = (int)numPrice.Value;
                newTovar_.supplier = txtSupplier.Text;
                newTovar_.manufacturer = txtManufacturer.Text;
                newTovar_.category = txtCategory.Text;
                newTovar_.discountPercent = (int)numDiscount.Value;
                newTovar_.stockQuantity = (int)numStockQuantity.Value;
                newTovar_.description = txtDescription.Text;
                newTovar_.picture = selectedImagePath_; // Обновляем имя файла в базе данных
                DialogResult = DialogResult.OK;
            }
        }

        private void AddEditForm_Load(object sender, EventArgs e)
        {
            if (type_ == 0)
            {
                txtArticul.Text = string.Empty;
                txtName.Text = string.Empty;
                txtUnit.Text = string.Empty;
                numPrice.Value = 0;
                txtSupplier.Text = string.Empty;
                txtManufacturer.Text = string.Empty;
                txtCategory.Text = string.Empty;
                numDiscount.Value = 0;
                numStockQuantity.Value = 0;
                txtDescription.Text = string.Empty;
                this.Text = "Добавление товара";
            }

            if (type_ == 1)
            {
                txtArticul.Text = newTovar_.article;
                txtName.Text = newTovar_.name;
                txtUnit.Text = newTovar_.unit;
                numPrice.Value = newTovar_.price;
                txtSupplier.Text = newTovar_.supplier;
                txtManufacturer.Text = newTovar_.manufacturer;
                txtCategory.Text = newTovar_.category;
                numDiscount.Value = newTovar_.discountPercent;
                numStockQuantity.Value = newTovar_.stockQuantity;
                txtDescription.Text = newTovar_.description;
                btnImage.Text = "Обновить";

                // Чтение при открытии на редактирование: ищем файл в корне bin/Debug
                if (!string.IsNullOrEmpty(newTovar_.picture))
                {
                    string fullPath = Path.Combine(Application.StartupPath, newTovar_.picture);
                    if (File.Exists(fullPath))
                    {
                        imageBox.Image = Image.FromFile(fullPath);
                        imageBox.SizeMode = PictureBoxSizeMode.Zoom;
                        selectedImagePath_ = newTovar_.picture; // сохраняем имя файла
                    }
                }
                this.Text = "Редактирование товара";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        public Product GetNewTovar()
        {
            return newTovar_;
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";
                openFileDialog.Title = "Выберите изображение для товара";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string sourcePath = openFileDialog.FileName;
                        string fileName = Path.GetFileName(sourcePath);

                        // Путь КОПИРОВАНИЯ прямо в корень папки bin/Debug (Application.StartupPath)
                        string targetPath = Path.Combine(Application.StartupPath, fileName);

                        // Освобождаем старую картинку в форме, чтобы не заблокировать перезапись файла
                        if (imageBox.Image != null)
                        {
                            imageBox.Image.Dispose();
                            imageBox.Image = null;
                        }

                        // Копируем картинку прямо в bin/Debug
                        File.Copy(sourcePath, targetPath, true);

                        // Записываем только чистое имя файла (например: "be71984604...jpg")
                        selectedImagePath_ = fileName;

                        // Показываем в PictureBox формы добавления
                        imageBox.Image = Image.FromFile(targetPath);
                        imageBox.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}