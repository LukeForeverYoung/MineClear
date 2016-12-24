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
using System.Windows.Navigation;
using MineClear.ModelView;
using MineClear.Model;
using System.Drawing;
using System.Threading;
namespace MineClear
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public ModelData model;
        BitmapImage[] bitImage = new BitmapImage[10];
        Image loadImg;
        bool clickFlag;
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            model = new ModelData();
            
            clickFlag = false;
            for (int i = 0; i < 8; i++)
            {
                bitImage[i] = new BitmapImage(new Uri("pack://application:,,,/Source/" + (i + 1).ToString() + ".jpg"));
                Console.WriteLine("Source/" + (i+1).ToString()+".jpg");
            }
                
            bitImage[8] = new BitmapImage(new Uri("pack://application:,,,/Source/mine.jpg"));
        }

        
        private void diffSel(object sender, RoutedEventArgs e)
        {
            
            ModeleView.DifficultySlect(sender, e,model);
            clickFlag = true;
            Button[,] btn = new Button[model.height, model.width];
            gameMap.Width = model.width * model.size;
            gameMap.Height = model.height * model.size;
            
            for (int i = 0; i < model.height; i++)
                for (int j = 0; j < model.width; j++)
                {
                    btn[i, j] = new Button();
                    btn[i, j].Tag = model.mineMap[i, j];
                    /*
                    img[0] = new Image();
                    img[0].Source = new BitmapImage(new Uri("pack://application:,,,/Source/mine.jpg"));
                    if ((int)btn[i, j].Tag == -1)
                        btn[i, j].Content = img[0];
                                            //btn[i, j].Tag.ToString();
                    */
                    btn[i, j].Click += Btn_Click;
                    btn[i, j].Width = model.size;
                    btn[i, j].Height = model.size;
                    btn[i, j].Margin = new Thickness(j * model.size, i * model.size,0,0);
                    gameMap.Children.Add(btn[i, j]);
                    //Thread.Sleep(10);
                    
                }
            
        }
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            if(!clickFlag)
            {
                MessageBox.Show("请先开始游戏");
                return;
            }
           
            Button btn = sender as Button;
            Console.WriteLine(btn.Tag);
            if ((int)btn.Tag == 0)
                return;
            if ((int)btn.Tag == -1)
            {
                loadImg = new Image();
                loadImg.Source = bitImage[8];
                btn.Content = loadImg;
                MessageBox.Show("Game over!");
                clickFlag = false;
            }
            else
            {
                loadImg = new Image();
                loadImg.Source = bitImage[(int)btn.Tag - 1];
                btn.Content = loadImg;
            }
            
        }
    }
}
