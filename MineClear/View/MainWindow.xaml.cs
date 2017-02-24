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
        static bool debugFlag = false;
        private void setDebugMode(Object sender,RoutedEventArgs e)
        {
            if (debugFlag)
                debugFlag = false;
            else
                debugFlag = true;
            MessageBox.Show("重新开始游戏生效");
        }
        protected struct BlockInf
        {
            public int i, j;
            public int Tag;
            public bool RedFlagSet;
            public bool IsClicked;
            public BlockInf(int i,int j,int Tag)
            {
                this.i = i;
                this.j = j;
                this.Tag = Tag;
                IsClicked = false;
                RedFlagSet = false;
            }
        }
        static int NullBlockNum;
        static int ClickCounter;
        static int NullBlockCounter;
        internal static void AddClickCounter(int value)
        {
            ClickCounter += value;
            if(ClickCounter==(model.width*model.height))
            {
                CheckMap();
            }
        }
        
        static public ModelData model;
        /// <summary>
        /// 0-7 数字1-8;
        /// 8 红旗;
        /// 9 炸弹;
        /// </summary>
        static BitmapImage[] bitImage = new BitmapImage[10];

        static Image loadImg;
        static bool clickFlag;
        static Button[,] btn;
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;//初始化在屏幕中央
            this.SizeToContent = SizeToContent.WidthAndHeight;//窗口大小随Content改变
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
            MenuItem mI = sender as MenuItem;
            DifficultySlect(mI.Tag,model);
            clickFlag = true;
            ClickCounter = 0;
            gameMap.Children.Clear();
            btn = new Button[model.height, model.width];
            gameMap.Width = model.width * model.size;
            gameMap.Height = model.height * model.size;
            
            for (int i = 0; i < model.height; i++)
                for (int j = 0; j < model.width; j++)
                {
                    btn[i, j] = new Button();
                    BlockInf Tag = new BlockInf(i, j, ModelData.mineMap[i, j]);
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
                    //debug
                    if(debugFlag)
                        if(ModelData.mineMap[i, j] == -1)
                            btn[i, j].BorderBrush = Brushes.Pink;
                    //
                    btn[i, j].Margin = new Thickness(j * model.size, i * model.size,0,0);
                    gameMap.Children.Add(btn[i, j]);
                    //Thread.Sleep(10);
                }
            NullBlockNum = model.height * model.width - model.mineNum;
            Console.WriteLine(NullBlockNum);
        }
        static internal void setBlockValue(int i,int j)
        {
            setBlockValue(btn[i, j]);
        }
        static internal void setBlockValue(Button btn)
        {
          
            //Console.WriteLine(btn.Tag);
            BlockInf Tag = (BlockInf)btn.Tag;
            BlockInf bf = (BlockInf)btn.Tag;
            if (bf.IsClicked||bf.RedFlagSet)
                return;
            bf.IsClicked = true;
            btn.Tag = bf;
            if (Tag.Tag == -1)
            {
                loadImg = new Image();
                loadImg.Source = bitImage[9];
                btn.Content = loadImg;
                MessageBox.Show("Game over!");
                clickFlag = false;
                return;
            }

            if (Tag.Tag == 0)
            {
                btn.SetValue(Button.StyleProperty, Application.Current.Resources["Click0"]);
            }
            else
            {
                loadImg = new Image();
                loadImg.Source = bitImage[((BlockInf)btn.Tag).Tag - 1];
                btn.Content = loadImg;
            }
            NullBlockCounter++;
            if (NullBlockCounter == NullBlockNum)
                AutoSetMineFlag();
            AddClickCounter(1);

        }
        private void Btn_Right_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.SetValue(Button.StyleProperty, Application.Current.Resources["Click0"]);
            BlockInf bf = (BlockInf)btn.Tag;
            loadImg = new Image();
            if (bf.RedFlagSet)
            {
                bf.RedFlagSet = false;
                btn.Tag = bf;
                AddClickCounter(-1);
                btn.SetValue(Button.StyleProperty, Application.Current.Resources["neverClick"]);
            }
            else
            {
                if (bf.IsClicked)
                    return;
                bf.RedFlagSet = true;
                btn.Tag = bf;
                AddClickCounter(1);
                loadImg.Source = bitImage[8];
            }
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
            BlockInf bf = (BlockInf)btn.Tag;
            if (bf.IsClicked)
                return;
            if (bf.RedFlagSet)
            {
                bf.RedFlagSet = false;
                btn.Tag = bf;
                AddClickCounter(-1);
            }
            setBlockValue(btn);

            if(((BlockInf)btn.Tag).Tag==0)
            {
                model.SpreadBlock(((BlockInf)btn.Tag).i, ((BlockInf)btn.Tag).j);
                /*
                model.BfsSolution bfs = new model.BfsSolution();
                bfs.bfs(((BlockInf)btn.Tag).i, ((BlockInf)btn.Tag).j);
                */
            }
        }
        static void AutoSetMineFlag()
        {

            for (int i = 0; i < model.height; i++)
                for (int j = 0; j < model.width; j++)
                {
                    BlockInf bf = (BlockInf)btn[i, j].Tag;
                    if ((!bf.RedFlagSet && bf.Tag == -1))
                    {
                        loadImg = new Image();
                        loadImg.Source = bitImage[8];
                        btn[i, j].Content = loadImg;
                        AddClickCounter(1);
                    }
                }
        }
        static void CheckMap()
        {
            for(int i=0;i<model.height;i++)
                for(int j=0;j<model.width;j++)
                {
                    BlockInf bf = (BlockInf)btn[i, j].Tag;
                    if((bf.RedFlagSet&&bf.Tag!=-1))
                    {
                        
                        Console.WriteLine(bf.RedFlagSet + " " + bf.Tag);
                        btn[i, j].BorderBrush = Brushes.Red;
                        MessageBox.Show("插错旗了");
                    }
                }
            MessageBox.Show("游戏胜利");
            clickFlag = false;
        }
        public static void DifficultySlect(Object Tag,ModelData model)
        {
            
            int diff = 0;
            switch (Int32.Parse(Tag.ToString()))
            {
                case 1:
                    diff = 1;
                    //MessageBox.Show("123");
                    //setDifficulty(50, 50);
                    break;
                case 2:
                    diff = 2;
                    break;
                case 3:
                    diff = 3;
                    break;
                case 4:
                    Window mW = Application.Current.MainWindow;
                    SetDifficulty sd = new SetDifficulty(mW);
                    sd.Show();
                    break;
                default:
                    break;
            }
            if (diff == 0)
            {
                Console.WriteLine("Difficulty set error diff==0");
                return;
            }
            model.setDiff(diff);
        }
    }
}
