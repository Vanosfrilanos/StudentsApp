using System;
using System.Data;
using System.Data.SqlClient; // Обязательно для работы с базой
using System.Windows.Forms;

namespace StudentsApp2
{
    public partial class Form1 : Form
    {
        // Твоя строка подключения
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MyFirstDB;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
        }

        // 1. ЗАГРУЗКА ПРИ СТАРТЕ
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData(); // Как только окно открывается, сразу грузим данные
        }

        // 2. МЕТОД ДЛЯ ЗАГРУЗКИ ДАННЫХ В ТАБЛИЦУ
        private void LoadData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT * FROM Students";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt; // Показываем данные в таблице на форме
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        // 3. КНОПКА "ДОБАВИТЬ"
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Пожалуйста, заполни имя и фамилию!");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    // ID не пишем, он добавится сам
                    string query = "INSERT INTO Students (FirstName, LastName, Age) VALUES (@fn, @ln, @age)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@fn", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@ln", txtLastName.Text);

                        // Пытаемся превратить текст в число. Если не получается - пишем 0.
                        int age;
                        if (!int.TryParse(txtAge.Text, out age))
                        {
                            age = 0;
                        }
                        cmd.Parameters.AddWithValue("@age", age);

                        cmd.ExecuteNonQuery(); // Выполняем запрос к базе
                    }
                }

                LoadData(); // Обновляем таблицу
                MessageBox.Show("Студент успешно добавлен!");

                // Очищаем поля после добавления
                txtFirstName.Clear();
                txtLastName.Clear();
                txtAge.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении: " + ex.Message);
            }
        }

        // 4. КНОПКА "УДАЛИТЬ"
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выбери студента из таблицы, чтобы удалить его.");
                return;
            }

            try
            {
                // Берем ID (первый столбец) из выделенной строки
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "DELETE FROM Students WHERE ID = @id";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadData(); // Обновляем таблицу
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message);
            }
        }

        // 5. КНОПКА "ОБНОВИТЬ"
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        // Пустые методы, которые VS создала автоматически (их можно оставить пустыми)
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выбери студента из таблицы, чтобы удалить его.");
                return;
            }

            try
            {
                // Берем ID (первый столбец) из выделенной строки
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "DELETE FROM Students WHERE ID = @id";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadData(); // Обновляем таблицу
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message);
            }
        }
    }
}