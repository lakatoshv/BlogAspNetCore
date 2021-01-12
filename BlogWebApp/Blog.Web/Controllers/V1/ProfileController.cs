using Blog.Services.ControllerContext;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.Web.Contracts.V1;
using Blog.Web.VIewModels.AspNetUser;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Web.Controllers.V1
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="BaseController" />
    [Route(ApiRoutes.ProfileController.Profile)]
    [ApiController]
    public class ProfileController : BaseController
    {
        /// <summary>
        /// The profile service.
        /// </summary>
        private readonly IProfileService _profileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileController"/> class.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="profileService">The profile service.</param>
        public ProfileController(
            IControllerContext controllerContext,
            IProfileService profileService) 
            : base(controllerContext)
        {
            _profileService = profileService;
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
        public async Task<ActionResult> Show([FromRoute] int id)
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
        public async Task<IActionResult> EditAsync([FromRoute] int profileId, [FromBody] ProfileViewModel model)
        {
            if (CurrentUser == null)
            {
                return BadRequest(new {ErrorMessage = "Unauthorized"});
            }

            var profile = await _profileService.GetProfile(profileId);
            if (!profile.Profile.UserId.Equals(CurrentUser.Id))
            {
                return BadRequest(new {ErrorMessage = "You are not allowed to edit profile."});
            }

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