using System;
using System.IO;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionApp3
{
    public class Function
    {
        private readonly ILogger<Function> _logger;
        private readonly AppDbContext _appDbContext;

        public Function(ILogger<Function> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        [Function(nameof(Function))]
        public void Run([BlobTrigger("samples-workitems/{name}", Connection = "AzureWebJobsStorage")] Stream stream, string name)
        {
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {stream.Length} Bytes");
            _appDbContext.FileRecords.Add(new FileRecords
            {
                FileName = name,
                IsCompleted = true
            });
            _appDbContext.SaveChanges();
        }
    }
}
