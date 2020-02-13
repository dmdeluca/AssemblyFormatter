using Autofac;

namespace AssemblyFormatter
{
    public class Program
    {
        private static void Main(string[] args) => ContainerConfig
            .Configure()
            .Resolve<IApplication>()
            .Run(args);
    }
}