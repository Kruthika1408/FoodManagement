using FoodDataAccessLayer;
using KitchenStoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace KitchenStoryManagement.Controllers
{
    public class FoodItemController : Controller
    {
        FoodManagement foodItemDal = new FoodManagement();
        List<FoodItemModel> foodItemModelList = new List<FoodItemModel>();
        List<FoodDTO> foodItemsList;

        // GET: FoodItem
        public ActionResult Index()
        {
            foodItemsList = foodItemDal.GetAllFoodItem();
            foreach (var item in foodItemsList)
            {
                FoodItemModel foodItemModel = new FoodItemModel();

                foodItemModel.Id = item.Id;
                foodItemModel.FoodName = item.FoodName;
                foodItemModel.Price = item.Price;

                foodItemModelList.Add(foodItemModel);
            }
            return View(foodItemModelList);
        }

        // GET: FoodItem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Content("Id not found");
            }

            try
            {
                FoodDTO foodMaster = foodItemDal.GetFoodItemById(id.Value);
                if (foodMaster != null)
                {
                    FoodItemModel foodItemModel = new FoodItemModel()
                    {
                        Id = foodMaster.Id,
                        FoodName = foodMaster.FoodName,
                        Price = foodMaster.Price
                    };
                    return View(foodItemModel);
                }
                else
                {
                    return Content("Invalid FoodItem id");
                }
            }
            catch (Exception)
            {
                return Content("Error fetching FoodItem details");
            }
        }

        // GET: FoodItem/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FoodItem/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                FoodItemModel foodItemModel = new FoodItemModel()
                {
                    FoodName = collection["FoodName"].ToString(),
                    Price = Convert.ToSingle(collection["Price"])
                };
                FoodDTO foodMaster = new FoodDTO()
                {
                    FoodName = foodItemModel.FoodName,
                    Price = foodItemModel.Price
                };

                bool result = foodItemDal.AddFoodItem(foodMaster);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return Content("Invalid Item entry");
            }
            return View();
        }

        // GET: FoodItem/Edit/5
        public ActionResult Edit(int id)
        {
            FoodDTO foodMaster = foodItemDal.GetFoodItemById(id);
            FoodItemModel foodItemModel = new FoodItemModel()
            {
                Id = foodMaster.Id,
                FoodName = foodMaster.FoodName,
                Price = foodMaster.Price
            };
            return View(foodItemModel);
        }

        // POST: FoodItem/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                FoodItemModel foodItemModel = new FoodItemModel()
                {
                    Id = int.Parse(collection["Id"]),
                    FoodName = collection["FName"].ToString(),
                    Price = Convert.ToSingle(collection["Price"])
                };
                FoodDTO foodMaster = new FoodDTO()
                {
                    Id = foodItemModel.Id,
                    FoodName = foodItemModel.FoodName,
                    Price = foodItemModel.Price
                };
                bool result = foodItemDal.UpdateFoodItem(foodMaster);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return Content("Invalid entry of the item to be Updated");
            }
            return View();
        }

        // GET: FoodItem/Delete/5
        public ActionResult Delete(int id)
        {
            FoodDTO foodMaster = foodItemDal.GetFoodItemById(id);
            FoodItemModel foodItemModel = new FoodItemModel()
            {
                Id = foodMaster.Id,
                FoodName = foodMaster.FoodName,
                Price = foodMaster.Price
            };
            return View(foodItemModel);
        }

        // POST: FoodItem/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                bool result = foodItemDal.DeleteFoodItem(id);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return Content("No item with this id");
            }
            return View();
        }

        public ActionResult FoodMenu()
        {
            var foodItems = foodItemDal.GetAllFoodItem();

            var foodItemModels = new List<FoodItemModel>();
            foreach (var foodItem in foodItems)
            {
                var foodItemModel = new FoodItemModel
                {
                    Id = foodItem.Id,
                    FoodName = foodItem.FoodName,
                    Price = foodItem.Price
                };
                foodItemModels.Add(foodItemModel);
            }
            return View(foodItemModels);
        }

        [HttpPost]
        public ActionResult FoodMenu(string searchString)
        {
            var foodItems = foodItemDal.GetAllFoodItem();

            if (!string.IsNullOrEmpty(searchString))
            {
                string searchLower = searchString.ToLower();
                foodItems = foodItems.Where(f => f.FoodName.ToLower().Contains(searchLower)).ToList();
            }

            var foodItemModels = foodItems.Select(foodItem => new FoodItemModel
            {
                Id = foodItem.Id,
                FoodName = foodItem.FoodName,
                Price = foodItem.Price
            }).ToList();

            if (!foodItemModels.Any())
            {
                ViewBag.NoResultsMessage = "No products found for the given search criteria.";
            }
            return View(foodItemModels);
        }

        public ActionResult SelectedItems(int id)
        {
            foodItemsList = foodItemDal.GetAllFoodItem();
            FoodDTO foodItem = foodItemsList.Find(f => f.Id == id);
            TempData["Price"] = foodItem.Price;
            TempData["FoodItem"] = foodItem.FoodName;
            TempData.Keep();
            return View();
        }

        [HttpPost]
        public ActionResult SelectedItems(string deliveryAddress, int itemQuantity)
        {
            string price = TempData["Price"].ToString();
            float totalPrice = float.Parse(price) * itemQuantity;
            TempData["TotalPrice"] = totalPrice;
            TempData["Address"] = deliveryAddress;
            TempData.Keep();
            return RedirectToAction("PaymentMode");
        }

        public ActionResult PaymentMode()
        {
            return View();

        }

        public ActionResult OrderSuccess()
        {
            return View();

        }

    }
}