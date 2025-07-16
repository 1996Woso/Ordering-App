using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Ordering_App.Interfaces;
using Ordering_App.Models;
using Ordering_App.Models.DTOs;

namespace Ordering_App.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Employees([FromQuery] UserParams userParams,
         decimal minBalance, decimal maxBalance, string lastDepositMonth
         )
        {
            if (minBalance >= 0) userParams.MinBalance = minBalance;
            if (maxBalance != 0 && maxBalance != userParams.MaxBalance) userParams.MaxBalance = maxBalance;
            userParams.PageSize = 5;
            if (!lastDepositMonth.IsNullOrEmpty())
            {
                userParams.LastDepositMonth = await employeeRepository.ConvertMonthAsync(lastDepositMonth);
            }
            var users = await employeeRepository.GetAllAsync(userParams);
            ViewBag.TotalCount = users.TotalCount;
            ViewBag.TotalPages = users.TotalPages;
            ViewBag.CurrentPage = userParams.PageNumber;
            ViewBag.MinBalance = userParams.MinBalance;
            ViewBag.MaxBalance = userParams.MaxBalance;
            ViewBag.LastDepositMonth = userParams.LastDepositMonth;
            return View(users);
        }
        public IActionResult AddEmployee()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeDTO addEmployeeDTO)
        {
            string errorMessage = "", successMessage = "";
            if (await employeeRepository.UserExistsAsync(addEmployeeDTO.Name))
            {
                errorMessage = $"{addEmployeeDTO.Name.ToUpper()} allready exists.";
            }
            else
            {
                await employeeRepository.CreateAsync(addEmployeeDTO);
                if (await employeeRepository.SaveAllAsync())
                {
                    successMessage = $"{addEmployeeDTO.Name.ToUpper()} has been successful created.";
                }
            }
            TempData["Error"] = errorMessage;
            TempData["Success"] = successMessage;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Deposit(int id)
        {
            var employee = await employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                TempData["Error"] = "Employee not found";
                return RedirectToAction("Employees");
            }
            var updateEmployeeDTO = new UpdateEmployeeDTO { EmployeeNumber = employee.EmployeeNumber , Balance = employee.Balance, LastDepositMonth = $"{DateTime.Now.ToString("MMMM yyyy")}" };
            return View(updateEmployeeDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Deposit(UpdateEmployeeDTO updateEmployeeDTO)
        {
            string errorMessage = "", successMessage = "";
            if (await employeeRepository.GetByEmpoyeeNoAsync(updateEmployeeDTO.EmployeeNumber) == null)
            {
                errorMessage = "Employe cannot be found";
            }
            else
            {
                await employeeRepository.UpdateAsync(updateEmployeeDTO);
                if (await employeeRepository.SaveAllAsync())
                {
                    successMessage = "Deposit has been made successfully";
                }
            }
            TempData["Error"] = errorMessage;
            TempData["Success"] = successMessage;
            return View();
        }
    }
}
