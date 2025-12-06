// <copyright file="CommentsService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.EntityServices;

using System.Linq;
using System.Threading.Tasks;
using Contracts.V1.Responses.Chart;
using Core.Helpers;
using Data.Models;
using Data.Repository;
using Data.Specifications;
using Services.Core.Dtos;
using Blog.Services.Core.Dtos.Posts;
using GeneralService;
using Interfaces;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Comments service.
/// </summary>
/// <seealso cref="GeneralService{Comment}" />
/// <seealso cref="ICommentsService" />
/// <remarks>
/// Initializes a new instance of the <see cref="CommentsService"/> class.
/// </remarks>
/// <param name="repo">The repo.</param>
public class CommentsService(IRepository<Comment> repo)
    : GeneralService<Comment>(repo), ICommentsService
{
    /// <inheritdoc cref="ICommentsService"/>
    public async Task<CommentsViewDto> GetPagedComments(SortParametersDto sortParameters)
    {
        var comments = await this.Repository.TableNoTracking
            .Include(x => x.User)
            .Select(x => new Comment
            {
                Id = x.Id,
                CommentBody = x.CommentBody,
                CreatedAt = x.CreatedAt,
                Dislikes = x.Dislikes,
                Email = x.Email,
                Name = x.Name,
                Likes = x.Likes,
                PostId = x.PostId,
                UserId = x.UserId,
                User = x.User == null
                    ? new ApplicationUser()
                    : new ApplicationUser
                    {
                        Id = x.User.Id,
                        Email = x.User.Email,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        PhoneNumber = x.User.PhoneNumber,
                    },
            }).ToListAsync();

        var commentsViewModel = new CommentsViewDto
        {
            Comments = comments,
        };

        if (sortParameters.CurrentPage == null || sortParameters.PageSize == null)
        {
            return commentsViewModel;
        }

        commentsViewModel.Comments = commentsViewModel.Comments
            .Skip((sortParameters.CurrentPage.Value - 1) * sortParameters.PageSize.Value)
            .Take(sortParameters.PageSize.Value).ToList();

        commentsViewModel.PageInfo = new PageInfo
        {
            PageNumber = sortParameters.CurrentPage.Value,
            PageSize = sortParameters.PageSize.Value,
            TotalItems = comments.Count,
        };

        return commentsViewModel;
    }

    /// <inheritdoc cref="ICommentsService"/>
    public async Task<CommentsViewDto> GetPagedCommentsByPostId(int postId, SortParametersDto sortParameters)
    {
        var comments = await this.Repository.TableNoTracking
            .Where(new CommentSpecification(comment => comment.PostId.Equals(postId)).Filter)
            .Select(x => new Comment
            {
                Id = x.Id,
                CommentBody = x.CommentBody,
                CreatedAt = x.CreatedAt,
                Dislikes = x.Dislikes,
                Email = x.Email,
                Name = x.Name,
                Likes = x.Likes,
                PostId = x.PostId,
                UserId = x.UserId,
                User = x.User == null
                    ? new ApplicationUser()
                    : new ApplicationUser
                    {
                        Id = x.User.Id,
                        Email = x.User.Email,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        PhoneNumber = x.User.PhoneNumber,
                    },
            }).ToListAsync();

        var commentsViewModel = new CommentsViewDto
        {
            Comments = comments,
        };

        if (sortParameters.CurrentPage == null || sortParameters.PageSize == null)
        {
            return commentsViewModel;
        }

        commentsViewModel.Comments = commentsViewModel.Comments
            .Skip((sortParameters.CurrentPage.Value - 1) * sortParameters.PageSize.Value)
            .Take(sortParameters.PageSize.Value).ToList();

        commentsViewModel.PageInfo = new PageInfo
        {
            PageNumber = sortParameters.CurrentPage.Value,
            PageSize = sortParameters.PageSize.Value,
            TotalItems = comments.Count,
        };

        return commentsViewModel;
    }

    /// <inheritdoc cref="ICommentsService"/>
    public async Task<Comment> GetCommentAsync(int id)
        => await this.Repository.Table
            .Where(new CommentSpecification(x => x.Id.Equals(id)).Filter)
            .FirstOrDefaultAsync();

    /// <inheritdoc cref="ICommentsService"/>
    public async Task<ChartDataModel> GetCommentsActivity()
        => new ()
        {
            Name = "Comments",
            Series = await this.Repository.TableNoTracking
                .GroupBy(x => x.CreatedAt)
                .Select(x => new ChartItem
                {
                    Name = x.Key.ToString("dd/MM/yyyy"),
                    Value = x.Count(),
                })
                .ToListAsync(),
        };
}