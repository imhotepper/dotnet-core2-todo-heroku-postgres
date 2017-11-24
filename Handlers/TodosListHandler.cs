


using System.Collections.Generic;
using System.Linq;
using MediatR;


public class TodosList : IRequest<List<Todo>> { }

public class TodosListHandler : IRequestHandler<TodosList, List<Todo>> {
    DbCtx _db;
    public TodosListHandler(DbCtx db) => _db = db;

    public List<Todo> Handle(TodosList request) {
        return _db.Todos.ToList();
    }
}

