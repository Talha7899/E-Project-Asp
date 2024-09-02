using Health_Insurance_Management_System.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Health_Insurance_Management_System.Controllers
{

    public class AdminController : Controller
    {
		private readonly HealthInsuranceContext db;
		public AdminController(HealthInsuranceContext _db)
		{
			this.db = _db;
		}

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

		// User Registration:

		//public IActionResult Signup()
		//{
		//	return View();
		//}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public IActionResult Signup(User u)
		//{	
		//	var checkExictingUser = db.Users.FirstOrDefault(x => x.Email == u.Email);

		//		if (checkExictingUser != null)
		//		{
		//			ViewBag.msg = "User Already Exists";
		//			return View();
		//		}

		//		var hasher = new PasswordHasher<string>();
		//		u.Password = hasher.HashPassword(u.Email, u.Password);
		//		db.Users.Add(u);
		//		db.SaveChanges();
		//		return RedirectToAction("Login");
			
		//}

		// User Login:

		public IActionResult Login()
		{
          return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Login(User u)
		{
			bool IsAuthenticated = false;
			string controller = "";
			ClaimsIdentity? identity = null;

			var checkUser = db.Users.FirstOrDefault(u1 => u1.Email == u.Email);
			if (checkUser != null)
			{
				var hasher = new PasswordHasher<string>();
				var verifyPass = hasher.VerifyHashedPassword(u.Email, checkUser.Password, u.Password);

				if (verifyPass == PasswordVerificationResult.Success && checkUser.Role == 1)
				{
					identity = new ClaimsIdentity(new[]
					{
					new System.Security.Claims.Claim(ClaimTypes.Name ,checkUser.Username),
					new System.Security.Claims.Claim(ClaimTypes.Role ,"Admin"),
				}
				   , CookieAuthenticationDefaults.AuthenticationScheme);

					HttpContext.Session.SetString("email", checkUser.Email);
					HttpContext.Session.SetString("username", checkUser.Username);
                    HttpContext.Session.SetInt32("User_ID", checkUser.Id);

                    IsAuthenticated = true;
					controller = "Admin";
				}
				else if (verifyPass == PasswordVerificationResult.Success && checkUser.Role == 2)
				{
					IsAuthenticated = true;
					identity = new ClaimsIdentity(new[]
				   {
					new System.Security.Claims.Claim(ClaimTypes.Name ,checkUser.Username),
					new System.Security.Claims.Claim(ClaimTypes.Role ,"User"),
				}
				   , CookieAuthenticationDefaults.AuthenticationScheme);
					controller = "Employee";
					HttpContext.Session.SetString("email", checkUser.Email);
					HttpContext.Session.SetString("username", checkUser.Username);
					HttpContext.Session.SetInt32("User_ID", checkUser.Id);
				}
				else
				{
					IsAuthenticated = false;

				}
				if (IsAuthenticated)
				{
					var principal = new ClaimsPrincipal(identity);

					var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

					return RedirectToAction("Index", controller);
				}

				else
				{
					ViewBag.msg = "Invalid Credentials";
					return View();
				}
			}
			else
			{
				ViewBag.msg = "User not found";
				return View();
			}
		}

		public IActionResult Logout()
		{
			var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login");
		}

		// Crud Operations:

		// Insurance Work Start:

		[Authorize (Roles ="Admin")]
		public IActionResult ViewInsurance()
		{
			var insList = db.InsuranceCompanies.ToList();
			return View(insList);
		}

        [Authorize(Roles = "Admin")]
		public IActionResult AddInsurance()
		{
			return View();
		}
        [Authorize(Roles = "Admin")]

        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddInsurance(InsuranceCompany Ins)
		{
			if (ModelState.IsValid)
			{
				db.InsuranceCompanies.Add(Ins);
				db.SaveChanges();
				return RedirectToAction("ViewInsurance");
			}
			else
			{
				return View();
			}
		}

        [Authorize(Roles = "Admin")]
        public IActionResult UpdateInsurance(int Id)
		{
			var InsuranceId = db.InsuranceCompanies.Find(Id);
			return View(InsuranceId);
		}

        [Authorize(Roles = "Admin")]
        [HttpPost]
		[ValidateAntiForgeryToken]
        public IActionResult UpdateInsurance(InsuranceCompany Ins)
        {
			if (ModelState.IsValid)
			{
				db.InsuranceCompanies.Update(Ins);
				db.SaveChanges();
				return RedirectToAction("ViewInsurance");
			}
			else
			{
				return View();
			}
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteInsurance(int id)
		{
			var delID = db.InsuranceCompanies.Find(id);
			return View(delID);
		}
        [Authorize(Roles = "Admin")]
        [HttpPost]
		[ValidateAntiForgeryToken]
        public IActionResult DeleteInsurance(InsuranceCompany d)
        {
			
				db.InsuranceCompanies.Remove(d);
				db.SaveChanges();
				return RedirectToAction("ViewInsurance");		
        }
		// Insurance Work End.

		// Policy Work Start:

		[Authorize(Roles = "Admin")]
		public IActionResult ViewPolicy()
		{
			var policyData = db.Policies.Include(p => p.InsuranceCompany);
			return View(policyData);
		}

		
        [Authorize(Roles = "Admin")]

        public IActionResult AddPolicy()
		{
			ViewBag.InsuranceCompanyId = new SelectList(db.InsuranceCompanies, "InsuranceCompanyId", "Name");
			return View();

		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddPolicy(Policy p)
		{
			if (ModelState.IsValid)
			{
				db.Policies.Add(p);
				db.SaveChanges();
			}
			else
			{
				return View();
			}
			
            ViewBag.InsuranceCompanyId = new SelectList(db.InsuranceCompanies, "InsuranceCompanyId", "Name");
            return RedirectToAction("ViewPolicy");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UpdatePolicy(int Id)
        {
            ViewBag.InsuranceCompanyId = new SelectList(db.InsuranceCompanies, "InsuranceCompanyId", "Name");

			var PolicyId = db.Policies.Find(Id);
            return View(PolicyId);

        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdatePolicy(Policy p)
        {
			if (ModelState.IsValid)
			{
				db.Policies.Update(p);
				db.SaveChanges();
			}

            ViewBag.InsuranceCompanyId = new SelectList(db.InsuranceCompanies, "InsuranceCompanyId", "Name");

            return RedirectToAction("ViewPolicy");

        }
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePolicy(int id)
		{
            ViewBag.InsuranceCompanyId = new SelectList(db.InsuranceCompanies, "InsuranceCompanyId", "Name");
            var policyId = db.Policies.Find(id);
			if(policyId == null)
			{
				return RedirectToAction("ViewPolicy");
			}
			else
			{
				return View(policyId);
			}

		}
        [Authorize(Roles = "Admin")]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePolicy(Policy p)
		{
			db.Policies.Remove(p);
			db.SaveChanges();
            ViewBag.InsuranceCompanyId = new SelectList(db.InsuranceCompanies, "InsuranceCompanyId", "Name");
			return RedirectToAction("ViewPolicy");
        }

		// Policy Work End.


		// Employee Support:

		// Add User Start:


		//public IActionResult AddUser()
		//{
		//	return View();
		//}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//      public IActionResult AddUser(User u)
		//      {
		//          var checkExictingUser = db.Users.FirstOrDefault(x => x.Email == u.Email);

		//          if (checkExictingUser != null)
		//          {
		//              ViewBag.msg = "User Already Exists";
		//              return View();
		//          }

		//          var hasher = new PasswordHasher<string>();
		//          u.Password = hasher.HashPassword(u.Email, u.Password);
		//          db.Users.Add(u);
		//          db.SaveChanges();
		//          return RedirectToAction("ViewEmployees");
		//      }

		//Add User End.

		// Add Employee Start:

		[Authorize(Roles = "Admin")]
		public IActionResult AddEmployee()
		{

			Employee emp = new Employee();
			ViewBag.Employee = emp;
			return View();
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
        public IActionResult AddEmployee(User u,Employee emp)
        {
            var checkExictingUser = db.Users.FirstOrDefault(x => x.Email == u.Email);

            if (checkExictingUser != null)
            {
                ViewBag.msg = "User Already Exists";
                return View();
            }

            var hasher = new PasswordHasher<string>();
            u.Password = hasher.HashPassword(u.Email, u.Password);
            db.Users.Add(u);
            db.SaveChanges();
         var insertedUser= db.Users.FirstOrDefault(m=>m.Email== u.Email);
			if(insertedUser != null)
			{
				emp.UserId = insertedUser.Id;
                    db.Employees.Add(emp);
                    db.SaveChanges();
                    return RedirectToAction("ViewEmployees");
               
            }
			else
			{
				return RedirectToAction("ViewEmployees");
			}

          
        }
		//Add Employee End.

		// Edit Employee Start:

		[Authorize(Roles = "Admin")]
		public IActionResult EditEmployee(int Id)
        {
			var empl = db.Employees.Include(e => e.User);
			var empdata = empl.FirstOrDefault(e => e.UserId==Id);
			if(empdata != null)
			{
                User user = new User() { 
				Username= empdata.User.Username,
				Email=empdata.User.Email,
				Password=empdata.User.Password,
				

				
				};
                ViewBag.User = user;
                return View(empdata);
			}
			else
			{
				ViewBag.msg = "Something went wrong.";
                return RedirectToAction("ViewEmployees");

            }

            
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditEmployee(User user, Employee emp)
        {

			var updUser= db.Users.FirstOrDefault(m=> m.Id==emp.UserId);
			var updEmp= db.Employees.FirstOrDefault(m=> m.UserId==emp.UserId);
			updUser.Email = user.Email;
			updUser.Password = user.Password;
			updUser.Username = user.Username;
			updEmp.Name = emp.Name;
			updEmp.Department = emp.Department;
			updEmp.Name = emp.Name;
			updEmp.Phone = emp.Phone;
            db.Users.Update(updUser);
			db.Employees.Update(updEmp);
            db.SaveChanges();
            return RedirectToAction("ViewEmployees");
}
		//DELETE Employee Start.
		[Authorize(Roles = "Admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteEmp(int Userid)
        {

			var updUser= db.Users.FirstOrDefault(m=> m.Id== Userid);
			var updEmp= db.Employees.FirstOrDefault(m=> m.UserId== Userid);
			if(updUser!=null && updEmp != null)
			{
                db.Employees.Remove(updEmp);
                db.Users.Remove(updUser);
                db.SaveChanges();
               
            }
            return RedirectToAction("ViewEmployees");
        }
		//DELETE Employee End.

		// View Employee List Start.

		[Authorize(Roles = "Admin")]
		public IActionResult ViewEmployees()
		{
			var empList = db.Employees.Include(e => e.User);
			return View(empList);
		}
		//View Employee List End.

		[Authorize(Roles = "Admin")]
		public IActionResult ViewPoliciesRequest()
        {
            var empList = db.Requests.Include(e=> e.Employee)
									 .Include(e=> e.Policy)
									 .Include(e => e.InsuranceCompany).ToList();
            return View(empList);
        }

		[Authorize(Roles = "Admin")]
		public IActionResult EditPoliciesRequest(int id)
		{
			var reqId = db.Requests.Find(id);
			return View(reqId);
		}
		[Authorize(Roles = "Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
        public IActionResult EditPoliciesRequest(Request r)
        {
            db.Requests.Update(r);
			db.SaveChanges();
            return View("ViewPoliciesRequest");
        }
    }
}
