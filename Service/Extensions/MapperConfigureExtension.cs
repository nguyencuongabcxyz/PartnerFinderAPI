using AutoMapper;

namespace Service.Extensions
{
    public static class MapperConfigureExtension
    {
        public static TDestination Map<TSource, TDestination>(this TDestination destination, TSource source, IMapper mapper)
        {
            return mapper.Map(source, destination);
        }
    }
}
