using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edu.Models.Models
{
    public class Paging
    {
        private int _pageSiz;
        /// <summary>
        /// 每页数据量
        /// </summary>
        public int PageSiz
        {
            get { return _pageSiz == 0 ? 15 : _pageSiz; }
            set { _pageSiz = value; }
        }
        private int _pageCount;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get { return CounterPageCount(); }
            set { _pageCount = value; }
        }
        private int _pageNumber;
        /// <summary>
        /// 页码
        /// </summary>
        public int PageNumber
        {
            get { return _pageNumber - 1 == -1 ? 0 : _pageNumber - 1; }
            set { _pageNumber = value; }
        }
        /// <summary>
        /// 总数据量
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 实体
        /// </summary>
        public IQueryable Entity { get; set; }

        public IEnumerable EntityList { get; set; }
        /// <summary>
        /// 计算总页数
        /// </summary>
        /// <returns></returns>
        private int CounterPageCount()
        {
            if (Amount % PageSiz == 0)
            {
                return Amount / PageSiz >= 0 ? Amount / PageSiz : 0;
            }
            else
            {
                return Amount / PageSiz >= 0 ? Amount / PageSiz + 1 : 0;
            }
        }

    }
}
