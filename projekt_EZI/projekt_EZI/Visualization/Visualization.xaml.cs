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
        public Polygon editedPolygon = null;
        public TextBlock editedText = null;
        public Point startPoint = new Point(int.MaxValue, int.MaxValue), endPoint = new Point(int.MaxValue, int.MaxValue);
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
                myCanvas.MouseDown += new MouseButtonEventHandler(PolygonMouseDown);
                myCanvas.MouseMove += new MouseEventHandler(PolygonMouseMove);
                scrollView.MouseMove += new MouseEventHandler(PolygonMouseMove);
                myCanvas.MouseUp += new MouseButtonEventHandler(PolygonMouseUp);
                TextBlock tb = new TextBlock();
                string s = "";
                s += page.document.name;
                tb.Text = s;
                tb.Width = 90;
                tb.Height = 15;
                tb.MouseDown += new MouseButtonEventHandler(PolygonMouseDown);
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
            DrawNodes();
            DrawConnections();
            

        }

        public void PolygonMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender.GetType() == typeof(Polygon))
            {
                editedPolygon = (Polygon)sender;
                editedText = nodes.Where(item => item.polygon == editedPolygon).FirstOrDefault().text;
            }
            else if (sender.GetType() == typeof(TextBlock))
            {
                editedText = (TextBlock)sender;
                editedPolygon = nodes.Where(item => item.text == editedText).FirstOrDefault().polygon;
            }
        }

        public void PolygonMouseMove(object sender, MouseEventArgs e)
        {
            if (editedPolygon != null && System.Windows.Input.Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var position = e.GetPosition(myCanvas);
                if (startPoint.X == int.MaxValue)
                {
                    startPoint = new Point(position.X, position.Y);
                }
                endPoint = new Point(position.X, position.Y);                
                myCanvas.Children.Clear();
                MovePolygon();
                DrawNodes();
                DrawConnections();
            }
            else
            {
                editedPolygon = null;
                startPoint = new Point(int.MaxValue, int.MaxValue);
                endPoint = new Point(int.MaxValue, int.MaxValue);
            }
        }

        public void PolygonMouseUp(object sender, MouseButtonEventArgs e)
        {
            editedPolygon = null;
            startPoint = new Point(int.MaxValue, int.MaxValue);
            endPoint = new Point(int.MaxValue, int.MaxValue);
        }

        public void MovePolygon()
        {
            var diffX = endPoint.X - startPoint.X;
            var diffY = endPoint.Y - startPoint.Y;
            if (diffX != 0 && diffY != 0)
            {
                var node = nodes.Where(item => item.polygon == editedPolygon).FirstOrDefault();
                var points = editedPolygon.Points;
                editedPolygon.Points = new PointCollection(points.Select(p => new Point(p.X + diffX, p.Y + diffY)));
                startPoint.X += diffX;
                startPoint.Y += diffY;
                Canvas.SetTop(node.text, points[0].Y + diffY + 5);
                Canvas.SetLeft(node.text, points[0].X + diffX + 5);
            }
        }

        public void DrawNodes()
        {
            foreach (var node in nodes)
            {
                myCanvas.Children.Add(node.polygon);
                myCanvas.Children.Add(node.text);
            }
        }

        private void btnSaveCanvas_Click(object sender, RoutedEventArgs e)
        {
            Point leftTop = new Point(int.MaxValue, int.MaxValue), rightBottom = new Point(int.MinValue, int.MinValue);
            foreach (var node in nodes)
            {
                var points = node.polygon.Points.ToList();
                foreach(var point in points)
                {
                    if (point.X > rightBottom.X) rightBottom.X = point.X + 20 > 2000 ? 2000 : point.X + 20;
                    if (point.Y > rightBottom.Y) rightBottom.Y = point.Y + 20 > 2000 ? 2000 : point.Y + 20;
                    if (point.X < leftTop.X) leftTop.X = point.X - 20 < 0 ? 0 : point.X - 20;
                    if (point.Y < leftTop.Y) leftTop.Y = point.Y - 20 < 0 ? 0 : point.Y - 20;
                }
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)myCanvas.RenderSize.Width,
    (int)myCanvas.RenderSize.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(myCanvas);

            var crop = new CroppedBitmap(rtb, new Int32Rect((int)leftTop.X, (int)leftTop.Y, (int)rightBottom.X, (int)rightBottom.Y));

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(crop));

            using (var fs = System.IO.File.OpenWrite("graf.png"))
            {
                pngEncoder.Save(fs);
            }
        }

        public void DrawConnections()
        {
            foreach (var node in nodes)
            {
                foreach (var link in node.nodes)
                {
                    //var p1 = new Point(node.polygon.Points[0].X, node.polygon.Points[0].Y);
                    //var p2 = new Point(link.polygon.Points[0].X,link.polygon.Points[0].Y);
                    (var p1, var p2) = CalculateClosest(node.polygon, link.polygon);
                    var pointCollection = ArrowLine.CreateLineWithArrowPointCollection(p1, p2, 1);
                    Polygon p = new Polygon();
                    p.Stroke = Brushes.Black;
                    p.Points = pointCollection;
                    myCanvas.Children.Add(p);
                }
            }
        }

        public Tuple<Point, Point> CalculateClosest(Polygon p1, Polygon p2)
        {
            var distance = double.MaxValue;
            Point p1return, p2return;
            var pointP1 = p1.Points.ToList();
            var pointP2 = p2.Points.ToList();
            foreach(var point1 in pointP1)
            {
                foreach(var point2 in pointP2)
                {
                    var newDist = (int)Point.Subtract(point2, point1).Length;
                    if (newDist < distance)
                    {
                        distance = newDist;
                        p1return = point1;
                        p2return = point2;
                    }
                }
            }
            return new Tuple<Point, Point>(p1return, p2return);
        }
    }
}
