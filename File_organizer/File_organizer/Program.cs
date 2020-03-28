using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ImageMagick;
using System.Diagnostics;

namespace File_organizer
{
    class Program
    {
        static public Stopwatch _stopwatch = new Stopwatch();

        static void Main(string[] args)
        {
            string srcDirPath = @"C:\Users\dljdg\Desktop\test";
            string trgDirPath = @"C:\Users\dljdg\Desktop\test\hi";

            _stopwatch.Start();
            OnProcess(srcDirPath, trgDirPath);
            _stopwatch.Stop();

            Console.WriteLine("총 걸린 시간은 : " + _stopwatch.Elapsed.TotalSeconds.ToString() + "sec");
            //var info = new FileInfo(srcDirPath);
            //Console.WriteLine(info);
        }

        static private void OnProcess(string srcDirPath, string trgDirPath)
        {
            DirectoryInfo srcDirInfo = new DirectoryInfo(srcDirPath);
            DirectoryInfo trgDirInfo = new DirectoryInfo(trgDirPath);

            if (!trgDirInfo.Exists)
                trgDirInfo.Create();

            foreach(FileInfo file in srcDirInfo.GetFiles())
            {
                FileByDate(file.FullName, trgDirPath);
            }

        }

        static private void FileByDate(string filePath, string trgDirPath)
        {
            using (MagickImage img = new MagickImage(filePath))
            {
                IExifProfile profile = img.GetExifProfile();

                foreach (IExifValue value in profile.Values)
                {
                    if (value.Tag.ToString() == "DateTimeOriginal")
                    {
                        Console.WriteLine("{0}({1}): {2}", value.Tag, value.DataType, value.ToString());
                    }

                    // Console.WriteLine("{0}({1}): {2}", value.Tag, value.DataType, value.ToString());
                }

                //string saveDirPath = Path.Combine(trgDirPath, "");
            }
        }
    }
}
