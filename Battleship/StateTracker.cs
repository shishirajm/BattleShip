using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using static Battleship.Player.Enums;

// Assumption: withing a given games events occur sequentially
namespace Battleship.Player
{

#nullable enable
    public interface IStateTracker
    {
        public BoardState CurrentState(string key);

        public void BoardCreated(string key);

        public void ShipCreated(string key, int shipId, int shipSize);

        public void AllShipCreated(string key);

        public void ShipPlaced(string key, int shipId, Spot spot, Direction direction, int totalShips);

        public void Attack(string key, Spot spot);

        public void ShipSunk(string key, int shipId, int totalShips);
    }

    // This is a repository, can be moved into another binary/project.
    // for now leaving it here.
    public class StateTracker : IStateTracker
    {
        // Just dumping all the events
        private Dictionary<string, List<BattleShipEvent>> GameEvents { get; set; }
        private static StateTracker? stateTracker = null;

        public BoardState CurrentState(string key)
        {
            if (GameEvents.ContainsKey(key))
            {
                var state = GameEvents[key];
                var e = state.FindLast(e => e.EventType == BattleShipEventTypes.BoardState);
                if (e == null) return BoardState.Unknown;
                var json = JObject.Parse(e.EventDigest);
                return json["state"].ToObject<BoardState>();
            }
            else
            {
                return BoardState.Unknown;
            }
        }

        public List<BattleShipEvent>? GetGameEvents(string key)
        {
            if (GameEvents.ContainsKey(key))
            {
                return GameEvents[key];
            }
            else
            {
                return null;
            }
        }

        private StateTracker()
        {
            GameEvents = new Dictionary<string, List<BattleShipEvent>>();
        }

        public static StateTracker GetInstance()
        {
            if (stateTracker == null)
            {
                stateTracker = new StateTracker();
            }

            return stateTracker;
        }

        public void BoardCreated(string key)
        {
            StoreBoardEvent(key, BoardState.Created);
            StoreBoardEvent(key, BoardState.BoardInitialised);
            StoreBattleShipEvents(new BattleShipEvent
            {
                Key = key,
                EventType = BattleShipEventTypes.BoardCreated,
                EventDigest = @"{""rows"": 10, ""columns"": 10}"
            });
        }

        public void ShipCreated(string key, int shipId, int shipSize)
        {
            StoreBoardEvent(key, BoardState.BoardInitialised);
            StoreBattleShipEvents(new BattleShipEvent
            {
                Key = key,
                EventType = BattleShipEventTypes.ShipCreated,
                EventDigest = $"{{\"shipId\": {shipId}, \"shipSize\": {shipSize}}}"
            });
        }

        public void AllShipCreated(string key)
        {
            StoreBoardEvent(key, BoardState.AllShipsCreated);
        }

        public void ShipPlaced(string key, int shipId, Spot spot, Direction direction, int totalShips)
        {
            var shipsPlaced = GetBoardEventByState(key, BoardState.ShipPlaced);
            if (shipsPlaced.Count == 0)
            {
                StoreBoardEvent(key, BoardState.ShipsBeingPlaced);
            }

            StoreBoardEvent(key, BoardState.ShipPlaced);

            if (shipsPlaced.Count == totalShips - 1)
            {
                StoreBoardEvent(key, BoardState.AllShipsPlaced);
            }

            StoreBattleShipEvents(new BattleShipEvent
            {
                Key = key,
                EventType = BattleShipEventTypes.ShipPlaced,
                EventDigest = $"{{\"shipId\": {shipId}, \"spot\": \"{spot}\", \"direction\": \"{direction}\"}}"
            });
        }

        public void Attack(string key, Spot spot)
        {
            if (CurrentState(key) == BoardState.AllShipsPlaced)
            {
                StoreBoardEvent(key, BoardState.BattleStarted);
            }

            StoreBattleShipEvents(new BattleShipEvent
            {
                Key = key,
                EventType = BattleShipEventTypes.Attacked,
                EventDigest = $"{{\"spot\": \"{spot}\"}}"
            });
        }

        public void ShipSunk(string key, int shipId, int totalShips)
        {
            StoreBoardEvent(key, BoardState.ShipSunk);
            var shipsSunk = GetBoardEventByState(key, BoardState.ShipSunk);

            StoreBattleShipEvents(new BattleShipEvent
            {
                Key = key,
                EventType = BattleShipEventTypes.ShipSunk,
                EventDigest = $"{{\"shipId\": {shipId}}}"
            });

            if (shipsSunk.Count == totalShips)
            {
                StoreBoardEvent(key, BoardState.AllShipsSunk);
                StoreBoardEvent(key, BoardState.Lost);
                StoreBattleShipEvents(new BattleShipEvent
                {
                    Key = key,
                    EventType = BattleShipEventTypes.Result,
                    EventDigest = $"{{\"result\": \"Lost\"}}"
                });
            }
        }

        private void StoreBoardEvent(string key, BoardState state)
        {
            StoreBattleShipEvents(new BattleShipEvent
            {
                Key = key,
                EventType = BattleShipEventTypes.BoardState,
                EventDigest = $"{{\"state\": \"{state}\"}}"
            });
        }

        private void StoreBattleShipEvents(BattleShipEvent e)
        {
            if (e.Key == null) return;

            List<BattleShipEvent> list;
            if (GameEvents.ContainsKey(e.Key))
            {
                list = GameEvents[e.Key];
                list.Add(e);
                GameEvents[e.Key] = list;
            }
            else
            {
                list = new List<BattleShipEvent>();
                list.Add(e);
                GameEvents.Add(e.Key, list);
            }
        }

        private List<BattleShipEvent> GetBoardEventByState(string key, BoardState boardState)
        {
            var state = GameEvents[key];
            var e = state.FindAll(e => e.EventType == BattleShipEventTypes.BoardState);
            return e.FindAll(ev => JObject.Parse(ev.EventDigest)["state"].ToObject<BoardState>() == boardState).ToList();
        }
    }

    public class BattleShipEvent
    {
        public string? Key { get; set; }
        public string? EventType { get; set; }
        public string? EventDigest { get; set; }
    }

    public class BattleShipEventTypes
    {
        public const string BoardCreated = "BoardCreated";
        public const string ShipCreated = "ShipCreated";
        public const string ShipPlaced = "ShipPlaced";
        public const string ShipSunk = "ShipSunk";
        public const string Attacked = "Attacked";
        public const string Result = "Result";
        public const string BoardState = "BoardState";
    }
#nullable disable
}
