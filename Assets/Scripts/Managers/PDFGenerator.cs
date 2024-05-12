using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syncfusion;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using Syncfusion.Pdf.Tables;
using static VariationFactory;
using System;
using System.Text.RegularExpressions;

public class PDFGenerator : MonoBehaviour
{
    List<LessonViceData> currentList;

    private void Awake()
    {
        currentList = RefrenceManager.instance.questionManager.currentResultScreenData;
    }

    public void GeneratePDFFile(LessonViceData lessonData)
    {
        #region old code
        //string lessonName = "Lesson " + (currentList.IndexOf(lessonData) + 1).ToString();
        //string fileName = $"{lessonName}_{DateTime.Now:yyyy-MM-dd}.pdf";
        //string filePath = Path.Combine(Application.dataPath, fileName);

        //if(File.Exists(filePath))
        //{
        //    File.Delete(filePath);
        //}

        //PdfDocument document = new PdfDocument();

        //PdfPage page = document.Pages.Add();

        //PdfGraphics graphics = page.Graphics;

        //PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
        //PdfBrush brush = PdfBrushes.Black;


        //float x = 10, y = 10;
        //float cellWidth = 100, cellHeight = 20;

        //graphics.DrawString($"Report for {lessonName}", font, brush, new Syncfusion.Drawing.PointF(x, y));

        //y += cellHeight;

        //y += cellHeight;

        //float pageHeight = page.GetClientSize().Height;
        //int correctCount = 0;
        //int totalCount = lessonData.lesson.Count;

        //foreach (ResultScreenData resultData in lessonData.lesson)
        //{
        //    // If the content exceeds the page height, add a new page
        //    if (y > pageHeight - 2 * cellHeight) // Adjust 2 * cellHeight for spacing
        //    {
        //        page = document.Pages.Add();
        //        graphics = page.Graphics;
        //        y = 10; // Reset y-coordinate for the new page

        //        // Draw lesson name again on the new page
        //        graphics.DrawString($"Report for {lessonName}", font, brush, 
        //                            new Syncfusion.Drawing.PointF(x, y));

        //        y += cellHeight;
        //    }

        //    // Draw question and status for each entry
        //    graphics.DrawString($"Question: {resultData.question}", font, brush, 
        //                        new Syncfusion.Drawing.PointF(x, y));

        //    y += cellHeight;

        //    graphics.DrawString($"Status: {(resultData.status ? "Correct" : "Incorrect")}", font, brush, 
        //                        new Syncfusion.Drawing.PointF(x, y));
        //    y += cellHeight * 2; // Adding extra space between each question and its status

        //    // Count correct answers
        //    if (resultData.status)
        //    {
        //        correctCount++;
        //    }
        //}

        //// Draw correct/total at the end of the report
        //y += cellHeight;
        //graphics.DrawString($"Correct/Total: {correctCount}/{totalCount}", font, brush, 
        //                    new Syncfusion.Drawing.PointF(x, y));


        ////int lessonNum = Constants.currentLesson - 1;

        ////for(int i = 0; i < lessonData.lesson.Count; i++)
        ////{
        ////    graphics.DrawString(lessonData.lesson[i].question, font, brush, new Syncfusion.Drawing.PointF(x, y));
        ////    graphics.DrawString(lessonData.lesson[i].status.ToString(), font, brush, new Syncfusion.Drawing.PointF(x + 2 * cellWidth, y));

        ////    y += cellHeight;
        ////}

        //using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        //{
        //    document.Save(fileStream);
        //    fileStream.Close();
        //}

        //document.Close(true);
        #endregion

        #region new code
        //string lessonName = "Lesson: " + (currentList.IndexOf(lessonData) + 1).ToString();
        
        string lessonName = lessonData.name;
        string studentName = Constants.userName;
        string fileName = RemoveInvalidFileNameChars($"{studentName}_{lessonName}{DateTime.Now:yyyy-MM-dd}.pdf");

        
        //Create a directory and save all pdf reports in that directory
        string outputPath = Path.Combine(Application.persistentDataPath, "PDFReports");

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        string filePath = Path.Combine(outputPath, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"Previous report for {lessonName} deleted successfully.");
        }

        PdfDocument document = new PdfDocument();
        PdfPage page = document.Pages.Add();
        PdfGraphics graphics = page.Graphics;

        PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
        PdfBrush brush = PdfBrushes.Black;

        float x = 10, y = 10;
        float cellHeight = 20;
        float pageHeight = page.GetClientSize().Height;
        float pageWidth = page.GetClientSize().Width;

        int correctCount = 0;
        int totalCount = lessonData.lesson.Count;

        // Draw header only on the first page
        DrawHeader(studentName, graphics, font, brush, lessonName, pageWidth, x, ref y);

        #region new code
        foreach (ResultScreenData resultData in lessonData.lesson)
        {
            if (y > page.GetClientSize().Height - 2 * cellHeight)
            {
                page = document.Pages.Add();
                graphics = page.Graphics;
                y = 10;

                // Draw header on subsequent pages
                DrawHeader(studentName, graphics, font, brush, lessonName, pageWidth, x, ref y);
            }

            y += cellHeight;

            graphics.DrawString($"Question: {resultData.question}", font, brush, new PointF(x, y));
            y += cellHeight;

            graphics.DrawString($"Status: {(resultData.status ? "Correct" : "Incorrect")}", font, brush, new PointF(x, y));
            y += cellHeight * 2;

            if (resultData.status)
            {
                correctCount++;
            }
        }

        y += cellHeight;
        graphics.DrawString($"Correct/Total: {correctCount}/{totalCount}", font, brush, new PointF(x, y));

        #endregion

        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            document.Save(fileStream);
            fileStream.Close();
        }

        document.Close(true);

        Debug.Log($"PDF report generated for {lessonName} at: {filePath}");

        #endregion

    }

    // Function to draw header on the page
    void DrawHeader(string name, PdfGraphics graphics, PdfFont font, PdfBrush brush, string lessonName, float pageWidth, float x, ref float y)
    {
        // Draw Name at top left
        graphics.DrawString("Name: " + name, font, brush, new PointF(x, y));

        // Draw Lesson Name at top center
        float lessonNameWidth = font.MeasureString(lessonName).Width;
        float centerX = (pageWidth - lessonNameWidth) / 2;
        graphics.DrawString(lessonName, font, brush, new PointF(centerX, y));

        // Draw Date at top right
        float dateWidth = font.MeasureString(DateTime.Now.ToString("yyyy-MM-dd")).Width;
        float dateX = pageWidth - dateWidth - x;
        graphics.DrawString(DateTime.Now.ToString("yyyy-MM-dd"), font, brush, new PointF(dateX, y));

        y += 2 * font.Size; // Move down the y-coordinate after the header
    }

    // Function to remove invalid characters from the file name
    string RemoveInvalidFileNameChars(string fileName)
    {
        string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
        string regexPattern = $"[{Regex.Escape(invalidChars)}]";
        return Regex.Replace(fileName, regexPattern, "_");
    }


}
