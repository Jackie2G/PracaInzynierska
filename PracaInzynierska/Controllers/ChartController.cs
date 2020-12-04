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
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (user != null)
            {
                GetExercises();
                TrainingDateDropDownList();
                return View();
            }
            else
            {
                return Redirect("https://localhost:44396/Identity/Account/Register");
            }
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

        [HttpGet]
        [Route("Chart/GetDataToChart/{exercise}/{dateFrom}/{dateTo}")]
        public  IActionResult GetDataToChart(string exercise, DateTime dateFrom, DateTime dateTo)
        {
            var test = Convert.ToDateTime(dateTo);
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = _db.ExercisesDb.Include(i => i.trainingHistory).ToList();
            var finalList = list.Where(x => x.trainingHistory.Id.Equals(user)).Where(x => x.Name.Equals(exercise)).Where(x => x.trainingHistory.Date >= dateFrom && x.trainingHistory.Date <= dateTo).OrderBy(x => x.trainingHistory.Date).ToList();

            return Json(finalList);
        }
    }
}
