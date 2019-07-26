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
        public string EXCHANGE
        {
            get => exchange;
            set => exchange = value;
        }
        public string SYMBOL
        {
            get => symbol;
            set => symbol = value;
        }
        public string YM
        {
            get => ym;
            set => ym = value;
        }
        public CPEnum CP
        {
            get => cp;
            set => cp = value;
        }
        public string STRIKEPRICE
        {
            get => strike;
            set => strike = value;
        }
    }
    /// <summary>
}
