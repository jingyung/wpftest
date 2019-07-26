using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace QuoteService
{
    public class QuoteService : QuoteServiceBase
    {
        SQuotaAPI.SQuotaAPI _api;
        public QuoteService()
        {
            _api = new SQuotaAPI.SQuotaAPI();
            _api.Connected += _api_Connected;
            _api.Disconnected += _api_Disconnected;
            _api.FQuotaData += _api_FQuotaData;
            _api.FQueryData += _api_FQueryData;

        }

        private void _api_Disconnected(string kind)
        {
            OnDisconnected();
        }

        private void _api_Connected(string kind)
        {
            OnConnected();
        }

        public override void Connect(string pHost, int pPort)
        {
            _api.FConnect(pHost, pPort);
        }

        public override void Subscribe(SymbolContract SymbolContract)
        {
            _api.SubscribeForginPrice(SymbolContract.EXCHANGE, SymbolContract.SYMBOL, SymbolContract.YM, ((char)SymbolContract.CP).ToString(), SymbolContract.STRIKEPRICE);
        }

        public override void UnSubscribe(SymbolContract SymbolContract)
        {
            _api.unSubscribeForginPrice(SymbolContract.EXCHANGE, SymbolContract.SYMBOL, SymbolContract.YM, ((char)SymbolContract.CP).ToString(), SymbolContract.STRIKEPRICE);

        }
        public override void Query(SymbolContract SymbolContract, string seq)
        {
            _api.QueryForginPrice(seq, SymbolContract.EXCHANGE, SymbolContract.SYMBOL, SymbolContract.YM, ((char)SymbolContract.CP).ToString(), SymbolContract.STRIKEPRICE);
        }


        private void _api_FQueryData(string Seq, string Format, string Data)
        {

        }

        private void _api_FQuotaData(string Format, string Data)
        {
            try
            {
                parseFMarketInfoData(Format + Data);

            }
            catch (Exception ex)
            {

            }
        }

        void parseFMarketInfoData(string strData)
        {

            try
            {
                string Format = strData.Substring(0, 2);
                string Data = strData.Substring(2);
                if (Format.Trim() == "PT")
                {
                    string[] Body = Data.Split('@');
                    string[] symbols = Body[0].Split('\x01');
                    string exchange = symbols[0];
                    string symbol = symbols[1];
                    string ym = symbols[2];
                    string cp = symbols[4];
                    string strike = symbols[3];

                    decimal DISPLAY_DENOMINATOR = decimal.Parse(symbols[5]);
                    decimal DISPLAY_MULTIPLY = decimal.Parse(symbols[6]);
                    for (int i = 1; i < Body.Length - 1; i++)
                    {
                        string[] data = Body[i].Split('\x01');
                        switch (data[0])
                        {
                            case "P1"://成交
                                {
                                    int Total = int.Parse(data[1]);
                                    decimal LastPrice = decimal.Parse(data[2]);
                                    int LastVolume = int.Parse(data[3]);
                                    string Time = data[4];
                                    TickDataTrade TickDataTrade = new TickDataTrade();
                                    TickDataTrade.EXCHAGE = exchange;
                                    TickDataTrade.SYMBOL = symbol;
                                    TickDataTrade.YM = ym;
                                    TickDataTrade.CP = cp;
                                    TickDataTrade.STRIKE = strike;
                                    TickDataTrade.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                    TickDataTrade.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                    TickDataTrade.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                    TickDataTrade.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                    TickDataTrade.Total = Total;
                                    TickDataTrade.LastPrice = LastPrice;
                                    TickDataTrade.LastVolume = LastVolume;
                                    TickDataTrade.Time = Time;
                                    OnTickDataTrade(TickDataTrade);
                                }
                                break;
                            case "P2"://biddom
                                decimal BidDOM1Price = decimal.Parse(data[1]);
                                int BidDOM1Volume = int.Parse(data[2]);
                                decimal BidDOM2Price = decimal.Parse(data[3]);
                                int BidDOM2Volume = int.Parse(data[4]);
                                decimal BidDOM3Price = decimal.Parse(data[5]);
                                int BidDOM3Volume = int.Parse(data[6]);
                                decimal BidDOM4Price = decimal.Parse(data[7]);
                                int BidDOM4Volume = int.Parse(data[8]);
                                decimal BidDOM5Price = decimal.Parse(data[9]);
                                int BidDOM5Volume = int.Parse(data[10]);
                                decimal BidDOM6Price = decimal.Parse(data[11]);
                                int BidDOM6Volume = int.Parse(data[12]);
                                decimal BidDOM7Price = decimal.Parse(data[13]);
                                int BidDOM7Volume = int.Parse(data[14]);
                                decimal BidDOM8Price = decimal.Parse(data[15]);
                                int BidDOM8Volume = int.Parse(data[16]);
                                decimal BidDOM9Price = decimal.Parse(data[17]);
                                int BidDOM9Volume = int.Parse(data[18]);
                                decimal BidDOM10Price = decimal.Parse(data[19]);
                                int BidDOM10Volume = int.Parse(data[20]);
                                TickDataBid TickDataBid = new TickDataBid();
                                TickDataBid.EXCHAGE = exchange;
                                TickDataBid.SYMBOL = symbol;
                                TickDataBid.YM = ym;
                                TickDataBid.CP = cp;
                                TickDataBid.STRIKE = strike;
                                TickDataBid.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                TickDataBid.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;

                                TickDataBid.BidDOM1Price = BidDOM1Price;
                                TickDataBid.BidDOM1Volume = BidDOM1Volume;
                                TickDataBid.BidDOM2Price = BidDOM2Price;
                                TickDataBid.BidDOM2Volume = BidDOM2Volume;
                                TickDataBid.BidDOM3Price = BidDOM3Price;
                                TickDataBid.BidDOM3Volume = BidDOM3Volume;
                                TickDataBid.BidDOM4Price = BidDOM4Price;
                                TickDataBid.BidDOM4Volume = BidDOM4Volume;
                                TickDataBid.BidDOM5Price = BidDOM5Price;
                                TickDataBid.BidDOM5Volume = BidDOM5Volume;
                                TickDataBid.BidDOM6Price = BidDOM6Price;
                                TickDataBid.BidDOM6Volume = BidDOM6Volume;
                                TickDataBid.BidDOM7Price = BidDOM7Price;
                                TickDataBid.BidDOM7Volume = BidDOM7Volume;
                                TickDataBid.BidDOM8Price = BidDOM8Price;
                                TickDataBid.BidDOM8Volume = BidDOM8Volume;
                                TickDataBid.BidDOM9Price = BidDOM9Price;
                                TickDataBid.BidDOM9Volume = BidDOM9Volume;
                                TickDataBid.BidDOM10Price = BidDOM10Price;
                                TickDataBid.BidDOM10Volume = BidDOM10Volume;
                                OnTickDataBid(TickDataBid);
                                break;
                            case "P3"://offerdom
                                decimal OfferDOM1Price = decimal.Parse(data[1]);
                                int OfferDOM1Volume = int.Parse(data[2]);
                                decimal OfferDOM2Price = decimal.Parse(data[3]);
                                int OfferDOM2Volume = int.Parse(data[4]);
                                decimal OfferDOM3Price = decimal.Parse(data[5]);
                                int OfferDOM3Volume = int.Parse(data[6]);
                                decimal OfferDOM4Price = decimal.Parse(data[7]);
                                int OfferDOM4Volume = int.Parse(data[8]);
                                decimal OfferDOM5Price = decimal.Parse(data[9]);
                                int OfferDOM5Volume = int.Parse(data[10]);
                                decimal OfferDOM6Price = decimal.Parse(data[11]);
                                int OfferDOM6Volume = int.Parse(data[12]);
                                decimal OfferDOM7Price = decimal.Parse(data[13]);
                                int OfferDOM7Volume = int.Parse(data[14]);
                                decimal OfferDOM8Price = decimal.Parse(data[15]);
                                int OfferDOM8Volume = int.Parse(data[16]);
                                decimal OfferDOM9Price = decimal.Parse(data[17]);
                                int OfferDOM9Volume = int.Parse(data[18]);
                                decimal OfferDOM10Price = decimal.Parse(data[19]);
                                int OfferDOM10Volume = int.Parse(data[20]);
                                TickDataOffer TickDataOffer = new TickDataOffer();
                                TickDataOffer.EXCHAGE = exchange;
                                TickDataOffer.SYMBOL = symbol;
                                TickDataOffer.YM = ym;
                                TickDataOffer.CP = cp;
                                TickDataOffer.STRIKE = strike;
                                TickDataOffer.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                TickDataOffer.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;

                                TickDataOffer.OfferDOM1Price = OfferDOM1Price;
                                TickDataOffer.OfferDOM1Volume = OfferDOM1Volume;
                                TickDataOffer.OfferDOM2Price = OfferDOM2Price;
                                TickDataOffer.OfferDOM2Volume = OfferDOM2Volume;
                                TickDataOffer.OfferDOM3Price = OfferDOM3Price;
                                TickDataOffer.OfferDOM3Volume = OfferDOM3Volume;
                                TickDataOffer.OfferDOM4Price = OfferDOM4Price;
                                TickDataOffer.OfferDOM4Volume = OfferDOM4Volume;
                                TickDataOffer.OfferDOM5Price = OfferDOM5Price;
                                TickDataOffer.OfferDOM5Volume = OfferDOM5Volume;
                                TickDataOffer.OfferDOM6Price = OfferDOM6Price;
                                TickDataOffer.OfferDOM6Volume = OfferDOM6Volume;
                                TickDataOffer.OfferDOM7Price = OfferDOM7Price;
                                TickDataOffer.OfferDOM7Volume = OfferDOM7Volume;
                                TickDataOffer.OfferDOM8Price = OfferDOM8Price;
                                TickDataOffer.OfferDOM8Volume = OfferDOM8Volume;
                                TickDataOffer.OfferDOM9Price = OfferDOM9Price;
                                TickDataOffer.OfferDOM9Volume = OfferDOM9Volume;
                                TickDataOffer.OfferDOM10Price = OfferDOM10Price;
                                TickDataOffer.OfferDOM10Volume = OfferDOM10Volume;
                                OnTickDataOffer(TickDataOffer);
                                break;
                            case "P4":// Implied bid Offer
                                //decimal ImpliedBidPrice = decimal.Parse(data[1]);
                                //int ImpliedBidVolume = int.Parse(data[2]);
                                //decimal ImpliedOfferPrice = decimal.Parse(data[3]);
                                //int ImpliedOfferVolume = int.Parse(data[4]);

                                //TickDataImplied TickDataImplied = new TickDataImplied();
                                //TickDataImplied.EXCHAGE = exchange;
                                //TickDataImplied.SYMBOL = symbol;
                                //TickDataImplied.YM = ym;
                                //TickDataImplied.CP = cp;
                                //TickDataImplied.STRIKE = strike;
                                //TickDataImplied.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                //TickDataImplied.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                //TickDataImplied.ImpliedBidPrice = ImpliedBidPrice;
                                //TickDataImplied.ImpliedBidVolume = ImpliedBidVolume;
                                //TickDataImplied.ImpliedOfferPrice = ImpliedOfferPrice;
                                //TickDataImplied.ImpliedOfferVolume = ImpliedOfferVolume;

                                //objReturn.Add(TickDataImplied);
                                break;
                            case "P5"://high low 
                                decimal high = decimal.Parse(data[1]);
                                decimal low = decimal.Parse(data[2]);
                                TickDataHighLow TickDataHighLow = new TickDataHighLow();
                                TickDataHighLow.EXCHAGE = exchange;
                                TickDataHighLow.SYMBOL = symbol;
                                TickDataHighLow.YM = ym;
                                TickDataHighLow.CP = cp;
                                TickDataHighLow.STRIKE = strike;
                                TickDataHighLow.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                TickDataHighLow.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                TickDataHighLow.High = high;
                                TickDataHighLow.Low = low;
                                OnTickDataHighLow(TickDataHighLow);
                                break;
                            case "P6"://open  close

                                decimal open = decimal.Parse(data[1]);
                                decimal close = decimal.Parse(data[2]);
                                TickDataOpenclose TickDataOpenclose = new TickDataOpenclose();
                                TickDataOpenclose.EXCHAGE = exchange;
                                TickDataOpenclose.SYMBOL = symbol;
                                TickDataOpenclose.YM = ym;
                                TickDataOpenclose.CP = cp;
                                TickDataOpenclose.STRIKE = strike;
                                TickDataOpenclose.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                TickDataOpenclose.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                TickDataOpenclose.Opening = open;
                                TickDataOpenclose.Closing = close;
                                OnTickDataOpenclose(TickDataOpenclose);
                                break;
                            case "P7"://Settlement   
                                decimal currentsettl = decimal.Parse(data[1]);
                                decimal newsettl = decimal.Parse(data[2]);
                                TickDataSettle TickDataSettle = new TickDataSettle();
                                TickDataSettle.EXCHAGE = exchange;
                                TickDataSettle.SYMBOL = symbol;
                                TickDataSettle.YM = ym;
                                TickDataSettle.CP = cp;
                                TickDataSettle.STRIKE = strike;
                                TickDataSettle.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                TickDataSettle.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                TickDataSettle.CurrStl = currentsettl;
                                TickDataSettle.NewStl = newsettl;
                                OnTickDataSettle(TickDataSettle);
                                break;

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
             ;
        }
    }
}
