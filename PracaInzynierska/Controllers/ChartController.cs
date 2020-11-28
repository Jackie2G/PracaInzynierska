using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PracaInzynierska.Data;
using PracaInzynierska.Models;

namespace PracaInzynierska.Controllers
{
    public class ChartController : Controller
    {
        private readonly ApplicationContext _db;
        [BindProperty]
        public TrainingHistory training { get; set; }
        [BindProperty]
        public Exercises exercise { get; set; }
        public SelectList trainingDate { get; set; }
        public SelectList exerciseList { get; set; }

        public ChartController(ApplicationContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            GetExercises();
            TrainingDateDropDownList();
            return View();
        }

        public void TrainingDateDropDownList(object selectedData = null)
        {
            var trainingQuery = (from d in _db.TrainingsDb
                                orderby d.Date
                                select d.Date).Distinct();

            trainingDate = new SelectList(trainingQuery);
                                          
            ViewBag.listOfDates = trainingDate.Distinct();
        }

        [HttpGet]
        public async Task<IActionResult> Test(string name, object selectedData = null)
        {
            var trainingQuery = from d in _db.TrainingsDb
                                orderby d.Date
                                select d;

            trainingDate = new SelectList(trainingQuery.AsNoTracking(),
                                          "Id", "Date", selectedData);

            var traininglist = await trainingQuery.ToListAsync();

            ViewBag.listOfDates = trainingDate.Distinct();

            return Ok(traininglist);
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
