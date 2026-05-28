using System.ComponentModel.DataAnnotations;

namespace csharp_training_202605.Presentations.ViewModels;

/// <summary>
/// 部署登録ViewModel
/// </summary>
public class DepartmentRegisterViewModel
{
    [Display(Name = "部署名")]
    [Required(ErrorMessage = "{0}は入力必須です。")]
    [StringLength(20, ErrorMessage = "{0}は{1}文字以内で入力してください。")]
    public string? Name { get; set; }
    public override string ToString()
    {
        return $"Name={Name}";
    }
}
