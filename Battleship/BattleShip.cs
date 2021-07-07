using System;
namespace Battleship.Player
{
    public class BattleShip
    {
        public int Length { get; private set; }
        public int ShipId { get; private set; }

        public BattleShip(int shipId, int length = 1)
        {
            ShipId = shipId;
            Length = length;
        }
    }
}
