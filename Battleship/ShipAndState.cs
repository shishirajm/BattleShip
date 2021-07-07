using System;
using Battleship.Player.Dtos;
using static Battleship.Player.Enums;

namespace Battleship.Player
{
    public interface IShipAndState
    {
        BattleShip Ship { get; }
        bool Placed { get; }
        int AttackedCount { get; }

        void SetPlaced(Direction direction);
        void Attacked();
        bool HasSunk();
        ShipStateDto GetShipState();
    }

    public class ShipAndState : IShipAndState
    {
        public BattleShip Ship { get; private set; }
        public bool Placed { get; private set; }
        public Direction Placement { get; private set; }
        public int AttackedCount { get; private set; }

        public ShipAndState(BattleShip ship, bool placed)
        {
            Ship = ship;
            Placed = placed;
            AttackedCount = 0;
        }

        public void SetPlaced(Direction direction)
        {
            Placed = true;
            Placement = direction;
        }

        public void Attacked()
        {
            AttackedCount++;
        }

        public bool HasSunk()
        {
            return AttackedCount >= Ship.Length;
        }

        public ShipStateDto GetShipState()
        {
            return new ShipStateDto {
                Id = Ship.ShipId,
                ShipLength = Ship.Length,
                Placed = Placed,
                AttackCount = AttackedCount,
                Placement = Placement == Direction.Horizontal? "horizontal": "vartical" };
        }
    }

    public interface IShipStateFactory
    {
        public IShipAndState CreateShipAndState(BattleShip ship, bool placed);
    }

    public class ShipStateFactory: IShipStateFactory
    {
        public IShipAndState CreateShipAndState(BattleShip ship, bool placed)
        {
            return new ShipAndState(ship, placed);
        }
    }
}
