using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DailyProduction.Model;
using Azure.Data.Tables;
using Azure;

namespace IbasAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DailyProductionController : ControllerBase
    {
        private static TableClient tableClient;
        private List<DailyProductionDTO> _productionRepo;
        private readonly ILogger<DailyProductionController> _logger;



        public DailyProductionController(ILogger<DailyProductionController> logger)
        {
            _logger = logger;
            var serviceUri = "https://ibastestgroupto.table.core.windows.net/IBASProduktion2020";
            var tableName = "IBASProduktion2020";
            var accountName = "ibastestgroupto";
            var storageAccountKey = "B+KJEM0Mj+0mYhoQdGhpvE3OtcJ03Cz8q9liPnfHLjEJf/jXEc0tWoCVdpF4RN8ylrOPGI4tHMFm+AStNmI9wA==";



            tableClient = new TableClient(
             new Uri(serviceUri),
             tableName,
             new TableSharedKeyCredential(accountName, storageAccountKey));
        }
        [HttpGet]
        public IEnumerable<DailyProductionDTO> Get()
        {
            var _productionrepo = new List<DailyProductionDTO>();
            Pageable<TableEntity> entities = tableClient.Query<TableEntity>();
            foreach (TableEntity entity in entities)
            {
                var dto = new DailyProductionDTO
                {
                    Date = DateTime.Parse(entity.RowKey),
                    Model = (BikeModel)Enum.ToObject(typeof(BikeModel), Int32.Parse(entity.PartitionKey)),
                    ItemsProduced = (int)entity.GetInt32("itemsProduced")
                };
                _productionrepo.Add(dto);
            }
            return _productionrepo;
        }
    }
}

