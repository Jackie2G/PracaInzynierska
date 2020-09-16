using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PracaInzynierska.Models;

namespace PracaInzynierska.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string nickName { get; set; }

        [PersonalData]
        public string Gender { get; set; }

        [PersonalData]
        public string WeightCategory { get; set; }

        //public virtual TrainingHistory TrainingHistory { get; set; }

        //public virtual ICollection<TrainingHistory> trainingHistories { get; set; }
    }
}
