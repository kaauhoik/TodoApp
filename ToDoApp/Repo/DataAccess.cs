using System.Collections.Generic;
using System.Linq;
using ToDoApp.Models;
using System.Text.Json;
using System.IO;

namespace ToDoApp.Repo
{
    public class DataAccess : IDataAccess
    {
        private readonly string path = @"c:\temp\TodoItems.txt";

      
        //Lisätään 'kantaan' item
        public void AddItem(TodoItem item)
        {
            
            if (!File.Exists(path))
            {
                List<TodoItem> list = new List<TodoItem>();
                list.Add(item);

                string json = JsonSerializer.Serialize(list);
                File.WriteAllText(path, json);
            } else
            {
                string itemsJson = File.ReadAllText(path);
                List<TodoItem>? tempList = JsonSerializer.Deserialize<List<TodoItem>>(itemsJson);

                if (tempList != null && !tempList.Contains(item)) tempList.Add(item);

                itemsJson = JsonSerializer.Serialize(tempList);
                File.WriteAllText(path, itemsJson);
            }
        }

        //Poistetaan 'kannasta' item
        public void DeleteItem(TodoItem item)
        {
            if (File.Exists(path))
            {
                string itemsJson = File.ReadAllText(path);
                List<TodoItem>? tempList = JsonSerializer.Deserialize<List<TodoItem>>(itemsJson);

                if (tempList!= null && tempList.Any(x => x.Id == item.Id)) tempList.Remove(tempList.First(x=> x.Id == item.Id));

                itemsJson = JsonSerializer.Serialize(tempList);
                File.WriteAllText(path, itemsJson);

            }
        }
        // Haetaan kaikki itemit 
        public List<TodoItem> GetItems()
        {
            if (File.Exists(path))
            {
                string itemsJson = File.ReadAllText(path);

                List<TodoItem>? tempList =  JsonSerializer.Deserialize<List<TodoItem>>(itemsJson);
                if (tempList != null)
                {
                    return tempList;
                }
                else
                {
                    return new List<TodoItem> { };
                }
            }
            else
            {
                return new List<TodoItem>();
            }
        }

        //Tallennetaan muutokset (Kaikki edellä mainitut funktiot voitaisiin toteuttaa tällä, mutta annetaan niiden olla demonstraationa.)
        public void SaveChanges(List<TodoItem> list)
        {
            string itemsJson = JsonSerializer.Serialize(list);
            File.WriteAllText(path, itemsJson);
        }
    }
}
