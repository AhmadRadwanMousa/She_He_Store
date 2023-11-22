using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using She_He_Store.Models;

namespace She_He_Store.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ModelContext _context;
        public ToDoController(ModelContext context)
        {
            _context = context;
        }
        public IActionResult GetToDos()
        {
            var toDos = _context.Todos.ToList();
            bool isEmpty = toDos.Count() == 0;
            ViewBag.toDoList = toDos;
            ViewBag.emptyToDo = isEmpty;
            return PartialView("_ToDoPartial",ViewBag);
        }

        public IActionResult AddToDo(string? ToDoDescription)
        {
            if (ToDoDescription == null) {
                return RedirectToAction("GetToDos", "ToDo");
            }
            Todo newToDo = new Todo
            {
                Tododescription = ToDoDescription,
            };
            _context.Todos.Add(newToDo);
            _context.SaveChanges();
            return RedirectToAction("GetToDos", "ToDo");
        }
        public IActionResult DeleteToDo(int? toDoId)
        {
            var DeletedToDo= _context.Todos.SingleOrDefault(t=>t.Todolistid==toDoId);
            _context.Todos.Remove(DeletedToDo);
            _context.SaveChanges();
            return RedirectToAction("GetToDos", "ToDo");
        }
        public IActionResult UpdateTodoState(int? todoId,bool isChecked )
        {
          var toDo = _context.Todos.SingleOrDefault(t => t.Todolistid == todoId);
            if (isChecked)
            {
                toDo.Todostatues = "Done";
                _context.Todos.Update(toDo);
                _context.SaveChanges();
            }
            else
            {
                toDo.Todostatues = null;
                _context.Todos.Update(toDo);
                _context.SaveChanges();
            }
        return RedirectToAction("GetToDos", "ToDo");

        }
    }
}
