using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracaInzynierska.Data;
using PracaInzynierska.Models;

namespace PracaInzynierska.Controllers
{
    [Serializable]
    public class Hold
    {
        [TempData]
        public DateTime date { get; set; }
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
        [TempData]
        public string date { get; set; }
        //Hold hold = new Hold();

        public TrainingController(ApplicationContext db)
        {
            _db = db;
        }

        public IActionResult Index(Hold hold)
        {
            
            //this.HttpContext.Session.SetString("date", hold.date.ToString());
            return View(hold);
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

        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public IActionResult SetData(string data)
        //{
        //    //Debug.WriteLine(test1);
        //    //Debug.WriteLine(trainingDay);
        //    //test1 = Request.Query["test"];
        //    //var test = form["test"];
        //    //string testtt = HttpContext.Request.Form["test"];
        //    //return View();
        //    //return Json(new { data = test1 });
        //    //return "dane" + data;
        //    var time = Convert.ToDateTime(data.Replace("/", ".") + " 00:00:00");
        //    test1 = data;
        //    //hold = new Hold();
        //    //hold.date = Convert.ToDateTime(data.Replace("/", ".") + " 00:00:00");
        //    training.Date = Convert.ToDateTime(data.Replace("/", ".") + " 00:00:00");
        //    //this.HttpContext.Session.SetString("date", hold.date.ToString());
        //    TempData["date"] = data;

        //    return RedirectToAction("Index", new { hold = new Hold() { date = time } });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertExercise(string data)
        {
            //var test = Convert.ToDateTime(data.Replace("/", ".") + " 00:00:00");
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
            var list = await _db.ExercisesDb.Include(i => i.trainingHistory).ToListAsync();

            var finalList = list.Where(x => x.trainingHistory.Id.Equals(user)).ToList();

            List <Exercises> lista = new List<Exercises>(finalList);

            lista.ForEach(x => x.trainingHistory.Date.ToString("dd/MM//yyyy"));
            //return Json(new { data = await _db.ExercisesDb.Where(x => x.trainingHistory.Id.Equals(user)).ToListAsync() });
            return Json(new { data = finalList });
        }

        [HttpGet]
        public async Task<IActionResult> SetData(string data)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var time = data.Replace("/", ".");
    
            var list = await _db.ExercisesDb.Include(i => i.trainingHistory).ToListAsync();
            var finalList = list.Where(x => x.trainingHistory.Id.Equals(user)).Where(x => x.trainingHistory.Date.ToString().Contains(time)).ToList();

            var tescik = await _db.ExercisesDb.ToListAsync();

            return Json(new { data = tescik });
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
        //    date = "test1";
        //    return Json(new { data = date });
        //}
    }
}
