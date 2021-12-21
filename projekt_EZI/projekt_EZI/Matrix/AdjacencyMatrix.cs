using projekt_EZI.Matrix;
using System;
using System.Collections.Generic;
using System.Text;

namespace projekt_EZI
{
    public class AdjacencyMatrix
    {
        public List<RowOfMatrix> rows { get; set; }

        public AdjacencyMatrix()
        {
            this.rows = new List<RowOfMatrix>();
        }
    }
}
