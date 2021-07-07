using System;
namespace Battleship.Player
{
    public class Spot : IEquatable<Spot>
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        public Spot(int row, int column)
        {
            if (row < 0 || column < 0) throw new ArgumentException();
            Row = row;
            Column = column;
        }

        public Spot(string spot)
        {
            if(spot.Contains("-"))
            {
                var splits =spot.Split("-");
                if (splits.Length == 2)
                {
                    Row = int.Parse(splits[0]);
                    Column = int.Parse(splits[1]);
                }
                else
                {
                    throw new Exception("Unable to create spot");
                }
            }
            else
            {
                throw new Exception("Unable to create spot");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return Equals(obj as Spot);
        }

        public bool Equals(Spot other)
        {
            return Row == other.Row && Column == other.Column;
        }

        public override int GetHashCode()
        {
            return $"{Row}-{Column}".GetHashCode();
        }

        public override string ToString()
        {
            return $"{Row}-{Column}";
        }
    }
}
