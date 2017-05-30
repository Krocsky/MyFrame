using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Common.Utilities
{
    /// <summary>
    /// 图片工具类
    /// </summary>
    public static class ImageHelper
    {
        public static Image Resize(Image image, int width, int height)
        {
            if (width > image.Height && height > image.Width)
            {
                return image;
            }

            Size bitmapSize = Size.Empty;
            Size outputSize = GetNewSize(image, new Size(width, height), out bitmapSize);

            Bitmap outputBmp = new Bitmap(bitmapSize.Width, bitmapSize.Height);

            using (Graphics g = Graphics.FromImage(outputBmp))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;

                Rectangle destRect = new Rectangle(new Point(0, 0), outputSize);
                Rectangle sourceRect = new Rectangle(0, 0, image.Width, image.Height);

                float outputAspect = (float)outputSize.Width / (float)outputSize.Height;

                g.DrawImage(image, destRect, sourceRect, GraphicsUnit.Pixel);
            }
            image.Dispose();
            return outputBmp;
        }

        public static Image Crop(Image image, int x, int y, int width, int height)
        {
            if (width > image.Height && height > image.Width)
            {
                return image;
            }

            Size imageSize = image.Size;
            Rectangle srcRect = new Rectangle(x, y, width, height);

            int x2 = srcRect.X + srcRect.Width;
            if (x2 > image.Width)
                srcRect.Width -= (x2 - image.Width);

            int y2 = srcRect.Y + srcRect.Height;
            if (y2 > image.Height)
                srcRect.Height -= (y2 - image.Height);

            Bitmap outputBitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(outputBitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;

                Rectangle destRect = new Rectangle(0, 0, width, height);
                g.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            }

            image.Dispose();

            return outputBitmap;
        }


        private static Size GetNewSize(Image img, Size requestedSize, out Size bitmapSize)
        {
            Size outputSize = new Size();

            if (img.Width <= requestedSize.Width && img.Height <= requestedSize.Height)
            {
                outputSize.Width = img.Width;
                outputSize.Height = img.Height;
            }
            else
            {
                float imgRatio = (float)img.Width / (float)img.Height;
                float requestedRatio = (float)requestedSize.Width / (float)requestedSize.Height;

                if (imgRatio <= requestedRatio)
                {
                    outputSize.Width = (int)((float)requestedSize.Height * imgRatio);
                    outputSize.Height = requestedSize.Height;
                }
                else
                {
                    outputSize.Width = requestedSize.Width;
                    outputSize.Height = (int)((float)requestedSize.Width / imgRatio);
                }
            }

            bitmapSize = outputSize;

            return outputSize;
        }
    }
}
