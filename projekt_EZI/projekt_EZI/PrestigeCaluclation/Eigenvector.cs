using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MathNet.Numerics.LinearAlgebra.Storage;
using MathNet.Symbolics;
using Accord.Math.Decompositions;

namespace projekt_EZI.PrestigeCalculation
{
    public class Eigenvector
    {
        public Matrix<double> aMatrix;

        public FlowDocument flowDoc;
        public Table table1;
        public TableRow currentRow;

        public Eigenvector(AdjacencyMatrix adjacencyMatrixText)
        {
            flowDoc = new FlowDocument();
            table1 = new Table();
            // ilość stron
            var recordsCount = adjacencyMatrixText.rows.Count;

            var array = new double[recordsCount, recordsCount];
            //wypełnienie tablicy
            for(int i = 0; i < recordsCount; i++)
            {
                int j = 0;
                foreach(var doc in adjacencyMatrixText.rows[i].listOfDocuments)
                {
                    array[i, j++] = doc.Value;
                }
            }
            // stworzenie macierzy 
            //array = new double[,] { { 0, 1, 0, 1, 1, 0 }, { 0, 0, 1, 0, 1, 1 }, { 0, 1, 0, 1, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 1, 1, 0, 0, 1 }, { 0, 0, 0, 0, 1, 0 } };
            array = new double[,] { { 0, 0.5, 0.5 }, { 0, 0, 1 }, { 1, 0, 0 } };
            aMatrix = SparseMatrix.OfArray(array).Transpose();

            var lambda = SymbolicExpression.Variable("l");



            // obliczenie wektorów własnych
            var eigenCalculations = aMatrix.Evd();
            var eigen = new double[3, 3];
            var tmp = eigenCalculations.EigenVectors.Storage;
            for (int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    eigen[i,j] = tmp[i, j];
                }
            }
            var eigenV = SparseMatrix.OfArray(eigen);
            var t = eigenV.Solve(DenseMatrix.OfArray(new double[,] { { 0 }, { 0 }, { 0 } }));

            EigenvalueDecomposition eigenvalueDecomposition = new EigenvalueDecomposition(array, false, true);
            var dialog = new Window() { Content = createMatrix(eigenvalueDecomposition.Eigenvectors, recordsCount) };
            dialog.ShowDialog();
            int w = 0;
        }

        public FlowDocument createMatrix(double [,] matrix, int rowsCount)
        {
            flowDoc.Blocks.Add(table1);
            table1.CellSpacing = 14;
            table1.Background = Brushes.White;
            table1.FontSize = 10;

            createRows(matrix, rowsCount);

            return flowDoc;
        }

        public void createRows(double[,] matrix, int rowsCount)
        {
            table1.RowGroups.Add(new TableRowGroup());
            table1.RowGroups[0].Rows.Add(new TableRow());
            addHeader(matrix, rowsCount);
            int rowCounter = 1;
            for (int i = 0; i < rowsCount; i++)
            {
                currentRow = table1.RowGroups[0].Rows[rowCounter];
                table1.RowGroups[0].Rows.Add(new TableRow());
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("[" + rowCounter + "]"))));

                for(int j = 0; j < rowsCount; j++)
                {
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(matrix[i, j].ToString()))));
                }

                rowCounter++;
            }

        }

        public void addHeader(double[,] matrix, int rowsCount)
        {
            currentRow = table1.RowGroups[0].Rows[0];
            table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            for (int i = 0; i < rowsCount; i++)
            {
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("[" + i + "]"))));
            }
        }
    }
}
