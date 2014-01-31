﻿using Lostics.NCryptoExchange.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lostics.NCryptoExchange.VaultOfSatoshi
{
    public static class VoSParsers
    {
        /// <summary>
        /// Parse market depth data (a paired list of bids and asks)
        /// </summary>
        /// <param name="depthJson"></param>
        /// <returns></returns>
        public static Book ParseOrderBook(JObject depthJson)
        {
            List<MarketDepth> asks = depthJson.Value<JArray>("asks").Select(depth => ParseMarketDepthEntry(depth)).ToList();
            List<MarketDepth> bids = depthJson.Value<JArray>("bids").Select(depth => ParseMarketDepthEntry(depth)).ToList();

            return new Book(asks, bids);
        }

        private static MarketDepth ParseMarketDepthEntry(JToken depth)
        {
            decimal quantity = ParseCurrencyObject(depth.Value<JObject>("quantity"));
            decimal price = ParseCurrencyObject(depth.Value<JObject>("price"));

            return new MarketDepth(price, quantity);
        }

        internal static decimal ParseCurrencyObject(JObject currencyJson)
        {
            return currencyJson.Value<decimal>("value");
        }

        internal static DateTime ParseTime(int secondsSinceEpoch)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(secondsSinceEpoch);
        }

        /// <summary>
        /// Parses an object which maps currency codes to a decimal value.
        /// </summary>
        /// <param name="jObject">The object to parse data from.</param>
        /// <returns></returns>
        internal static Dictionary<string, decimal> ParseCurrencyValueObj(JObject jObject)
        {
            Dictionary<string, decimal> currencyValues = new Dictionary<string, decimal>();

            foreach (JProperty property in jObject.Properties())
            {
                string currency = property.Name;
                decimal value = jObject.Value<decimal>(currency);

                currencyValues.Add(currency, value);
            }

            return currencyValues;
        }
    }
}
