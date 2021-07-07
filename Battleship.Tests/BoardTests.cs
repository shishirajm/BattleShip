using System;
using NUnit.Framework;
using static Battleship.Player.Enums;

namespace Battleship.Player.Tests
{
    // I feel there are lot more tests which I can write
    public class Tests
    {
        private Board board;
        [SetUp]
        public void Setup()
        {
            board = new Board(null);
        }

        [Test]
        public void PlaceShip_ShouldPlaceShipSuccessfully_OnValidSpotAndDirectionHorizontal()
        {
            var ship = new BattleShip(2);
            board.PlaceShip(ship, new Spot(0, 0), Direction.Horizontal);
            var actual1 = board.Attack(new Spot(0, 0));
            var actual2 = board.Attack(new Spot(0, 1));
            
            Assert.AreEqual(ship, actual1);
            Assert.AreEqual(ship, actual2);
            Assert.IsNull(board.Attack(new Spot(0, 2)));
            Assert.IsNull(board.Attack(new Spot(1, 0)));
            Assert.IsNull(board.Attack(new Spot(1, 1)));
        }

        [Test]
        public void PlaceShip_ShouldPlaceShipSuccessfully_OnValidSpotAndDirectionVertical()
        {
            var ship = new BattleShip(2);
            board.PlaceShip(ship, new Spot(0, 0), Direction.Vertical);
            var actual1 = board.Attack(new Spot(0, 0));
            var actual2 = board.Attack(new Spot(1, 0));

            Assert.AreEqual(ship, actual1);
            Assert.AreEqual(ship, actual2);
            Assert.IsNull(board.Attack(new Spot(2, 0)));
            Assert.IsNull(board.Attack(new Spot(0, 1)));
            Assert.IsNull(board.Attack(new Spot(0, 2)));
        }

        [Test]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void PlaceShip_ShouldThrowException_OnInValidSpotForShipWhenVertical(int shipSize)
        {
            var ship = new BattleShip(shipSize);
            Assert.Throws<Exception>(() => board.PlaceShip(ship, new Spot(9, 0), Direction.Vertical));
            Assert.Throws<Exception>(() => board.PlaceShip(ship, new Spot(9, 9), Direction.Vertical));
        }

        [Test]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void PlaceShip_ShouldThrowException_OnInValidSpotForShipWhenHorizontal(int shipSize)
        {
            var ship = new BattleShip(shipSize);
            Assert.Throws<Exception>(() => board.PlaceShip(ship, new Spot(0, 9), Direction.Horizontal));
            Assert.Throws<Exception>(() => board.PlaceShip(ship, new Spot(9, 9), Direction.Horizontal));
        }
    }
}