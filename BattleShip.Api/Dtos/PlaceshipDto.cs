using System;
using static Battleship.Player.Enums;

namespace BattleShip.Api.Dtos
{
    public class PlaceshipDto
    {
        public int ShipId { get; set; }
        public int RowPosition { get; set; }
        public int ColumnPosition { get; set; }
        public string Direction { get; set; }
    }
}
