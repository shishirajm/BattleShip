using System;
using static Battleship.Player.Enums;

namespace Battleship.Player
{
#nullable enable
    public class Position
    {
        public bool Attacked { get; private set; }
        public Spot Spot { get; private set; }
        public BattleShip? BattleShip { get; private set; }

        public Position(Spot spot)
        {
            Attacked = false;
            Spot = spot;
        }

        public void Attack()
        {
            Attacked = true;
        }

        public void PlaceShip(BattleShip ship)
        {
            BattleShip = ship;
        }

        public bool ContainsShip() => BattleShip != null;
    }
#nullable disable
}
