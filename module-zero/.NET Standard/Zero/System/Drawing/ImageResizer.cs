using System;
using System.Drawing;
using System.IO;

namespace Zero.System.Drawing
{
    public class ImageResizer
    {
        public void ScaleAndSaveImage(byte[] file, int maxWidth, string savePath)
        {
            using (var memoryStream = new MemoryStream(file))
            {
                var image = Image.FromStream(memoryStream);
                if (image.Width < maxWidth && image.Height < maxWidth)
                {
                    image.Save(savePath);
                }
                else
                {
                    int maxLength = Math.Max(image.Width, image.Height);
                    float radio = maxLength / (float)maxWidth;
                    int height = (int)(image.Height / radio);
                    int width = (int)(image.Width / radio);

                    var scaleImage = GetScaleImage(image, width, height);
                    scaleImage.Save(savePath);
                    scaleImage.Dispose();
                }
                image.Dispose();
            }
        }

        private Bitmap GetScaleImage(Image image, int width, int height)
        {
            var newImage = new Bitmap(width, height);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, width, height);
            Bitmap bmp = new Bitmap(newImage);

            return bmp;
        }
    }
}