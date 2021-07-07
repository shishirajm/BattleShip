using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleship.Player;
using Battleship.Player.Dtos;
using BattleShip.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Battleship.Player.Enums;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BattleShip.Api.Controllers
{
    [ApiController]
    [Route("battleship")]
    public class BattleShipPlayController : Controller
    {
        private readonly ILogger<BattleShipPlayController> _logger;
        private readonly ILoader _loader;

        public BattleShipPlayController(ILogger<BattleShipPlayController> logger,
            ILoader loader)
        {
            _logger = logger;
            _loader = loader;
        }

        [HttpGet("state/{key}")]
        public IActionResult PlaceShip(string key)
        {
            var player = _loader.LoadPlayer(key);
            try
            {
                ThrowIfPlayerIsEmpty(key, player);
                return Ok(player.GetState());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("placeship/{key}")]
        public IActionResult PlaceShip(string key,
            [FromBody] PlaceshipDto dto)
        {
            var player = _loader.LoadPlayer(key);
            var validationResult = ValidatePlaceShipData(key, dto, player);
            if (validationResult != null) return validationResult;

            var ship = player.ShipAndStates[dto.ShipId].Ship;
            var direction = dto.Direction.ToLower() == "horizontal" ? Direction.Horizontal : Direction.Vertical;

            try
            {
                player.PlaceShip(ship, new Spot(dto.RowPosition, dto.ColumnPosition), direction);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(player.GetState());
        }

        [HttpPost("attack/{key}")]
        public IActionResult Attack(string key,
            [FromBody] AttackDto dto)
        {
            var player = _loader.LoadPlayer(key);
            var validationResult = ValidateAttackData(key, dto, player);
            if (validationResult != null) return validationResult;

            try {
                var result = player.Attack(new Spot(dto.RowPosition, dto.ColumnPosition));
                return Ok(result.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private IActionResult ValidatePlaceShipData(string key,
            PlaceshipDto dto,
            Player player)
        {
            try
            {
                ThrowIfPlayerIsEmpty(key, player);
                ThrowIfPlayerLost(player);
                ThrowIfInvalidShipId(dto.ShipId, player);
                ThrowIfInvalidSpot(dto.RowPosition, dto.ColumnPosition);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return null;
        }

        private IActionResult ValidateAttackData(string key,
            AttackDto dto,
            Player player)
        {
            try
            {
                ThrowIfPlayerIsEmpty(key, player);
                ThrowIfPlayerLost(player);
                ThrowIfInvalidSpot(dto.RowPosition, dto.ColumnPosition);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return null;
        }

        private void ThrowIfPlayerIsEmpty(string key, Player player)
        {
            if (player == null) throw new Exception($"No player found! invalid {key}");
        }

        private void ThrowIfPlayerLost(Player player)
        {
            if (player.State == BoardState.Lost) throw new Exception($"Game is already finalised.");
        }

        private void ThrowIfInvalidShipId(int shipId, Player player)
        {
            foreach(var shipState in player.ShipAndStates)
            {
                if (shipState.Ship.ShipId == shipId) return;
            }

            throw new Exception($"Invalid Ship Id {shipId}");
        }

        private void ThrowIfInvalidSpot(int rowPosition, int columnPosition)
        {
            if (rowPosition < 0 || rowPosition > 9) throw new Exception($"Invalid row position {rowPosition}");
            if (columnPosition < 0 || columnPosition > 9) throw new Exception($"Invalid column position {columnPosition}");
        }
    }
}