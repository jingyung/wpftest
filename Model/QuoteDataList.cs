using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using NLog;
namespace Model
{
    public class QuoteData : BindableBase
    {

        private bool _Settleflag = false;
        public bool Settleflag
        {
            get { return _Settleflag; }
            set { SetProperty(ref _Settleflag, value); }
        }

        private bool _matchflag = false;
        public bool MatchFlag
        {
            get { return _matchflag; }
            set { SetProperty(ref _matchflag, value); }
        }
        private bool _highlag = false;
        public bool HighFlag
        {
            get { return _highlag; }
            set { SetProperty(ref _highlag, value); }
        }
        private bool _lowlag = false;
        public bool LowFlag
        {
            get { return _lowlag; }
            set { SetProperty(ref _lowlag, value); }
        }

        private decimal _Price;
        public decimal Price
        {
            get { return _Price; }
            set { SetProperty(ref _Price, value); }
        }

        private int? _bid;
        public int? Bid
        {
            get { return _bid; }
            set { SetProperty(ref _bid, value); }
        }
        private int? _Offer;
        public int? Offer
        {
            get { return _Offer; }
            set { SetProperty(ref _Offer, value); }
        }

        private int? _bidworkqty;
        public int? BidWorkQty
        {
            get { return _bidworkqty; }
            set { SetProperty(ref _bidworkqty, value); }
        }
        private int? _offerworkqty;
        public int? OfferWorkQty
        {
            get { return _offerworkqty; }
            set { SetProperty(ref _offerworkqty, value); }
        }
        public QuoteData()
        {

        }

    }
    public class QuoteDataList : ObservableCollection<QuoteData>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        Dictionary<decimal, QuoteData> _data = new Dictionary<decimal, QuoteData>();

        QuoteData _tmpTrade = null;
        QuoteData[] _tmpBid = null;
        QuoteData[] _tmpOffer = null;
        decimal[] _tmptmpBid = null;
        decimal[] _tmptmpOffer = null;

        public QuoteDataList()
        {
            _tmpBid = new QuoteData[10];
            _tmpOffer = new QuoteData[10];
            _tmptmpBid = new decimal[10];
            _tmptmpOffer = new decimal[10];
        }
        public void AddTick(decimal key, QuoteData data)
        {
            try
            {


                _data[key] = data;
                this.Add(data);
            }
            catch (Exception ex)
            {

            }
        }
        public void UpdateLastTrade(TickDataTrade Tick)
        {
            if (_data.TryGetValue(Tick.LastPrice, out QuoteData val))
            {
                if (val.Price == _tmpTrade.Price) return;
                val.MatchFlag = true;
                _tmpTrade.MatchFlag = false;
                _tmpTrade = val;
            }
        }
        object Bidlokcer = new object();
        object Offerloker = new object();
        public void UpdateBid(TickDataBid Tick)
        {
            lock (Bidlokcer)
            {
                string id = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
                logger.Info(id + " " + Tick.ToString());
                string beforetmp = id + " " + " bidbeforetmp ";
                string temp = id + " " + " bidtmp ";
                QuoteData val;
                if (_data.TryGetValue(Tick.BidDOM1Price, out val))
                {
                    beforetmp += _tmpBid[0].Price + " ";
                    _tmpBid[0].Bid = null;
                    val.Bid = Tick.BidDOM1Volume;
                    _tmpBid[0] = val;
                    temp += _tmpBid[0].Price + " ";
                }
                if (_data.TryGetValue(Tick.BidDOM2Price, out val))
                {
                    beforetmp += _tmpBid[1].Price + " ";
                    _tmpBid[1].Bid = null;
                    val.Bid = Tick.BidDOM2Volume;
                    _tmpBid[1] = val;
                    temp += _tmpBid[1].Price + " ";
                }
                if (_data.TryGetValue(Tick.BidDOM3Price, out val))
                {
                    beforetmp += _tmpBid[2].Price + " ";
                    _tmpBid[2].Bid = null;
                    val.Bid = Tick.BidDOM3Volume;
                    _tmpBid[2] = val;
                    temp += _tmpBid[2].Price + " ";
                }
                if (_data.TryGetValue(Tick.BidDOM4Price, out val))
                {
                    beforetmp += _tmpBid[3].Price + " ";
                    _tmpBid[3].Bid = null;
                    val.Bid = Tick.BidDOM4Volume;
                    _tmpBid[3] = val;
                    temp += _tmpBid[3].Price + " ";
                }
                if (_data.TryGetValue(Tick.BidDOM5Price, out val))
                {
                    beforetmp += _tmpBid[4].Price + " ";
                    _tmpBid[4].Bid = null;
                    val.Bid = Tick.BidDOM5Volume;
                    _tmpBid[4] = val;
                    temp += _tmpBid[4].Price + " ";
                }
                if (_data.TryGetValue(Tick.BidDOM6Price, out val))
                {
                    beforetmp += _tmpBid[5].Price + " ";
                    _tmpBid[5].Bid = null;
                    val.Bid = Tick.BidDOM6Volume;
                    _tmpBid[5] = val;
                    temp += _tmpBid[5].Price + " ";
                }
                if (_data.TryGetValue(Tick.BidDOM7Price, out val))
                {
                    beforetmp += _tmpBid[6].Price + " ";
                    _tmpBid[6].Bid = null;
                    val.Bid = Tick.BidDOM7Volume;
                    _tmpBid[6] = val;
                    temp += _tmpBid[6].Price + " ";
                }
                if (_data.TryGetValue(Tick.BidDOM8Price, out val))
                {
                    beforetmp += _tmpBid[7].Price + " ";
                    _tmpBid[7].Bid = null;
                    val.Bid = Tick.BidDOM8Volume;
                    _tmpBid[7] = val;
                    temp += _tmpBid[7].Price + " ";
                }
                if (_data.TryGetValue(Tick.BidDOM9Price, out val))
                {
                    beforetmp += _tmpBid[8].Price + " ";
                    _tmpBid[8].Bid = null;
                    val.Bid = Tick.BidDOM9Volume;
                    _tmpBid[8] = val;
                    temp += _tmpBid[8].Price + " ";
                }
                if (_data.TryGetValue(Tick.BidDOM10Price, out val))
                {
                    beforetmp += _tmpBid[9].Price + " ";
                    _tmpBid[9].Bid = null;
                    val.Bid = Tick.BidDOM10Volume;
                    _tmpBid[9] = val;
                    temp += _tmpBid[9].Price + " ";
                }
                logger.Info(beforetmp);
                logger.Info(temp);
                if (_data.Where(x => x.Value.Bid != null).Count() > 10)
                {
                    string t = id + " " + " biderror ";
                    foreach (var data in _data.Where(x => x.Value.Bid != null).ToList())
                    {
                        t += data.Key + " ";
                    }
                    logger.Info(t);
                }
            }
        }
        public void UpdateOffer(TickDataOffer Tick)
        {
            lock (Offerloker)
            {
                string id = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
                logger.Info(id + " " + Tick.ToString());
                string beforetmp = id + " " + " Offerbeforetmp ";
                string temp = id + " " + " Offertmp ";
                QuoteData val;
                if (_data.TryGetValue(Tick.OfferDOM1Price, out val))
                {
                    beforetmp += _tmpOffer[0].Price + " ";
                    _tmpOffer[0].Offer = null;
                    val.Offer = Tick.OfferDOM1Volume;
                    _tmpOffer[0] = val;
                    temp += _tmpOffer[0].Price + " ";
                }
                if (_data.TryGetValue(Tick.OfferDOM2Price, out val))
                {
                    beforetmp += _tmpOffer[1].Price + " ";
                    _tmpOffer[1].Offer = null;
                    val.Offer = Tick.OfferDOM2Volume;
                    _tmpOffer[1] = val;
                    temp += _tmpOffer[1].Price + " ";
                }
                if (_data.TryGetValue(Tick.OfferDOM3Price, out val))
                {
                    beforetmp += _tmpOffer[2].Price + " ";
                    _tmpOffer[2].Offer = null;
                    val.Offer = Tick.OfferDOM3Volume;
                    _tmpOffer[2] = val;
                    temp += _tmpOffer[2].Price + " ";
                }
                if (_data.TryGetValue(Tick.OfferDOM4Price, out val))
                {
                    beforetmp += _tmpOffer[3].Price + " ";
                    _tmpOffer[3].Offer = null;
                    val.Offer = Tick.OfferDOM4Volume;
                    _tmpOffer[3] = val;
                    temp += _tmpOffer[3].Price + " ";
                }
                if (_data.TryGetValue(Tick.OfferDOM5Price, out val))
                {
                    beforetmp += _tmpOffer[4].Price + " ";
                    _tmpOffer[4].Offer = null;
                    val.Offer = Tick.OfferDOM5Volume;
                    _tmpOffer[4] = val;
                    temp += _tmpOffer[4].Price + " ";
                }
                if (_data.TryGetValue(Tick.OfferDOM6Price, out val))
                {
                    beforetmp += _tmpOffer[5].Price + " ";
                    _tmpOffer[5].Offer = null;
                    val.Offer = Tick.OfferDOM6Volume;
                    _tmpOffer[5] = val;
                    temp += _tmpOffer[5].Price + " ";
                }
                if (_data.TryGetValue(Tick.OfferDOM7Price, out val))
                {
                    beforetmp += _tmpOffer[6].Price + " ";
                    _tmpOffer[6].Offer = null;
                    val.Offer = Tick.OfferDOM7Volume;
                    _tmpOffer[6] = val;
                    temp += _tmpOffer[6].Price + " ";
                }
                if (_data.TryGetValue(Tick.OfferDOM8Price, out val))
                {
                    beforetmp += _tmpOffer[7].Price + " ";
                    _tmpOffer[7].Offer = null;
                    val.Offer = Tick.OfferDOM8Volume;
                    _tmpOffer[7] = val;
                    temp += _tmpOffer[7].Price + " ";
                }
                if (_data.TryGetValue(Tick.OfferDOM9Price, out val))
                {
                    beforetmp += _tmpOffer[8].Price + " ";
                    _tmpOffer[8].Offer = null;
                    val.Offer = Tick.OfferDOM9Volume;
                    _tmpOffer[8] = val;
                    temp += _tmpOffer[8].Price + " ";
                }
                if (_data.TryGetValue(Tick.OfferDOM10Price, out val))
                {
                    beforetmp += _tmpOffer[9].Price + " ";
                    _tmpOffer[9].Offer = null;
                    val.Offer = Tick.OfferDOM10Volume;
                    _tmpOffer[9] = val;
                    temp += _tmpOffer[9].Price + " ";
                }
                logger.Info(beforetmp);
                logger.Info(temp);
                if (_data.Where(x => x.Value.Offer != null).Count() > 10)
                {
                    string t = id + " " + " offererror ";
                    foreach (var data in _data.Where(x => x.Value.Offer != null).ToList())
                    {
                        t += data.Key + " ";
                    }
                    logger.Info(t);
                }
            }
        }
        public QuoteData UpdateTick(decimal key)
        {
            if (_data.TryGetValue(key, out QuoteData val))
            {
                return val;
            }
            return null;
        }
        public void init(TickData data)
        {
            if (data == null) return;
            ClearTick();

            QuoteData Quote = new QuoteData();
            Quote.Settleflag = true;
            if (data.Settle == 0) data.Settle = data.LastPrice;
            Quote.Price = data.Settle;
            Quote.MatchFlag = true;
            _tmpTrade = Quote;
            this.AddTick(Quote.Price, Quote);
            for (int i = 1; i < 500; i++)
            {
                Quote = new QuoteData();
                Quote.Price = data.Settle + i * 1M;
                if (data.LastPrice == Quote.Price)
                {

                    Quote.MatchFlag = true;
                    _tmpTrade = Quote;
                }
                this.AddTick(Quote.Price, Quote);
                Quote = new QuoteData();
                Quote.Price = data.Settle - i * 1M;
                if (data.LastPrice == Quote.Price)
                {

                    Quote.MatchFlag = true;
                    _tmpTrade = Quote;
                }
                this.AddTick(Quote.Price, Quote);
            }


            for (int i = 0; i < 10; i++)
            {
                _tmpBid[i] = Quote;
                _tmpOffer[i] = Quote;

            }


        }

        public QuoteData GetLastTrade() { return _tmpTrade; }
        public void ClearTick()
        {
            this.Clear();
            _data.Clear();
        }
    }
}
