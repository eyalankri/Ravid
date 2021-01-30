using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ravid.Utilities
{
    public static class FileExtension
    {
        public static string GetExtension(string contentType)
        {
            switch (contentType)
            {
                case "image/jpeg":
                    return ".jpeg";

                case "image/png":
                    return ".png";

                case "text/plain":
                    return ".txt";

                case "application/pdf":
                    return ".pdf";

                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet": //excel
                    return ".xlsx";

                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document": // word
                    return ".docx";

                default:
                    return "";

            }

        }
    }
}
