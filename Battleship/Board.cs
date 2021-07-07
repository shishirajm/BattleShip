using System;
using System.Collections.Generic;
using System.Linq;
using Battleship.Player.Dtos;
using static Battleship.Player.Enums;

namespace Battleship.Player
{
#nullable enable
    public interface IBoard
    {
        string Key { get; }

        void PlaceShip(BattleShip battleShip, Spot spot, Direction direction);

        BattleShip? Attack(Spot spot);

        IEnumerable<BoardStateDto> GetBoardState();
    }

    public class Board : IBoard
    {
        public string Key { get; private set; }
        private Position[,] Positions { get; set; }

        public Board(string key)
        {
            int numberOfRows = 10, numberOfColumns = 10;
            Key = string.IsNullOrWhiteSpace(key)? Guid.NewGuid().ToString(): key;
            if (numberOfRows <= 0 || numberOfColumns <= 0) throw new ArgumentException();

            Positions = new Position[numberOfRows, numberOfColumns];

            for (var i = 0; i < numberOfRows; i++)
            {
                for (var j = 0; j < numberOfColumns; j++)
                {
                    Positions[i, j] = new Position(new Spot(i, j));
                }
            }
        }

        public void PlaceShip(BattleShip battleShip, Spot spot, Direction direction)
        {
            ThrowIfUnableToPlaceShip(battleShip, spot, direction);

            if (direction == Direction.Horizontal) PlaceShipHorizontal(battleShip, spot);
            if (direction == Direction.Vertical) PlaceShipVertical(battleShip, spot);
        }

        public BattleShip? Attack(Spot spot)
        {
            var position = GetPosition(spot);
            if (position.Attacked) throw new Exception($"Spot is already attacked ({spot.Row}, {spot.Column})");

            position.Attack();
            return position.BattleShip;
        }

        public IEnumerable<BoardStateDto> GetBoardState()
        {
            var board = new List<BoardStateDto>();
            for (var i = 0; i < Positions.GetLength(0); i++)
            {
                for (var j = 0; j < Positions.GetLength(1); j++)
                {
                    board.Add(new BoardStateDto { Spot = new Spot(i, j), Attacked = Positions[i, j].Attacked, HasShip = Positions[i, j].Attacked && Positions[i, j].ContainsShip() ? true : false });
                }
            }

            return board;
        }

        private void ThrowIfUnableToPlaceShip(BattleShip battleShip, Spot spot, Direction direction)
        {
            var canPlace = AvailableSpotsFor(battleShip)
                .Where(x => x.Item1.Spot.Equals(spot) && x.Item2 == direction);

            if (canPlace == null || canPlace.ToList().Count == 0)
            {
                throw new Exception("Can't place the ship");
            }
        }

        private List<(Position, Direction)> AvailableSpotsFor(BattleShip battleShip)
        {
            var availablePositions = new List<(Position, Direction)>();
            for (var i = 0; i < Positions.GetLength(0); i++)
            {
                for (var j = 0; j < Positions.GetLength(1); j++)
                {
                    if (CanPlaceBattleShipHorizontal(i, j, battleShip))
                    {
                        availablePositions.Add((Positions[i, j], Direction.Horizontal));
                    }

                    if (CanPlaceBattleShipVertical(i, j, battleShip))
                    {
                        availablePositions.Add((Positions[i, j], Direction.Vertical));
                    }
                }
            }

            return availablePositions;
        }

        private bool CanPlaceBattleShipHorizontal(int rowIndex, int columnIndex, BattleShip battleShip)
        {
            for (var j = columnIndex; j < columnIndex + battleShip.Length; j++)
            {
                if (j >= Positions.GetLength(1) ||
                    Positions[rowIndex, j].ContainsShip() ||
                    Positions[rowIndex, j].Attacked)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CanPlaceBattleShipVertical(int rowIndex, int columnIndex, BattleShip battleShip)
        {
            for (var i = rowIndex; i < rowIndex + battleShip.Length; i++)
            {
                if (i >= Positions.GetLength(0) ||
                    Positions[i, columnIndex].ContainsShip() ||
                    Positions[i, columnIndex].Attacked)
                {
                    return false;
                }
            }

            return true;
        }

        private void PlaceShipHorizontal(BattleShip battleShip, Spot spot)
        {
            for (var j = spot.Column; j < spot.Column + battleShip.Length; j++)
            {
                Positions[spot.Row, j].PlaceShip(battleShip);
            }
        }

        private void PlaceShipVertical(BattleShip battleShip, Spot spot)
        {
            for (var i = spot.Row; i < spot.Row + battleShip.Length; i++)
            {
                Positions[i, spot.Column].PlaceShip(battleShip);
            }
        }

        private Position GetPosition(Spot spot)
        {
            return Positions[spot.Row, spot.Column];
        }
    }
#nullable disable
}
