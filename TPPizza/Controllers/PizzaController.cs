using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                    Pizza pizza = vm.Pizza;

                    pizza.Pate = FakeDBPizza.Instance.PatesDisponibles.FirstOrDefault(x => x.Id == vm.PateId);

                    pizza.Ingredients = FakeDBPizza.Instance.IngredientsDisponibles.Where(
                        x => vm.IngredientIds.Contains(x.Id))
                        .ToList();

                    // Insuffisant
                    //pizza.Id = FakeDb.Instance.Pizzas.Count + 1;

                    pizza.Id = FakeDBPizza.Instance.Pizzas.Count == 0 ? 1 : FakeDBPizza.Instance.Pizzas.Max(x => x.Id) + 1;

                    FakeDBPizza.Instance.Pizzas.Add(pizza);

                    return RedirectToAction("Index");
                }
                else
                {
          //          vm.Pates = FakeDBPizza.Instance.PatesDisponibles.Select(
            //    x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
              //  .ToList();
              //
                //    vm.Ingredients = FakeDBPizza.Instance.IngredientsDisponibles.Select(
                  //      x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                    //    .ToList();

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

            vm.Pate = FakeDBPizza.Instance.PatesDisponibles.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

            vm.Ingredients = FakeDBPizza.Instance.IngredientsDisponibles.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

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
            return View();
        }

        // POST: Pizza/Edit/5
        [HttpPost]
        public ActionResult Edit(PizzaCreateViewModel vm)
        {
            try
            {
                Pizza pizza = FakeDBPizza.Instance.Pizzas.FirstOrDefault(x => x.Id == vm.Pizza.Id);
                pizza.Nom = vm.Pizza.Nom;
                pizza.Pate = FakeDBPizza.Instance.PatesDisponibles.FirstOrDefault(x => x.Id == vm.PateId);
                pizza.Ingredients = FakeDBPizza.Instance.IngredientsDisponibles.Where(x => vm.IngredientIds.Contains(x.Id)).ToList();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
