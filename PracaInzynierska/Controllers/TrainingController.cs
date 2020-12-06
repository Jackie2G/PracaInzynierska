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
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public SelectList exerciseList { get; set; }

        public TrainingController(ApplicationContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (user != null)
            {
                GetExercises();
                return View();
            }
            else
            {
                return Redirect("https://localhost:44396/Identity/Account/Register");
            }
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
                    var exerciseList = _db.ExercisesDb.Where(x => x.trainingHistory.Id.Equals(training.Id)).Where(x => x.Name.Equals(exercise.Name)).Where(x => x.Reps.Equals(exercise.Reps)).Where(x => x.Series.Equals(exercise.Series)).ToList();

                    if (exerciseList.Count().Equals(0))
                    {
                        exercise.CurrentPr = "PR";
                    }
                    else
                    {
                        var lastExercise = exerciseList.Where(x => x.Weight.Equals(exerciseList.Max(x => x.Weight))).FirstOrDefault();
                        double maxWeight = exerciseList.Max(x => x.Weight);

                        if (maxWeight > exercise.Weight && exercise.Done == true)
                        {
                            exercise.CurrentPr = maxWeight.ToString();
                        }
                        else
                        {
                            exercise.CurrentPr = "PR";
                            foreach (var ex in exerciseList)
                            {
                                ex.CurrentPr = exercise.Weight.ToString();
                                _db.ExercisesDb.Update(ex);
                            }
                        }
                    }

                    _db.ExercisesDb.Add(exercise);
                    _db.TrainingsDb.Add(training);
                    
                    _db.SaveChanges();
                }
                else
                {
                    var exerciseList = _db.ExercisesDb.Where(x => x.trainingHistory.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).Where(x => x.Name.Equals(exercise.Name)).Where(x => x.Reps.Equals(exercise.Reps)).Where(x => x.Series.Equals(exercise.Series)).ToList();

                    if (exerciseList.Count().Equals(0))
                    {
                        exercise.CurrentPr = "PR";
                    }
                    else
                    {
                        var lastExercise = exerciseList.Where(x => x.Weight.Equals(exerciseList.Max(x => x.Weight))).FirstOrDefault();
                        double maxWeight = exerciseList.Max(x => x.Weight);

                        if (maxWeight > exercise.Weight && exercise.Done == true)
                        {
                            exercise.CurrentPr = maxWeight.ToString();
                        }
                        else
                        {
                            exercise.CurrentPr = "PR";
                            foreach (var ex in exerciseList)
                            {
                                ex.CurrentPr = exercise.Weight.ToString();
                                _db.ExercisesDb.Update(ex);
                            }
                        }
                    }
                    _db.SaveChanges();

                    foreach (var entry in exerciseList)
                        _db.Entry<Exercises>(entry).State = EntityState.Detached;

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

            var finalList = list.Where(x => x.trainingHistory.Id.Equals(user)).ToList().OrderByDescending(x => x.trainingHistory.Date);

            return Json(new { data = finalList });
        }

        [HttpGet]
        [Route("Training/GetAllDay/{trainingDay}")]
        public async Task<IActionResult> GetAllDay(DateTime trainingDay)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _db.ExercisesDb.Where(x => x.trainingHistory.Id.Equals(user)).Include(i => i.trainingHistory).ToListAsync();

            var finalList = list.Where(x => x.trainingHistory.Date.Equals(trainingDay)).ToList();

            return Json(new { data = finalList });
        }

        [HttpGet]
        [Route("Training/GetExercises/{exerciseName}")]
        public async Task<IActionResult> GetExercises(string exerciseName)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _db.ExercisesDb.Where(x => x.Name.Equals(exerciseName) && x.trainingHistory.Id.Equals(user)).Include(x => x.trainingHistory).ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        [Route("Training/GetDataBetween/{dateFrom}/{dateTo}")]
        public async Task<IActionResult> GetDataBetween(DateTime dateFrom, DateTime dateTo)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _db.ExercisesDb.Where(x => x.trainingHistory.Id.Equals(user) && x.trainingHistory.Date >= dateFrom && x.trainingHistory.Date <= dateTo).Include(x => x.trainingHistory).OrderByDescending(x => x.trainingHistory.Date).ToListAsync();

            return Json(new { data = list });
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

        public void GetExercises(object selectedData = null)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var exerciseQuery = (from d in _db.ExercisesDb
                                 where d.trainingHistory.Id.Equals(user)
                                 orderby d.Name
                                 select d.Name).Distinct();


            exerciseList = new SelectList(exerciseQuery);

            ViewBag.listOfExercises = exerciseList;
        }
    }
}
