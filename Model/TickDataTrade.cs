using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TickDataTrade
    {
        private string _EXCHAGE = "";
        private string _SYMBOL = "";
        private string _YM = "";
        private string _CP = "";
        private string _STRIKE = "";
        private decimal _DISPLAY_DENOMINATOR = 0;//分母
        private decimal _DISPLAY_MULTIPLY = 0;//倍率 
        private int _Total = 0;
        private decimal _LastPrice = 0;
        private int _LastVolume = 0;
        private string _Time = "";
        public TickDataTrade(string pExchange, string pSymbol, string pYearMonth, string CP, string pStrike)
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
        /// 總量
        /// </summary>
        public int Total
        {
            get { return _Total; }
            set { _Total = value; }
        }
        /// <summary>
        /// 成交價
        /// </summary>
        public decimal LastPrice
        {
            get { return _LastPrice; }
            set { _LastPrice = value; }
        }
        /// <summary>
        /// 成交量
        /// </summary>
        public int LastVolume
        {
            get { return _LastVolume; }
            set { _LastVolume = value; }
        }
        /// <summary>
        /// 成交時間
        /// </summary>
        public string Time
        {
            get { return _Time; }
            set { _Time = value; }
        }
        public decimal maxPriceByTime;
        public decimal minPriceByTime;
    }
}
