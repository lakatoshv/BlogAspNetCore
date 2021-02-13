using Blog.Services.ControllerContext;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Requests.UsersRequests;
using Blog.Contracts.V1.Responses.UsersResponses;
using Blog.Core.Consts;
using Blog.Web.Cache;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Web.Controllers.V1
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="BaseController" />
    [Route(ApiRoutes.ProfileController.Profile)]
    [ApiController]
    [Produces(Consts.JsonType)]
    public class ProfileController : BaseController
    {
        /// <summary>
        /// The profile service.
        /// </summary>
        private readonly IProfileService _profileService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileController"/> class.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="profileService">The profile service.</param>
        /// <param name="mapper"></param>
        public ProfileController(
            IControllerContext controllerContext,
            IProfileService profileService,
            IMapper mapper) 
            : base(controllerContext)
        {
            _profileService = profileService;
            _mapper = mapper;
        }

        // GET: Profile/5        
        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the user profile.</response>
        /// <response code="404">Unable to gets the user profile.</response>
        [ProducesResponseType(typeof(ApplicationUserResponse), 200)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        [Cached(600)]
        public async Task<ActionResult> Show([FromRoute] int id)
        {
            var profile = await _profileService.GetProfile(id);
            if (profile == null)
            {
                return NotFound();
            }

            profile.Profile.User = null;

            return Ok(_mapper.Map<ApplicationUserResponse>(profile));
        }

        // PUT: Posts/5        
        /// <summary>
        /// Edits the asynchronous.
        /// </summary>
        /// <param name="profileId">The profile identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns>Task</returns>
        /// <response code="204">Edits the user profile.</response>
        /// <response code="400">Unable to edits the user profile, model is invalid.</response>
        /// <response code="404">Unable to edits the user profile, profile not found.</response>
        [HttpPut("{profileId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApplicationUserResponse), 204)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> EditAsync([FromRoute] int profileId, [FromBody] UpdateProfileRequest model)
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

            return Ok(_mapper.Map<ApplicationUserResponse>(editedProfile));
        }
    }
}