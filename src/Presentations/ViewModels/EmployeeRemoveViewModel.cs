using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using csharp_training_202605.Applications.Domains;

namespace csharp_training_202605.Presentations.ViewModels;

/// <summary>
/// 部署登録ViewModel
/// </summary>
public class EmployeeRemoveViewModel
{
    [Display(Name = "ID")]
    public int? Id { get; set; } = 0;
    [Display(Name = "氏名")]

    public string? Name { get; set; } = string.Empty;
    [Display(Name = "メールアドレス")]
    public string? Email { get; set; } = string.Empty;
    /// <summary>
    /// 所属部署
    /// </summary>
    [Display(Name = "所属部署")]
    public int? DeptId { get; set; } = 0;
    [Display(Name = "部署名")]
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 部署のリストをSelectListItemのリストに変換してプロパティに設定する
    /// </summary>
    /// <param name="employees"></param>
    public void SetEmployees(List<Employee> employees)
    {
        // SelectListItemのリストを作成
        var selectItems = new List<SelectListItem>();
        foreach (var emp in employees)
        {
            if (emp.Id.HasValue)
            {
                var item = new SelectListItem();
                item.Value = emp.Id.Value.ToString();
                item.Text = string.IsNullOrEmpty(emp.Name) ? "(名称未設定)" : emp.Name;
                selectItems.Add(item);
            }
        }
        Employees = selectItems;
    }
    // 部署のリスト
    public List<SelectListItem>? Employees { get; set; } = null;
}
