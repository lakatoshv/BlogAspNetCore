namespace Blog.FSharp.Core.Mapping

open System
open System.Reflection
open AutoMapper
open Blog.FSharp.Core.Mapping.Interfaces

/// AutoMapper Configuration
module AutoMapperConfig =

    /// Create standard mappings for TypesMap
    let private createMappings (configuration: IProfileExpression) (maps: TypesMap seq) =
        maps |> Seq.iter (fun map -> configuration.CreateMap(map.Source, map.Destination) |> ignore)

    /// Create custom mappings
    let private createCustomMappings (configuration: IMapperConfigurationExpression) (maps: ICustomMappings seq) =
        maps |> Seq.iter (fun map -> map.CreateMappings(configuration))

    /// Get "from" maps
    let private getFromMaps (types: Type list) : TypesMap seq =
        types
        |> Seq.filter (fun t -> not (t.GetTypeInfo().IsAbstract) && not (t.GetTypeInfo().IsInterface))
        |> Seq.collect (fun t ->
            t.GetTypeInfo().GetInterfaces()
            |> Seq.filter (fun i -> i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() = typedefof<IMapFrom<_>>)
            |> Seq.map (fun i -> 
                let map = TypesMap()
                map.Source <- i.GetGenericArguments().[0]
                map.Destination <- t
                map))

    /// Get "to" maps
    let private getToMaps (types: Type list) : TypesMap seq =
        types
        |> Seq.filter (fun t -> not (t.GetTypeInfo().IsAbstract) && not (t.GetTypeInfo().IsInterface))
        |> Seq.collect (fun t ->
            t.GetTypeInfo().GetInterfaces()
            |> Seq.filter (fun i -> i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() = typedefof<IMapTo<_>>)
            |> Seq.map (fun i ->
                let map = TypesMap()
                map.Source <- t
                map.Destination <- i.GetGenericArguments().[0]
                map))

    /// Get custom mappings
    let private getCustomMappings (types: Type list) : ICustomMappings seq =
        types
        |> Seq.filter (fun t -> not (t.GetTypeInfo().IsAbstract) && not (t.GetTypeInfo().IsInterface))
        |> Seq.filter (fun t -> typeof<ICustomMappings>.GetTypeInfo().IsAssignableFrom(t))
            |> Seq.map (fun t -> Activator.CreateInstance(t) :?> ICustomMappings)

    /// Register standard "from" mappings
    let private registerStandardFromMappings (configuration: IProfileExpression) (types: Type list) =
        let maps = getFromMaps types
        createMappings configuration maps

    /// Register standard "to" mappings
    let private registerStandardToMappings (configuration: IProfileExpression) (types: Type list) =
        let maps = getToMaps types
        createMappings configuration maps

    /// Register custom mappings
    let private registerCustomMaps (configuration: IMapperConfigurationExpression) (types: Type list) =
        let maps = getCustomMappings types
        createCustomMappings configuration maps

    /// Register mappings from assemblies
    let registerMappings ([<ParamArray>] assemblies: Assembly[]) =
        let types =
            assemblies
            |> Seq.collect (fun a -> a.GetExportedTypes())
            |> Seq.toList

        let config =
            MapperConfiguration(fun cfg ->
                registerStandardFromMappings cfg types
                registerStandardToMappings cfg types
                registerCustomMaps cfg types
            )

        config.CreateMapper() |> ignore
