using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.T00L
{
    //通用的生成条码类
    public static class SerialnoHelp
    {
        /// <summary>
        /// 获取序列号
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<string> GetSerialnos(int count)
        {
            List<string> serialnos = new List<string>();
            for (int i = 0; i < count; i++)
            {
                string code = "1" + DateTime.Now.ToString("MMdd") + getSqu(Guid.NewGuid().ToString("N"));
                if (serialnos.Find(x => x == code) != null)
                {
                    i--;
                    continue;
                }
                serialnos.Add(code);
            }
            return serialnos;
        }

        public static string getSqu(string ss)
        {
            if (ss.Length >= 8)
            {
                ss = ss.Substring(ss.Length - 8, 8);
            }
            else
            {
                ss = "00000000" + ss;
                ss = ss.Substring(ss.Length - 8, 8);
            }
            return ss;
        }

    }
}
