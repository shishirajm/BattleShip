using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleship.Player.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Battleship.Player.Api.Controllers
{
    [ApiController]
    [Route("battleship")]
    public class BattleShipController : ControllerBase
    {
        private readonly ILogger<BattleShipController> _logger;
        private readonly IPlayer _player;

        public BattleShipController(ILogger<BattleShipController> logger,
            IPlayer player)
        {
            _logger = logger;
            _player = player;
        }

        [HttpPost("createboard")]
        public StateDto CreateBoard() => _player.GetState();

        
    }
}
