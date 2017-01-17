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
        protected struct BlockInf
        {
            public int i, j;
            public int Tag;
            public bool IsClicked;
            public BlockInf(int i,int j,int Tag)
            {
                this.i = i;
                this.j = j;
                this.Tag = Tag;
                IsClicked = false;
            }
        }
        public ModelData model;
        BitmapImage[] bitImage = new BitmapImage[10];
        Image loadImg;
        bool clickFlag;
        Button[,] btn;
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            model = new ModelData();
            clickFlag = false;
            for (int i = 0; i <= 8; i++)
            {
                bitImage[i] = new BitmapImage(new Uri("pack://application:,,,/Source/" + (i + 1).ToString() + ".jpg"));
                Console.WriteLine("Source/" + (i+1).ToString()+".jpg");
            }
            bitImage[9] = new BitmapImage(new Uri("pack://application:,,,/Source/mine.jpg"));
        }
        private void diffSel(object sender, RoutedEventArgs e)
        {
            
            ModeleView.DifficultySlect(sender, e,model);
            clickFlag = true;
            gameMap.Children.Clear();
            btn = new Button[model.height, model.width];
            gameMap.Width = model.width * model.size;
            gameMap.Height = model.height * model.size;
            
            for (int i = 0; i < model.height; i++)
                for (int j = 0; j < model.width; j++)
                {
                    btn[i, j] = new Button();
                    BlockInf Tag = new BlockInf(i, j, model.mineMap[i, j]);
                    btn[i, j].Tag = Tag;
                    /*
                    img[0] = new Image();
                    img[0].Source = new BitmapImage(new Uri("pack://application:,,,/Source/mine.jpg"));
                    if ((int)btn[i, j].Tag == -1)
                        btn[i, j].Content = img[0];
                                            //btn[i, j].Tag.ToString();
                    */
                    btn[i, j].Click += Btn_Click;
                    btn[i, j].MouseRightButtonDown += Btn_Right_Click;
                    btn[i, j].Width = model.size;
                    btn[i, j].Height = model.size;
                    btn[i, j].SetValue(Button.StyleProperty, Application.Current.Resources["neverClick"]);
                    btn[i, j].Margin = new Thickness(j * model.size, i * model.size,0,0);
                    gameMap.Children.Add(btn[i, j]);
                    //Thread.Sleep(10);
                }
        }
        internal void setBlockValue(int i,int j)
        {
            setBlockValue(btn[i, j]);
        }
        internal void setBlockValue(Button btn)
        {
            btn.Content = null;
            Console.WriteLine(btn.Tag);
            BlockInf Tag = (BlockInf)btn.Tag;
            if (Tag.Tag == 0)
            {
                btn.SetValue(Button.StyleProperty, Application.Current.Resources["Click0"]);
                return;
            }
            if (Tag.Tag == -1)
            {
                loadImg = new Image();
                loadImg.Source = bitImage[9];
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

        private void Btn_Right_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.SetValue(Button.StyleProperty, Application.Current.Resources["Click0"]);
            loadImg = new Image();
            loadImg.Source = bitImage[8];
            btn.Content = loadImg;
        }
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            if(!clickFlag)
            {
                MessageBox.Show("请先开始游戏");
                return;
            }
            Button btn = sender as Button;
            setBlockValue(btn);
            if(((BlockInf)btn.Tag).Tag==0)
            {
                ModelData.BfsSolution bfs = new ModelData.BfsSolution();
                bfs.bfs(((BlockInf)btn.Tag).i, ((BlockInf)btn.Tag).j);
            }
        }
    }
}
