using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;


namespace BILBasic.Basing
{
    public class LogInfo
    {
        public static void InfoLog(string txt)
        {
            try
            {
                txt = "InfoLog-" + DateTime.Now.ToString() + "----" + txt+ "\r\n";
                txt = txt + "----------------------------------------------\r\n";
                //string path = ConfigurationManager.AppSettings["logpath"];
                string path = Directory.GetCurrentDirectory() + "\\Log\\Info\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path += DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!File.Exists(path))
                {
                    File.Create(path);
                }
                FileStream fs;
                StreamWriter sw;
                fs = new FileStream(path, FileMode.Append);
                sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(txt);
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                //InfoLog("日志程序发生异常（WriteLog）。详情：" + ex.Message);
            }
        }

        public static void ErrorLog(string txt)
        {
            try
            {
                txt = "ErrorLog-" + DateTime.Now.ToString() + "----" + txt+"\r\n";
                txt = txt+"----------------------------------------------\r\n";
                //string path = ConfigurationManager.AppSettings["logpath"];
                string path = Directory.GetCurrentDirectory() + "\\Log\\Info\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path += DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!File.Exists(path))
                {
                    File.Create(path);
                }
                FileStream fs;
                StreamWriter sw;
                fs = new FileStream(path, FileMode.Append);
                sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(txt);
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                //ErrorLog("日志程序发生异常（WriteLog）。详情：" + ex.Message);

            }
        }


        public static void ErpLog(string txt)
        {
            try
            {
                txt = "ErpLog-" + DateTime.Now.ToString() + "----" + txt + "\r\n";
                txt = txt + "----------------------------------------------\r\n";
                //string path = ConfigurationManager.AppSettings["logpath"];
                string path = Directory.GetCurrentDirectory() + "\\Log\\Erp\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path += DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!File.Exists(path))
                {
                    File.Create(path);
                }
                FileStream fs;
                StreamWriter sw;
                fs = new FileStream(path, FileMode.Append);
                sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(txt);
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                //InfoLog("日志程序发生异常（WriteLog）。详情：" + ex.Message);
            }
        }
    }
}
