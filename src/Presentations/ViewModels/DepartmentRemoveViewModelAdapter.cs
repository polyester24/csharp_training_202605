using csharp_training_202605.Applications.Adapters;
using csharp_training_202605.Applications.Domains;
using csharp_training_202605.Presentations.ViewModels;

namespace csharp_training_202605.Presentations.ViewModels;

/// <summary>
/// DepartmentRegisterViewModelをドメインオブジェクト:Departmentに変換するアダプター
/// </summary>
public class DepartmentRemoveViewModelAdapter : IRestorer<Department, DepartmentRemoveViewModel>
{
    /// <summary>
    /// DepartmentRegisterViewModelをドメインオブジェクト:Departmentに変換する
    /// </summary>
    /// <param name="target">DepartmentRegisterViewModel</param>
    /// <returns>ドメインオブジェクト:Department</returns>
    public Department Restore(DepartmentRemoveViewModel target)
    {
        var department = new Department(target.DeptId!.Value, target.DeptName);
        return department;
    }
}