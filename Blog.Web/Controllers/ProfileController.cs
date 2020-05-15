using AutoMapper;
using Blog.Services.ControllerContext;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.Data.Models;
using Blog.Web.VIewModels.AspNetUser;
using Blog.Web.VIewModels.Posts;
using Microsoft.AspNetCore.Authorization;
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
            profile.Profile.User = null;

            return Ok(profile);
        }

        // PUT: Posts/5        
        /// <summary>
        /// Edits the asynchronous.
        /// </summary>
        /// <param name="profileId">The profile identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut("{profileId}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> EditAsync(int profileId, [FromBody] ProfileViewModel model)
        {
            if (CurrentUser == null) return BadRequest(new { ErrorMessage = "Unauthorized" });

            var profile = await _profileService.GetProfile(profileId);
            if (!profile.Profile.UserId.Equals(CurrentUser.Id)) return BadRequest(new { ErrorMessage = "You are not allowed to edit profile." });

            profile.Profile.User.Email = model.Email;
            profile.Profile.User.FirstName = model.FirstName;
            profile.Profile.User.LastName = model.LastName;
            profile.Profile.User.PhoneNumber = model.PhoneNumber;
            profile.Profile.About = model.About;
            _profileService.Update(profile.Profile);

            var editedProfile = await _profileService.GetProfile(profileId);

            return Ok(editedProfile);
        }
    }
}