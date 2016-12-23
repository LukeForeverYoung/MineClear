using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MineClear
{
    /// <summary>
    /// SetDifficulty.xaml 的交互逻辑
    /// </summary>
    public partial class SetDifficulty : Window
    {
        public SetDifficulty(Window father)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.Owner = father;
        }
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tx = sender as TextBox;
           
            String sx = tx.Text;
            if (sx == null||sx=="")
                return;

            if (sx.Last<char>() < '0' || sx.Last<char>() > '9')
                tx.Text.Remove(sx.Length - 1);

        }

        private void textBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox tx = sender as TextBox;
            if (tx.Text == "宽度" || tx.Text == "高度")
                tx.Text = "";
        }
    }
}
