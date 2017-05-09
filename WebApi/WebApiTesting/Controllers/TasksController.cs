using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiTesting.DataConnection;
using WebApiTesting.Models;

namespace WebApiTesting.Controllers
{
    public class TasksController : ApiController
    {
        IRepository<Task> tasks = new GenericRepository<Task>();
        
        // GET api/<controller>
        public IEnumerable<Task> GetTasks()
        {
            return tasks.All();
        }

        // GET api/<controller>/5
        public IHttpActionResult GetTask(int id)
        {
            Task searchingTask = tasks.GetExactly((task) => task.Id == id);

            if (searchingTask != null)
            {
                return Ok(searchingTask);
            }
            return NotFound();
        }

        public IHttpActionResult GetLastTaskId()
        {
            if (tasks.All().Count > 0)
            {
                return Ok(tasks.All().Last().Id);
            }else
            {
                return NotFound();
            }
        }

        // POST api/<controller>
        public void CreateTask([FromBody]Task newTask)
        {
            tasks.Add(newTask);
            tasks.SaveChanges();
        }

        [HttpPut]
        // PUT api/<controller>/5
        public void UpdateTask([FromBody]Task editedTask)
        {
            tasks.Update(editedTask);
            tasks.SaveChanges();
        }
    }
}