using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace projekt_EZI.Matrix
{
    public class Matrix
    {
        public FlowDocument flowDoc;
        public Table table1;
        public AdjacencyMatrix incidentList;
        public TableRow currentRow;

        public Matrix(AdjacencyMatrix incident)
        {
            this.incidentList = incident;
            flowDoc = new FlowDocument();
            table1 = new Table();
        }
        public FlowDocument createMatrix()
        {
            flowDoc.Blocks.Add(table1);
            table1.CellSpacing = 14;
            table1.Background = Brushes.White;
            table1.FontSize = 10;

            createRows();

            return flowDoc;
        }

        public void createRows()
        {
            table1.RowGroups.Add(new TableRowGroup());
            table1.RowGroups[0].Rows.Add(new TableRow());
            addHeader();
            int rowCounter = 1;
            for (int i = 0; i < incidentList.rows.Count; i++)
            {
                currentRow = table1.RowGroups[0].Rows[rowCounter];
                table1.RowGroups[0].Rows.Add(new TableRow());
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(incidentList.rows[i].document.name))));
                foreach (var refelem in incidentList.rows[i].listOfDocuments)
                {
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(refelem.Value.ToString()))));
                }

                rowCounter++;
            }
           
        }

        public void addHeader()
        {
            currentRow = table1.RowGroups[0].Rows[0];
            table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

            for(int i=0;i<incidentList.rows.Count;i++)
            {
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(incidentList.rows[i].document.name))));
            }
        }
    }
}
