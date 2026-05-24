using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Applications.Repositories;
/// <summary>
/// ドメインオブジェクト:従業員のCRUD操作インターフェイス
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// 従業員を永続化する
    /// </summary>
    /// <param name="employee">永続化対象の従業員</param>
    void Create(Employee employee);

    /// <summary>
    /// すべての従業員を取得する
    /// </summary>
    /// <returns>従業員のリスト</returns>
    List<Employee> FindAll();

    /// <summary>
    /// 指定された従業員Idの従業員を取得する
    /// </summary>
    /// <param name="id">従業員Id</param>
    /// <returns>従業員またはnull</returns>
    Employee? FindById(int id);
}