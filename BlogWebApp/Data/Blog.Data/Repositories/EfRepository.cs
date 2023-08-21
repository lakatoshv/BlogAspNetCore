// <copyright file="EfRepository.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data.Repositories;

/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLog.Core;
using BLog.Core.Infrastructure.Pagination;
using BLog.Core.TableFilters;
using BLog.Data.Core.Repositories;
using Microsoft.EntityFrameworkCore;

public class EfRepository<TEntity> : IRepository<TEntity>
    where TEntity : IEntity
{
    public EfRepository(ApplicationDbContext context)
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
        this.DbSet = Context.Set<TEntity>();
    }

    protected DbSet<TEntity> DbSet { get; set; }

    protected ApplicationDbContext Context { get; set; }

    public IQueryable<TEntity> Table => throw new NotImplementedException();

    public IQueryable<TEntity> TableNoTracking => throw new NotImplementedException();

    public virtual IQueryable<TEntity> All() => this.DbSet;

    public virtual IQueryable<TEntity> AllAsNoTracking() => this.DbSet.AsNoTracking();

    public virtual Task<TEntity> GetByIdAsync(params object[] id) => this.DbSet.FindAsync(id);

    public virtual void Add(TEntity entity)
    {
        this.DbSet.Add(entity);
    }

    public virtual void Update(TEntity entity)
    {
        var entry = this.Context.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            this.DbSet.Attach(entity);
        }

        entry.State = EntityState.Modified;
    }

    public virtual void Delete(TEntity entity)
    {
        this.DbSet.Remove(entity);
    }

    public Task<int> SaveChangesAsync() => this.Context.SaveChangesAsync();

    public void Dispose() => this.Context.Dispose();

    public IQueryable<TEntity> GetAll()
    {
        throw new NotImplementedException();
    }

    public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public TEntity GetById(object id)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetByIdAsync(object id)
    {
        throw new NotImplementedException();
    }

    public Task<PagedListResult<TEntity>> SearchAsync(SearchQuery<TEntity> searchQuery)
    {
        throw new NotImplementedException();
    }

    public Task<PagedListResult<TEntity>> SearchBySquenceAsync(SearchQuery<TEntity> searchQuery, IQueryable<TEntity> sequence)
    {
        throw new NotImplementedException();
    }

    public SearchQuery<TEntity> GenerateQuery(TableFilter tableFilter, string includeProperties = null)
    {
        throw new NotImplementedException();
    }

    public void Insert(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void Insert(IEnumerable<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public void Update(IEnumerable<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public void Delete(IEnumerable<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public bool Any(Expression<Func<TEntity, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public TEntity LastOrDefault(Expression<Func<TEntity, bool>> expression)
    {
        throw new NotImplementedException();
    }
}*/