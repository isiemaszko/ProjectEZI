using System;
using System.Collections.Generic;
using System.Text;

namespace projekt_EZI
{
    public class Document
    {
        public string name { get; set; }
        public List<Document> listOFReferenceDocument { get; set; }

        public Document(string name)
        {
            this.name = name;
            this.listOFReferenceDocument = new List<Document>();
        }

    }
}
