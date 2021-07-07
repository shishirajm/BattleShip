using System;
namespace Battleship.Player.Dtos
{
    public class BoardStateDto
    {
        public Spot Spot { get; set; }
        public bool Attacked { get; set; }

        // This HasShip can be exposed based
        // on enemy view or own player view
        // havent handled it here
        public bool HasShip { get; set; }

        public override string ToString()
        {
            return $"{Spot.Row} - {Spot.Column} {Attacked} {HasShip}";
        }
    }
}
