using System;
using System.Collections.Generic;
using System.Diagnostics;
//using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        public IActionResult UpsertExercise()
        {
            if (ModelState.IsValid)
            {
                if (exercise.ID == 0)
                {
                    _db.ExercisesDb.Add(exercise);
                    //training.
                    //training.User = "test";
                    //training.
                    //_db.Trainings.Add("test", DateTime.Now, 1);
                    _db.SaveChanges();
                }
                else
                {
                    _db.ExercisesDb.Update(exercise);
                    _db.SaveChanges();
                }
                //_db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(exercise);
        }

        public IActionResult Upsert(string? id)
        {
            training = new TrainingHistory();
            if (id == null)
            {
                return View(training);
            }
            training = _db.TrainingsDb.FirstOrDefault(u => u.Id == id);
            if (training == null)
            {
                return NotFound();
            }
            return View(training);
        }

        //#region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Debug.WriteLine(_db.ExercisesDb.ToListAsync().ToString());
            var data =  Json(new { data = await _db.ExercisesDb.ToListAsync() });
            //Debug.WriteLine(data.Value.ToString());
            return data;
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
        //#endregion
    }
}
