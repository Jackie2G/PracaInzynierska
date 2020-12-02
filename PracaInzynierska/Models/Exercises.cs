using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PracaInzynierska.Models
{
    public class Exercises
    {
        //[Key]
        public int ID { get; set; }
        //public int TraningHistoryID { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public int Reps { get; set; }
        public bool Done { get; set; }
        public int Series { get; set; }
        public string Description { get; set; }
        public string CurrentPr { get; set; }

        public TrainingHistory trainingHistory { get; set; }
    }
}