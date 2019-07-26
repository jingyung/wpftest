using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
namespace Model
{
    public class TickData : BindableBase
    {
        private bool _matchflag;
        public bool MatchFlag
        {
            get { return _matchflag; }
            set { SetProperty(ref _matchflag, value); }
        }
        private bool _highlag;
        public bool HighFlag
        {
            get { return _highlag; }
            set { SetProperty(ref _highlag, value); }
        }
        private bool _lowlag;
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

        private int _bid;
        public int Bid
        {
            get { return _bid; }
            set { SetProperty(ref _bid, value); }
        }
        private int _ask;
        public int Ask
        {
            get { return _ask; }
            set { SetProperty(ref _ask, value); }
        }

        private int _bidworkqty;
        public int BidWorkQty
        {
            get { return _bidworkqty; }
            set { SetProperty(ref _bidworkqty, value); }
        }
        private int _askworkqty;
        public int AskWorkQty
        {
            get { return _askworkqty; }
            set { SetProperty(ref _askworkqty, value); }
        }

    }
    public class TickDataList : ObservableCollection<TickData>
    {
        Dictionary<string, TickData> _data = new Dictionary<string, TickData>();
        public TickDataList()
        {

        }
        public void AddTick(string key, TickData data)
        {
            _data[key] = data;
            this.Add(data);
        }
        public TickData UpdateTick(string key )
        {
            if (_data.TryGetValue(key, out TickData val))
            {
                return val;
            }
            return null;
        }
        public void ClearTick()
        {
            this.Clear();
            _data.Clear();
        }
    }
}
