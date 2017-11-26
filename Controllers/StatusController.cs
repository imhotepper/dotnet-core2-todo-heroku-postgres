using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers
{
    [Route("api/status")]
    public class StatusController
    {
        private readonly DbCtx _db;

        public StatusController(DbCtx db) => _db = db;

        [Route("simple")]
        public object Simple()  => new {Api = "OK'"};
        

        [Route("complex")]
        public object Complex()
        {
            var dbOk = false;
            dynamic result;
            try
            {
               dbOk = _db.Todos.Any();
            }
            finally
            {
                result = new {Api = "OK", Database = dbOk ? "OK" : "NOK"};
            }

            return result;
        }
    }
}