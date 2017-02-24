using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineClear.Model
{
    public class ModelData
    {
        static readonly int[,] diffNum = { { 9, 9, 10 }, { 16, 16, 40 }, { 30, 16, 99 } };
        public int width;
        public int height;
        public int mineNum;
        static public int[,] mineMap;
        static bool[,] vis;
        public int size = 30;
        class Map
        {
            //地图信息
            public int[,] mineMap;//-1为雷
   
            int width;
            int height;
            int mineNum;
            //用线段树维护区间中未被作为雷的区块数量，完全避免随机重复问题，查找复杂度log(n)。
            class segTree 
            {
                struct tree
                {
                    public int sum;
                    public int l, r;
                }
                tree[] node;
                int len;
                public segTree(int len)
                {
                    this.len = len;
                    node = new tree[len << 2];
                    build(1,0,len-1);
                }
                void build(int root,int l,int r)
                {
                    node[root].sum = r-l+1;
                    node[root].l = l;
                    node[root].r = r;
                    if (l == r)
                        return;
                    int mid = (l + r) >> 1;
                    build(root << 1, l, mid);
                    build(root << 1 | 1, mid + 1, r);
                }
                public int setMine(int num)
                {
                    return insert(1, 0, len - 1, num);
                }
                int insert(int root,int l,int r,int num)
                {
                    if (l == r)
                    {
                        if (node[root].sum == 0)
                            Console.WriteLine("root=" + root + " l=" + l + " r=" + r + " make a mistake");
                        node[root].sum = 0;
                        return l;
                    }
                    int mid = (l + r) >> 1;
                    node[root].sum--;
                    if (node[root << 1].sum >= num)
                        return insert(root << 1, l, mid, num);
                    num -= node[root << 1].sum;
                    return insert(root << 1 | 1, mid + 1, r, num);
                }
            }
            void setMine()
            {
                int[] mineList = new int[width * height];
                for (int i = 0; i < mineList.Length; i++)
                    mineList[i] = 0;
                segTree sT = new segTree(this.width * this.height);
                Random rnd = new Random();
                for(int i=0;i<mineNum;i++)
                {
                    int pos = rnd.Next(1, width * height - i + 1);
                    //Console.WriteLine(pos);
                    pos = sT.setMine(pos);
                    int x = pos % width;
                    int y = pos / width;
                    mineMap[y, x] = -1;
                }
            }
            public Map(int width,int height,int mineNum)
            {
                this.width = width;
                this.height = height;
                this.mineNum = mineNum;
                mineMap = new int[height, width];
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                        mineMap[i, j] = 0;
                setMine();
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                        if(mineMap[i,j]!=-1)
                        {
                            int sum = 0;
                            for (int k = -1; k <= 1; k++)
                                for (int s = -1; s <= 1; s++)
                                    if (i + k >= 0 && i + k < height && j + s >= 0 && j + s < width)
                                        if (mineMap[i + k, j + s] == -1)
                                            sum++;
                            mineMap[i, j] = sum;
                        }
                //test show mine map in console
                /*
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                        Console.Write(mineMap[i, j]);
                    Console.WriteLine();
                }
                */
                //TODO
            } 
        }
        public void SpreadBlock(int i,int j)
        {
            BfsSolution bs = new BfsSolution(this);
            bs.bfs(i,j);
        }
        class BfsSolution
        {
            ModelData Outer;
            int[,] dir = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };
            struct node :IComparable
            {
                public int i, j;
                public int step;
                public node(int i,int j)
                {
                    this.i = i;
                    this.j = j;
                    this.step = 0;
                }
                public int CompareTo(Object obj)
                {
                    node x = (node)obj;
                    if (x.i == i && x.j == j && x.step == step)
                        return 0;
                    if (x.step == step)
                    {
                        if (x.i == i)
                            return x.j - j;
                        return x.i - i;
                    }
                    return x.step - step;
                    throw new NotImplementedException();
                }
            }
            /*
            class SortedSet : IComparer<node>
            {
                int IComparer<node>.Compare(node x, node y)
                {
                    if (x.step < y.step)
                        return 1;
                    return 0;
                    throw new NotImplementedException();
                }
            }
            */
            
            bool ok(node x)
            {
                if (x.i < 0 || x.j < 0 || x.i >= Outer.height || x.j >= Outer.width)
                {
                    //Console.WriteLine("范围超出");
                    return false;
                }
                    
                if (vis[x.i, x.j])
                {
                    //Console.WriteLine("被访问过");
                    return false;
                }
                    
                if (mineMap[x.i, x.j] == -1)
                {
                    //Console.WriteLine("雷");
                    return false;
                }
                    
                return true;
            }
            SortedSet<node> Q;
            public void bfs(int i,int j)
            {
                Q = new SortedSet<node>();
                node now = new node(i, j),next;
                vis[now.i, now.j] = true;
                Q.Add(now);
                while(Q.Count!=0)
                {
                    now = Q.Max;
                    if (Q.Remove(now)) 
                        Console.WriteLine("yes");
                    Console.WriteLine(Q.Count+" "+now.i+" "+now.j+" "+now.step);
                    MainWindow.setBlockValue(now.i, now.j);
                    if (mineMap[now.i, now.j] != 0)//空格(继续拓展)，非空格(停止)
                        continue;
                    for (int d=0;d<8;d++)
                    {
                        next = now;
                        next.step++;
                        next.i += dir[d, 1];
                        next.j += dir[d, 0];
                        //Console.WriteLine("next" + next.i +" "+ next.j);
                        if(ok(next))
                        {
                            vis[next.i, next.j] = true;
                            Q.Add(next);
                        }
                    }
                }
            }
            public BfsSolution(ModelData _Outer)
            {
                Outer = _Outer;
                vis = new bool[Outer.height, Outer.width];
                for (int i = 0; i < Outer.height; i++)
                    for (int j = 0; j < Outer.width; j++)
                        vis[i, j] = false;
            }
        }
        public void setDiff(int diff)
        {
            diff--;
            width = diffNum[diff, 0];
            height = diffNum[diff, 1];
            mineNum = diffNum[diff, 2];
            Map map = new Map(width,height,mineNum);
            mineMap = new int[height, width];
            vis = new bool[height, width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    vis[i, j] = false;
                    mineMap[i, j] = map.mineMap[i, j];
                }
        }
    }
}
