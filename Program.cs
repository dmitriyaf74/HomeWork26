using System.IO;
using System.IO.Enumeration;
using System.Security.AccessControl;
using System.Security.Principal;
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
                var filePath = Path.Combine(dirInfo.FullName, $"File{i}");
                if (File.Exists(filePath))
                {
                    //FileSecurity fileSecurity = System.Security.AccessControl.(filePath);
                    File.Delete(filePath);
                }
                using var fs = File.CreateText(filePath);
            }

        }

        static void ScanDirAndRead(DirectoryInfo dirInfo)
        {
            foreach (var file in dirInfo.GetFiles())
            {
                Console.WriteLine($"{file.FullName}: {File.ReadAllText(file.FullName)}");
            }
        }


        static bool CheckGrantOnWrite(string FileName) 
        {
            try
            {
                FileInfo fInfo = new FileInfo(FileName);

                FileSecurity fileSecurity = fInfo.GetAccessControl();
                AuthorizationRuleCollection rules = fileSecurity.GetAccessRules(true, true, typeof(NTAccount));
                WindowsIdentity currentUser = WindowsIdentity.GetCurrent();
                foreach (FileSystemAccessRule rule in rules) 
                {
                    if ((FileSystemRights.Write & rule.FileSystemRights) == FileSystemRights.Write)
                    { 
                        return true;
                    }
                }
                return false;
            }
            catch 
            {
                return false;
            }
        }

        static void ScanDirAndWriteFileName(DirectoryInfo dirInfo)
        {
            foreach (var file in dirInfo.GetFiles())
            {
                if (file.Exists)
                {
                    if (CheckGrantOnWrite(file.FullName))
                    {
                        using (var stream = new FileStream(file.FullName, FileMode.Open))
                        {
                            using (var writer = new StreamWriter(stream, Encoding.UTF8))
                            {
                                writer.Write($"{file.Name}");
                            }
                        }
                    }
                }
            }
        }

        static void ScanDirAndWriteFileNow(DirectoryInfo dirInfo)
        {
            foreach (var file in dirInfo.GetFiles())
            {
                using (var stream = new FileStream(file.FullName, FileMode.Open))
                {
                    stream.Seek(0, SeekOrigin.End);
                    using (var writer = new StreamWriter(stream, Encoding.UTF8))
                    {
                        writer.Write($" {DateTime.Now}");
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            var baseDir = @"c:\\Otus\\TestDir";
            //1. Создать директории c:\Otus\TestDir1 и c:\Otus\TestDir2 с помощью класса DirectoryInfo.
            var dirInfo1 = directoryInfo(@$"{baseDir}1");
            var dirInfo2 = directoryInfo(@$"{baseDir}2");
            //2. В каждой директории создать несколько файлов File1...File10 с помощью класса File.
            CreateTenFiles(dirInfo1);
            CreateTenFiles(dirInfo2);
            //3. В каждый файл записать его имя в кодировке UTF8. Учесть, что файл может быть удален, либо отсутствовать права на запись.
            //UTF-8 - default
            ScanDirAndWriteFileName(dirInfo1);
            ScanDirAndWriteFileName(dirInfo2);
            //4. Каждый файл дополнить текущей датой(значение DateTime.Now) любыми способами: синхронно и\или асинхронно.
            ScanDirAndWriteFileNow(dirInfo1);
            ScanDirAndWriteFileNow(dirInfo2);
            //5. Прочитать все файлы и вывести на консоль: имя_файла: текст + дополнение.
            ScanDirAndRead(dirInfo1);
            ScanDirAndRead(dirInfo2);
        }
    }
}
