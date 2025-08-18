namespace Blog.FSharp.Core.Mapping

open System
open System.Linq
open System.Linq.Expressions
open AutoMapper
open AutoMapper.QueryableExtensions

[<AutoOpen>]
module QueryableMappingExtensions =

    type IQueryable with
        member this.To<'TDestination>([<ParamArray>] membersToExpand: Expression<Func<'TDestination, obj>>[]) =
            if isNull this then
                nullArg "source"
            this.ProjectTo(null, membersToExpand)

        member this.To<'TDestination>(parameters: obj) =
            if isNull this then
                nullArg "source"
            this.ProjectTo<'TDestination>(parameters :?> IConfigurationProvider)