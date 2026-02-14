// <copyright file="ProfileService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.EntityServices.EntityFrameworkServices;

using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Models;
using Data.Repository;
using Blog.Services.Core.Dtos.User;
using GeneralService;
using Interfaces;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Profile service.
/// </summary>
/// <seealso cref="GeneralService{Profile}" />
/// <seealso cref="IProfileService" />
/// <remarks>
/// Initializes a new instance of the <see cref="ProfileService"/> class.
/// </remarks>
/// <param name="repo">The repo.</param>
/// <param name="mapper">The mapper.</param>
public class ProfileService(
    IRepository<Data.Models.Profile> repo,
    IMapper mapper)
    : GeneralService<Data.Models.Profile>(repo), IProfileService
{
    /// <summary>
    /// The mapper.
    /// </summary>
    private readonly IMapper mapper = mapper;

    /// <inheritdoc cref="IProfileService"/>
    public async Task<ApplicationUserDto> GetProfile(int profileId)
    {
        var profile = await this.Table.Where(x => x.Id == profileId).Include(x => x.User).FirstOrDefaultAsync();
        var user = this.mapper.Map<ApplicationUser, ApplicationUserDto>(profile.User);

        return user;
    }
}