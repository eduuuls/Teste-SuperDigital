using Microsoft.Extensions.Configuration;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SuperDigital.ContaCorrente.Infra.Data.Context
{
    public sealed class ContaCorrenteContext<T>
    {
        private FileInfo _fileInfo;
        private List<T> _entidades;
        public List<T> Entidades
        {
            get { return _entidades; }
            set { _entidades = value; }
        }

        public ContaCorrenteContext()
        {
            Initialize();
        }

        public void SaveChanges()
        {
            var file = _fileInfo.OpenWrite();

            using (StreamWriter writer = new StreamWriter(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(writer, _entidades);
            }
        }

        private void Initialize()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: true)
                .Build();
            
            _fileInfo = new FileInfo(config.GetValue<string>("JsonDataBasePath"));

            var file = _fileInfo.Exists? _fileInfo.OpenRead(): _fileInfo.Create();

            using (TextReader reader = new StreamReader(file))
            {
                JsonSerializer serializer = new JsonSerializer();

                JsonTextReader jsonReader = new JsonTextReader(reader);
                //serialize object directly into file stream
                _entidades = serializer.Deserialize<List<T>>(jsonReader);
            }
        }
    }
}
