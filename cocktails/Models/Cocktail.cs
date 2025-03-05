using System;
using System.Diagnostics;

namespace cocktails.Models
{
    public class Cocktail
    {
        public Guid UuidCocktail => Guid.NewGuid();
        public string Name { get; set; }
        public string Picture {  get; set; }
        public string GlassType {  get; set; }
        public string Description { get; set; }
        public bool AlcoolFree { get; set; }
        public string Difficulty { get; set; }
        public int Popularity { get; set; }
        public string Recipe {  get; set; }
        public float PreparationTime { get; set; }
    }
}
