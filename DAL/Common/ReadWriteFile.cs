using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DAL.Models
{
    public class ReadWriteFile
    {
        public static bool SaveFile(string path, string Contents)
        {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            try
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(Contents);
                    return true;
                }
            }
            catch { 
                return false; 
            }
            finally { fs.Close(); }
        }
        public static string ReadFile(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    return sr.ReadToEnd();
                }
            }
            catch { return ""; }
        }
        public static void LogFile(string path, string Contents)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
                {
                    byte[] data = new UTF8Encoding(true).GetBytes(Contents);
                    fs.Write(data, 0, data.Length);
                }
            }
            catch { }
        }
        public static void DeleteFiles(string directoryPath)
        {
            foreach (string item in System.IO.Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories))
            {
                try
                {
                    FileInfo obFileInfo = new FileInfo(item);
                    obFileInfo.Refresh();
                    DateTime fileCreation = obFileInfo.LastWriteTime;
                    DateTime NewTime = DateTime.Now.AddMinutes(-30);
                    if (NewTime > fileCreation)
                    {
                        try
                        {
                            System.IO.File.Delete(obFileInfo.FullName);
                        }
                        catch { }
                    }
                }
                catch (Exception ex)
                {
                    Common.ErrorHandler.WriteError(ex);
                }
            }
        }
        public static void DeleteErrorFiles(string directoryPath)
        {
            foreach (string item in System.IO.Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories))
            {
                try
                {
                    FileInfo obFileInfo = new FileInfo(item);
                    obFileInfo.Refresh();
                    DateTime fileCreation = obFileInfo.LastWriteTime;
                    DateTime NewTime = DateTime.Now.AddMinutes(-1440);
                    if (NewTime > fileCreation)
                    {
                        try
                        {
                            System.IO.File.Delete(obFileInfo.FullName);
                        }
                        catch { }
                    }
                }
                catch (Exception ex)
                {
                    Common.ErrorHandler.WriteError(ex);
                }
            }
        }
        public static void DeleteLoginFiles(string directoryPath)
        {
            foreach (string item in System.IO.Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories))
            {
                try
                {
                    FileInfo obFileInfo = new FileInfo(item);
                    obFileInfo.Refresh();
                    DateTime fileCreation = obFileInfo.LastWriteTime;
                    DateTime NewTime = DateTime.Now.AddMinutes(-120);
                    if (NewTime > fileCreation)
                    {
                        try
                        {
                            System.IO.File.Delete(obFileInfo.FullName);
                        }
                        catch { }
                    }
                }
                catch (Exception ex)
                {
                    Common.ErrorHandler.WriteError(ex);
                }
            }
        }
    }
}
