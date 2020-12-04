using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PracaInzynierska.Data;
using PracaInzynierska.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PracaInzynierska.Controllers
{
    public class PredictionController : Controller
    {
        private readonly ApplicationContext _db;
        public SelectList exerciseList { get; set; }

        public PredictionController(ApplicationContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            GetExercises();
            return View();
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

        [HttpGet]
        [Route("Prediction/{exerciseName}")]
        public async Task <IActionResult> GetExercise(string exerciseName)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _db.ExercisesDb.Include(x => x.trainingHistory).ToListAsync();

            var finalList = list.Where(x => x.trainingHistory.Id.Equals(user) && x.Name.Equals(exerciseName)).ToList();

            return Json(new { data = finalList });
        }
    }
}
