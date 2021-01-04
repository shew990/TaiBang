using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.compter
{
    public class CompterService : DbContext<T_Compter>
    {
        /// <summary>
        /// 是否首道工序电脑
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool IsFirstStation(string ipAddress)
        {
            var firstIp = GetSugarQueryable().OrderBy(x => x.Id).First();
            if (firstIp.IpAddress == ipAddress)
                return true;
            return false;
        }

        public T_Compter GetCompter(string ipAddress)
        {
            var compter = GetSugarQueryable(x => x.IpAddress == ipAddress).First();
            return compter;
        }
    }
}
