using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class FileUploadValidator
    {
        public bool IsWebpageFrendlyFile(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return false;
            }

            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024)
            {
                return false;
            }

            var extention = Path.GetExtension(file.FileName);

            if(extention != ".pdf" && extention != ".docx" && extention != ".xlsx" && extention != ".doc" && extention != ".xls" &&
                extention != ".png" && extention != ".jpeg" && extention != ".jpg")
            {
                return false;
            }

            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    return ImageFormat.Jpeg.Equals(img.RawFormat) || ImageFormat.Png.Equals(img.RawFormat) || ImageFormat.Gif.Equals(img.RawFormat);
                }
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}