using BILBasic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.WMS.Models
{
    public class BaseModel<T>
    {

        /// <summary>
        /// 状态 1成功 0 失败
        /// </summary>
        public int Result { get; set; }
        public string ResultValue { get; set; }
        public T Data { get; set; }
        public R_Pagedata PageData { get; set; }

    }

    public class R_Pagedata
    {
        public int totalCount { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public int totalPages { get; set; }
    }

    public class PageRequest<T>
    {
        public PageRequest()
        {
            _RecordCounts = 0;
            _CurrentPageNumber = 1;
            _PagesCount = 2;
        }

        private int _RecordCounts = 0;
        /// <summary>
        /// 记录总数
        /// </summary>
        public int RecordCounts
        {
            get
            {
                return _RecordCounts;
            }
            set
            {
                _RecordCounts = value;
            }
        }


        private int _CurrentPageRecordCounts;
        /// <summary>
        /// 当前页记录数
        /// </summary>
        public int CurrentPageRecordCounts
        {
            get
            {
                return _CurrentPageRecordCounts;
            }

            set
            {
                _CurrentPageRecordCounts = value;
            }
        }

        private int _CurrentPageShowCounts = 20;
        /// <summary>
        /// 当前页显示行数
        /// </summary>
        public int CurrentPageShowCounts
        {
            get
            {
                return _CurrentPageShowCounts;
            }

            set
            {
                _CurrentPageShowCounts = value;
            }
        }

        private int _CurrentPageNumber;
        /// <summary>
        /// 当前页数
        /// </summary>
        public int CurrentPageNumber
        {
            get
            {
                return _CurrentPageNumber;
            }

            set
            {
                _CurrentPageNumber = value;
            }
        }

        private int _PagesCount;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PagesCount
        {
            get
            {
                return _PagesCount;
            }

            set
            {
                _PagesCount = value;
            }
        }
        public T model { get; set; }
    }
}