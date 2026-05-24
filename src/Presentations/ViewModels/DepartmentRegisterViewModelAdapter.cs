using csharp_training_202605.Applications.Adapters;
using csharp_training_202605.Applications.Domains;
using csharp_training_202605.Presentations.ViewModels;

namespace csharp_training_202605.Presentations.ViewModels;

/// <summary>
/// DepartmentRegisterViewModelをドメインオブジェクト:Departmentに変換するアダプター
/// </summary>
public class DepartmentRegisterViewModelAdapter : IRestorer<Department, DepartmentRegisterViewModel>
{
    /// <summary>
    /// DepartmentRegisterViewModelをドメインオブジェクト:Departmentに変換する
    /// </summary>
    /// <param name="target">DepartmentRegisterViewModel</param>
    /// <returns>ドメインオブジェクト:Department</returns>
    public Department Restore(DepartmentRegisterViewModel target)
    {
        var department = new Department(target.Name);
        return department;
    }
}
