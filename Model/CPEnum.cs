using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum CPEnum
    {
        /// <summary>
        /// 期貨
        /// </summary>
        Future = 'F',
        /// <summary>
        /// 買權
        /// </summary>
        Call = 'C',
        /// <summary>
        /// 賣權
        /// </summary>
        Put = 'P'
    }
}
