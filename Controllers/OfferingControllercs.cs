using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectGoogleSpannerPOC.Models;
using Google.Cloud.Spanner.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ConnectGoogleSpannerPOC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OfferingController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        //TODO: Set your project id here
        private readonly string _myProject = "august-strata-270720";
        private readonly string _myDatabase= "school";
        private readonly string _spannerInstanceId = "school";
        private readonly ILogger<OfferingController> _logger;

        public OfferingController(ILogger<OfferingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = new List<Offering>();

            using (var connection =
               new SpannerConnection(
                   $"Data Source=projects/{_myProject}/instances/{_spannerInstanceId}/databases/{_myDatabase}"))
            {
                var selectCmd = connection.CreateSelectCommand("SELECT * FROM Offerings");
                using (var reader = await selectCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        result.Add(new Offering
                        {
                            OfferingId = reader.GetFieldValue<string>("OfferingId"),
                            OfferingName = reader.GetFieldValue<string>("OfferingName"),
                            CourseDurationInYears = reader.GetFieldValue<int>("CourseDurationInYears"),
                            TotalCourseFee = reader.GetFieldValue<int>("TotalCourseFee")
                        });
                }
            }

            return Ok(result);
        }

        [HttpGet("GetOfferingById/{OfferingId}")]
        [Route("{OfferingId}", Name = "GetOfferingById")]
        public async Task<IActionResult> Get(string OfferingId)
        {
            using (var connection =
                new SpannerConnection(
                    $"Data Source=projects/{_myProject}/instances/{_spannerInstanceId}/databases/{_myDatabase}"))
            {
                var selectCommand = connection.CreateSelectCommand($"SELECT * FROM Offerings WHERE OfferingId='{OfferingId}'");
                using (var reader = await selectCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        return Ok(new Offering
                        {
                            OfferingId = reader.GetFieldValue<string>("OfferingId"),
                            OfferingName = reader.GetFieldValue<string>("OfferingName"),
                            CourseDurationInYears = reader.GetFieldValue<int>("CourseDurationInYears"),
                            TotalCourseFee = reader.GetFieldValue<int>("TotalCourseFee")
                        });
                }
            }

            return NotFound();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Offering item)
        {
            // Insert a new item.
            using (var connection =
                new SpannerConnection(
                    $"Data Source=projects/{_myProject}/instances/{_spannerInstanceId}/databases/{_myDatabase}"))
            {
                await connection.OpenAsync();

                item.OfferingId = Guid.NewGuid().ToString("N");
                var cmd = connection.CreateInsertCommand(
                    "Offerings", new SpannerParameterCollection
                    {
                        {"OfferingId", SpannerDbType.String, item.OfferingId},
                        {"OfferingName", SpannerDbType.String, item.OfferingName},
                        {"CourseDurationInYears", SpannerDbType.Int64, item.CourseDurationInYears},
                        {"TotalCourseFee", SpannerDbType.Int64, item.TotalCourseFee}
                    });

                await cmd.ExecuteNonQueryAsync();
            }

            return CreatedAtRoute("GetOfferingById", new { OfferingId = item.OfferingId }, item);
        }
    }
}