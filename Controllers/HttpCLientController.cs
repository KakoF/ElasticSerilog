using Infrastructure.Clients;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSerilog.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class HttpCLientController : ControllerBase
	{
		private readonly ILogger<LogginExampleController> _logger;
        private readonly AdviceClient _adviceClient;
        private readonly ChuckNorrisClient _chuckClient;

        public HttpCLientController(ILogger<LogginExampleController> logger, AdviceClient adviceClient, ChuckNorrisClient chuckClient)
        {
            _logger = logger;
            _adviceClient = adviceClient;
            _chuckClient = chuckClient;
        }


        [HttpGet("Advice")]
        public async Task<string> Advice()
        {
            return await _adviceClient.Get();
        }

        [HttpGet("ChuckNorris")]
		public async Task<string> ChuckNorris()
		{
            return await _chuckClient.Get();
        }
	}
}
