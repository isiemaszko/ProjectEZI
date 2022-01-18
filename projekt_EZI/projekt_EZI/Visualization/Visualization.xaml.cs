using projekt_EZI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace projekt_EZI
{

    public partial class Visualization : Window
    {
        public double currentX = 10, currentY = 10;
        public List<Node> nodes = new List<Node>();
        public Visualization()
        {
            InitializeComponent();
        }

        public Visualization(AdjacencyMatrix adjacencyMatrix)
        {
            InitializeComponent();
            int i = 0;

            foreach (var page in adjacencyMatrix.rows)
            {
                if (i % 3 == 0 && i != 0)
                {
                    currentX = 10;
                    currentY += 100;
                }
                Point p1 = new Point(currentX, currentY);
                Point p2 = new Point(currentX + 100, currentY);
                Point p3 = new Point(currentX + 100, currentY + 25);
                Point p4 = new Point(currentX, currentY + 25);
                PointCollection pc = new PointCollection();
                pc.Add(p1);
                pc.Add(p2);
                pc.Add(p3);
                pc.Add(p4);
                var polygon = new Polygon();
                polygon.Points = pc;
                polygon.Stroke = Brushes.Black;
                polygon.Fill = Brushes.White;
                polygon.StrokeThickness = 1;
                //myCanvas.Children.Add(polygon);


                TextBlock tb = new TextBlock();
                string s = "";
                s += page.document.name;
                tb.Text = s;
                tb.Width = 90;
                tb.Height = 15;
                Canvas.SetLeft(tb, p1.X + 5);
                Canvas.SetTop(tb, p1.Y + 5);
                //myCanvas.Children.Add(tb);
                nodes.Add(new Node(polygon, tb));
                currentX += 200;
                i++;
            }
            foreach (var page in adjacencyMatrix.rows)
            {
                var toLink = nodes.Where(item => item.text.Text == page.document.name).FirstOrDefault();
                foreach (var link in page.listOfDocuments)
                {
                    if (link.Value != 0)
                    {
                        var linked = nodes.Where(item => item.text.Text == link.Key.name).FirstOrDefault();
                        if (linked != null)
                        {
                            toLink.nodes.Add(linked);
                        }
                    }
                }
            }

            DrawConnections();
            DrawNodes();

        }

        public void DrawNodes()
        {
            foreach (var node in nodes)
            {
                myCanvas.Children.Add(node.polygon);
                myCanvas.Children.Add(node.text);
            }
        }

        public void DrawConnections()
        {
            foreach (var node in nodes)
            {
                foreach (var link in node.nodes)
                {
                    var p1 = new Point(node.polygon.Points[0].X, node.polygon.Points[0].Y);
                    var p2 = new Point(link.polygon.Points[0].X,link.polygon.Points[0].Y);
                    var pointCollection = ArrowLine.CreateLineWithArrowPointCollection(p1, p2, 1);
                    Polygon p = new Polygon();
                    p.Stroke = Brushes.Black;
                    p.Points = pointCollection;
                    myCanvas.Children.Add(p);
                }
            }
        }
    }
}
