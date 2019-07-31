using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TickData
    {
        public TickData()
        {

        }
        public TickData(string pExchange, string pSymbol, string pYearMonth, string CP, string pStrike)
        {
            SymbolContract = new SymbolContract(pExchange, pSymbol, pYearMonth, CP, pStrike);
        }
        public SymbolContract SymbolContract;

        public decimal DISPLAY_DENOMINATOR;
        public int Total;
        public decimal LastPrice;
        public int LastVolume;
        public string Time;
        public decimal BidDOM1Price;
        public int BidDOM1Volume;
        public decimal BidDOM2Price;
        public int BidDOM2Volume;
        public decimal BidDOM3Price;
        public int BidDOM3Volume;
        public decimal BidDOM4Price;
        public int BidDOM4Volume;
        public decimal BidDOM5Price;
        public int BidDOM5Volume;
        public decimal BidDOM6Price;
        public int BidDOM6Volume;
        public decimal BidDOM7Price;
        public int BidDOM7Volume;
        public decimal BidDOM8Price;
        public int BidDOM8Volume;
        public decimal BidDOM9Price;
        public int BidDOM9Volume;
        public decimal BidDOM10Price;
        public int BidDOM10Volume;

        public decimal OfferDOM1Price;
        public int OfferDOM1Volume;
        public decimal OfferDOM2Price;
        public int OfferDOM2Volume;
        public decimal OfferDOM3Price;
        public int OfferDOM3Volume;
        public decimal OfferDOM4Price;
        public int OfferDOM4Volume;
        public decimal OfferDOM5Price;
        public int OfferDOM5Volume;
        public decimal OfferDOM6Price;
        public int OfferDOM6Volume;
        public decimal OfferDOM7Price;
        public int OfferDOM7Volume;
        public decimal OfferDOM8Price;
        public int OfferDOM8Volume;
        public decimal OfferDOM9Price;
        public int OfferDOM9Volume;
        public decimal OfferDOM10Price;
        public int OfferDOM10Volume;

        public decimal High;
        public decimal Low;
        public decimal Opening;
        public decimal Closing;
        public decimal Settle;
    }
}