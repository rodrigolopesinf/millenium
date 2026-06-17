using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Site.Models
{
    public class PDFFooter : PdfPageEventHelper
    {
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            PdfPCell cell;
            tabFot.TotalWidth = 300F;
            cell = new PdfPCell(new Phrase("Ressaltamos que as Informações Prestadas são de CARÁTER ESTRITAMENTE CONFIDENCIAL, e para vosso uso Exclusivo.", FontFactory.GetFont("Times-Italic", 10)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                BorderWidthBottom = 0,
                BorderWidthLeft = 0,
                BorderWidthTop = 0,
                BorderWidthRight = 0
            };
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, 150, document.Bottom, writer.DirectContent);            
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }
    }
}
