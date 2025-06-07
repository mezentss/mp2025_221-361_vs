using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Linq;

namespace wfaPaint.Managers
{
    public class ImageManager
    {
        private readonly PictureBox canvas;
        private readonly SaveFileDialog saveFileDialog;

        public ImageManager(PictureBox canvas)
        {
            this.canvas = canvas;
            this.saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG Image|*.png|JPEG Image|*.jpg|TIFF Image|*.tiff|HEIF Image|*.heif",
                Title = "Сохранить изображение",
                DefaultExt = "png"
            };
        }

        public void SaveImageAs()
        {
            string defaultFileName = "wfaPaintImage.png";
            saveFileDialog.FileName = defaultFileName;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveImageToFile(saveFileDialog.FileName);
            }
        }

        private void SaveImageToFile(string fileName)
        {
            try
            {
                using (Bitmap bitmap = new Bitmap(canvas.Width, canvas.Height))
                {
                    canvas.DrawToBitmap(bitmap, new Rectangle(0, 0, canvas.Width, canvas.Height));
                    
                    string extension = System.IO.Path.GetExtension(fileName).ToLower();
                    ImageFormat format = GetFormatFromExtension(extension);
                    
                    if (format == ImageFormat.Tiff)
                    {
                        ImageCodecInfo tiffCodec = ImageCodecInfo.GetImageEncoders()
                            .FirstOrDefault(codec => codec.FormatID == ImageFormat.Tiff.Guid);
                        
                        if (tiffCodec != null)
                        {
                            EncoderParameters encoderParams = new EncoderParameters(1);
                            encoderParams.Param[0] = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionLZW);
                            bitmap.Save(fileName, tiffCodec, encoderParams);
                        }
                        else
                        {
                            bitmap.Save(fileName, format);
                        }
                    }
                    else if (format == ImageFormat.Jpeg)
                    {
                        ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageEncoders()
                            .FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
                        
                        if (jpegCodec != null)
                        {
                            EncoderParameters encoderParams = new EncoderParameters(1);
                            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                            bitmap.Save(fileName, jpegCodec, encoderParams);
                        }
                        else
                        {
                            bitmap.Save(fileName, format);
                        }
                    }
                    else
                    {
                        bitmap.Save(fileName, format);
                    }
                }
                MessageBox.Show("Изображение успешно сохранено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Ошибка при сохранении изображения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ImageFormat GetFormatFromExtension(string extension)
        {
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".tiff":
                    return ImageFormat.Tiff;
                case ".heif":
                    return ImageFormat.Heif;
                default:
                    return ImageFormat.Png;
            }
        }
    }
} 