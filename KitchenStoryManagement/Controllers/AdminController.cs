using KitchenStoryManagement.Models;
using System;
using System.Web.Mvc;
using AdminDataAccessLayer;

namespace KitchenStoryManagement.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        AdminManagement adminManagementDal = new AdminManagement();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AdminModel adminModel)
        {
            AdminDTO adminMaster = new AdminDTO()
            {
                EmailId = adminModel.EmailId,
                Password = adminModel.Password,
            };
            try
            {
                bool result = adminManagementDal.AdminLogin(adminMaster);
                if (result)
                {
                    return RedirectToAction("Index", "FoodItem");
                }
                else
                {
                    return Content("Invalid Login Credentials");
                }
            }
            catch (Exception)
            {
                return Content("Invalid Login");
            }
        }

        public ActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PasswordChange(AdminModel adminModel)
        {
            bool validEmail = adminManagementDal.validEmail(adminModel.EmailId);
            if (validEmail)
            {
                AdminDTO adminMaster = new AdminDTO()
                {
                    EmailId = adminModel.EmailId,
                    Password = adminModel.Password,
                };
                try
                {
                    bool result = adminManagementDal.ChangePassword(adminMaster);
                    if (result)
                    {
                        return RedirectToAction("SuccessPage");
                    }
                }
                catch (Exception)
                {
                    return Content("Invalid Login");
                }
            }
            return View();
        }

        public ActionResult SuccessPage()
        {
            return View();
        }
    }
}