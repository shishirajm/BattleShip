using System;
using System.Collections.Generic;
using static Battleship.Player.Enums;

namespace Battleship.Player.Dtos
{
    public class StateDto
    {
        public string Id { get; set; }
        public IEnumerable<ShipStateDto> ShipStates { get; set; }
        public string CurrentState { get; set; }
        public IEnumerable<BoardStateDto> BoardPositions { get; set; }

        public override string ToString()
        {
            var strings = new List<string>();
            foreach (var b in BoardPositions)
            {
                strings.Add(b.ToString());
            }

            foreach (var s in ShipStates)
            {
                strings.Add(s.ToString());
            }

            strings.Add(CurrentState.ToString());

            return string.Join("\n", strings);
        }
    }
}
