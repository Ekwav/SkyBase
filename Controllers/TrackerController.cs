using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coflnet.Sky.McConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Coflnet.Sky.McConnect.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrackerController : ControllerBase
    {
        private readonly ILogger<ConnectController> _logger;
        private readonly ConnectContext db;
        private readonly ConnectService connectService;

        public TrackerController(ILogger<ConnectController> logger, ConnectContext context)
        {
            _logger = logger;
            db = context;
        }

        //TODO: Gleiche Klasse für hier und in den Command-Klassen verwenden (NewFlipTrackingModel & TrackFliipEventModel)

        [HttpPost]
        [Route("newFlip")]
        public Task<User> trackFlip([FromBody] NewFlipTrackingModel flip)
        {
            // TODO
        }

        [HttpPost]
        [Route("trackFlipEvent")]
        public async Task<User> GetUser(FlipEventTrackingModel mcUuid)
        {return await db.McIds.Where(id => id.AccountUuid == mcUuid).Select(id => id.User).FirstOrDefaultAsync();
            // TODO
        }
    }
}
