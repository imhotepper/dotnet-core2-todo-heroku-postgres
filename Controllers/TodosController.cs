
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Net.Http;

namespace app2.Controllers
{
    [Route("api/[controller]")]
    public class TodosController
    {
        DbCtx _db;
        public TodosController(DbCtx db) => _db = db;

        [HttpPost]
        public Todo Post([FromBody] Todo todo){
                _db.Todos.Add(todo);
                _db.SaveChanges();
                return  todo;
        }

        [HttpGet]
        public IEnumerable<Todo> Get() => _db.Todos.ToList();

        [HttpPost("{id}/complete")]
        public Todo Complete(int id){
            var todo = _db.Todos.FirstOrDefault(x=>x.Id == id);
            todo.IsCompleted = true;
            _db.SaveChanges();
            return todo;
        }

        [HttpDelete("{id}")]
        public Todo Delete(int id){
            var todo = GetById(id);
            _db.Todos.Remove(todo);
            _db.SaveChanges();
            return todo;
        }


        private Todo GetById(int id) => _db.Todos.FirstOrDefault(x => x.Id == id);
    }
}