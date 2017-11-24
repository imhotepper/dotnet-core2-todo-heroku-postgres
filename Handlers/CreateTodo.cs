

using System.Collections.Generic;
using System.Linq;
using MediatR;
public class CreateTodo: IRequest<Todo>{
    public Todo Todo { get; set; }
}

public class CreateTodoHandler : IRequestHandler<CreateTodo, Todo>
{
    DbCtx _db;
    public CreateTodoHandler(DbCtx db) => _db = db;

    Todo IRequestHandler<CreateTodo, Todo>.Handle(CreateTodo createTodo)
    {
        _db.Todos.Add(createTodo.Todo);
        _db.SaveChanges();
        return createTodo.Todo;    
    }
}