using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineClear.Model
{
    public class ModelData
    {
        class Map
        {
            //地图信息
            public int[,] mineMap;
            
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
                segTree sT = new segTree(width * height);
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
        static readonly int[,] diffNum = { { 9, 9,10 },  { 16, 16,40 }, { 30, 16,99 } };
        public int width;
        public int height;
        public int mineNum;
        public int[,] mineMap;
        public int size = 20;
        public void setDiff(int diff)
        {
            diff--;
            width = diffNum[diff, 0];
            height = diffNum[diff, 1];
            mineNum = diffNum[diff, 2];
            Map map = new Map(width,height,mineNum);
            mineMap = new int[height, width];
            
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    mineMap[i, j] = map.mineMap[i, j];

        }

    }
}
