using Microsoft.Extensions.Logging;

namespace FM.Web.Logger
{
    public static class CustomLoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory,
            string filePath)
        {
            factory.AddProvider(new CustomLoggerProvider(filePath));
            return factory;
        }
    }
}