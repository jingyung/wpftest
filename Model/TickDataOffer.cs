using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TickDataOffer
    {
        private string _EXCHAGE = "";
        private string _SYMBOL = "";
        private string _YM = "";
        private string _CP = "";
        private string _STRIKE = "";
        private decimal _DISPLAY_DENOMINATOR = 0;//分母
        private decimal _DISPLAY_MULTIPLY = 0;//倍率 

        private decimal _OfferDOM1Price = 0;
        private int _OfferDOM1Volume = 0;
        private decimal _OfferDOM2Price = 0;
        private int _OfferDOM2Volume = 0;
        private decimal _OfferDOM3Price = 0;
        private int _OfferDOM3Volume = 0;
        private decimal _OfferDOM4Price = 0;
        private int _OfferDOM4Volume = 0;
        private decimal _OfferDOM5Price = 0;
        private int _OfferDOM5Volume = 0;
        private decimal _OfferDOM6Price = 0;
        private int _OfferDOM6Volume = 0;
        private decimal _OfferDOM7Price = 0;
        private int _OfferDOM7Volume = 0;
        private decimal _OfferDOM8Price = 0;
        private int _OfferDOM8Volume = 0;
        private decimal _OfferDOM9Price = 0;
        private int _OfferDOM9Volume = 0;
        private decimal _OfferDOM10Price = 0;
        private int _OfferDOM10Volume = 0;

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
        /// 最佳賣價1
        /// </summary>
        public decimal OfferDOM1Price
        {
            get { return _OfferDOM1Price; }
            set { _OfferDOM1Price = value; }
        }
        /// <summary>
        /// 最佳賣量1
        /// </summary>
        public int OfferDOM1Volume
        {
            get { return _OfferDOM1Volume; }
            set { _OfferDOM1Volume = value; }
        }
        /// <summary>
        /// 最佳賣價2
        /// </summary>
        public decimal OfferDOM2Price
        {
            get { return _OfferDOM2Price; }
            set { _OfferDOM2Price = value; }
        }
        /// <summary>
        /// 最佳賣量2
        /// </summary>
        public int OfferDOM2Volume
        {
            get { return _OfferDOM2Volume; }
            set { _OfferDOM2Volume = value; }
        }
        /// <summary>
        /// 最佳賣價3
        /// </summary>
        public decimal OfferDOM3Price
        {
            get { return _OfferDOM3Price; }
            set { _OfferDOM3Price = value; }
        }
        /// <summary>
        /// 最佳賣量3
        /// </summary>
        public int OfferDOM3Volume
        {
            get { return _OfferDOM3Volume; }
            set { _OfferDOM3Volume = value; }
        }
        /// <summary>
        /// 最佳賣價4
        /// </summary>
        public decimal OfferDOM4Price
        {
            get { return _OfferDOM4Price; }
            set { _OfferDOM4Price = value; }
        }
        /// <summary>
        /// 最佳賣量4
        /// </summary>
        public int OfferDOM4Volume
        {
            get { return _OfferDOM4Volume; }
            set { _OfferDOM4Volume = value; }
        }
        /// <summary>
        /// 最佳賣價5
        /// </summary>
        public decimal OfferDOM5Price
        {
            get { return _OfferDOM5Price; }
            set { _OfferDOM5Price = value; }
        }
        /// <summary>
        /// 最佳賣量5
        /// </summary>
        public int OfferDOM5Volume
        {
            get { return _OfferDOM5Volume; }
            set { _OfferDOM5Volume = value; }
        }
        /// <summary>
        /// 最佳賣價6
        /// </summary>
        public decimal OfferDOM6Price
        {
            get { return _OfferDOM6Price; }
            set { _OfferDOM6Price = value; }
        }
        /// <summary>
        /// 最佳賣量6
        /// </summary>
        public int OfferDOM6Volume
        {
            get { return _OfferDOM6Volume; }
            set { _OfferDOM6Volume = value; }
        }
        /// <summary>
        /// 最佳賣價7
        /// </summary>
        public decimal OfferDOM7Price
        {
            get { return _OfferDOM7Price; }
            set { _OfferDOM7Price = value; }
        }
        /// <summary>
        /// 最佳賣量7
        /// </summary>
        public int OfferDOM7Volume
        {
            get { return _OfferDOM7Volume; }
            set { _OfferDOM7Volume = value; }
        }
        /// <summary>
        /// 最佳賣價8
        /// </summary>
        public decimal OfferDOM8Price
        {
            get { return _OfferDOM8Price; }
            set { _OfferDOM8Price = value; }
        }
        /// <summary>
        /// 最佳賣量8
        /// </summary>
        public int OfferDOM8Volume
        {
            get { return _OfferDOM8Volume; }
            set { _OfferDOM8Volume = value; }
        }
        /// <summary>
        /// 最佳賣價9
        /// </summary>
        public decimal OfferDOM9Price
        {
            get { return _OfferDOM9Price; }
            set { _OfferDOM9Price = value; }
        }
        /// <summary>
        /// 最佳賣量9
        /// </summary>
        public int OfferDOM9Volume
        {
            get { return _OfferDOM9Volume; }
            set { _OfferDOM9Volume = value; }
        }
        /// <summary>
        /// 最佳賣價10
        /// </summary>
        public decimal OfferDOM10Price
        {
            get { return _OfferDOM10Price; }
            set { _OfferDOM10Price = value; }
        }
        /// <summary>
        /// 最佳賣量10
        /// </summary>
        public int OfferDOM10Volume
        {
            get { return _OfferDOM10Volume; }
            set { _OfferDOM10Volume = value; }
        }
    }
}
