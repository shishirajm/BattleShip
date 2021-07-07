using System;
using System.Collections.Generic;

namespace Battleship.Player
{
    public interface IShipConfiguration
    {
        public Dictionary<int, int> ShipSizes { get; }
    }

    public class ShipConfiguration: IShipConfiguration
    {
        public Dictionary<int, int> ShipSizes
        {
            get
            {
                // return new[] { 5, 4, 3, 3, 2 };
                return new Dictionary<int, int> { { 0, 2 } };
            }
        }
    }
}
