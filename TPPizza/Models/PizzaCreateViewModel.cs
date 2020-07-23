using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPPizza;

namespace TPPizza.Models
{
    public class PizzaCreateViewModel
    {
        public Pizza Pizza { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Pate> Pates { get; set; }
        public List<int> IngredientIds { get; set; }
        public int PateId { get; set; }
    }
}