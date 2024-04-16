using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FribergFastigheter.Server.Controllers.BrokerApi
{
	[Route("api/BrokerFirm")]
	[ApiController]
	public class BrokerFirmController : ControllerBase
	{
		// GET: api/<BrokerFirmController>
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/<BrokerFirmController>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<BrokerFirmController>
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/<BrokerFirmController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<BrokerFirmController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
