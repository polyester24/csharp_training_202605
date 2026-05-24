using Microsoft.AspNetCore.Mvc;
using csharp_training_202605.Applications.Domains;
using csharp_training_202605.Applications.Repositories;
using csharp_training_202605.Presentations.ViewModels;
using csharp_training_202605.Presentations.Controllers;

namespace csharp_training_202605.Presentations.Controllers;

public class DepartmentController : Controller
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly DepartmentRegisterViewModelAdapter _adapter;
    private readonly TempDataStore<DepartmentRegisterViewModel> _deptDataStore;

    public DepartmentController(
        IDepartmentRepository departmentRepository,
        DepartmentRegisterViewModelAdapter adapter,
        TempDataStore<DepartmentRegisterViewModel> deptDataStore)
    {
        _departmentRepository = departmentRepository;
        _adapter = adapter;
        _deptDataStore = deptDataStore;
    }

    public IActionResult Index()
    {
        var departments = _departmentRepository.FindAll();
        return View(departments);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new DepartmentRegisterViewModel());
    }

    [HttpPost("Department/Confirm")]
    public IActionResult Confirm(DepartmentRegisterViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("Create", viewModel);
        }
        return View(viewModel);
    }

    [HttpPost("Department/Register")]
    public IActionResult Register(DepartmentRegisterViewModel viewModel)
    {
        _deptDataStore.Save(this, viewModel);
        return RedirectToAction("Complete");
    }

    [HttpGet("Department/Complete")]
    public IActionResult Complete()
    {
        DepartmentRegisterViewModel? viewModel = _deptDataStore.Load(this);
        if (viewModel == null)
        {
            return RedirectToAction("Create");
        }

        var department = _adapter.Restore(viewModel);
        _departmentRepository.Create(department);

        return View(viewModel);
    }
}
