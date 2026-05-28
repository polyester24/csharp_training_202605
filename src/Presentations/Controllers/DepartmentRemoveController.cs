using Microsoft.AspNetCore.Mvc;
using csharp_training_202605.Applications.Domains;
using csharp_training_202605.Applications.Services;
using csharp_training_202605.Applications.Repositories;
using csharp_training_202605.Presentations.ViewModels;
using csharp_training_202605.Presentations.Controllers;

namespace csharp_training_202605.Presentations.Controllers;

[Route("DepartmentRemove")]

public class DepartmentRemoveController : Controller
{
    private readonly ILogger<EmployeeRegisterController> _logger;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDepartmentRemoveService _departmentRemoveService;
    private readonly IEmployeeRegisterService _employeeRegisterService;
    private readonly DepartmentRemoveViewModelAdapter _adapter;
    private readonly TempDataStore<DepartmentRemoveViewModel> _deptDataStore;

    public DepartmentRemoveController(
        ILogger<EmployeeRegisterController> logger,
        IDepartmentRemoveService departmentRemoveService,
        IEmployeeRegisterService employeeRegisterService,
        IDepartmentRepository departmentRepository,
        DepartmentRemoveViewModelAdapter departmentRemoveViewModelAdapter,
        TempDataStore<DepartmentRemoveViewModel> deptDataStore)
    {
        _logger = logger;
        _departmentRepository = departmentRepository;
        _departmentRemoveService = departmentRemoveService;
        _adapter = departmentRemoveViewModelAdapter;
        _deptDataStore = deptDataStore;
        _employeeRegisterService = employeeRegisterService;
    }

    [HttpGet("Enter")]
    public IActionResult Enter()
    {
        DepartmentRemoveViewModel? viewModel = null;
        viewModel = _deptDataStore.Load(this);
        if (viewModel == null)
        {

            viewModel = new DepartmentRemoveViewModel();
        }
        PopulateDepartments(viewModel);
        return View(viewModel);
    }

    [HttpPost("Confirm")]
    public IActionResult Confirm(DepartmentRemoveViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("Enter", viewModel);
        }
        var department = _employeeRegisterService.GetById(viewModel.DeptId ?? 0);
        _logger.LogInformation($"部署Id:{viewModel.DeptId ?? 0}の部署を取得する");
        // ViewModelに部署名を設定する
        viewModel.DeptName = department.Name;
        return View(viewModel);
    }

    [HttpPost("Remove")]
    public IActionResult Remove(DepartmentRemoveViewModel viewModel)
    {
        _deptDataStore.Save(this, viewModel);
        return RedirectToAction("Complete");
    }

    [HttpGet("Complete")]
    public IActionResult Complete()
    {
        DepartmentRemoveViewModel? viewModel = null!;
        viewModel = _deptDataStore.Load(this);
        if (viewModel == null)
        {
            return RedirectToAction("Enter");
        }
        viewModel.DeptName = "削除用データ";
        var department = _adapter.Restore(viewModel!);
        _departmentRemoveService.Remove(department);

        return View(viewModel);
    }
    private void PopulateDepartments(DepartmentRemoveViewModel viewModel)
    {
        // 従業員登録サービスから部署一覧を取得する
        var departments = _employeeRegisterService.GetDepartments();
        // 部署一覧をEmployeeRegisterViewModelに登録する
        viewModel.SetDepartments(departments);
        _logger.LogInformation("部署リストを設定");
    }
}