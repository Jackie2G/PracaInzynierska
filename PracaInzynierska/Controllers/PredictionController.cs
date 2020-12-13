using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PracaInzynierska.Areas.Identity.Data;
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
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (user != null)
            {
                GetExercises();
                return View();
            }
            else
            {
                return Redirect("https://trainingplan.azurewebsites.net/Identity/Account/Register");
            }
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
        [Route("Prediction/{exerciseName}/{expectedDate}")]
        public async Task <IActionResult> GetExercise(string exerciseName, DateTime expectedDate)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _db.ExercisesDb.Include(x => x.trainingHistory).ToListAsync();

            var finalList = list.Where(x => x.trainingHistory.Id.Equals(user) && x.Name.Equals(exerciseName)).ToList().OrderBy(x => x.trainingHistory.Date);

            string weights = string.Join(" ", finalList.Select(x => x.Weight.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)).ToList());
            string dates = string.Join(";", finalList.Select(x => x.trainingHistory.Date.ToString()).ToList());

            var py = new PythonScript();
            var test = expectedDate.ToString();
            var result = py.RunFromFile(weights, dates, expectedDate.ToString());

            var newResult = result.Replace("\r\n", string.Empty).Replace(".", ",");

            var endResult = Convert.ToDouble(newResult);

            return Json(new { endResult });

        }
    }
}
