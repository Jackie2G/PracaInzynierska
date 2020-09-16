using System;
using System.Collections.Generic;
using System.Diagnostics;
//using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracaInzynierska.Data;
using PracaInzynierska.Models;

namespace PracaInzynierska.Controllers
{

    public class Hold
    {
        public DateTime date;
    }


    public class TrainingController : Controller
    {
        private readonly ApplicationContext _db;
        [BindProperty]
        public TrainingHistory training { get; set; }
        [BindProperty]
        public Exercises exercise { get; set; }
        [BindProperty]
        public DateTime trainingDay { get; set; } 
        [BindProperty]
        public string test1 { get; set; }
        [BindProperty]
        public Hold hold { get; set; }

        public TrainingController(ApplicationContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UpsertExercise(int? id)
        {
            exercise = new Exercises();
            if (id == null)
            {
                return View(exercise);
            }
            exercise = _db.ExercisesDb.FirstOrDefault(u => u.ID == id);
            if (exercise == null)
            {
                return NotFound();
            }
            return View(exercise);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult SetData(string data)
        {
            //Debug.WriteLine(test1);
            //Debug.WriteLine(trainingDay);
            //test1 = Request.Query["test"];
            //var test = form["test"];
            //string testtt = HttpContext.Request.Form["test"];
            //return View();
            //return Json(new { data = test1 });
            //return "dane" + data;
            trainingDay = Convert.ToDateTime(data.Replace("/", ".") + " 00:00:00");
            test1 = data;
            hold = new Hold();
            hold.date = Convert.ToDateTime(data.Replace("/", ".") + " 00:00:00");
            training.Date = Convert.ToDateTime(data.Replace("/", ".") + " 00:00:00");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertExercise()
        {
            if (ModelState.IsValid)
            {
                if (exercise.ID == 0)
                {
                    training.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    Debug.WriteLine(training.Date.ToString());
                    //training.Date = SetData(data);
                    if (trainingDay == Convert.ToDateTime("01.01.0001 00:00:00"))
                    {
                        training.Date = DateTime.Now;
                    }
                    else
                    {
                        training.Date = trainingDay;
                    }
                    //training.Date = DateTime.Now;
                    training.ExercisesID = exercise.ID;
                    exercise.trainingHistory = training;

                    //var test = Request.Form["datepicker"];
                    //Debug.Write(test1.ToString());

                    _db.ExercisesDb.Add(exercise);
                    _db.TrainingsDb.Add(training);
                    
                    _db.SaveChanges();
                }
                else
                {
                    _db.ExercisesDb.Update(exercise);
                    _db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(exercise);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Json(new { data = await _db.ExercisesDb.Where(x => x.trainingHistory.Id.Equals(user)).ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _db.ExercisesDb.FirstOrDefaultAsync(u => u.ID == id);
            if (record == null)
            {
                return Json(new { success = false, message = "Error while deleting record" });
            }
            _db.ExercisesDb.Remove(record);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Record deleted" });
        }

        //[HttpPost]
        //public async Task<IActionResult> SetData()
        //{

        //}
    }
}
