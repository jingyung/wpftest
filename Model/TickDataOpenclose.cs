using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TickDataOpenclose
    {
        private string _EXCHAGE = "";
        private string _SYMBOL = "";
        private string _YM = "";
        private string _CP = "";
        private string _STRIKE = "";
        private decimal _DISPLAY_DENOMINATOR = 0;//分母
        private decimal _DISPLAY_MULTIPLY = 0;//倍率 

        private decimal _Opening = 0;
        private decimal _Closing = 0;
        /// <summary>
        /// 交易所
        /// </summary>
        public string EXCHAGE
        {
            get { return _EXCHAGE; }
            set { _EXCHAGE = value; }
        }
        /// <summary>
        /// 商品代碼
        /// </summary>
        public string SYMBOL
        {
            get { return _SYMBOL; }
            set { _SYMBOL = value; }
        }
        /// <summary>
        /// 年月
        /// </summary>
        public string YM
        {
            get { return _YM; }
            set { _YM = value; }
        }
        /// <summary>
        /// CP
        /// </summary>
        public string CP
        {
            get { return _CP; }
            set { _CP = value; }
        }
        /// <summary>
        /// 履約價
        /// </summary>
        public string STRIKE
        {
            get { return _STRIKE; }
            set { _STRIKE = value; }
        }
        /// <summary>
        /// 分母
        /// </summary>
        public decimal DISPLAY_DENOMINATOR
        {
            get { return _DISPLAY_DENOMINATOR; }
            set { _DISPLAY_DENOMINATOR = value; }
        }
        /// <summary>
        /// 倍率
        /// </summary>
        public decimal DISPLAY_MULTIPLY
        {
            get { return _DISPLAY_MULTIPLY; }
            set { _DISPLAY_MULTIPLY = value; }
        }

        /// <summary>
        /// 開盤價
        /// </summary>
        public decimal Opening
        {
            get { return _Opening; }
            set { _Opening = value; }
        }
        /// <summary>
        /// 收盤價
        /// </summary>
        public decimal Closing
        {
            get { return _Closing; }
            set { _Closing = value; }
        }
    }
}
