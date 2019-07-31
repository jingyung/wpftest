using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace QuoteService
{
    public abstract class QuoteServiceBase
    {
        public abstract void Connect(string phost, int pPort);
        public abstract void Subscribe(SymbolContract SymbolContract);
        public abstract void UnSubscribe(SymbolContract SymbolContract);
        public abstract TickData Query(SymbolContract SymbolContract, string seq);
        public delegate void ConnectedHandler();
        public delegate void DisconnectedHandler();
        public delegate void TickDataTradeHandler(TickDataTrade val);
        public delegate void TickDataBidHandler(TickDataBid val);
        public delegate void TickDataOfferHandler(TickDataOffer val);
        public delegate void TickDataHighLowHandler(TickDataHighLow val);
        public delegate void TickDataOpencloseHandler(TickDataOpenclose val);
        public delegate void TickDataSettleHandler(TickDataSettle val);
        public event TickDataTradeHandler TickDataTrade;
        public event TickDataBidHandler TickDataBid;
        public event TickDataOfferHandler TickDataOffer;
        public event TickDataOpencloseHandler TickDataOpenclose;
        public event TickDataHighLowHandler TickDataHighLow;
        public event TickDataSettleHandler TickDataSettle;
        public event ConnectedHandler Connected;
        public event DisconnectedHandler Disconnected;
        public void OnTickDataTrade(TickDataTrade val)
        {
            if (TickDataTrade != null)
            {
                TickDataTrade(val);
            }
        }
        public void OnTickDataBid(TickDataBid val)
        {
            if (TickDataBid != null)
            {
                TickDataBid(val);
            }
        }
        public void OnTickDataOffer(TickDataOffer val)
        {
            if (TickDataOffer != null)
            {
                TickDataOffer(val);
            }
        }
        public void OnTickDataHighLow(TickDataHighLow val)
        {
            if (TickDataHighLow != null)
            {
                TickDataHighLow(val);
            }
        }
        public void OnTickDataOpenclose(TickDataOpenclose val)
        {
            if (TickDataOpenclose != null)
            {
                TickDataOpenclose(val);
            }
        }
        public void OnTickDataSettle(TickDataSettle val)
        {
            if (TickDataSettle != null)
            {
                TickDataSettle(val);
            }
        }
        public void OnConnected()
        {
            if (Connected != null)
            {
                Connected();
            }
        }
        public void OnDisconnected()
        {
            if (Disconnected != null)
            {
                Disconnected();
            }
        }
    }
}
