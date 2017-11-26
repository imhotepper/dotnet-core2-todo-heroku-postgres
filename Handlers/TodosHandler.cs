using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Configuration;


#region TodoList handler

public class TodosListQuery : IRequest<List<Todo>>
{
}

public class TodosListHandler :  IRequestHandler<TodosListQuery, List<Todo>>
{
    private readonly DbCtx _db;

    public TodosListHandler(DbCtx db) => _db = db;

    public List<Todo> Handle(TodosListQuery request)
    {
        return _db.Todos.ToList();
    }
}

#endregion

#region Create TODO handler

public class CreateTodoCmd :  IRequest<Todo>
{
    public Todo Todo { get; set; }
}

public class CreateTodoHandler :  IRequestHandler<CreateTodoCmd, Todo>
{
    private readonly DbCtx _db;

    public CreateTodoHandler(DbCtx db) => _db = db;

    public Todo Handle(CreateTodoCmd todoCmd)
    {
        _db.Todos.Add(todoCmd.Todo);
        _db.SaveChanges();
        return todoCmd.Todo;
    }
}

#endregion

#region Delete TODO handler

public class DeleteTodoCmd : IRequest<Todo>
{
    public int Id { get; set; }
}

public class DeleteTodoHandler :  IRequestHandler<DeleteTodoCmd, Todo>
{
    private readonly DbCtx _db;

    public DeleteTodoHandler(DbCtx db) => _db = db;

    public Todo Handle(DeleteTodoCmd cmd)
    {
        var todo = _db.Todos.FirstOrDefault(x => x.Id == cmd.Id);
        if (todo == null) throw new ApplicationException($"There is no todo with id: {cmd.Id}");
        _db.Todos.Remove(todo);
        _db.SaveChanges();
        return todo;
    }
}
#endregion

#region Complete TODO handler

public class CompleteTodoCmd : IRequest<Todo>
{
    public int Id { get; set; }
}

public class CompleteTodoHabdler: IRequestHandler<CompleteTodoCmd, Todo>
{
    private readonly DbCtx _db;

    public CompleteTodoHabdler(DbCtx db) => _db = db;

    public Todo Handle(CompleteTodoCmd cmd)
    {
        var todo = _db.Todos.FirstOrDefault(x => x.Id == cmd.Id);
        todo.IsCompleted = true;
        _db.SaveChanges();
        return todo;
    }
}

#endregion 

