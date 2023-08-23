// <copyright file="ProfileService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services;

using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using Data.Repository;
using Core.Dtos.User;
using GeneralService;
using Interfaces;

/// <summary>
/// Profile service.
/// </summary>
/// <seealso cref="GeneralService{Profile}" />
/// <seealso cref="IProfileService" />
public class ProfileService : GeneralService<Data.Models.Profile>, IProfileService
{
    /// <summary>
    /// The mapper.
    /// </summary>
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileService"/> class.
    /// </summary>
    /// <param name="repo">The repo.</param>
    /// <param name="mapper">The mapper.</param>
    public ProfileService(
        IRepository<Data.Models.Profile> repo,
        IMapper mapper)
        : base(repo)
    {
        this.mapper = mapper;
    }

    /// <inheritdoc cref="IProfileService"/>
    public async Task<ApplicationUserDto> GetProfile(int profileId)
    {
        var profile = await this.Table.Where(x => x.Id == profileId).Include(x => x.User).FirstOrDefaultAsync();
        var user = this.mapper.Map<ApplicationUser, ApplicationUserDto>(profile.User);
        return user;
    }
}