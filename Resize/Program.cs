using System.Drawing;

namespace Resize
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Resizer");

            Thread thread = new Thread(Resize);
            thread.Start();

            static void Resize()
            {
                #region Directories
                string inputDirectory = "InputFiles";
                string resizedDirectory = "ResizedFiles";
                string finishedDirectory = "FinishedFiles";

                if (!Directory.Exists(inputDirectory))
                    Directory.CreateDirectory(inputDirectory);

                if (!Directory.Exists(resizedDirectory))
                    Directory.CreateDirectory(resizedDirectory);

                if (!Directory.Exists(finishedDirectory))
                    Directory.CreateDirectory(finishedDirectory);
                #endregion

                FileStream fs;
                FileInfo fi;

                while (true)
                {
                    var inputFiles = Directory.EnumerateFiles(inputDirectory);

                    int newHeight = 200;

                    foreach (var file in inputFiles)
                    {
                        fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                        fi = new FileInfo(file);

                        string path = Environment.CurrentDirectory + @"\" + resizedDirectory + @"\" + DateTime.Now.Millisecond.ToString() + "_" + fi.Name;

                        Resizer(Image.FromStream(fs), newHeight, path);

                        fs.Close();

                        string finishedPath = Environment.CurrentDirectory + @"\" + finishedDirectory + @"\" + fi.Name;

                        fi.MoveTo(finishedPath);
                    }

                    Thread.Sleep(new TimeSpan(0, 0, 5));
                }
            }

            static void Resizer(Image image, int height, string path)
            {
                double ratio = (double)height / image.Width;
                int newWidth = (int)(image.Width * ratio);
                int newHeight = (int)(image.Height * ratio);

                Bitmap newImg = new Bitmap(newWidth, newHeight);

                using (Graphics g = Graphics.FromImage(newImg))
                {
                    g.DrawImage(image, 0, 0, newWidth, newHeight);
                }

                newImg.Save(path);
                image.Dispose();
            }
        }
    }
}