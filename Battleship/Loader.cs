using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using static Battleship.Player.Enums;

namespace Battleship.Player
{
#nullable enable
    public interface ILoader
    {
        public Player? LoadPlayer(string key);
    }

    public class Loader: ILoader
    {
        public Loader()
        {
        }

        // Rehydrate the Player from events
        public Player? LoadPlayer(string key)
        {
            var tracker = StateTracker.GetInstance();
            var events = tracker.GetGameEvents(key);
            var shipConfig = new Dictionary<int, int>();

            Player? player = null;
            if (events == null) return player;

            foreach (var e in events) {
                if (e.EventType == null || e.EventDigest == null) continue;

                try
                {
                    if (e.EventType == BattleShipEventTypes.BoardCreated)
                    {
                        var jObj = JObject.Parse(e.EventDigest);
                        int row, column;
                        foreach (var kv in jObj)
                        {
                            if (kv.Key == "rows")
                                row = int.Parse(kv.Value.ToString());
                            if (kv.Key == "columns")
                                column = int.Parse(kv.Value.ToString());
                        }
                    }

                    if (e.EventType == BattleShipEventTypes.ShipCreated)
                    {
                        var jObj = JObject.Parse(e.EventDigest);
                        int shipId = 0, shipSize = 0;
                        foreach (var kv in jObj)
                        {
                            if (kv.Key == "shipId")
                                shipId = int.Parse(kv.Value.ToString());
                            if (kv.Key == "shipSize")
                                shipSize = int.Parse(kv.Value.ToString());
                        }
                        shipConfig.Add(shipId, shipSize);
                    }

                    if (e.EventType == BattleShipEventTypes.BoardState)
                    {
                        var jObj = JObject.Parse(e.EventDigest);
                        var state = jObj["state"].ToObject<BoardState>();
                        if (state == BoardState.AllShipsCreated)
                        {
                            player = new Player(key, new ShipConfig(shipConfig), true);
                        }
                    }

                    if (e.EventType == BattleShipEventTypes.ShipPlaced)
                    {
                        var jObj = JObject.Parse(e.EventDigest);

                        var shipId = jObj["shipId"].ToObject<int>();
                        var spot = new Spot(jObj["spot"].ToObject<string>());
                        var direction = jObj["direction"].ToObject<Direction>();

                        if (null != player)
                        {
                            player.PlaceShip(player.ShipAndStates[shipId].Ship, spot, direction);
                        }
                    }

                    if (e.EventType == BattleShipEventTypes.Attacked)
                    {
                        var jObj = JObject.Parse(e.EventDigest);

                        var spot = new Spot(jObj["spot"].ToObject<string>());

                        if (null != player)
                        {
                            player.Attack(spot);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // I know sophesticated logger will be good
                    Console.WriteLine($"Error loading player {key}: {ex.Message}");
                }
            }

            if (player != null) player.SetLoading(false);

            return player;
        }

        private class ShipConfig : IShipConfiguration
        {
            public Dictionary<int, int> ShipSizes { get; private set; }

            public ShipConfig(Dictionary<int, int> shipSizes)
            {
                ShipSizes = shipSizes;
            }
        }
    }
#nullable disable
}
