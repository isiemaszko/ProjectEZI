using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace projekt_EZI
{
    public class Node
    {
        public Polygon polygon { get; set; }
        public TextBlock text { get; set; }
        public List<Node> nodes { get; set; } = new List<Node>();

        public Node(Polygon polygon, TextBlock textBlock)
        {
            this.polygon = polygon;
            this.text = textBlock;
        }
    }
}
