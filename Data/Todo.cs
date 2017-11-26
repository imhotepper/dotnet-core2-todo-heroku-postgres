using System;


public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedDT { get; set; }

    public Todo() => CreatedDT = DateTime.Now;
}