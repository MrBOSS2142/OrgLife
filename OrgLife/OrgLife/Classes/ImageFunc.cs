using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OrgLife
{
    public class ImageFunc
    {
        public static byte[] ConvertImageToBinary(string path)
        {
            byte[] images = null;
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            images = br.ReadBytes((int)fs.Length);
            return images;
        }

        public static ImageSource ConvertImageFromBinary(byte[] img)
        {
            MemoryStream ms = new MemoryStream(img);
            System.Drawing.Image im = System.Drawing.Image.FromStream(ms);
            MemoryStream mes = new MemoryStream();
            im.Save(mes, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = mes;
            bitmap.EndInit();
            return bitmap;
        }
    }
}
