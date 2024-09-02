using Health_Insurance_Management_System.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Health_Insurance_Management_System.Controllers
{
	public class EmployeeController : Controller
	{
        private readonly HealthInsuranceContext db;

        public EmployeeController(HealthInsuranceContext _db)
        {
            this.db = _db;
        }

        public IActionResult Index()
		{
			return View();
		}

		public IActionResult Profile()
		{
			var userId = Convert.ToInt32(HttpContext.Session.GetInt32("User_ID"));
			var users = db.Employees.Include(u => u.User);
			var user = users.FirstOrDefault(u => u.UserId == userId);
            return View(user);
		}

		public IActionResult EditProfile(int id)
		{
			var users = db.Employees.Include(u => u.User);
			var user = users.FirstOrDefault(u => u.UserId == id);
			return View(user);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult EditProfile(Employee e,string uname,string pass)
		{
			var updUser = db.Users.FirstOrDefault(m => m.Id == e.UserId);
			var updEmp = db.Employees.FirstOrDefault(m => m.UserId == e.UserId);
			updUser.Username = uname;
            var hasher = new PasswordHasher<string>();
            updUser.Password = hasher.HashPassword(updUser.Email,pass);
			updEmp.Phone = e.Phone;
			db.Users.Update(updUser);
			db.Employees.Update(updEmp);
			db.SaveChanges();
			return RedirectToAction("Profile");
		}

        public IActionResult insuranceRequest()
		{
            ViewBag.InsuranceCompanyId = new SelectList(db.InsuranceCompanies, "InsuranceCompanyId", "Name");
            ViewBag.PolicyId = new SelectList(db.Policies, "PolicyId", "PolicyName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult insuranceRequest(Request r,int userId)
        {
       
            var Emp = db.Employees.FirstOrDefault(m => m.UserId == userId).EmployeeId;
			var multipleReq = db.Requests.FirstOrDefault(m => m.EmployeeId == Emp);
			if (multipleReq != null)
			{
				ViewBag.msg = "Can't Buy more than one policy.";
				return View();
			}

            r.EmployeeId = Emp;
            db.Requests.Add(r);
            db.SaveChanges();
            return RedirectToAction("myPolicy");
        }

		public IActionResult myPolicy()
		{
            var userId = Convert.ToInt32(HttpContext.Session.GetInt32("User_ID"));
            var empId = db.Employees.FirstOrDefault(m => m.UserId == userId).EmployeeId;
             var policyData = db.Requests.Include(p => p.InsuranceCompany).Include(p => p.Policy);
            var mypolicy = policyData.FirstOrDefault(m => m.EmployeeId == empId);
            return View(mypolicy);
           
        }

		public IActionResult EditPolicy(int pid)
		{
			
			return View();

		}

		public IActionResult DeleteRequest(int id)
		{
			var reqId = db.Requests.Find(id);
			return View(reqId);
		}

		[HttpPost]
		public IActionResult DeleteRequest(Request r)
		{
			db.Requests.Remove(r);
			db.SaveChanges();
			return RedirectToAction("myPolicy");
		}


	}
}
