using System;
using System.Collections.Generic;
using System.Text;

namespace projekt_EZI.PageRank
{
    public class PageRankAlgorithm
    {
        public double D { get; set; }
        public int N { get; set; }
        public double[] PageRank { get; set; }
        public AdjacencyMatrix matrix;
        public int Iteration;

        public PageRankAlgorithm(AdjacencyMatrix matrix, double D, int iter)
        {
            this.D = D;
            N = matrix.rows.Count;
            this.matrix = matrix;
            PageRank = new double[N];
            this.Iteration = iter;
            
        }

        public double[] CreateRanking()
        {
            MakeInitialRanking(); //PR={1,1,..,1}
            //PR(A) = (1 – d ) + d ( PR(t1) / C(t1) + ... + PR(tn)/C(tn) 
            double value = 1 - D;

            for (int i = 0; i < Iteration; i++)
            {
                for(int j = 0; j < N; j++)
                {
                    double sum = GetTotalLinkRanking(j);
                    PageRank[j] = Math.Round(value + D * sum,3);
                }
            }

            return PageRank;
            
        }

        public double GetTotalLinkRanking(int row)
        {
            double sum = 0;
            int pom = 0; 
            var currentRow = matrix.rows[row].document.name;
            for (int i = 0; i < N; i++)
            {
                if (i == row) continue;
                pom = 0;
                foreach (var refelem in matrix.rows[i].listOfDocuments)
                {

                    if (refelem.Key.name.Equals(currentRow) && refelem.Value!=0)
                    {
                        if (matrix.rows[i].LinkCounter == 0) return 0;
                        sum += PageRank[i] / matrix.rows[i].LinkCounter;
                        break;
                    }
                }
            }
          
            return sum;
            
        }
        public void MakeInitialRanking()
        {
            for(int i = 0; i < N; i++)
            {
                PageRank[i] = 1;
            }
        }

    
    }

    
}
