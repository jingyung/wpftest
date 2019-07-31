using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TickDataHighLow
    {
        private string _EXCHAGE = "";
        private string _SYMBOL = "";
        private string _YM = "";
        private string _CP = "";
        private string _STRIKE = "";
        private decimal _DISPLAY_DENOMINATOR = 0;//分母
        private decimal _DISPLAY_MULTIPLY = 0;//倍率 
        private decimal _High = 0;
        private decimal _Low = 0;
        public TickDataHighLow(string pExchange, string pSymbol, string pYearMonth, string CP, string pStrike)
        {
            SymbolContract = new SymbolContract(pExchange, pSymbol, pYearMonth, CP, pStrike);
        }
        public SymbolContract SymbolContract;

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
        /// 最高價
        /// </summary>
        public decimal High
        {
            get { return _High; }
            set { _High = value; }
        }
        /// <summary>
        /// 最低價
        /// </summary>
        public decimal Low
        {
            get { return _Low; }
            set { _Low = value; }
        }
    }
}
