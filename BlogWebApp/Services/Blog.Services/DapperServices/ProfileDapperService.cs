// <copyright file="ProfileDapperService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Data.Repository;
using Blog.EntityServices.DapperServices.Interfaces;
using Blog.EntityServices.GeneralService;
using Blog.EntityServices.Interfaces;
using System.Data;
using System.Threading.Tasks;
using Blog.Services.Core.Dtos.User;
using Dapper;
using ProfileModel = Blog.Data.Models.Profile;

namespace Blog.EntityServices.DapperServices;

/// <summary>
/// Profile dapper service.
/// </summary>
/// <seealso cref="GeneralDapperService{Profile}" />
/// <seealso cref="IProfileService" />
/// <remarks>
/// Initializes a new instance of the <see cref="ProfileDapperService"/> class.
/// </remarks>
/// <param name="profileRepository">The profile repository.</param>
/// <param name="connection">The connection.</param>
public class ProfileDapperService(
    IDapperRepository<ProfileModel> profileRepository,
    IDbConnection connection)
    : GeneralDapperService<ProfileModel>(profileRepository), IProfileDapperService
{
    /// <inheritdoc cref="ICommentsDapperService"/>
    public async Task<ApplicationUserDto> GetProfile(int profileId)
    {
        const string sql = """
                                   SELECT u.Id,
                                          u.FirstName,
                                          u.LastName,
                                          u.Email,
                                          u.UserName
                                   FROM Profiles p
                                   INNER JOIN AspNetUsers u ON u.Id = p.UserId
                                   WHERE p.Id = @ProfileId
                           """;

        return await connection.QueryFirstOrDefaultAsync<ApplicationUserDto>(
            sql,
            new { ProfileId = profileId });
    }
}