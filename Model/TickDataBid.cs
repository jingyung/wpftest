using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TickDataBid
    {
        private string _EXCHAGE = "";
        private string _SYMBOL = "";
        private string _YM = "";
        private string _CP = "";
        private string _STRIKE = "";
        private decimal _DISPLAY_DENOMINATOR = 0;//分母
        private decimal _DISPLAY_MULTIPLY = 0;//倍率 

        private decimal _BidDOM1Price = 0;
        private int _BidDOM1Volume = 0;
        private decimal _BidDOM2Price = 0;
        private int _BidDOM2Volume = 0;
        private decimal _BidDOM3Price = 0;
        private int _BidDOM3Volume = 0;
        private decimal _BidDOM4Price = 0;
        private int _BidDOM4Volume = 0;
        private decimal _BidDOM5Price = 0;
        private int _BidDOM5Volume = 0;
        private decimal _BidDOM6Price = 0;
        private int _BidDOM6Volume = 0;
        private decimal _BidDOM7Price = 0;
        private int _BidDOM7Volume = 0;
        private decimal _BidDOM8Price = 0;
        private int _BidDOM8Volume = 0;
        private decimal _BidDOM9Price = 0;
        private int _BidDOM9Volume = 0;
        private decimal _BidDOM10Price = 0;
        private int _BidDOM10Volume = 0;

        /// <summary>
        /// 交易所
        /// </summary>
        public string EXCHAGE
        {
            get { return _EXCHAGE; }
            set { _EXCHAGE = value; }
        }
        /// <summary>
        /// 商品代碼
        /// </summary>
        public string SYMBOL
        {
            get { return _SYMBOL; }
            set { _SYMBOL = value; }
        }
        /// <summary>
        /// 年月
        /// </summary>
        public string YM
        {
            get { return _YM; }
            set { _YM = value; }
        }
        /// <summary>
        /// CP
        /// </summary>
        public string CP
        {
            get { return _CP; }
            set { _CP = value; }
        }
        /// <summary>
        /// 履約價
        /// </summary>
        public string STRIKE
        {
            get { return _STRIKE; }
            set { _STRIKE = value; }
        }
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
        /// 最佳買價1
        /// </summary>
        public decimal BidDOM1Price
        {
            get { return _BidDOM1Price; }
            set { _BidDOM1Price = value; }
        }
        /// <summary>
        /// 最佳買量1
        /// </summary>
        public int BidDOM1Volume
        {
            get { return _BidDOM1Volume; }
            set { _BidDOM1Volume = value; }
        }
        /// <summary>
        /// 最佳買價2
        /// </summary>
        public decimal BidDOM2Price
        {
            get { return _BidDOM2Price; }
            set { _BidDOM2Price = value; }
        }
        /// <summary>
        /// 最佳買量2
        /// </summary>
        public int BidDOM2Volume
        {
            get { return _BidDOM2Volume; }
            set { _BidDOM2Volume = value; }
        }
        /// <summary>
        /// 最佳買價3
        /// </summary>
        public decimal BidDOM3Price
        {
            get { return _BidDOM3Price; }
            set { _BidDOM3Price = value; }
        }
        /// <summary>
        /// 最佳買量3
        /// </summary>
        public int BidDOM3Volume
        {
            get { return _BidDOM3Volume; }
            set { _BidDOM3Volume = value; }
        }
        /// <summary>
        /// 最佳買價4
        /// </summary>
        public decimal BidDOM4Price
        {
            get { return _BidDOM4Price; }
            set { _BidDOM4Price = value; }
        }
        /// <summary>
        /// 最佳買量4
        /// </summary>
        public int BidDOM4Volume
        {
            get { return _BidDOM4Volume; }
            set { _BidDOM4Volume = value; }
        }
        /// <summary>
        /// 最佳買價5
        /// </summary>
        public decimal BidDOM5Price
        {
            get { return _BidDOM5Price; }
            set { _BidDOM5Price = value; }
        }
        /// <summary>
        /// 最佳買量5
        /// </summary>
        public int BidDOM5Volume
        {
            get { return _BidDOM5Volume; }
            set { _BidDOM5Volume = value; }
        }
        /// <summary>
        /// 最佳買價6
        /// </summary>
        public decimal BidDOM6Price
        {
            get { return _BidDOM6Price; }
            set { _BidDOM6Price = value; }
        }
        /// <summary>
        /// 最佳買量6
        /// </summary>
        public int BidDOM6Volume
        {
            get { return _BidDOM6Volume; }
            set { _BidDOM6Volume = value; }
        }
        /// <summary>
        /// 最佳買價7
        /// </summary>
        public decimal BidDOM7Price
        {
            get { return _BidDOM7Price; }
            set { _BidDOM7Price = value; }
        }
        /// <summary>
        /// 最佳買量7
        /// </summary>
        public int BidDOM7Volume
        {
            get { return _BidDOM7Volume; }
            set { _BidDOM7Volume = value; }
        }
        /// <summary>
        /// 最佳買價8
        /// </summary>
        public decimal BidDOM8Price
        {
            get { return _BidDOM8Price; }
            set { _BidDOM8Price = value; }
        }
        /// <summary>
        /// 最佳買量8
        /// </summary>
        public int BidDOM8Volume
        {
            get { return _BidDOM8Volume; }
            set { _BidDOM8Volume = value; }
        }
        /// <summary>
        /// 最佳買價9
        /// </summary>
        public decimal BidDOM9Price
        {
            get { return _BidDOM9Price; }
            set { _BidDOM9Price = value; }
        }
        /// <summary>
        /// 最佳買量9
        /// </summary>
        public int BidDOM9Volume
        {
            get { return _BidDOM9Volume; }
            set { _BidDOM9Volume = value; }
        }
        /// <summary>
        /// 最佳買價10
        /// </summary>
        public decimal BidDOM10Price
        {
            get { return _BidDOM10Price; }
            set { _BidDOM10Price = value; }
        }
        /// <summary>
        /// 最佳買量10
        /// </summary>
        public int BidDOM10Volume
        {
            get { return _BidDOM10Volume; }
            set { _BidDOM10Volume = value; }
        }

    }
}
