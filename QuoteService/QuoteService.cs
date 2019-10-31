using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Model;
using System.Windows;
namespace QuoteService
{
    public class QuoteService : QuoteServiceBase
    {
        TickData _TickData;

        private readonly SynchronizationContext syncContext = SynchronizationContext.Current;
        Dictionary<string, TickDataTrade> _TickDataTrade = new Dictionary<string, TickDataTrade>();
        Dictionary<string, TickDataBid> _TickDataBid = new Dictionary<string, TickDataBid>();
        Dictionary<string, TickDataOffer> _TickDataOffer = new Dictionary<string, TickDataOffer>();
        Dictionary<string, TickDataHighLow> _TickDataHighLow = new Dictionary<string, TickDataHighLow>();
        Dictionary<string, TickDataSettle> _TickDataSettle = new Dictionary<string, TickDataSettle>();
        Dictionary<string, TickDataOpenclose> _TickDataOpenclose = new Dictionary<string, TickDataOpenclose>();
        private readonly Task _tasks;
        SQuotaAPI.SQuotaAPI _api;
        AutoResetEvent _AutoResetEvent = new AutoResetEvent(false);
        public QuoteService()
        {

            _api = new SQuotaAPI.SQuotaAPI();
            _api.Connected += _api_Connected;
            _api.Disconnected += _api_Disconnected;
            _api.FQuotaData += _api_FQuotaData;
            _api.FQueryData += _api_FQueryData;
            _tasks = Task.Factory.StartNew(() =>
            {
                while (true)
                {

                    syncContext.Post(new SendOrPostCallback(ExcuteFDisplay), null);
                    Thread.Sleep(10);
                }
            }, TaskCreationOptions.LongRunning);
        }
        private void ExcuteFDisplay(object obj)
        {
            long diff = 0;

            string ProductID = "";
            try
            {
                int tick = Environment.TickCount;
                lock (_TickDataTrade)
                {
                    Dictionary<string, TickDataTrade>.Enumerator e = _TickDataTrade.GetEnumerator();
                    while (e.MoveNext())
                    {
                        TickDataTrade item = e.Current.Value;
                        OnTickDataTrade(item);
                    }
                    int Count_TickDataTrade = _TickDataTrade.Count;
                    _TickDataTrade.Clear();


                }
                lock (_TickDataBid)
                {
                    Dictionary<string, TickDataBid>.Enumerator e = _TickDataBid.GetEnumerator();
                    while (e.MoveNext())
                    {
                        TickDataBid item = e.Current.Value;
                        OnTickDataBid(item);
                    }
                    int Count_TickDataTrade = _TickDataBid.Count;
                    _TickDataBid.Clear();

                }
                lock (_TickDataOffer)
                {
                    Dictionary<string, TickDataOffer>.Enumerator e = _TickDataOffer.GetEnumerator();
                    while (e.MoveNext())
                    {
                        TickDataOffer item = e.Current.Value;
                        OnTickDataOffer(item);
                    }
                    int Count_TickDataOffer = _TickDataOffer.Count;
                    _TickDataOffer.Clear();


                }

                lock (_TickDataHighLow)
                {
                    Dictionary<string, TickDataHighLow>.Enumerator e = _TickDataHighLow.GetEnumerator();
                    while (e.MoveNext())
                    {
                        TickDataHighLow item = e.Current.Value;
                        OnTickDataHighLow(item);
                    }
                    int Count_TickDataHighLow = _TickDataHighLow.Count;
                    _TickDataHighLow.Clear();
                }

                lock (_TickDataOpenclose)
                {
                    Dictionary<string, TickDataOpenclose>.Enumerator e = _TickDataOpenclose.GetEnumerator();
                    while (e.MoveNext())
                    {
                        TickDataOpenclose item = e.Current.Value;
                        OnTickDataOpenclose(item);
                    }
                    int Count_TickDataOpenclose = _TickDataOpenclose.Count;
                    _TickDataOpenclose.Clear();
                }


                lock (_TickDataSettle)
                {
                    Dictionary<string, TickDataSettle>.Enumerator e = _TickDataSettle.GetEnumerator();
                    while (e.MoveNext())
                    {
                        TickDataSettle item = e.Current.Value;
                        OnTickDataSettle(item);
                    }
                    int Count_TickDataSettle = _TickDataSettle.Count;
                    _TickDataSettle.Clear();
                }
            }
            catch (Exception ex)
            {
            }
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
            _api.SubscribeForginPrice(SymbolContract.Exchange, SymbolContract.Symbol, SymbolContract.YearMonth, ((char)SymbolContract.CP).ToString(), SymbolContract.StrikePrice);
        }

        public override void UnSubscribe(SymbolContract SymbolContract)
        {
            _api.unSubscribeForginPrice(SymbolContract.Exchange, SymbolContract.Symbol, SymbolContract.YearMonth, ((char)SymbolContract.CP).ToString(), SymbolContract.StrikePrice);

        }
        public override TickData Query(SymbolContract SymbolContract, string seq)
        {
            _api.QueryForginPrice(seq, SymbolContract.Exchange, SymbolContract.Symbol, SymbolContract.YearMonth, ((char)SymbolContract.CP).ToString(), SymbolContract.StrikePrice);
            if (_AutoResetEvent.WaitOne(5000))
                return _TickData;
            else return null;
        }


        private void _api_FQueryData(string Seq, string Format, string Data)
        {
            try
            {
                if (Data != "0")
                {
                    _TickData = parseQueryFMarketInfoData(Format + Data);

                }
                else
                {
                    _AutoResetEvent.Set();
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void _api_FQuotaData(string Format, string Data)
        {
            try
            {
                parseFMarketInfoData(Format + Data);
                parseFData(Format + Data);

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
                                    TickDataTrade TickDataTrade = new TickDataTrade(exchange, symbol, ym, cp, strike);

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
                                TickDataBid TickDataBid = new TickDataBid(exchange, symbol, ym, cp, strike);

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
                                TickDataOffer TickDataOffer = new TickDataOffer(exchange, symbol, ym, cp, strike);
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
                                TickDataHighLow TickDataHighLow = new TickDataHighLow(exchange, symbol, ym, cp, strike);
                                TickDataHighLow.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                TickDataHighLow.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                TickDataHighLow.High = high;
                                TickDataHighLow.Low = low;
                                OnTickDataHighLow(TickDataHighLow);
                                break;
                            case "P6"://open  close

                                decimal open = decimal.Parse(data[1]);
                                decimal close = decimal.Parse(data[2]);
                                TickDataOpenclose TickDataOpenclose = new TickDataOpenclose(exchange, symbol, ym, cp, strike);
                                TickDataOpenclose.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                TickDataOpenclose.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                TickDataOpenclose.Opening = open;
                                TickDataOpenclose.Closing = close;
                                OnTickDataOpenclose(TickDataOpenclose);
                                break;
                            case "P7"://Settlement   
                                decimal currentsettl = decimal.Parse(data[1]);
                                decimal newsettl = decimal.Parse(data[2]);
                                TickDataSettle TickDataSettle = new TickDataSettle(exchange, symbol, ym, cp, strike);
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

        TickData parseQueryFMarketInfoData(string strData)
        {

            TickData Tick = null;
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
                    Tick = new TickData(exchange, symbol, ym, cp, strike);
                    decimal DISPLAY_DENOMINATOR = decimal.Parse(symbols[5]);
                    decimal DISPLAY_MULTIPLY = decimal.Parse(symbols[6]);
                    Tick.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
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
                                    Tick.LastPrice = LastPrice;
                                    Tick.LastVolume = LastVolume;
                                    Tick.Time = Time;
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
                                Tick.BidDOM1Price = BidDOM1Price;
                                Tick.BidDOM1Volume = BidDOM1Volume;
                                Tick.BidDOM2Price = BidDOM2Price;
                                Tick.BidDOM2Volume = BidDOM2Volume;
                                Tick.BidDOM3Price = BidDOM3Price;
                                Tick.BidDOM3Volume = BidDOM3Volume;
                                Tick.BidDOM4Price = BidDOM4Price;
                                Tick.BidDOM4Volume = BidDOM4Volume;
                                Tick.BidDOM5Price = BidDOM5Price;
                                Tick.BidDOM5Volume = BidDOM5Volume;
                                Tick.BidDOM6Price = BidDOM6Price;
                                Tick.BidDOM6Volume = BidDOM6Volume;
                                Tick.BidDOM7Price = BidDOM7Price;
                                Tick.BidDOM7Volume = BidDOM7Volume;
                                Tick.BidDOM8Price = BidDOM8Price;
                                Tick.BidDOM8Volume = BidDOM8Volume;
                                Tick.BidDOM9Price = BidDOM9Price;
                                Tick.BidDOM9Volume = BidDOM9Volume;
                                Tick.BidDOM10Price = BidDOM10Price;
                                Tick.BidDOM10Volume = BidDOM10Volume;
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
                                Tick.OfferDOM1Price = OfferDOM1Price;
                                Tick.OfferDOM1Volume = OfferDOM1Volume;
                                Tick.OfferDOM2Price = OfferDOM2Price;
                                Tick.OfferDOM2Volume = OfferDOM2Volume;
                                Tick.OfferDOM3Price = OfferDOM3Price;
                                Tick.OfferDOM3Volume = OfferDOM3Volume;
                                Tick.OfferDOM4Price = OfferDOM4Price;
                                Tick.OfferDOM4Volume = OfferDOM4Volume;
                                Tick.OfferDOM5Price = OfferDOM5Price;
                                Tick.OfferDOM5Volume = OfferDOM5Volume;
                                Tick.OfferDOM6Price = OfferDOM6Price;
                                Tick.OfferDOM6Volume = OfferDOM6Volume;
                                Tick.OfferDOM7Price = OfferDOM7Price;
                                Tick.OfferDOM7Volume = OfferDOM7Volume;
                                Tick.OfferDOM8Price = OfferDOM8Price;
                                Tick.OfferDOM8Volume = OfferDOM8Volume;
                                Tick.OfferDOM9Price = OfferDOM9Price;
                                Tick.OfferDOM9Volume = OfferDOM9Volume;
                                Tick.OfferDOM10Price = OfferDOM10Price;
                                Tick.OfferDOM10Volume = OfferDOM10Volume;
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
                                Tick.High = high;
                                Tick.Low = low;
                                break;
                            case "P6"://open  close

                                decimal open = decimal.Parse(data[1]);
                                decimal close = decimal.Parse(data[2]);
                                Tick.Opening = open;
                                Tick.Closing = close;
                                break;
                            case "P7"://Settlement   
                                decimal currentsettl = decimal.Parse(data[1]);
                                decimal newsettl = decimal.Parse(data[2]);
                                Tick.Settle = newsettl;
                                break;

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Tick;
        }

        public void parseFData(string strData)
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
                    string key = exchange + symbol + ym + cp + strike;
                    for (int i = 1; i < Body.Length - 1; i++)
                    {
                        string[] data = Body[i].Split('\x01');
                        switch (data[0])
                        {
                            case "P1"://成交
                                int Total = int.Parse(data[1]);
                                decimal LastPrice = decimal.Parse(data[2]);
                                int LastVolume = int.Parse(data[3]);
                                string Time = data[4];


                                lock (_TickDataTrade)
                                {
                                    TickDataTrade item = null;
                                    if (_TickDataTrade.TryGetValue(key, out item))
                                    {

                                        item.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                        item.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                        item.Total = Total;
                                        item.LastPrice = LastPrice;
                                        item.LastVolume = LastVolume;
                                        item.Time = Time;
                                        decimal preMaxprice = item.maxPriceByTime;
                                        decimal preMinprice = item.minPriceByTime;
                                        if (LastPrice > preMaxprice)
                                        {
                                            item.maxPriceByTime = LastPrice;
                                        }
                                        else if (LastPrice < preMinprice)
                                        {
                                            item.minPriceByTime = LastPrice;
                                        }

                                        _TickDataTrade[key] = item;
                                    }
                                    else
                                    {
                                        item = new TickDataTrade(exchange, symbol, ym, cp, strike);
                                        item.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                        item.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                        item.Total = Total;
                                        item.LastPrice = LastPrice;
                                        item.LastVolume = LastVolume;
                                        item.Time = Time;
                                        decimal preMaxprice = item.maxPriceByTime;
                                        decimal preMinprice = item.minPriceByTime;
                                        if (LastPrice > preMaxprice)
                                        {
                                            item.maxPriceByTime = LastPrice;
                                        }
                                        else if (LastPrice < preMinprice)
                                        {
                                            item.minPriceByTime = LastPrice;
                                        }
                                        _TickDataTrade[key] = item;
                                    }
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


                                lock (_TickDataBid)
                                {
                                    TickDataBid item = null;
                                    if (_TickDataBid.TryGetValue(key, out item))
                                    {

                                        item.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                        item.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                        item.BidDOM1Price = BidDOM1Price;
                                        item.BidDOM1Volume = BidDOM1Volume;
                                        item.BidDOM2Price = BidDOM2Price;
                                        item.BidDOM2Volume = BidDOM2Volume;
                                        item.BidDOM3Price = BidDOM3Price;
                                        item.BidDOM3Volume = BidDOM3Volume;
                                        item.BidDOM4Price = BidDOM4Price;
                                        item.BidDOM4Volume = BidDOM4Volume;
                                        item.BidDOM5Price = BidDOM5Price;
                                        item.BidDOM5Volume = BidDOM5Volume;
                                        item.BidDOM6Price = BidDOM6Price;
                                        item.BidDOM6Volume = BidDOM6Volume;
                                        item.BidDOM7Price = BidDOM7Price;
                                        item.BidDOM7Volume = BidDOM7Volume;
                                        item.BidDOM8Price = BidDOM8Price;
                                        item.BidDOM8Volume = BidDOM8Volume;
                                        item.BidDOM9Price = BidDOM9Price;
                                        item.BidDOM9Volume = BidDOM9Volume;
                                        item.BidDOM10Price = BidDOM10Price;
                                        item.BidDOM10Volume = BidDOM10Volume;


                                        _TickDataBid[key] = item;
                                    }
                                    else
                                    {
                                        item = new TickDataBid(exchange, symbol, ym, cp, strike);
                                        item.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                        item.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                        item.BidDOM1Price = BidDOM1Price;
                                        item.BidDOM1Volume = BidDOM1Volume;
                                        item.BidDOM2Price = BidDOM2Price;
                                        item.BidDOM2Volume = BidDOM2Volume;
                                        item.BidDOM3Price = BidDOM3Price;
                                        item.BidDOM3Volume = BidDOM3Volume;
                                        item.BidDOM4Price = BidDOM4Price;
                                        item.BidDOM4Volume = BidDOM4Volume;
                                        item.BidDOM5Price = BidDOM5Price;
                                        item.BidDOM5Volume = BidDOM5Volume;
                                        item.BidDOM6Price = BidDOM6Price;
                                        item.BidDOM6Volume = BidDOM6Volume;
                                        item.BidDOM7Price = BidDOM7Price;
                                        item.BidDOM7Volume = BidDOM7Volume;
                                        item.BidDOM8Price = BidDOM8Price;
                                        item.BidDOM8Volume = BidDOM8Volume;
                                        item.BidDOM9Price = BidDOM9Price;
                                        item.BidDOM9Volume = BidDOM9Volume;
                                        item.BidDOM10Price = BidDOM10Price;
                                        item.BidDOM10Volume = BidDOM10Volume;
                                        _TickDataBid[key] = item;
                                    }
                                }

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


                                lock (_TickDataOffer)
                                {
                                    TickDataOffer item = null;
                                    if (_TickDataOffer.TryGetValue(key, out item))
                                    {

                                        item.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                        item.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                        item.OfferDOM1Price = OfferDOM1Price;
                                        item.OfferDOM1Volume = OfferDOM1Volume;
                                        item.OfferDOM2Price = OfferDOM2Price;
                                        item.OfferDOM2Volume = OfferDOM2Volume;
                                        item.OfferDOM3Price = OfferDOM3Price;
                                        item.OfferDOM3Volume = OfferDOM3Volume;
                                        item.OfferDOM4Price = OfferDOM4Price;
                                        item.OfferDOM4Volume = OfferDOM4Volume;
                                        item.OfferDOM5Price = OfferDOM5Price;
                                        item.OfferDOM5Volume = OfferDOM5Volume;
                                        item.OfferDOM6Price = OfferDOM6Price;
                                        item.OfferDOM6Volume = OfferDOM6Volume;
                                        item.OfferDOM7Price = OfferDOM7Price;
                                        item.OfferDOM7Volume = OfferDOM7Volume;
                                        item.OfferDOM8Price = OfferDOM8Price;
                                        item.OfferDOM8Volume = OfferDOM8Volume;
                                        item.OfferDOM9Price = OfferDOM9Price;
                                        item.OfferDOM9Volume = OfferDOM9Volume;
                                        item.OfferDOM10Price = OfferDOM10Price;
                                        item.OfferDOM10Volume = OfferDOM10Volume;

                                        _TickDataOffer[key] = item;
                                    }
                                    else
                                    {
                                        item = new TickDataOffer(exchange, symbol, ym, cp, strike);
                                        item.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                        item.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                        item.OfferDOM1Price = OfferDOM1Price;
                                        item.OfferDOM1Volume = OfferDOM1Volume;
                                        item.OfferDOM2Price = OfferDOM2Price;
                                        item.OfferDOM2Volume = OfferDOM2Volume;
                                        item.OfferDOM3Price = OfferDOM3Price;
                                        item.OfferDOM3Volume = OfferDOM3Volume;
                                        item.OfferDOM4Price = OfferDOM4Price;
                                        item.OfferDOM4Volume = OfferDOM4Volume;
                                        item.OfferDOM5Price = OfferDOM5Price;
                                        item.OfferDOM5Volume = OfferDOM5Volume;
                                        item.OfferDOM6Price = OfferDOM6Price;
                                        item.OfferDOM6Volume = OfferDOM6Volume;
                                        item.OfferDOM7Price = OfferDOM7Price;
                                        item.OfferDOM7Volume = OfferDOM7Volume;
                                        item.OfferDOM8Price = OfferDOM8Price;
                                        item.OfferDOM8Volume = OfferDOM8Volume;
                                        item.OfferDOM9Price = OfferDOM9Price;
                                        item.OfferDOM9Volume = OfferDOM9Volume;
                                        item.OfferDOM10Price = OfferDOM10Price;
                                        item.OfferDOM10Volume = OfferDOM10Volume;

                                        _TickDataOffer[key] = item;
                                    }
                                }

                                break;
                            case "P4":// Implied bid Offer

                                break;
                            case "P5"://high low 
                                decimal high = decimal.Parse(data[1]);
                                decimal low = decimal.Parse(data[2]);

                                lock (_TickDataHighLow)
                                {
                                    TickDataHighLow item = null;
                                    if (_TickDataHighLow.TryGetValue(key, out item))
                                    {

                                        item.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                        item.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                        item.High = high;
                                        item.Low = low;
                                        _TickDataHighLow[key] = item;
                                    }
                                    else
                                    {
                                        item = new TickDataHighLow(exchange, symbol, ym, cp, strike);
                                        item.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                        item.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                        item.High = high;
                                        item.Low = low;
                                        _TickDataHighLow[key] = item;
                                    }
                                }
                                break;
                            case "P6"://open  close
                                decimal open = decimal.Parse(data[1]);
                                decimal close = decimal.Parse(data[2]);
                                lock (_TickDataOpenclose)
                                {
                                    TickDataOpenclose item = null;
                                    if (_TickDataOpenclose.TryGetValue(key, out item))
                                    {

                                        item.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                        item.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                        item.Opening = open;
                                        item.Closing = close;
                                        _TickDataOpenclose[key] = item;
                                    }
                                    else
                                    {
                                        item = new TickDataOpenclose(exchange, symbol, ym, cp, strike);
                                        item.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                        item.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                        item.Opening = open;
                                        item.Closing = close;
                                        _TickDataOpenclose[key] = item;
                                    }
                                }
                                break;
                            case "P7"://Settlement  
                                decimal currentsettl = decimal.Parse(data[1]);
                                decimal newsettl = decimal.Parse(data[2]);
                                lock (_TickDataSettle)
                                {
                                    TickDataSettle item = null;
                                    if (_TickDataSettle.TryGetValue(key, out item))
                                    {

                                        item.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                        item.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                        item.CurrStl = currentsettl;
                                        item.NewStl = newsettl;
                                        _TickDataSettle[key] = item;
                                    }
                                    else
                                    {
                                        item = new TickDataSettle(exchange, symbol, ym, cp, strike);
                                        item.DISPLAY_DENOMINATOR = DISPLAY_DENOMINATOR;
                                        item.DISPLAY_MULTIPLY = DISPLAY_MULTIPLY;
                                        item.CurrStl = currentsettl;
                                        item.NewStl = newsettl;
                                        _TickDataSettle[key] = item;
                                    }
                                }
                                break;

                        }
                    }

                }



            }
            catch (Exception ex)
            {
            }

        }
    }

    public class AsyncQueue<T>
    {
        #region Private Fields

        private readonly Action<T> _Action;
        private readonly Task[] _tasks;
        private readonly Queue<T> queue;
        private readonly object sync = new object();

        #endregion Private Fields

        #region Public Constructors

        public AsyncQueue(int threadCount, Action<T> action)
        {
            _Action = action;
            queue = new Queue<T>();

            _tasks = new Task[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                _tasks[i] = Task.Factory.StartNew(Process, TaskCreationOptions.LongRunning);
            }
        }

        #endregion Public Constructors

        #region Public Methods

        public void Close(bool waitOnCompletion = true)
        {
            T f = default(T);
            for (int i = 0; i < _tasks.Length; i++)
            {
                Enqueue(f);
            }
            if (waitOnCompletion)
            {
                Task.WaitAll(_tasks);
            }
        }

        public void Enqueue(T t)
        {
            lock (sync)
            {
                queue.Enqueue(t);
                Monitor.Pulse(sync);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Process()
        {
            while (true)
            {
                T t;
                lock (sync)
                {
                    while (queue.Count == 0)
                    {
                        Monitor.Wait(sync);
                    }
                    t = queue.Dequeue();
                }
                if (t == null)
                {
                    break;
                }
                if (_Action != null)
                {
                    _Action(t);
                }
            }
        }

        #endregion Private Methods
    }
}
