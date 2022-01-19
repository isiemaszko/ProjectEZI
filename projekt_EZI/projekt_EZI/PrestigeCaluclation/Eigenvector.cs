using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;


namespace projekt_EZI.PrestigeCalculation
{
    public static class Eigenvector
    {
        public static double[] CalculateEigenvectorsRanking(AdjacencyMatrix adjacencyMatrixText)
        {
            SaveAdjacencyMatrix(adjacencyMatrixText);


            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "py" ;//cmd is full path to python.exe
            start.Arguments = "eigenvectorCalc.py";//args is path to .py file and any cmd line args
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            Process.Start(start);

            double[] result = new double[adjacencyMatrixText.rows.Count];

            var lines = System.IO.File.ReadLines("result.txt");
            int i = 0;
            foreach (string line in lines)
            {
                var value = line.Substring(0, adjacencyMatrixText.rows.Count + 4).Replace(".", ",");
                result[i++] = double.Parse(value);
            }
            return result;
        }

        public static void SaveAdjacencyMatrix(AdjacencyMatrix adjacencyMatrixText)
        {
            using (FileStream fs = File.Create("matrix.csv"))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (var page in adjacencyMatrixText.rows)
                    {
                        var row = string.Empty;
                        foreach (var content in page.listOfDocuments)
                        {
                            row += (content.Value != 0 ? "1" : "0") + ",";
                        }
                        row = row.Remove(row.Length - 1);
                        sw.WriteLine(row);
                    }
                }
            }
        }
    }
}
