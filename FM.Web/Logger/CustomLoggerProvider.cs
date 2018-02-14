using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FM.Web.Logger
{
    public class CustomLoggerProvider:ILoggerProvider
    {
        private string path;

        public CustomLoggerProvider(string _path)
        {
            path = _path;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new CustomLogger(path);
        }

        public void Dispose()
        {
        }
    }
}