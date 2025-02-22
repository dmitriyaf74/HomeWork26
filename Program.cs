using System.IO;
using System.IO.Enumeration;
using System.Text;
namespace HomeWork26
{
    internal class Program
    {

        static DirectoryInfo directoryInfo(string filePath)
        {
            var dirInfo = new DirectoryInfo(filePath);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            return dirInfo;
        }

        static void CreateTenFiles(DirectoryInfo dirInfo)
        {
            for (int i = 0; i < 10; i++)
            {
                var filePath = $"{dirInfo.FullName}\\File{i}";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                File.Create(filePath).Close();
            }

        }

        static void ScanDirAndRead(DirectoryInfo dirInfo)
        {
            foreach (var file in dirInfo.GetFiles())
            {
                Console.WriteLine($"{file.FullName}: {File.ReadAllText(file.FullName)}");
            }
        }

        static void ScanDirAndWriteFileName(DirectoryInfo dirInfo)
        {
            foreach (var file in dirInfo.GetFiles())
            {
                var fs = file.AppendText();
                try
                {
                    fs.Write($"РуТест-{file.Name} {DateTime.Now}");
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        static void Main(string[] args)
        {
            var BaseDir = @"c:\\Otus\\TestDir";
            //1. Создать директории c:\Otus\TestDir1 и c:\Otus\TestDir2 с помощью класса DirectoryInfo.
            var dirInfo1 = directoryInfo(@$"{BaseDir}1");
            var dirInfo2 = directoryInfo(@$"{BaseDir}2");
            //2. В каждой директории создать несколько файлов File1...File10 с помощью класса File.
            CreateTenFiles(dirInfo1);
            CreateTenFiles(dirInfo2);
            //3. В каждый файл записать его имя в кодировке UTF8. Учесть, что файл может быть удален, либо отсутствовать права на запись.
            //UTF-8 - default
            //4. Каждый файл дополнить текущей датой(значение DateTime.Now) любыми способами: синхронно и\или асинхронно.
            ScanDirAndWriteFileName(dirInfo1);
            ScanDirAndWriteFileName(dirInfo2);
            //5. Прочитать все файлы и вывести на консоль: имя_файла: текст + дополнение.
            ScanDirAndRead(dirInfo1);
            ScanDirAndRead(dirInfo2);
        }
    }
}
