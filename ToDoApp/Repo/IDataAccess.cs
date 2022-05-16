using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Repo
{
    internal interface IDataAccess
    {
        public List<TodoItem> GetItems();
        public void DeleteItem(TodoItem item);
        public void AddItem(TodoItem item);
        public void SaveChanges(List<TodoItem> list);
    }
}
