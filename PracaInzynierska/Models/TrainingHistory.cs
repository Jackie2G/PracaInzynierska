using EllipticCurve;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PracaInzynierska.Areas.Identity.Data;
using PracaInzynierska.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PracaInzynierska.Models
{
    public class TrainingHistory
    {
        //    private readonly IHttpContextAccessor _httpContextAccessor;


        //    public TrainingHistory(IHttpContextAccessor httpContextAccessor)
        //    {
        //        _httpContextAccessor = httpContextAccessor;
        //    }

        //    public void GetUserId()
        //    {
        //        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        //moze zadziala
        //    }
        //}

        //[Key, ForeignKey("ApplicationUser")]
        //public string UserId { get; set; }
        //[ForeignKey("ApplicationUser")]

        //public int TrainingHistoryId { get; set; }
        //[ForeignKey("AspNetUsers")]
        //public int TrainingId { get; set; }
        [Key]
        public int TrainingId { get; set; }
        public string Id { get; set; }
        [ForeignKey("Id")]
        public virtual ApplicationUser User { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }
        //public bool Done { get; set; }
        public int ExercisesID { get; set; }
        
    }
}
