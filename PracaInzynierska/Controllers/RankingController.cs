using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracaInzynierska.Data;
using PracaInzynierska.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracaInzynierska.Controllers
{
    public class RankingController : Controller
    {
        private readonly ApplicationContext _db;
        [BindProperty]
        public TrainingHistory training { get; set; }
        [BindProperty]
        public Exercises exercise { get; set; }

        public RankingController(ApplicationContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Ranking/GetUsersBestExercises/{exercise}/{gender}/{weightCategory}")]
        public IActionResult GetUsersBestExercises(string exercise, string gender, string weightCategory)
        {
            var list = _db.ExercisesDb.Where(x => x.Name.Equals(exercise)).
                Join(_db.Users, x => x.trainingHistory.Id, y => y.Id, (x, y) => new
                {
                    Nick = y.nickName,
                    Gender = y.Gender,
                    WeightCategory = y.WeightCategory,
                    ExerciseName = x.Name,
                    Series = x.Series,
                    Reps = x.Reps,
                    Weight = x.Weight
                }).ToList();

            return Json(new { data = list });
        }
    }
}
