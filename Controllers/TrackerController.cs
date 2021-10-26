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

        [HttpPost]
        [Route("copy/{auctionId}")]
        public Task<User> GetConnections(string userId)
        {
            return GetOrCreateUser(userId);
        }

        private async Task<User> GetOrCreateUser(string userId)
        {
            var user = await db.Users.Where(u => u.ExternalId == userId).Include(u => u.Accounts).FirstOrDefaultAsync();
            if (user == null)
            {
                user = new User() { ExternalId = userId };
                db.Users.Add(user);
                await db.SaveChangesAsync();
            }

            return user;
        }

        [HttpGet]
        [Route("minecraft/{mcUuid}")]
        public async Task<User> GetUser(string mcUuid)
        {
            return await db.McIds.Where(id => id.AccountUuid == mcUuid).Select(id => id.User).FirstOrDefaultAsync();
        }
    }
}
