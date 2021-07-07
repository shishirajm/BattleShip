using System;
using System.Collections.Generic;
using System.Linq;
using Battleship.Player.Dtos;
using static Battleship.Player.Enums;

namespace Battleship.Player
{
    public interface IPlayer
    {
        void PlaceShip(BattleShip ship, Spot spot, Direction direction);
        AttackResult Attack(Spot spot);
        StateDto GetState();
        void SetLoading(bool loading);
    }

    public class Player : IPlayer
    {
        public IBoard Board { get; private set; }
        public IShipAndState[] ShipAndStates { get; private set; }
        private IShipStateFactory shipStateFactory;
        private readonly IStateTracker tracker;
        private readonly Dictionary<int, int> shipSizes;
        private bool loading = false;

        public BoardState State
        {
            get { return tracker.CurrentState(Board.Key); }
        }

        public Player(IShipConfiguration shipConfiguration, bool loading = false)
        {
            this.loading = loading;
            ThrowIfInvalidShipConfiguration(shipConfiguration);
            shipSizes = shipConfiguration.ShipSizes;

            Board = new Board(null);
            shipStateFactory = new ShipStateFactory();

            tracker = StateTracker.GetInstance();

            if(!loading) tracker.BoardCreated(Board.Key);
            CreateBattleShips();
        }

        public Player(string key, IShipConfiguration shipConfiguration, bool loading = false)
        {
            this.loading = loading;
            ThrowIfInvalidShipConfiguration(shipConfiguration);
            shipSizes = shipConfiguration.ShipSizes;

            Board = new Board(key);
            shipStateFactory = new ShipStateFactory();

            tracker = StateTracker.GetInstance();

            if (!loading) tracker.BoardCreated(Board.Key);
            CreateBattleShips();
        }

        // For unit testing
        public Player(IBoard board, IStateTracker stateTracker, IShipConfiguration shipConfiguration,
            IShipStateFactory shipAndStateFactory)
            : this(shipConfiguration)
        {
            Board = board;
            tracker = stateTracker;
            shipStateFactory = shipAndStateFactory;

            CreateBattleShips();
        }

        public void PlaceShip(BattleShip ship, Spot spot, Direction direction)
        {
            var shipPlacement = GetShipAndState(ship);

            if (!shipPlacement.Placed)
            {
                Board.PlaceShip(shipPlacement.Ship, spot, direction);
                shipPlacement.SetPlaced(direction);
                if (!loading) tracker.ShipPlaced(Board.Key, ship.ShipId, spot, direction, ShipAndStates.Length);
            }
            else
            {
                throw new Exception("Ship is already placed!");
            }
        }

        public AttackResult Attack(Spot spot)
        {
            // Assuming all ships must be placed
            ThrowIfAllShipsAreNotPlaced();

            var possibleShip = Board.Attack(spot);
            if (!loading) tracker.Attack(Board.Key, spot);
            if (possibleShip != null)
            {
                HandleShipAttacked(possibleShip);
            }

            return possibleShip == null ? AttackResult.Miss : AttackResult.Hit;
        }

        public StateDto GetState()
        {
            var boardState = Board.GetBoardState();
            var shipStates = new List<ShipStateDto>();
            foreach (var s in ShipAndStates)
            {
                shipStates.Add(s.GetShipState());
            }

            return new StateDto
            {
                Id = Board.Key,
                BoardPositions = boardState,
                ShipStates = shipStates,
                CurrentState = tracker.CurrentState(Board.Key).ToString()
            };
        }

        public void SetLoading(bool loading)
        {
            this.loading = loading;
        }

        private void HandleShipAttacked(BattleShip ship)
        {
            var placedShip = GetShipAndState(ship);
            placedShip.Attacked();
            if (placedShip.HasSunk())
            {
                if (!loading) tracker.ShipSunk(Board.Key, ship.ShipId, ShipAndStates.Length);
            }
        }

        private void CreateBattleShips()
        {
            ShipAndStates = new ShipAndState[shipSizes.Keys.Count];
            var i = 0;

            foreach (var kv in shipSizes)
            {
                var ship = new BattleShip(kv.Key, kv.Value);
                if (!loading) tracker.ShipCreated(Board.Key, kv.Key, kv.Value);
                ShipAndStates[i] = shipStateFactory.CreateShipAndState(ship, false);
                i++;
            }

            if (!loading) tracker.AllShipCreated(Board.Key);
        }

        private void ThrowIfInvalidShipConfiguration(IShipConfiguration shipConfiguration)
        {
            // Assuming there should be at least one ship
            if (shipConfiguration is null || shipConfiguration.ShipSizes == null || shipConfiguration.ShipSizes.Count == 0)
            {
                throw new ArgumentException(nameof(shipConfiguration));
            }
        }

        private IShipAndState GetShipAndState(BattleShip inputShip)
        {
            foreach (var shipAndState in ShipAndStates)
            {
                if (shipAndState.Ship == inputShip) return shipAndState;
            }

            throw new Exception("Unknown Battleship!");
        }

        private void ThrowIfAllShipsAreNotPlaced()
        {
            foreach(var ship in ShipAndStates)
            {
                if (ship.Placed == false) throw new Exception("Attack can happen only after ship is placed.");
            }
        }
    }
}
