using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class CarePlanController : Controller
    {
        // GET: CarePlanController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CarePlanController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CarePlanController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CarePlanController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CarePlanController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CarePlanController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CarePlanController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CarePlanController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
