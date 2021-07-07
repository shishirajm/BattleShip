using System;
using static Battleship.Player.Enums;

namespace Battleship.Player.Dtos
{
    public class ShipStateDto
    {
        public int Id { get; set; }
        public int ShipLength { get; set; }
        public bool Placed { get; set; }
        public int AttackCount { get; set; }
        public string Placement { get; set; }

        public override string ToString()
        {
            return $"Ship - {ShipLength} Placed {Placed} Direction {Placement} Attacked {AttackCount} times";
        }
    }
}
