using System.Collections.Generic;

namespace Zero.Ocr.Tesseract
{
    public interface ITesseractContext
    {
        string[] ReadText(string imageFilePath, Dictionary<string, string> option = null);
    }
}