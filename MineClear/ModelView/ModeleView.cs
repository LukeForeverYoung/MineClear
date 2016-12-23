using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MineClear;
namespace MineClear.ModelView
{
    public abstract class  ModeleView : INotifyPropertyChanged
    {
        //属性改变事件
        public event PropertyChangedEventHandler PropertyChanged;
        
        public static void DifficultySlect(object sender, RoutedEventArgs e)
        {
            MenuItem obj = sender as MenuItem;
            switch (Int32.Parse(obj.Tag.ToString()))
            {
                case 1:

                    //MessageBox.Show("123");
                    //setDifficulty(50, 50);
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    Window mW = Application.Current.MainWindow;
                    SetDifficulty sd = new SetDifficulty(mW);
                    sd.Show();
                    break;
                default:
                    break;
            }
        }

    }
}
