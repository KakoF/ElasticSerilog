using Microsoft.AspNetCore.Mvc;

namespace ElasticSerilog.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LogginExampleController : ControllerBase
	{
		private readonly ILogger<LogginExampleController> _logger;

		public LogginExampleController(ILogger<LogginExampleController> logger)
		{
			_logger = logger;
		}

        [HttpGet("Parameters")]
        public string Parameters()
        {
            var varivel1 = "Valor aleatório para variável";
            var outro = "Outro exemplo";
            _logger.LogInformation("Exemplo de log de Information como variaveis {varivel1} - {outro}", varivel1, outro);
            return "Parameters";
        }

        [HttpGet("Information")]
		public string Information()
		{
			_logger.LogInformation("Exemplo de log de Information");
			return "Information";
		}

        [HttpGet("Warning")]
        public string Warning()
        {
            _logger.LogWarning("Exemplo de log de Warning");
            return "Warning";
        }

        [HttpGet("Error")]
        public string Error()
        {
			try
			{
                throw new Exception("Custom message Exception");
            }
			catch (Exception ex)
			{
                _logger.LogError(ex, "Exemplo de log de Exception");
                return "Error";
            }
           
        }
	}
}
