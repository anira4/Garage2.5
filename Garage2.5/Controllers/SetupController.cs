using System.Linq;
using System.Web.Mvc;
using Garage2._5.DAL;
using Garage2._5.Models;

namespace Garage2._5.Controllers
{
    public class SetupController : Controller
    {
        private readonly GarageContext db = new GarageContext();

        // GET: Setup/Create
        public ActionResult Index()
        {
            if (db.IsConfigured)
                return RedirectToAction("Index", "Members");
            return View(new Configuration());
        }

        // POST: Setup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Id,ParkingSpaces,PricePerMinute,MembersPerPage,VehiclesPerPage")] Configuration configuration)
        {
            if (ModelState.IsValid)
            {
                db.Configurations.RemoveRange(db.Configurations.ToArray()); // Drop old configs
                db.Vehicles.RemoveRange(db.Vehicles.ToArray()); // Drop old vehicles
                db.Members.RemoveRange(db.Members.ToArray()); // Drop old members
                db.Configurations.Add(configuration);
                db.SaveChanges();
                return RedirectToAction("Checkin", "Vehicles");
            }

            return View(configuration);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
