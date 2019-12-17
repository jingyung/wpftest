using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface ITickUpdate
    {
        void onTickDataTrade(TickDataTrade val);
        void onTickDataOffer(TickDataOffer val);
        void onTickDataBid(TickDataBid val);
    }
}
