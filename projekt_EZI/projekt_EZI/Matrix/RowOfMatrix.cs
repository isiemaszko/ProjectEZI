using System;
using System.Collections.Generic;
using System.Text;

namespace projekt_EZI.Matrix
{
    public class RowOfMatrix
    {
        public Document document { get; set; }
        public Dictionary<Document, double> listOfDocuments { get; set; }

        public int LinkCounter { get; set; }

        public RowOfMatrix(Document doc, Dictionary<Document, double> list, int links)
        {
            this.document = doc;
            this.listOfDocuments = list;
            this.LinkCounter = links;
        }
    }
}
