using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MineClear.ModelView
{
    public abstract class  ModeleView : INotifyPropertyChanged
    {
        //属性改变事件
        public event PropertyChangedEventHandler PropertyChanged;

        private void DifficultySlect(object sender, RoutedEventArgs e)
        {
            MenuItem obj = sender as MenuItem;
            switch ((int)obj.Tag)
            {
                case 1:

                    MessageBox.Show("123");
                    //setDifficulty(50, 50);
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    break;
            }
        }

    }
}
