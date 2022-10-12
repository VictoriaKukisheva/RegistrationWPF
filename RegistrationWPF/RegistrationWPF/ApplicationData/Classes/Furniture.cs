using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RegistrationWPF.ApplicationData
{
    public partial class Furniture
    {
        public BitmapImage CorrectImage
        {
            get
            {
                string fullpath = System.IO.Path.GetFullPath("Resourses");
                fullpath = fullpath.Replace(@"\bin\Debug", "");
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();

                if (String.IsNullOrEmpty(Image) || String.IsNullOrWhiteSpace(Image))
                {
                    fullpath += @"\Images\picture.png";
                    bmp.StreamSource = new MemoryStream(File.ReadAllBytes(fullpath));
                    bmp.EndInit();
                    return bmp;
                }
                else
                {
                    fullpath += @"\Images\" + Image;
                    bmp.StreamSource = new MemoryStream(File.ReadAllBytes(fullpath));
                    bmp.EndInit();
                    return bmp;
                }
            }
        }

        public Brush CountZero
        {
            get
            {
                if (Existance <= 0)
                {
                    Brush myBrush = new SolidColorBrush(Color.FromRgb(78,78,78));
                    return myBrush;
                }
                else
                {
                    Brush myBrush = new SolidColorBrush(Colors.White) { Opacity = 0.2 };
                    return myBrush;
                }
            }
        }
    }
}
