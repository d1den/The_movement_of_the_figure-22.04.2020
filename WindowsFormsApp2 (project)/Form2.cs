using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            trackBar1.Scroll += trackBar1_Scroll; // Добавляем событие скролл
            trackBar2.Scroll += trackBar2_Scroll;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
            try // Отлавливаем ошибки
            {
                Microsoft.Win32.RegistryKey currentUserKey = Microsoft.Win32.Registry.CurrentUser;
                Microsoft.Win32.RegistryKey settings = currentUserKey.OpenSubKey("Settings");
                trackBar1.Value = Convert.ToInt32(settings.GetValue("Speed")); // Задаём ползункам значения из реестра, если они там записаны
                trackBar2.Value = Convert.ToInt32(settings.GetValue("Size"));
                settings.Close();
            }
            catch { }
            label1.Text = String.Format("Текущее значение скорости: {0}", trackBar1.Value); // Выводим на лабел текущее значение
            label2.Text = String.Format("Текущий размер фигуры: {0}", trackBar2.Value);
        }

        // Регулируем скорость
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = String.Format("Текущее значение скорости: {0}", trackBar1.Value);
            Microsoft.Win32.RegistryKey currentUserKey = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey settings = currentUserKey.OpenSubKey("Settings",true); //Отрыкрваем ключ для записи
            settings.SetValue("Speed", Convert.ToString(trackBar1.Value)); // Записываем значения скорости
            settings.Close();

        }
        // Регулируем размер
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label2.Text = String.Format("Текущий размер фигуры: {0}", trackBar2.Value);
            Microsoft.Win32.RegistryKey currentUserKey = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey settings = currentUserKey.OpenSubKey("Settings", true); // Аналогично
            settings.SetValue("Size", Convert.ToString(trackBar2.Value));
            settings.Close();
        }

        // Нажимаем на кнопку выбрать 1-ый цвет
        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.FullOpen = true; // Окно ыбора цветов с раширенными возможностями
            if (colorDialog1.ShowDialog() == DialogResult.Cancel) // ОТкрываем окно выбора цвета
                return;
            Microsoft.Win32.RegistryKey currentUserKey = Microsoft.Win32.Registry.CurrentUser; 
            Microsoft.Win32.RegistryKey settings = currentUserKey.OpenSubKey("Settings", true);
            // Считываем цвет, как ARGB, заносим в строку, разделяя знаком ;
            string color = String.Format("{0};{1};{2};{3}", colorDialog1.Color.A, colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);
            settings.SetValue("Color1", color); // Заносим строку в реестр
            settings.Close();
        }

        // Ок - закрытие окна
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close(); // Закрываем окно
        }
        // Нажимаем на кнопку выбрать 2й цвет
        private void button3_Click(object sender, EventArgs e)
        {
            colorDialog2.FullOpen = true;
            if (colorDialog2.ShowDialog() == DialogResult.Cancel) // Аналогично всё
                return;
            Microsoft.Win32.RegistryKey currentUserKey = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey settings = currentUserKey.OpenSubKey("Settings", true);
            string color = String.Format("{0};{1};{2};{3}", colorDialog2.Color.A, colorDialog2.Color.R, colorDialog2.Color.G, colorDialog2.Color.B);
            settings.SetValue("Color2", color);
            settings.Close();
        }
    }
}
