using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TickDataSettle
    {
        private string _EXCHAGE = "";
        private string _SYMBOL = "";
        private string _YM = "";
        private string _CP = "";
        private string _STRIKE = "";
        private decimal _DISPLAY_DENOMINATOR = 0;//分母
        private decimal _DISPLAY_MULTIPLY = 0;//倍率 
        private decimal _CurrStl = 0;//現在結算價
        private decimal _NewStl = 0;
        public TickDataSettle(string pExchange, string pSymbol, string pYearMonth, string CP, string pStrike)
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
        /// 目前結算價
        /// </summary>
        public decimal CurrStl
        {
            get { return _CurrStl; }
            set { _CurrStl = value; }
        }
        /// <summary>
        /// 最新結算價
        /// </summary>
        public decimal NewStl
        {
            get { return _NewStl; }
            set { _NewStl = value; }
        }
    }
}
