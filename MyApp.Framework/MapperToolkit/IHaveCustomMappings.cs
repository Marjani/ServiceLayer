using AutoMapper;

namespace MyApp.Framework.MapperToolkit
{
    public interface IHaveCustomMappings
    {
        void CreateMappings(IMapperConfigurationExpression configuration);
    }
}