using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        }

        private void matrixBtn_Click(object sender, RoutedEventArgs e)
        {
            incidentList.createAdjacencyMatrix();
            createMatrix();
        }

        private void createMatrix()
        {
            var matrix = new Matrix.Matrix(incidentList.matrix);
            
            var dialog = new Window() { Content = matrix.createMatrix() };
            dialog.ShowDialog();
            
            // this.Content = flowDoc;
            // matrixGrid.Children.Add((UIElement)table1);
        }
    }
}
