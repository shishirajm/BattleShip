using System;
namespace BattleShip.Api.Dtos
{
    public class AttackDto
    {
        public int RowPosition { get; set; }
        public int ColumnPosition { get; set; }
    }
}
