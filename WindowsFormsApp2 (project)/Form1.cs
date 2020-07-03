using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;// Разрешаем считывание кнопок
            timer1.Interval = 100; // Устанавливаем начальное значение интервала таймера
            timer1.Tick += timer1_Tick; // Добавляем событие тик
            timer1.Start(); // Включаем таймер
        }
        int size = 60; // Переменные, задающие размер круга
        int x = 0, y = 0; // Координаты круга
        int dx = 5; // Шаг пермещенения
        Color Color1 = Color.Red, Color2 = Color.Green; // Задаём изначально два цвета

        enum STATUS { RightAndUp, LeftAndUp, LeftDown, RightDown };  //направления движения, в перечислении
        STATUS flag=STATUS.LeftDown;		//флаг изменения направления движения
        SolidBrush brush = new SolidBrush(Color.Red); // кисть
        Rectangle rc; //прямоугольная область, в которой находиться фигура

        public int MySpeed // Свойство, задающее принимающее значение скорости, и задающее интервал таймера
        {
            get
            {
                return 100 - timer1.Interval;
            }
            set
            {
                timer1.Interval = 100 - value;
            }
        }
        public int MySize // Свойство, задающее размер
        {
            get
            {
                return size;
            }
            set
            {
                size = 60 + (4 * value);
            }
        }
        public Color MyColor //Задаём цвет кисти
        {
            get
            {
                return brush.Color;
            }
            set
            {
                brush.Color = value;
            }
        }

        // Загрузка 1 формы
        private void Form1_Load(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey currentUserKey = Microsoft.Win32.Registry.CurrentUser; // Открываем в реестре область ключей пользователя
            Microsoft.Win32.RegistryKey settings = currentUserKey.CreateSubKey("Settings"); // Создаём ключ с именем Сеттингс
            settings.Close(); // Закрываем ключ
            this.Height = 370; // При загрузке формы устанваливаем её размер
            this.Width = 400;
            button1.Left = (this.Width - button1.Width)/2; // Так же указываем рамположение кнопки внизу и по центру
            button1.Top = this.Height-100;
            x = 0; // Задаём стартовую позицию фигуры - низ, слева
            y = this.ClientSize.Height-size;
        }
        // Функция нажатия на кнопку в форме
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) // Если нажата Esc
                this.Close(); // То закроем форму
        }
        // Функция изменения размера формы
        private void Form1_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;

            // Ensure the Form remains square (Height = Width).
            if (control.Size.Height != control.Size.Width) // Размер меняется с сохранением сторон
            {
                control.Size = new Size(control.Size.Width, control.Size.Width);
            }
            button1.Left = (this.Width - button1.Width) / 2; // Задаём кнопке то же положение относительно низа
            button1.Top = this.Height - 100;
        }
        // Функция нажатия на клваишу открытие настроек
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2(); // Создаём вторую форму
            newForm.Show(); // ОТкрываем её в немодальном режиме
        }
        // Функция закрытия формы. Здесь будем удалять ключ их реестра
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Microsoft.Win32.RegistryKey currentUserKey = Microsoft.Win32.Registry.CurrentUser;
            currentUserKey.DeleteSubKey("Settings"); //Удаляем ключ
        }
        // Функция рисования на форме
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillEllipse(brush, rc);  // рисуем закрашенный эллипс
        }

        // Функция тика таймера
        private void timer1_Tick(object sender, EventArgs e)
        {
            try // Отлавливаем ошибки
            {
                Microsoft.Win32.RegistryKey currentUserKey = Microsoft.Win32.Registry.CurrentUser;
                Microsoft.Win32.RegistryKey settings = currentUserKey.OpenSubKey("Settings"); // Открываем наш ключ с настройками для чтения
                MySpeed = Convert.ToInt32(settings.GetValue("Speed")); // Считываем в св-во скорости скорость из реестра
                MySize = Convert.ToInt32(settings.GetValue("Size")); // Аналогично считываем размер
                var col1 = Convert.ToString(settings.GetValue("Color1")).Split(';'); // Считываем из реестра первое значение цвета, который записан строкой A;R;G;B
                Color1 = Color.FromArgb(int.Parse(col1[0]), int.Parse(col1[1]), int.Parse(col1[2]), int.Parse(col1[3]));
                var col2 = Convert.ToString(settings.GetValue("Color2")).Split(';'); // Аналогично
                Color2 = Color.FromArgb(int.Parse(col2[0]), int.Parse(col2[1]), int.Parse(col2[2]), int.Parse(col2[3]));
                settings.Close(); // Закрываем ключ
            }
            catch { } // Если ошибки, то просто пропускаем
            rc = new Rectangle(x, y, size, size); // размер прямоугольной области
            this.Invalidate(rc, true);      // вызываем прорисовку области
            if (flag == STATUS.LeftDown) // Если дожны переместится в левый нижний угл
            {
                x = 0; // Тозадаём соответсвующие координаты
                y = this.ClientSize.Height-size;
                flag = STATUS.RightAndUp; // И устанавливаем теперь состояние - движение по диагонали вправо
            }
            else if (flag == STATUS.RightDown) // Аналогично
            {
                x = this.ClientSize.Width-size;
                y= this.ClientSize.Height-size;
                flag = STATUS.LeftAndUp;
            }
            else if (flag == STATUS.RightAndUp) // движение вправо по диагонали
            {
                x += dx; // Увеличиваем координату на шаг
                y -= dx; // Уменьшаем на шаг
            }
            else if (flag == STATUS.LeftAndUp) // движение влево по диагонали
            {
                x -= dx;
                y -= dx;
            }

            if ((x >= (this.ClientSize.Width - size) || y<=1) && flag==STATUS.RightAndUp) // если достигли правого угла формы
            {
                MyColor = Color2; // Задаём новый цвет отрисовки
                flag = STATUS.RightDown; // меняем статус движения на левый
            }
            else if ((x <= 1 || y<=1) && flag == STATUS.LeftAndUp) // если достигли левого края формы
            {
                MyColor = Color1;
                flag = STATUS.LeftDown;    // меняем статус движения на правый
            }

            rc = new Rectangle(x, y, size, size); // новая прямоугольная область
            this.Invalidate(rc, true);  // вызываем прорисовку этой области
        }
    }
}
