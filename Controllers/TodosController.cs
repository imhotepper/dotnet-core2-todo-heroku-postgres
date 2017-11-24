
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Net.Http;
using MediatR;

namespace app2.Controllers
{
    [Route("api/[controller]")]
    public class TodosController
    {
        DbCtx _db;
        IMediator _mediatr;
        public TodosController(DbCtx db, IMediator mediatr){
             _db = db;
             _mediatr = mediatr;
        }

        [HttpPost]
        public async Task<Todo> Post([FromBody] Todo todo)
        {
            return await _mediatr.Send(new CreateTodo{Todo = todo});
        }

        [HttpGet]
        public async Task<IEnumerable<Todo>> Get() =>await _mediatr.Send(new TodosList());

        [HttpPost("{id}/complete")]
        public Todo Complete(int id)
        {
            var todo = _db.Todos.FirstOrDefault(x => x.Id == id);
            todo.IsCompleted = true;
            _db.SaveChanges();
            return todo;
        }

        [HttpDelete("{id}")]
        public Todo Delete(int id)
        {
            var todo = GetTodoById(id);
            _db.Todos.Remove(todo);
            _db.SaveChanges();
            return todo;
        }

        private Todo GetTodoById(int id) => _db.Todos.FirstOrDefault(x => x.Id == id);
    }
}