using System.Collections.ObjectModel;
using System.Linq;

namespace Zero.Constants
{
    public static class FileExtensions
    {
        public static readonly string[] Image = { ".jpg", ".jpeg", ".png" };

        // MS Office
        public static readonly string[] MsWord = { ".doc", ".docx" };
        public static readonly string[] MsExcel = { ".xls", ".xlsx" };
        public static readonly string[] MsPowerPoint = { ".ppt", ".pptx" };

        public static readonly ReadOnlyCollection<string> MsOffice = new ReadOnlyCollection<string>(MsWord.Union(MsExcel).Union(MsPowerPoint).ToArray());

        public static readonly string[] Pdf = { ".pdf" };
        public static readonly ReadOnlyCollection<string> Documents = new ReadOnlyCollection<string>(MsOffice.Union(Pdf).ToArray());
    }
}