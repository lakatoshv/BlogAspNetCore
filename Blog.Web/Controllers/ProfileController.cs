using AutoMapper;
using Blog.Services.ControllerContext;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="BaseController" />
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : BaseController
    {
        private readonly IProfileService _profileService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(
            IControllerContext controllerContext,
            IProfileService profileService,
            UserManager<ApplicationUser> userManager) 
            : base(controllerContext)
        {
            _profileService = profileService;
            _userManager = userManager;
        }

        // GET: Profile/5        
        /// <summary>
        /// Shows the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public async Task<ActionResult> Show(int id)
        {
            var profile = await _profileService.GetProfile(id);

            if (profile == null)
            {
                return NotFound();
            }
            
            return Ok(profile);
        }
    }
}