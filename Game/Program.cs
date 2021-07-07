using System;
using Battleship.Player;
using Battleship.Player.Dtos;
using static Battleship.Player.Enums;

namespace Game
{
    class Program
    {
        // Just added this to test
        static void Main(string[] args)
        {
            var shipConfiguration =  new ShipConfiguration();
            var user1 = new Player(shipConfiguration);
            WriteState(user1);
            user1.PlaceShip(user1.ShipAndStates[0].Ship, new Spot(1, 1), Direction.Vertical);
            WriteState(user1);
            var res = user1.Attack(new Spot(1, 1 ));
            Console.WriteLine(res);
            WriteState(user1);
            res = user1.Attack(new Spot(9, 9));
            Console.WriteLine(res);
            WriteState(user1);
            res = user1.Attack(new Spot(2, 1));
            Console.WriteLine(res);
            WriteState(user1);
            PrintState(user1.GetState());

            var user2 = new Player(shipConfiguration);
            var user3 = new Player(shipConfiguration);
            WriteState(user2);
            WriteState(user3);
            user2.PlaceShip(user2.ShipAndStates[0].Ship, new Spot(1, 1), Direction.Vertical);
            user3.PlaceShip(user3.ShipAndStates[0].Ship, new Spot(1, 1), Direction.Vertical);
            WriteState(user2);
            WriteState(user3);
            res = user2.Attack(new Spot(1, 1));
            Console.WriteLine(res);
            res = user3.Attack(new Spot(1, 1));
            Console.WriteLine(res);
            WriteState(user2);
            WriteState(user3);
            res = user2.Attack(new Spot(9, 9));
            Console.WriteLine(res);
            WriteState(user2);
            res = user2.Attack(new Spot(2, 1));
            Console.WriteLine(res);
            WriteState(user2);
            PrintState(user2.GetState());

            var loader = new Loader();
            loader.LoadPlayer(user3.Board.Key);
            res = user3.Attack(new Spot(9, 9));
            Console.WriteLine(res);
            WriteState(user3);
            res = user3.Attack(new Spot(2, 1));
            Console.WriteLine(res);
            WriteState(user3);
            PrintState(user3.GetState());
        }

        private static void WriteState(Player player)
        {
            Console.WriteLine(player.State);
        }

        private static void PrintState(StateDto dto)
        {
            Console.WriteLine(dto);
        }
    }
}
