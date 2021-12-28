using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Shapes;

namespace projekt_EZI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IncidentList incidentList;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void openFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
                txtEditor.Text = File.ReadAllText(openFileDialog.FileName);

            incidentList = new IncidentList(txtEditor.Text);
            incidentList.createAdjacencyMatrix();
        }

        private void matrixBtn_Click(object sender, RoutedEventArgs e)
        {
            createMatrix();
        }

        private void createMatrix()
        {
            var matrix = new Matrix.Matrix(incidentList.matrix);
            
            var dialog = new Window() { Content = matrix.createMatrix() };
            dialog.ShowDialog();
            
        }

        private void btnPageRank_Click(object sender, RoutedEventArgs e)
        {
            //liczba itercji, d
            double D = double.Parse(DTextBox.Text);
            int iteration = int.Parse(IterationTextBox.Text);
            var algorithmPageRank = new PageRank.PageRankAlgorithm(incidentList.matrix, D, iteration);
            double[] ranking =algorithmPageRank.CreateRanking();
            CreateRanking(ranking);
        }
        private void CreateRanking(double[] ranking)
        {
            for (int i = 0; i < incidentList.incList.Count; i++)
            {
                this.RankingListView.Items.Add(new MyItem { Name = incidentList.incList[i].name, Ranking = ranking[i] });
            }
           

        }

        void rankingColumnHeader_Click(object sender,
                                           RoutedEventArgs e)
        {
            int ddd = 0;
        }

       
    }
}
