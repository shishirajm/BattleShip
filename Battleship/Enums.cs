using System;
namespace Battleship.Player
{
    public static class Enums
    {
        public enum BoardState
        {
            Unknown,
            Created,
            BoardInitialised,
            AllShipsCreated,
            ShipsBeingPlaced,
            ShipPlaced,
            AllShipsPlaced,
            BattleStarted,
            ShipSunk,
            AllShipsSunk,
            Lost
        }

        public enum AttackResult
        {
            Miss,
            Hit
        }

        public enum Direction
        {
            Horizontal,
            Vertical
        }
    }
}
