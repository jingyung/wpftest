using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SymbolContract
    {
        private string exchange, symbol, ym, strike = "";
        CPEnum cp;
        public SymbolContract()
        {

        }
        public SymbolContract(string pExchange, string pSymbol, string pYearMonth, CPEnum CPEnum, string pStrike)
        {
            exchange = pExchange;
            symbol = pSymbol;
            ym = pYearMonth;
            strike = pStrike;
            cp = CPEnum;
        }
        public SymbolContract(string pExchange, string pSymbol, string pYearMonth, string CP, string pStrike)
        {
            exchange = pExchange;
            symbol = pSymbol;
            ym = pYearMonth;
            strike = pStrike;
            if (CP == "C") cp = CPEnum.Call;
            else if (CP == "P") cp = CPEnum.Put;
            else cp = CPEnum.Future;
        }
        public string Key { get => (exchange + Symbol + YearMonth + (CP== CPEnum.Future?"F":(CP == CPEnum.Call ? "C":"P")) + StrikePrice); }
        public string Exchange
        {
            get => exchange;
            set => exchange = value;
        }
        public string Symbol
        {
            get => symbol;
            set => symbol = value;
        }
        public string YearMonth
        {
            get => ym;
            set => ym = value;
        }
        public CPEnum CP
        {
            get => cp;
            set => cp = value;
        }
        public string StrikePrice
        {
            get => strike;
            set => strike = value;
        }
    }
    /// <summary>
}
