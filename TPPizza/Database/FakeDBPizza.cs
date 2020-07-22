using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPPizza.Database
{

    public sealed class FakeDBPizza
    {
        private FakeDBPizza()
        {
            this.Pizzas = this.GetChoixPizza();
        }
        private static readonly object padlock = new object();
        private static FakeDBPizza instance = null;
        public static FakeDBPizza Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new FakeDBPizza();
                    }
                    return instance;
                }
            }
        }
        public List<Pizza> Pizzas { get; set; }

        private List<Pizza> GetChoixPizza()
        {
            var i = 1;
            return new List<Pizza>
            {
                new Pizza{Id=i++,Nom = "Mozza", Ingredients = this.IngredientsDisponibles.Where(x=>x.Id<4).ToList(), Pate = this.PatesDisponibles.FirstOrDefault(x=>x.Id==1) },
                new Pizza{Id=i++,Nom = "Margarita", Ingredients = this.IngredientsDisponibles.Where(x=>x.Id>5).ToList(), Pate = this.PatesDisponibles.FirstOrDefault(x=>x.Id==2) },
                new Pizza{Id=i++,Nom = "Margarita", Ingredients = this.IngredientsDisponibles.Where(x=>x.Id<2).ToList(), Pate = this.PatesDisponibles.FirstOrDefault(x=>x.Id==2) },
                new Pizza{Id=i++,Nom = "Margarita", Ingredients = this.IngredientsDisponibles.Where(x=>x.Id>6).ToList(), Pate = this.PatesDisponibles.FirstOrDefault(x=>x.Id==3) },

            };
        }
        public List<Ingredient> IngredientsDisponibles => new List<Ingredient>
        {
            new Ingredient{Id=1,Nom="Mozzarella"},
            new Ingredient{Id=2,Nom="Jambon"},
            new Ingredient{Id=3,Nom="Tomate"},
            new Ingredient{Id=4,Nom="Oignon"},
            new Ingredient{Id=5,Nom="Cheddar"},
            new Ingredient{Id=6,Nom="Saumon"},
            new Ingredient{Id=7,Nom="Champignon"},
            new Ingredient{Id=8,Nom="Poulet"}
        };

        public List<Pate> PatesDisponibles => new List<Pate>
        {
            new Pate{ Id=1,Nom="Pate fine, base crême"},
            new Pate{ Id=2,Nom="Pate fine, base tomate"},
            new Pate{ Id=3,Nom="Pate épaisse, base crême"},
            new Pate{ Id=4,Nom="Pate épaisse, base tomate"}
        };

    }
}