using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Applications.Services;
/// <summary>
/// 部署登録サービスインターフェイス
/// </summary>
public interface IDepartmentRegisterService
{
    void Register(Department department);
}