using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using back.Models;

namespace back.Controllers
{


    [Route("api/convite")]
    [ApiController]
    public class ConviteController : ControllerBase
    {
        private readonly GestaoServicosClientesContext _context;

        public ConviteController(GestaoServicosClientesContext context)
        {
            _context = context;
        }


        // GET: api/convite
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Convite>>> GetUtilizadors()
        {
            return await _context.Convites.ToListAsync();
        }

    }
}
