using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Net.Http;
using MediatR;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodosController
    {
        readonly IMediator _mediatr;
        public TodosController(IMediator mediatr) => _mediatr = mediatr;

        [HttpPost]
        public async Task<Todo> Post([FromBody] Todo todo) => await _mediatr.Send(new CreateTodoCmd {Todo = todo});

        [HttpGet]
        public async Task<IEnumerable<Todo>> Get() => await _mediatr.Send(new TodosListQuery());

        [HttpPost("{id}/complete")]
        public async Task<Todo> Complete(int id) => await _mediatr.Send(new CompleteTodoCmd {Id = id});

        [HttpDelete("{id}")]
        public async Task<Todo> Delete(int id) => await _mediatr.Send(new DeleteTodoCmd {Id = id});
    }
}