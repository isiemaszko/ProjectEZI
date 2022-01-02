using Microsoft.Win32;
using projekt_EZI.PrestigeCalculation;
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
        Matrix.Matrix Matrix;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateIncidentList()
        {
            incidentList = new IncidentList(txtEditor.Text);
            incidentList.createAdjacencyMatrix();
        }
        private void openFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
                txtEditor.Text = File.ReadAllText(openFileDialog.FileName);

            CreateIncidentList();
        }

        private void GenerateBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateIncidentList();
        }

        private void matrixBtn_Click(object sender, RoutedEventArgs e)
        {
            createMatrix();
        }

        private void createMatrix()
        {
            Matrix = new Matrix.Matrix(incidentList.matrix);
            var dialog = new Window() { Content = Matrix.createMatrix()};
            
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

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        void rankingColumnHeader_Click(object sender,
                                           RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
            }
        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(RankingListView.Items);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private void btnEigenvector_Click(object sender, RoutedEventArgs e)
        {
            CreateRanking(Eigenvector.CalculateEigenvectorsRanking(incidentList.matrix));
        }
    }
}
