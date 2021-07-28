

using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TaxLab
{
    public class ResourceFileLoader
    {
        private readonly Assembly _assembly;

        public ResourceFileLoader(Type relativeAssemblyType)
        {
            _assembly = Assembly.GetAssembly(relativeAssemblyType);
        }

        public T LoadJsonResource<T>(Func<string, bool> predicate)
        {
            var resourcePath = GetResourcePath(predicate);
            return LoadJsonResource<T>(resourcePath);
        }

        public Stream LoadStreamResource(Func<string, bool> predicate)
        {
            var resourcePath = GetResourcePath(predicate);
            return LoadStreamResource(resourcePath);
        }

        public T LoadJsonResource<T>(string resourcePath)
        {
            var json = LoadTextResource(resourcePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default;
            }
        }

        public string LoadTextResource(Func<string, bool> predicate)
        {
            var resourcePath = GetResourcePath(predicate);
            return LoadTextResource(resourcePath);
        }

        public string LoadTextResource(string resourcePath)
        {
            var resourceStream = _assembly.GetManifestResourceStream(resourcePath);
            if (resourceStream == null)
            {
                return string.Empty;
            }

            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                var text = reader.ReadToEnd();
                return text;
            }
        }
        public Stream LoadStreamResource(string resourcePath)
        {
            var resourceStream = _assembly.GetManifestResourceStream(resourcePath);
            return resourceStream;
        }

        private string GetResourcePath(Func<string, bool> predicate)
        {
            var resourceName = _assembly.GetManifestResourceNames();
            var resourcePath = resourceName.Where(predicate).FirstOrDefault();

            return string.IsNullOrWhiteSpace(resourcePath) ? string.Empty : resourcePath;
        }

        public string GetResourceFullPath(Func<string, bool> predicate)
        {
            var resourceName = _assembly.GetManifestResourceNames();
            var resourcePath = resourceName.Where(predicate).FirstOrDefault();

            return string.IsNullOrWhiteSpace(resourcePath) ? string.Empty : resourcePath;
        }
    }
}
