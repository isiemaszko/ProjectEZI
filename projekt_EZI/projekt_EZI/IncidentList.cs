using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace projekt_EZI
{
   
    public class IncidentList
    {
        public List<Document> incList { get; set; }
        public AdjacencyMatrix matrix;
        public List<Document> listOfAllDocuments;
        public IncidentList(string txt)
        {
            var tmp = txt.Split(new string[] { "\r\n" }, System.StringSplitOptions.None).Select(item => item.Trim()).ToList();

            this.incList = new List<Document>();
            createAllDocuments(tmp);
            createListsOfReferenceDocuments(tmp);
        }

        public void createAllDocuments(List<string> text)
        {
            listOfAllDocuments = new List<Document>();
            foreach (var elem in text)
            {
                string[] elemOFRow = elem.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                incList.Add(new Document(elemOFRow[0]));
                listOfAllDocuments.Add(new Document(elemOFRow[0]));
            }
        }

        public void createListsOfReferenceDocuments(List<string> text)
        {
            foreach (var elem in text)
            {
                string[] elemOFRow = elem.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                if (elemOFRow.Length > 1)
                {
                    Document document = incList.FirstOrDefault(a => a.name.Equals(elemOFRow[0]));
                    string[] listOdDoc = elemOFRow[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();
                    foreach (var doc in listOdDoc)
                    {
                        document.listOFReferenceDocument.Add(incList.Find(a => a.name.Equals(doc)));
                    }
                }
               
            }
        }

        public void createAdjacencyMatrix()
        {
            matrix = new AdjacencyMatrix();
            foreach(var docRow in incList)
            {
                var dict = new Dictionary<Document, double>();
                var listOFReferenceDocumentCount = docRow.listOFReferenceDocument.Count;
                foreach (var docCol in incList)
                {
                    if (docRow.name.Equals(docCol.name) || listOFReferenceDocumentCount == 0 || docRow.listOFReferenceDocument.Find(a => a.name.Equals(docCol.name))==null) dict.Add(docCol, 0);
                    else {
                        dict.Add(docCol, Math.Round( (double)1 / listOFReferenceDocumentCount,2));
                    }
                    
                }
               matrix.rows.Add(new Matrix.RowOfMatrix(docRow, dict, listOFReferenceDocumentCount));
            }
        }

    }
}
