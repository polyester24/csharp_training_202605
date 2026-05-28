using Microsoft.AspNetCore.Mvc;
using csharp_training_202605.Applications.Domains;
using csharp_training_202605.Applications.Services;
using csharp_training_202605.Applications.Repositories;
using csharp_training_202605.Presentations.ViewModels;
using csharp_training_202605.Presentations.Controllers;

namespace csharp_training_202605.Presentations.Controllers;

[Route("EmployeeRemove")]

public class EmployeeRemoveController : Controller
{
    private readonly ILogger<EmployeeRegisterController> _logger;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEmployeeRemoveService _employeeRemoveService;
    private readonly IEmployeeRegisterService _employeeRegisterService;
    private readonly EmployeeRemoveViewModelAdapter _adapter;
    private readonly TempDataStore<EmployeeRemoveViewModel> _deptDataStore;

    public EmployeeRemoveController(
        ILogger<EmployeeRegisterController> logger,
        IEmployeeRemoveService employeeRemoveService,
        IEmployeeRegisterService employeeRegisterService,
        IDepartmentRepository departmentRepository,
        EmployeeRemoveViewModelAdapter employeeRemoveViewModelAdapter,
        TempDataStore<EmployeeRemoveViewModel> deptDataStore)
    {
        _logger = logger;
        _departmentRepository = departmentRepository;
        _employeeRemoveService = employeeRemoveService;
        _adapter = employeeRemoveViewModelAdapter;
        _deptDataStore = deptDataStore;
        _employeeRegisterService = employeeRegisterService;
    }

    [HttpGet("Enter")]
    public IActionResult Enter()
    {
        EmployeeRemoveViewModel? viewModel = null;
        viewModel = _deptDataStore.Load(this);
        if (viewModel == null)
        {

            viewModel = new EmployeeRemoveViewModel();
        }
        PopulateEmployees(viewModel);
        return View(viewModel);
    }

    [HttpPost("Confirm")]
    public IActionResult Confirm(EmployeeRemoveViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("Enter", viewModel);
        }
        var employee = _employeeRegisterService.GetByEmpId(viewModel.Id ?? 0);
        _logger.LogInformation($"部署Id:{viewModel.Id ?? 0}の部署を取得する");
        // ViewModelに部署名を設定する
        viewModel.Name = employee.Name;
        viewModel.Email = employee.Email;
        return View(viewModel);
    }

    [HttpPost("Remove")]
    public IActionResult Remove(EmployeeRemoveViewModel viewModel)
    {
        viewModel.Id = 0;
        _deptDataStore.Save(this, viewModel);
        return RedirectToAction("Complete");
    }

    [HttpGet("Complete")]
    public IActionResult Complete()
    {
        EmployeeRemoveViewModel? viewModel = null!;
        viewModel = _deptDataStore.Load(this);
        if (viewModel == null)
        {
            return RedirectToAction("Enter");
        }
        viewModel.Id = 0;
        var employee = _adapter.Restore(viewModel!);
        _employeeRemoveService.Remove(employee);

        return View(viewModel);
    }
    private void PopulateEmployees(EmployeeRemoveViewModel viewModel)
    {
        // 従業員登録サービスから部署一覧を取得する
        var employees = _employeeRegisterService.GetEmployees();
        // 部署一覧をEmployeeRegisterViewModelに登録する
        viewModel.SetEmployees(employees);
        _logger.LogInformation("部署リストを設定");
    }
}