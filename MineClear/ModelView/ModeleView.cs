using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MineClear;
using MineClear.Model;

namespace MineClear.ModelView
{
    public abstract class  ModeleView : INotifyPropertyChanged
    {
        //属性改变事件
        public event PropertyChangedEventHandler PropertyChanged;
        
        

    }
}
