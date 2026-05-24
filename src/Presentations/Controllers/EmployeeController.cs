using Microsoft.AspNetCore.Mvc;
using csharp_training_202605.Applications.Repositories;

namespace csharp_training_202605.Presentations.Controllers;

public class EmployeeController : Controller
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public IActionResult Index()
    {
        var employees = _employeeRepository.FindAll();
        return View(employees);
    }
}
