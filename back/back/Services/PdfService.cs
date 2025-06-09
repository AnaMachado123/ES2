using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using System.IO;
using BackendTesteESII.Models.DTOs;
using System.Collections.Generic;

public class PdfService
{
    public byte[] GerarRelatorioMensalPdf(List<RelatorioMensalDTO> relatorio, string nomeUtilizador, int mes, int ano)
    {
        using var ms = new MemoryStream();
        var writer = new PdfWriter(ms);
        var pdf = new PdfDocument(writer);
        var doc = new Document(pdf);

        PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        doc.SetFont(font);

        // Título
        doc.Add(new Paragraph($"Relatório Mensal - {nomeUtilizador}")
            .SetFont(boldFont)
            .SetFontSize(18)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(20));

        doc.Add(new Paragraph($"Mês: {mes} / Ano: {ano}")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(20));

        // Tabela
        Table table = new Table(UnitValue.CreatePercentArray(new float[] { 20, 30, 20, 20, 10 })).UseAllAvailableWidth();

        table.AddHeaderCell(CreateHeaderCell("Dia", boldFont));
        table.AddHeaderCell(CreateHeaderCell("Projeto", boldFont));
        table.AddHeaderCell(CreateHeaderCell("Horas Trabalhadas", boldFont));
        table.AddHeaderCell(CreateHeaderCell("Preço Total (€)", boldFont));
        table.AddHeaderCell(CreateHeaderCell("Excesso", boldFont));

        foreach (var item in relatorio)
        {
            table.AddCell(item.Dia.ToString());
            table.AddCell(item.NomeProjeto ?? "—");
            table.AddCell(item.TotalHoras.ToString());
            table.AddCell(item.TotalPreco.ToString("F2"));
            table.AddCell(item.ExcedeuLimite ? "Sim" : "Não");
        }

        doc.Add(table);
        doc.Close();
        return ms.ToArray();
    }

    private Cell CreateHeaderCell(string texto, PdfFont boldFont)
    {
        return new Cell()
            .Add(new Paragraph(texto).SetFont(boldFont))
            .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
            .SetTextAlignment(TextAlignment.CENTER);
    }
}
