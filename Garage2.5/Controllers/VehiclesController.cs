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

        public ActionResult DetailedList(string orderBy, string currentFilter, string searchString, int? selectedvehicletype = null, int page = 1)
        {
            return GetPagedVehiclesList(orderBy, currentFilter, searchString, selectedvehicletype, page, nameof(DetailedList));
        }

        // GET: Vehicles
        public ActionResult Index(string orderBy, string currentFilter, string searchString, int? selectedvehicletype = null, int page = 1)
        {
            return GetPagedVehiclesList(orderBy, currentFilter, searchString, selectedvehicletype, page, nameof(Index));
        }

        private ActionResult GetPagedVehiclesList(string orderBy, string currentFilter, string searchString, int? selectedvehicletype, int page, string view)
        {
            if (!db.IsConfigured)
                return RedirectToAction("Index", "Setup");
            if (!db.Vehicles.Any())
                return RedirectToAction("Checkin");
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
            if (!string.IsNullOrEmpty(searchString))
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
                case "brand":
                    vehicles = vehicles.OrderBy(v => v.Brand);
                    break;
                case "brand_dec":
                    vehicles = vehicles.OrderByDescending(v => v.Brand);
                    break;
                case "model":
                    vehicles = vehicles.OrderBy(v => v.Model);
                    break;
                case "model_dec":
                    vehicles = vehicles.OrderByDescending(v => v.Model);
                    break;
                case "wheels":
                    vehicles = vehicles.OrderBy(v => v.NumberOfWheels);
                    break;
                case "wheels_dec":
                    vehicles = vehicles.OrderByDescending(v => v.NumberOfWheels);
                    break;
                case "color":
                    vehicles = vehicles.OrderBy(v => v.VehicleColorId);
                    break;
                case "color_dec":
                    vehicles = vehicles.OrderByDescending(v => v.VehicleColorId);
                    break;
                case "time":
                    vehicles = vehicles.OrderBy(v => v.CheckinTime);
                    break;
                default:
                    vehicles = vehicles.OrderByDescending(v => v.CheckinTime);
                    break;
            }
            ViewBag.CurrentSort = orderBy;

            ViewBag.VehicleTypes = new SelectList(db.VehicleTypes, "Id", "Type");
            HasVacantSpots();
            return View(view, new PagedList.PagedList<Vehicle>(vehicles.ToList(), page, db.GarageConfiguration.VehiclesPerPage));
        }
        
        private bool HasVacantSpots() {
            var total = db.GarageConfiguration.ParkingSpaces;
            var vacant = (int)Math.Ceiling((total * 3 - db.Vehicles.ToArray().Sum(v => v.Units)) / 3.0);
            ViewBag.Vacant = $"Vacant parking spots: {vacant}/{total}";
            ViewBag.HasVacantSpots = vacant > 0;
            return ViewBag.HasVacantSpots;
        }

        // GET: Vehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (!db.IsConfigured)
                return RedirectToAction("Index", "Setup");
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

        private IEnumerable<SelectListItem> GetSupportedList(Vehicle vehicle, bool edit) {
            var sizes = db.VehicleTypes.Select(v => v.Size).Distinct().ToArray();
            var supportedSize = sizes.Where(s => FindFirstFreeUnit(s) + s <= db.GarageConfiguration.ParkingUnits);
            var selectList = db.VehicleTypes.ToArray().Select(t => new SelectListItem() {
                Disabled = !supportedSize.Contains(t.Size),
                Text = t.Type,
                Value = t.Id.ToString(),
                Selected = vehicle?.VehicleTypeId == t.Id
            }).ToArray();
            if (edit)
                foreach (var itm in selectList)
                    if (itm.Selected)
                        itm.Disabled = false; // Make sure the currently selected item is enabled
            return selectList;
        }

        // GET: Vehicles/Create
        public ActionResult Checkin() {
            if (!db.IsConfigured)
                return RedirectToAction("Index", "Setup");
            if (!HasVacantSpots())
                return RedirectToAction("Index");
            MakeCreateDropDowns(null);
            return View();
        }

        private long FindFirstFreeUnit(int size) {
            long lastFree = 0;
            foreach (var vehicle in db.Vehicles.Where(v => v.ParkingUnit >= 0).OrderBy(v => v.ParkingUnit)) {
                if (vehicle.ParkingUnit - size >= lastFree)
                    return lastFree;
                lastFree = vehicle.ParkingUnit + vehicle.Units;
                if (lastFree % 3 != 0)
                    lastFree += 3 - lastFree % 3;
            }
            return lastFree;
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkin([Bind(Include = "Id,Registration,VehicleTypeId, VehicleColorId, Brand, Model, NumberOfWheels")] Vehicle vehicle, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) {
                ModelState.AddModelError("username", "The Username field is required.");
                MakeCreateDropDowns(vehicle);
                return View(vehicle);
            }
            ViewBag.UserName = username;
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
                var user = db.Members.FirstOrDefault(m => m.Username == username);
                if (user == null)
                {
                    ModelState.AddModelError("username", $"No member with the username '{username}' could be found!");
                    MakeCreateDropDowns(vehicle);
                    return View(vehicle);
                }
                if (user.Password != password) {
                    ModelState.AddModelError("password", "Wrong password!");
                    MakeCreateDropDowns(vehicle);
                    return View(vehicle);
                }
                vehicle.MemberId = user.Id;
                vehicle.CheckinTime = DateTime.Now;
                vehicle.Type = db.VehicleTypes.FirstOrDefault(t => t.Id == vehicle.VehicleTypeId);
                vehicle.ParkingUnit = FindFirstFreeUnit(vehicle.Units);
                if (vehicle.ParkingUnit + vehicle.Units < db.GarageConfiguration.ParkingUnits)
                {
                    ModelState.AddModelError(nameof(vehicle.VehicleTypeId), $"Not enough space to park a {vehicle.Type.Type}");
                    MakeCreateDropDowns(null);
                    return View(vehicle);
                }
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
            if (!db.IsConfigured)
                return RedirectToAction("Index", "Setup");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            MakeCreateDropDowns(vehicle, true);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string registration, int vehicletypeid, int vehiclecolorid, string brand, string model, int numberofwheels)
        {
            var vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
                return HttpNotFound();
            vehicle.VehicleColorId = vehiclecolorid;
            vehicle.Brand = brand;
            vehicle.Model = model;
            vehicle.NumberOfWheels = numberofwheels;
            if (ModelState.IsValid) {
                if (vehicle.Registration != registration)
                {
                    vehicle.Registration = RegistrationNormalizer.NormalizeForStorage(registration);
                    if (!registrationVerifier.Verify(vehicle.Registration))
                    {
                        ModelState.AddModelError(nameof(vehicle.Registration), registrationVerifier.LastErrorMessage);
                        MakeCreateDropDowns(vehicle, true);
                        return View(vehicle);
                    }
                    if (db.Vehicles.Where(v => v.Id != vehicle.Id).Any(v => v.Registration == vehicle.Registration))
                    {
                        ModelState.AddModelError(nameof(vehicle.Registration), $"A vehicle with the registration '{RegistrationNormalizer.NormalizeForDisplay(vehicle.Registration)}' already exist in the garage");
                        MakeCreateDropDowns(vehicle, true);
                        return View(vehicle);
                    }
                }
                if (vehicle.VehicleTypeId != vehicletypeid) {
                    vehicle.VehicleTypeId = vehicletypeid; // Update the field
                    vehicle.ParkingUnit = -1; // Go negative so it don't count anymore as we're moving it
                    db.SaveChanges();
                    vehicle.ParkingUnit = FindFirstFreeUnit(db.VehicleTypes.Single(t => t.Id == vehicletypeid).Size); // find the new parking spot this vehicle should be in
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            MakeCreateDropDowns(vehicle, true);
            return View(vehicle);
        }

        private void MakeCreateDropDowns(Vehicle vehicle, bool edit = false)
        {
            ViewBag.VehicleTypeId = GetSupportedList(vehicle, edit);
            ViewBag.VehicleColorId = new SelectList(db.VehicleColors, "Id", "Name", vehicle?.VehicleColorId);
        }

        // GET: Vehicles/Delete/5
        public ActionResult Checkout(int? id)
        {
            if (!db.IsConfigured)
                return RedirectToAction("Index", "Setup");
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

        public ActionResult Overview(int page = 1)
        {
            if (!db.IsConfigured)
                return RedirectToAction("Index", "Setup");
            var spots = new OverviewViewModel[db.GarageConfiguration.ParkingSpaces];
            foreach (var vehicle in db.Vehicles.OrderBy(v => v.ParkingUnit))
            {
                var first = (int)vehicle.ParkingUnit / 3;
                if (spots[first] != null)
                {
                    var list = spots[first].ParkedVehicles.ToList();
                    list.Add(vehicle);
                    spots[first] = new OverviewViewModel(first + 1, list);
                }
                else
                {
                    spots[first] = new OverviewViewModel(first + 1, new []{ vehicle });
                    for (int i = 0; i < vehicle.Units / 3; i++)
                        spots[first + i] = new OverviewViewModel(first + i + 1, new[] { vehicle });
                }
            }
            for (var i = 0; i < spots.Length; i++)
            {
                if (spots[i] != null)
                    continue;
                spots[i] = new OverviewViewModel(i + 1);
            }
            return View(new PagedList.PagedList<OverviewViewModel>(spots, page, 100));
        }

        public ActionResult Statistics()
        {
            if (!db.IsConfigured)
                return RedirectToAction("Index", "Setup");
            var now = DateTime.Now;
            var statistics = new Statistics(db);
            foreach (var vehicle in db.Vehicles)
            {
                statistics.Update(vehicle, now, db.GarageConfiguration.PricePerMinute);
            }
            return View(statistics);
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
