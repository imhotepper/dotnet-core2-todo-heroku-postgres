using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
       // [HttpGet("environment")]
        public string GetEnvironment()
        {
            var enumerator = Environment.GetEnvironmentVariables().GetEnumerator();
            var sb = new StringBuilder();

            while (enumerator.MoveNext())
            {
                sb.AppendLine($"{enumerator.Key} - {enumerator.Value}");
            }
            return sb.ToString();
       }
       
    }
}
