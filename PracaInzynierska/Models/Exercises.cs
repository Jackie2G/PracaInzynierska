using System.ComponentModel.DataAnnotations;

namespace PracaInzynierska.Models
{
    public class Exercises
    {
        //[Key]
        public int ID { get; set; }
        //public int TraningHistoryID { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Reps { get; set; }
        public bool Done { get; set; }
    }
}