using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Contec.Framework.Utilities
{
    public class Images
    {
        public static string ResizeWithWidthConstraint(string filePath, int width)
        {
            string resizedFilePath = string.Empty;

            try
            {
                using (var originalImage = Image.FromFile(filePath))
                {
                    // Resize the image to the specified width
                    var resizedImage = Resize(originalImage, width, ConstraintType.Width);
                    resizedFilePath =
                        string.Format(@"{0}\{1}_thumb.png", Path.GetDirectoryName(filePath),
                            Path.GetFileNameWithoutExtension(filePath));

                    // Overwrite
                    resizedImage.Save(resizedFilePath, ImageFormat.Png);
                }
            }
            catch
            {

            }

            return resizedFilePath;
        }

        public static string ResizeWithHeightConstraint(string filePath, int height)
        {
            var resizedFilePath = string.Empty;

            try
            {
                using (var originalImage = Image.FromFile(filePath))
                {
                    // Resize the image to the specified width
                    var resizedImage = Resize(originalImage, height, ConstraintType.Height);
                    resizedFilePath =
                        string.Format(@"{0}\{1}_thumb.png", Path.GetDirectoryName(filePath),
                            Path.GetFileNameWithoutExtension(filePath));

                    // Overwrite
                    resizedImage.Save(resizedFilePath, ImageFormat.Png);
                }
            }
            catch
            {

            }

            return resizedFilePath;
        }

        public static Image Resize(Image originalImage, int width, int height)
        {
            var ratioX = (double)width / originalImage.Width;
            var ratioY = (double)height / originalImage.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(originalImage.Width * ratio);
            var newHeight = (int)(originalImage.Height * ratio);

            var resizedImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
            resizedImage.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);

            using (var gr = Graphics.FromImage(resizedImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.CompositingQuality = CompositingQuality.HighQuality;

                gr.DrawImage(resizedImage, new Rectangle(0, 0, newWidth, newHeight));
            }

            return resizedImage;
        }

        public static Image Resize(Image originalImage, int percent)
        {
            var nPercent = (Convert.ToSingle(percent) / 100);
            var originalWidth = originalImage.Width;
            var originalHeight = originalImage.Height;

            var resizedWidth = Convert.ToInt32((originalWidth * nPercent));
            var resizedHeight = Convert.ToInt32((originalHeight * nPercent));

            var resizedImage = new Bitmap(resizedWidth, resizedHeight, PixelFormat.Format24bppRgb);
            resizedImage.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);

            using (var gr = Graphics.FromImage(resizedImage))
            {
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.CompositingQuality = CompositingQuality.HighQuality;

                gr.DrawImage(originalImage, new Rectangle(0, 0, resizedWidth, resizedHeight), new Rectangle(0, 0, originalWidth, originalHeight), GraphicsUnit.Pixel);
            }

            return resizedImage;
        }

        public static Image Resize(Image originalImage, int size, ConstraintType constraintType)
        {
            var sourceWidth = originalImage.Width;
            var sourceHeight = originalImage.Height;
            const int sourceX = 0;
            const int sourceY = 0;
            const int destX = 0;
            const int destY = 0;
            float nPercent = 0;

            switch (constraintType)
            {
                case ConstraintType.Width:
                    nPercent = (Convert.ToSingle(size) / Convert.ToSingle(sourceWidth));

                    break;
                default:
                    nPercent = (Convert.ToSingle(size) / Convert.ToSingle(sourceHeight));

                    break;
            }

            var destWidth = Convert.ToInt32((sourceWidth * nPercent));
            var destHeight = Convert.ToInt32((sourceHeight * nPercent));

            var bmPhoto = new Bitmap(destWidth, destHeight);
            bmPhoto.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);

            using (var grPhoto = Graphics.FromImage(bmPhoto))
            {
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
                grPhoto.CompositingQuality = CompositingQuality.HighQuality;

                using (var imageAttribute = new ImageAttributes())
                {
                    imageAttribute.SetWrapMode(WrapMode.TileFlipXY);

                    var destRect = new Rectangle(destX, destY, destWidth, destHeight);

                    grPhoto.DrawImage(originalImage, destRect, sourceX, sourceY, sourceWidth, sourceHeight,
                        GraphicsUnit.Pixel, imageAttribute);
                }
                originalImage.Dispose();
            }

            return bmPhoto;
        }

        public static Image FixedSize(Image originalImage, int width, int height)
        {
            var sourceWidth = originalImage.Width;
            var sourceHeight = originalImage.Height;
            const int sourceX = 0;
            const int sourceY = 0;
            var destX = 0;
            var destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            var destWidth = 0;
            var destHeight = 0;

            nPercentW = (Convert.ToSingle(width) / Convert.ToSingle(sourceWidth));
            nPercentH = (Convert.ToSingle(height) / Convert.ToSingle(sourceHeight));

            //if we have to pad the height pad both the top and the bottom
            //with the difference between the scaled height and the desired height
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = Convert.ToInt32(((width - (sourceWidth * nPercent)) / 2));
            }
            else
            {
                nPercent = nPercentW;
                destY = Convert.ToInt32(((height - (sourceHeight * nPercent)) / 2));
            }

            destWidth = Convert.ToInt32((sourceWidth * nPercent));
            destHeight = Convert.ToInt32((sourceHeight * nPercent));

            var bmPhoto = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);

            var grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
            grPhoto.CompositingQuality = CompositingQuality.HighQuality;

            grPhoto.DrawImage(originalImage, new Rectangle(destX, destY, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);

            originalImage.Dispose();

            grPhoto.Dispose();

            return bmPhoto;
        }

        public static Image Crop(Image imgPhoto, int width, int height, AnchorPosition anchor)
        {
            var sourceWidth = imgPhoto.Width;
            var sourceHeight = imgPhoto.Height;
            const int sourceX = 0;
            const int sourceY = 0;
            var destX = 0;
            var destY = 0;

            float nPercent;

            var nPercentW = (Convert.ToSingle(width) / Convert.ToSingle(sourceWidth));
            var nPercentH = (Convert.ToSingle(height) / Convert.ToSingle(sourceHeight));

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentW;

                switch (anchor)
                {
                    case AnchorPosition.Top:
                        destY = 0;

                        break;
                    case AnchorPosition.Bottom:
                        destY = Convert.ToInt32((height - (sourceHeight * nPercent)));

                        break;
                    default:
                        destY = Convert.ToInt32(((height - (sourceHeight * nPercent)) / 2));

                        break;
                }
            }
            else
            {
                nPercent = nPercentH;

                switch (anchor)
                {
                    case AnchorPosition.Left:
                        destX = 0;

                        break;
                    case AnchorPosition.Right:
                        destX = Convert.ToInt32((width - (sourceWidth * nPercent)));

                        break;
                    default:
                        destX = Convert.ToInt32(((width - (sourceWidth * nPercent)) / 2));

                        break;
                }
            }

            var destWidth = Convert.ToInt32((sourceWidth * nPercent));
            var destHeight = Convert.ToInt32((sourceHeight * nPercent));

            var bmPhoto = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            var grPhoto = Graphics.FromImage(bmPhoto);

            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
            grPhoto.CompositingQuality = CompositingQuality.HighQuality;

            grPhoto.DrawImage(imgPhoto, new Rectangle(destX, destY, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);

            grPhoto.Dispose();

            return bmPhoto;
        }

        public enum ConstraintType
        {
            Width,
            Height
        }

        public enum AnchorPosition
        {
            Top,
            Center,
            Bottom,
            Left,
            Right
        }
    }
}