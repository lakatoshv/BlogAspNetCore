namespace Blog.FSharp.Core.Infrastructure

open System
open System.Linq
open System.Linq.Expressions
open System.Reflection

[<AutoOpen>]
module QueryableExtensions =

    /// <summary>
    /// Order By.
    /// </summary>
    let OrderBy<'TEntity when 'TEntity : not struct>
        (source: IQueryable<'TEntity>)
        (orderByProperty: string)
        (desc: bool) : IOrderedQueryable<'TEntity> =

        let command = if desc then "OrderByDescending" else "OrderBy"
        let t = typeof<'TEntity>
        let property = 
            t.GetProperty(orderByProperty,
                BindingFlags.Public ||| BindingFlags.Static ||| BindingFlags.Instance ||| BindingFlags.IgnoreCase)

        let parameter = Expression.Parameter(t, "p")
        let propertyAccess = 
            Expression.MakeMemberAccess(parameter, property |> Option.ofObj |> Option.defaultWith (fun () -> raise (InvalidOperationException())))
        let orderByExpression = Expression.Lambda(propertyAccess, parameter)
        let resultExpression =
            Expression.Call(
                typeof<Queryable>,
                command,
                [| t; property.PropertyType |],
                source.Expression,
                Expression.Quote(orderByExpression)
            )

        source.Provider.CreateQuery<'TEntity>(resultExpression) :?> IOrderedQueryable<'TEntity>


    /// <summary>
    /// Then By.
    /// </summary>
    let ThenBy<'TEntity when 'TEntity : not struct>
        (source: IQueryable<'TEntity>)
        (orderByProperty: string)
        (desc: bool) : IOrderedQueryable<'TEntity> =

        let command = if desc then "ThenByDescending" else "ThenBy"
        let t = typeof<'TEntity>
        let property = 
            t.GetProperty(orderByProperty,
                BindingFlags.Public ||| BindingFlags.Static ||| BindingFlags.Instance ||| BindingFlags.IgnoreCase)

        let parameter = Expression.Parameter(t, "p")
        let propertyAccess = 
            Expression.MakeMemberAccess(parameter, property |> Option.ofObj |> Option.defaultWith (fun () -> raise (InvalidOperationException())))
        let orderByExpression = Expression.Lambda(propertyAccess, parameter)
        let resultExpression =
            Expression.Call(
                typeof<Queryable>,
                command,
                [| t; property.PropertyType |],
                source.Expression,
                Expression.Quote(orderByExpression)
            )

        source.Provider.CreateQuery<'TEntity>(resultExpression) :?> IOrderedQueryable<'TEntity>