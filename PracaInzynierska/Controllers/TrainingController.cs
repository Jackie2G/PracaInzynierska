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
    public class TrainingController : Controller
    {
        private readonly ApplicationContext _db;
        [BindProperty]
        public TrainingHistory training { get; set; }
        [BindProperty]
        public Exercises exercise { get; set; }
        //[BindProperty]
        //public DateTime trainingDay { get; set; } 

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
        [ValidateAntiForgeryToken]
        public IActionResult UpsertExercise(string data)
        {
            if (ModelState.IsValid)
            {
                if (exercise.ID == 0)
                {
                    training.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    
                    training.ExercisesID = exercise.ID;
                    training.Date = exercise.trainingHistory.Date;
                    exercise.trainingHistory = training;

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

            return Json(new { data = finalList });
        }

        //[HttpGet]
        //public async Task<IActionResult> SetData(string data)
        //{
        //    var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var time = data.Replace("/", ".");
    
        //    var list = await _db.ExercisesDb.Include(i => i.trainingHistory).ToListAsync();
        //    var finalList = list.Where(x => x.trainingHistory.Id.Equals(user)).Where(x => x.trainingHistory.Date.ToString().Contains(time)).ToList();

        //    var tescik = await _db.ExercisesDb.ToListAsync();

        //    return Json(new { data = tescik });
        //}

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
    }
}
