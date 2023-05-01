using AutoMapper;

namespace OrderSystemOCS.Database.Mappings
{
    internal static class App
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.ShouldUseConstructor = ci => ci.IsPrivate;
                cfg.AddProfile<AppProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        internal static IMapper Mapper => Lazy.Value;
    }
}
