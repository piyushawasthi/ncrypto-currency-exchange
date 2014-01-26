﻿using Lostics.NCryptoExchange.CoinEx;
using Lostics.NCryptoExchange.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lostics.NCryptoExchange
{
    [TestClass]
    public class CoinExTest
    {
        /* [TestMethod]
        public void TestParseAccountInfo()
        {
            JObject jsonObj = LoadTestData("get_wallets.json");
            AccountInfo accountInfo = CoinExParsers.ParseAccountInfo(jsonObj);

            Assert.AreEqual(5, accountInfo.Wallets.Count);

            foreach (Wallet wallet in accountInfo.Wallets)
            {
                if (wallet.CurrencyCode.Equals("BTC"))
                {
                    Assert.AreEqual((decimal)3.05354081, wallet.Balance);
                }
            }
        }

        [TestMethod]
        public void TestParseCancelOrder()
        {
            JObject jsonObj = LoadTestData("cancel_order.json");
            CoinExMyOrder order = CoinExMyOrder.Parse(jsonObj.Value<JObject>("order"));

            Assert.AreEqual("B/0.00212300/5517761665040384", order.OrderId.ToString());
            Assert.AreEqual(CoinExOrderStatus.cancel_requested, order.Status);
        } */

        [TestMethod]
        public void TestParseCoinExCurrencies()
        {
            JObject jsonObj = LoadTestData("currencies.json");
            List<CoinExCurrency> coins = jsonObj.Value<JArray>("currencies").Select(
                coin => CoinExCurrency.Parse(coin as JObject)
            ).ToList();
            
            Assert.AreEqual(50, coins.Count);
            Assert.AreEqual("BOC", coins[0].CurrencyCode);
            Assert.AreEqual("BountyCoin", coins[0].Label);
            Assert.AreEqual("LKY", coins[1].CurrencyCode);
            Assert.AreEqual("LuckyCoin", coins[1].Label);
        }

        [TestMethod]
        public void TestParseCoinExMarketData()
        {
            JObject jsonObj = LoadTestData("trade_pairs.json");
            JArray marketsJson = jsonObj.Value<JArray>("trade_pairs");
            List<CoinExMarket> markets = marketsJson.Select(
                market => CoinExMarket.Parse((JObject)market)
            ).ToList();

            Assert.AreEqual(70, markets.Count);

            Assert.AreEqual(markets[0].Label, "phs_btc");
            Assert.AreEqual(markets[0].BaseCurrencyCode, "PHS");
            Assert.AreEqual(markets[0].Statistics.Volume24HBase, (decimal)56747538);
            Assert.AreEqual(markets[0].QuoteCurrencyCode, "BTC");

            Assert.AreEqual(markets[1].Label, "mnc_btc");
            Assert.AreEqual(markets[1].BaseCurrencyCode, "MNC");
            Assert.AreEqual(markets[1].Statistics.Volume24HBase, (decimal)3028885);
            Assert.AreEqual(markets[1].QuoteCurrencyCode, "BTC");
        }

        /* [TestMethod]
        public void TestParseListOrders()
        {
            JObject jsonObj = LoadTestData("list_orders.json");
            JArray ordersJson = jsonObj.Value<JArray>("orders");
            List<CoinExMyOrder> orders = ordersJson.Select(
                market => CoinExMyOrder.Parse(market as JObject)
            ).ToList();

            Assert.AreEqual(2, orders.Count);
            
            Assert.AreEqual(OrderType.Buy, orders[0].OrderType);
            Assert.AreEqual((decimal)1.00000000, orders[0].OriginalQuantity);
            Assert.AreEqual((decimal)0.00000000, orders[0].Quantity);
        }

        [TestMethod]
        public void TestParseNewOrder()
        {
            JObject jsonObj = LoadTestData("new_order.json");
            CoinExMyOrder order = CoinExMyOrder.Parse(jsonObj.Value<JObject>("order"));
            
            Assert.AreEqual("B/0.00212300/5517761665040384", order.OrderId.ToString());
            Assert.AreEqual((decimal)1.00000000, order.Quantity);
            Assert.AreEqual((decimal)0.00212300, order.Price);
            Assert.AreEqual(true, order.IsOpen);
            Assert.AreEqual(CoinExOrderStatus.queued, order.Status);
        }

        [TestMethod]
        public void TestParseMarketDepth()
        {
            JObject jsonObj = LoadTestData("depth.json");
            Book marketOrders = CoinExParsers.ParseMarketDepth(jsonObj.Value<JObject>("marketdepth"));

            Assert.AreEqual(3, marketOrders.Bids.Count);
            Assert.AreEqual(1, marketOrders.Asks.Count);

            CoinExMarketDepth lowestSellOrder = (CoinExMarketDepth)marketOrders.Asks[0];

            Assert.AreEqual((decimal)2.92858000, lowestSellOrder.Price);
            Assert.AreEqual((decimal)8.98400000, lowestSellOrder.Quantity);
            Assert.AreEqual((decimal)8.98400000, lowestSellOrder.CummulativeQuantity);

            CoinExMarketDepth highestBuyOrder = (CoinExMarketDepth)marketOrders.Bids[2];

            Assert.AreEqual((decimal)0.12230000, highestBuyOrder.Price);
            Assert.AreEqual((decimal)11.00000000, highestBuyOrder.Quantity);
            Assert.AreEqual((decimal)16.40500000, highestBuyOrder.CummulativeQuantity);
        } */

        [TestMethod]
        public void TestParseCoinExRecentTrades()
        {
            CoinExMarketId marketId = new CoinExMarketId(46, "doge_btc");
            JObject jsonObj = LoadTestData("market_trades.json");
            List<CoinExMarketTrade> trades = jsonObj.Value<JArray>("trades").Select(
                marketTrade => CoinExMarketTrade.Parse(marketId, marketTrade as JObject)
            ).ToList();

            Assert.AreEqual(50, trades.Count);
            Assert.AreEqual("384409", trades[0].TradeId.ToString());
            Assert.AreEqual((decimal)250000000000, trades[0].Quantity);
            Assert.AreEqual((decimal)220, trades[0].Price);

            Assert.AreEqual((decimal)1303931062, trades[1].Quantity);
            Assert.AreEqual((decimal)219, trades[1].Price);
        }

        private JObject LoadTestData(string filename)
        {
            return TestUtils.LoadTestData<JObject>("CoinEx", filename);
        }
    }
}
