using Autofac;

namespace AssemblyFormatter
{
    public static class ContainerConfig
    {
        public static IComponentContext Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Application>()
                .As<IApplication>();

            builder.RegisterType<MipsFormatter>()
                .As<IMipsFormatter>();

            return builder.Build();
        }
    }
}