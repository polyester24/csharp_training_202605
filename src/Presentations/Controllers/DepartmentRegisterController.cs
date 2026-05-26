using Microsoft.AspNetCore.Mvc;
using csharp_training_202605.Applications.Domains;
using csharp_training_202605.Applications.Services;
using csharp_training_202605.Applications.Repositories;
using csharp_training_202605.Presentations.ViewModels;
using csharp_training_202605.Presentations.Controllers;

namespace csharp_training_202605.Presentations.Controllers;

[Route("DepartmentRegister")]

public class DepartmentRegisterController : Controller
{
    private readonly ILogger<EmployeeRegisterController> _logger;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDepartmentRegisterService _departmentRegisterService;
    private readonly DepartmentRegisterViewModelAdapter _adapter;
    private readonly TempDataStore<DepartmentRegisterViewModel> _deptDataStore;

    public DepartmentRegisterController(
        ILogger<EmployeeRegisterController> logger,
        IDepartmentRegisterService departmentRegisterService,
        IDepartmentRepository departmentRepository,
        DepartmentRegisterViewModelAdapter departmentRegisterViewModelAdapter,
        TempDataStore<DepartmentRegisterViewModel> deptDataStore)
    {
        _logger = logger;
        _departmentRepository = departmentRepository;
        _adapter = departmentRegisterViewModelAdapter;
        _deptDataStore = deptDataStore;
    }

    public IActionResult Index()
    {
        var departments = _departmentRepository.FindAll();
        return View(departments);
    }

    [HttpGet("Enter")]
    public IActionResult Enter()
    {
        return View(new DepartmentRegisterViewModel());
    }

    [HttpPost("Confirm")]
    public IActionResult Confirm(DepartmentRegisterViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("Enter", viewModel);
        }
        return View(viewModel);
    }

    [HttpPost("Register")]
    public IActionResult Register(DepartmentRegisterViewModel viewModel)
    {
        _deptDataStore.Save(this, viewModel);
        return RedirectToAction("Complete");
    }

    [HttpGet("Complete")]
    public IActionResult Complete()
    {
        DepartmentRegisterViewModel? viewModel = _deptDataStore.Load(this);
        if (viewModel == null)
        {
            return RedirectToAction("Enter");
        }

        var department = _adapter.Restore(viewModel);
        _departmentRegisterService.Register(department);
        //_departmentRepository.Create(department);

        return View(viewModel);
    }
}
