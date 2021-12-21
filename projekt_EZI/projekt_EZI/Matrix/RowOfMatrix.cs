using System;
using System.Collections.Generic;
using System.Text;

namespace projekt_EZI.Matrix
{
    public class RowOfMatrix
    {
        public Document document { get; set; }
        public Dictionary<Document, double> listOfDocuments { get; set; }

        public RowOfMatrix(Document doc, Dictionary<Document, double> list)
        {
            this.document = doc;
            this.listOfDocuments = list;
        }
    }
}
