namespace AuthService.Core.Mapping.Interfaces
{
    using AutoMapper;

    public interface ICustomMappings
    {
        void CreateMappings(IMapperConfigurationExpression configuration);
    }
}
