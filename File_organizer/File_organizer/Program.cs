using System;
using System.IO;
using ImageMagick;
using System.Diagnostics;

namespace File_organizer
{
    class Program
    {
        // timer
        static public Stopwatch _stopwatch = new Stopwatch();

        static void Main(string[] args)
        {
            string srcDirPath = @"C:\Users\dljdg\Desktop\test";
            string trgDirPath = @"C:\Users\dljdg\Desktop\test\정리";

            _stopwatch.Start();
            OnProcess(srcDirPath, trgDirPath);
            _stopwatch.Stop();

            Console.WriteLine("총 걸린 시간은 : " + _stopwatch.Elapsed.TotalSeconds.ToString() + "sec");
        }

        /// <summary>
        /// 저장할 경로의 디렉토리를 생성하고 소스 디렉토리 파일을 처리
        /// </summary>
        /// <param name="srcDirPath"></param>
        /// <param name="trgDirPath"></param>
        static private void OnProcess(string srcDirPath, string trgDirPath)
        {
            DirectoryInfo srcDirInfo = new DirectoryInfo(srcDirPath);
            DirectoryInfo trgDirInfo = new DirectoryInfo(trgDirPath);

            if (!trgDirInfo.Exists)
                trgDirInfo.Create();

            foreach(FileInfo file in srcDirInfo.GetFiles())
            {
                FileByDateTimeOriginal(file.FullName, trgDirPath);

                FileProfileCheck(file.FullName);
            }
        }

        /// <summary>
        /// 찍은 날짜 기준으로 폴더 생성 후 파일 분류
        /// </summary>
        /// <param name="filePath">파일 경로</param>
        /// <param name="trgDirPath">저장 경로</param>
        static private void FileByDateTimeOriginal(string filePath, string trgDirPath)
        {
            using (MagickImage img = new MagickImage(filePath))
            {
                string dateTime = null;

                foreach (IExifValue value in img.GetExifProfile().Values)
                {
                    if (value.Tag.ToString() == "DateTimeOriginal")
                    {
                        Console.WriteLine("{0}({1}): {2}", value.Tag, value.DataType, value.ToString());
                        dateTime = value.ToString();
                    }
                }

                if (dateTime == null)
                    return;

                string[] dateTimes = dateTime.Split();
                string date = dateTimes[0].Replace(":", ".");
                string saveDirPath = Path.Combine(trgDirPath, date);

                DirectoryInfo saveDir = new DirectoryInfo(saveDirPath);
                FileInfo imgFile = new FileInfo(filePath);

                if (!saveDir.Exists)
                    saveDir.Create();

                string saveFilePath = Path.Combine(saveDirPath, Path.GetFileName(filePath));
                imgFile.MoveTo(saveFilePath);
            }
        }

        static private void FileByDate(string filePath, string trgDirPath)
        {
            using (MagickImage img = new MagickImage(filePath))
            {
                string dateTime = null;

                foreach (IExifValue value in img.GetExifProfile().Values)
                {
                    if (value.Tag.ToString() == "DateTime")
                    {
                        Console.WriteLine("{0}({1}): {2}", value.Tag, value.DataType, value.ToString());
                        dateTime = value.ToString();
                    }
                }

                if (dateTime == null)
                    return;

                string[] dateTimes = dateTime.Split();
                string date = dateTimes[0].Replace(":", ".");
                string saveDirPath = Path.Combine(trgDirPath, date);

                DirectoryInfo saveDir = new DirectoryInfo(saveDirPath);
                FileInfo imgFile = new FileInfo(filePath);

                if (!saveDir.Exists)
                    saveDir.Create();

                string saveFilePath = Path.Combine(saveDirPath, Path.GetFileName(filePath));
                imgFile.MoveTo(saveFilePath);
            }
        }

        static private void FileProfileCheck(string filePath)
        {
            using (MagickImage img = new MagickImage(filePath))
            {
                foreach (IExifValue value in img.GetExifProfile().Values)
                    Console.WriteLine("{0}({1}): {2}", value.Tag, value.DataType, value.ToString());
            }
        }
    }
}
