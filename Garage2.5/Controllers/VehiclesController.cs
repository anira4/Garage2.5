using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garage2._5.DAL;
using Garage2._5.Helper;
using Garage2._5.Models;
using Garage2._5.ViewModels;

namespace Garage2._5.Controllers
{
    public class VehiclesController : Controller
    {
        private GarageContext db = new GarageContext();
        private RegistrationVerifier registrationVerifier = new RegistrationVerifier();

        // GET: Vehicles
        public ActionResult Index(string orderBy, string currentFilter, string searchString, int? selectedvehicletype = null, int page = 1)
        {
            var vehicles = db.Vehicles.Include(v => v.Owner).Include(v => v.Type);

            if (selectedvehicletype != null)
            {
                vehicles = vehicles.Where(v => v.Type.Id == selectedvehicletype);
                ViewBag.selectedvehicletype = selectedvehicletype;
            }

            if (searchString != null)
            {
                page = 1; // If the search string is changed during paging, the page has to be reset to 1
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                vehicles = vehicles.Where(v => v.Registration.Contains(searchString));
            }

            switch (orderBy)
            {
                case "user":
                    vehicles = vehicles.OrderBy(v => v.Owner.Name);
                    break;
                case "user_dec":
                    vehicles = vehicles.OrderByDescending(v => v.Owner.Name);
                    break;
                case "type":
                    vehicles = vehicles.OrderBy(v => v.Type.Type);
                    break;
                case "type_dec":
                    vehicles = vehicles.OrderByDescending(v => v.Type.Type);
                    break;
                case "registration":
                    vehicles = vehicles.OrderBy(v => v.Registration);
                    break;
                case "registration_dec":
                    vehicles = vehicles.OrderByDescending(v => v.Registration);
                    break;
                case "spot":
                    vehicles = vehicles.OrderBy(v => v.ParkingUnit);
                    break;
                case "spot_dec":
                    vehicles = vehicles.OrderByDescending(v => v.ParkingUnit);
                    break;
                case "checkintime_dec":
                    vehicles = vehicles.OrderByDescending(v => v.CheckinTime);
                    break;
                default:
                    vehicles = vehicles.OrderBy(v => v.CheckinTime);
                    break;
            }
            ViewBag.CurrentSort = orderBy;

            ViewBag.VehicleTypes = new SelectList(db.VehicleTypes, "Id", "Type");
            HasVacantSpots();
            return View(vehicles.ToList());
        }


        private bool HasVacantSpots() {
            var total = 100;
            var vacant = (int)Math.Ceiling((total * 3 - db.Vehicles.ToArray().Sum(v => v.Units)) / 3.0);
            ViewBag.Vacant = $"Vacant parking spots: {vacant}/{total}";
            ViewBag.HasVacantSpots = vacant > 0;
            return ViewBag.HasVacantSpots;
        }

        // GET: Vehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // GET: Vehicles/Create
        public ActionResult Checkin() {
            if (!HasVacantSpots())
                return RedirectToAction("Index");
            MakeCreateDropDowns(null);
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkin([Bind(Include = "Id,Registration,CheckinTime,VehicleTypeId,MemberId")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                if (!registrationVerifier.Verify(vehicle.Registration))
                {
                    ModelState.AddModelError(nameof(vehicle.Registration), registrationVerifier.LastErrorMessage);
                    MakeCreateDropDowns(vehicle);
                    return View(vehicle);
                }
                vehicle.Registration = RegistrationNormalizer.NormalizeForStorage(vehicle.Registration);
                if (db.Vehicles.Any(v => v.Registration == vehicle.Registration))
                {
                    ModelState.AddModelError(nameof(vehicle.Registration), $"A vehicle with the registration '{RegistrationNormalizer.NormalizeForDisplay(vehicle.Registration)}' already exist in the garage");
                    MakeCreateDropDowns(vehicle);
                    return View(vehicle);
                }
                vehicle.CheckinTime = DateTime.Now;
                db.Vehicles.Add(vehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            MakeCreateDropDowns(vehicle);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username", vehicle.MemberId);
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Registration,VehicleTypeId,MemberId")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                if (!registrationVerifier.Verify(vehicle.Registration))
                {
                    ModelState.AddModelError(nameof(vehicle.Registration), registrationVerifier.LastErrorMessage);
                    MakeCreateDropDowns(vehicle);
                    return View(vehicle);
                }
                vehicle.Registration = RegistrationNormalizer.NormalizeForStorage(vehicle.Registration);
                if (db.Vehicles.Where(v => v.Id != vehicle.Id).Any(v => v.Registration == vehicle.Registration))
                {
                    ModelState.AddModelError(nameof(vehicle.Registration), $"A vehicle with the registration '{RegistrationNormalizer.NormalizeForDisplay(vehicle.Registration)}' already exist in the garage");
                    MakeCreateDropDowns(vehicle);
                    return View(vehicle);
                }
                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            MakeCreateDropDowns(vehicle);
            return View(vehicle);
        }

        private void MakeCreateDropDowns(Vehicle vehicle)
        {
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username", vehicle?.MemberId);
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type", vehicle?.VehicleTypeId);
        }

        // GET: Vehicles/Delete/5
        public ActionResult Checkout(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            var receiptViewModel = new ReceiptViewModel();
            receiptViewModel.Update(vehicle, DateTime.Now, 1);
            TempData["receiptViewModel"] = receiptViewModel;
            return View(receiptViewModel);
        }


        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Checkout")]
        [ValidateAntiForgeryToken]
        public ActionResult CheckoutConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);

            db.Vehicles.Remove(vehicle);
            db.SaveChanges();
            return View("Receipt", TempData["receiptViewModel"]);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
