using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using Microsoft.Ajax.Utilities;
using TPPizza.Database;
using TPPizza.Models;

namespace TPPizza.Controllers
{
    public class PizzaController : Controller
    {
        // GET: Pizza
        public ActionResult Index()
        {
            return View(FakeDBPizza.Instance.Pizzas);
        }

        // GET: Pizza/Details/5
        public ActionResult Details(int id)
        {
            PizzaCreateViewModel vm = new PizzaCreateViewModel();
            vm.Pizza = FakeDBPizza.Instance.Pizzas.FirstOrDefault(x => x.Id == id);
            return View(vm);
        }

        // GET: Pizza/Create
        public ActionResult Create()
        {
            pizzas = FakeDBPizza.Instance.Pizzas;
            PizzaCreateViewModel vm = new PizzaCreateViewModel();
                                  
            vm.Ingredients = FakeDBPizza.Instance.IngredientsDisponibles;
            vm.Pates = FakeDBPizza.Instance.PatesDisponibles;
            return View(vm);
        }

        // POST: Pizza/Create
        [HttpPost]
        public ActionResult Create(PizzaCreateViewModel vm)
        {
            try
            {
                if (ModelState.IsValid && ValidateVM(vm))
                {
                    pizzas = FakeDBPizza.Instance.Pizzas;
                    if (pizzas.Any(p => p.Nom.ToUpper() == vm.Pizza.Nom.ToUpper()))
                    {
                        ModelState.AddModelError("", "Il existe déjà une pizza avec ce nom");
                        return View(vm);
                    }

                    if (vm.Pizza.Ingredients.Count < 2 || vm.Pizza.Ingredients.Count > 5)
                    {
                        ModelState.AddModelError("", "Une pizza doit contenir au moins 2  et au plus 5 ingrédients");
                        return View(vm);
                    }

                    Pizza pizza = vm.Pizza;

                    pizza.Pate = FakeDBPizza.Instance.PatesDisponibles.FirstOrDefault(x => x.Id == vm.PateId);

                    pizza.Ingredients = FakeDBPizza.Instance.IngredientsDisponibles.Where(
                        x => vm.IngredientIds.Contains(x.Id))
                        .ToList();

                    
                    pizza.Id = FakeDBPizza.Instance.Pizzas.Count == 0 ? 1 : FakeDBPizza.Instance.Pizzas.Max(x => x.Id) + 1;

                    FakeDBPizza.Instance.Pizzas.Add(pizza);

                    return RedirectToAction("Index");
                }
                else
                {
                    vm.Pates = FakeDBPizza.Instance.PatesDisponibles;

                    vm.Ingredients = FakeDBPizza.Instance.IngredientsDisponibles;

                    return View(vm);
                }
            }
            catch
            {
                return View(vm);
            }
        }
        private bool ValidateVM(PizzaCreateViewModel vm)
        {
            bool result = true;
            return result;
        }

        // GET: Pizza/Edit/5
        public ActionResult Edit(int id)
        {
            PizzaCreateViewModel vm = new PizzaCreateViewModel();

            vm.Pates = FakeDBPizza.Instance.PatesDisponibles;

            vm.Ingredients = FakeDBPizza.Instance.IngredientsDisponibles;

            vm.Pizza = FakeDBPizza.Instance.Pizzas.FirstOrDefault(x => x.Id == id);

            if (vm.Pizza.Pate != null)
            {
                vm.PateId = vm.Pizza.Pate.Id;
            }

            if (vm.Pizza.Ingredients.Any())
            {
                vm.IngredientIds = vm.Pizza.Ingredients.Select(x => x.Id).ToList();
            }

            return View(vm);
            
        }

        // POST: Pizza/Edit/5

        private List<Pizza> pizzas = new List<Pizza>();
        private List<int> ingredientsPizza = new List<int>();
        
        [HttpPost]
        public ActionResult Edit(PizzaCreateViewModel vm)
        {
           try
            {
               
                if (ModelState.IsValid && ValidateVM(vm))
                {
                    pizzas = FakeDBPizza.Instance.Pizzas;
                    List<List<int>> ingredientsPizza = pizzas.Select(i=> i.Ingredients.Select(x=>x.Id).ToList()).ToList();
                    List<int> pizzaIngredients = new List<int>();
                                       
                    if (pizzas.Any(p=>p.Nom.ToUpper()==vm.Pizza.Nom.ToUpper() && vm.Pizza.Id != p.Id))
                    {
                        ModelState.AddModelError("", "Il existe déjà une pizza avec ce nom");
                        return View(vm);     
                    }

                    if (vm.Pizza.Ingredients.Count<2 || vm.Pizza.Ingredients.Count>5)
                    {
                        ModelState.AddModelError("", "Une pizza doit contenir au moins 2  et au plus 5 ingrédients");
                        return View(vm);
                    }
                                      
                    foreach (Pizza p in pizzas)
                    {
                       
                        foreach (Ingredient i in p.Ingredients ) 
                        {
                            
                            pizzaIngredients.Add(i.Id);                                                 
                             
                        }

                       for (int i = 0; i < pizzaIngredients.Count; i++)
                        {
                            if (vm.IngredientIds.ElementAt(i) != pizzaIngredients.ElementAt(i))
                            {
                                return RedirectToAction("Index");
                            }
                            ModelState.AddModelError("", "2 pizzas différentes ne peuvent contenir la même liste d'ingrédients");
                        return View(vm);
                        }
                        
                    }

                    Pizza pizza = FakeDBPizza.Instance.Pizzas.FirstOrDefault(x => x.Id == vm.Pizza.Id);
                    pizza.Nom = vm.Pizza.Nom;
                    pizza.Pate = FakeDBPizza.Instance.PatesDisponibles.FirstOrDefault(x => x.Id == vm.PateId);
                    pizza.Ingredients = FakeDBPizza.Instance.IngredientsDisponibles.Where(x => vm.IngredientIds.Contains(x.Id)).ToList();

                    return RedirectToAction("Index");
                }
                else 
                { 
                    return View(vm); 
                }
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: Pizza/Delete/5
        public ActionResult Delete(int id)
        {
            PizzaCreateViewModel vm = new PizzaCreateViewModel();
            vm.Pizza = FakeDBPizza.Instance.Pizzas.FirstOrDefault(x => x.Id == id);
            return View(vm);
        }

        // POST: Pizza/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Pizza pizza = FakeDBPizza.Instance.Pizzas.FirstOrDefault(x => x.Id == id);
                FakeDBPizza.Instance.Pizzas.Remove(pizza);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
