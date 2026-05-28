using csharp_training_202605.Infrastructures.Context;
using csharp_training_202605.Applications.Domains;
using csharp_training_202605.Applications.Repositories;
using csharp_training_202605.Infrastructures.Adapters;
using csharp_training_202605.Exceptions;
namespace csharp_training_202605.Infrastructures.Repositories;
/// <summary>
/// ドメインオブジェクト:従業員のCRUD操作インターフェイスの実装
/// </summary>
public class EmployeeRepository : IEmployeeRepository
{
    /// <summary>
    /// アプリケーション用DbContext
    /// </summary>
    private readonly AppDbContext _context;
    /// <summary>
    /// ドメインモデル:従業員と従業員エンティティの相互変換インターフェイスの実装
    /// </summary>
    private readonly EmployeeEntityAdapter _adapter;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context"></param>
    /// <param name="adapter"></param>
    public EmployeeRepository(AppDbContext context, EmployeeEntityAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    /// <summary>
    /// 従業員を永続化する
    /// </summary>
    /// <param name="employee">永続化対象の従業員</param>
    public void Create(Employee employee)
    {
        try
        {
            var entity = _adapter.Convert(employee);
            _context.Employees.Add(entity);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new InternalException(
                "従業員の永続化ができませんでした。", e);
        }
    }

    /// <summary>
    /// すべての従業員を取得する
    /// </summary>
    /// <returns>従業員のリスト</returns>
    public List<Employee> FindAll()
    {
        try
        {
            var departmentMap = _context.Departments.ToDictionary(d => d.DeptId);
            var entities = _context.Employees.ToList();
            var results = new List<Employee>();

            foreach (var entity in entities)
            {
                Department? department = null;
                if (entity.DeptId.HasValue && departmentMap.TryGetValue(entity.DeptId.Value, out var deptEntity))
                {
                    department = new Department(deptEntity.DeptId, deptEntity.DeptName);
                }

                results.Add(new Employee(
                    entity.EmpId,
                    entity.EmpName,
                    entity.Email,
                    department));
            }

            return results;
        }
        catch (Exception e)
        {
            throw new InternalException(
                "従業員一覧を取得できませんでした。", e);
        }
    }

    /// <summary>
    /// 指定された従業員Idの従業員を取得する
    /// </summary>
    /// <param name="id">従業員Id</param>
    /// <returns>従業員またはnull</returns>
    public Employee? FindById(int id)
    {
        try
        {
            var entity = _context.Employees.FirstOrDefault(e => e.EmpId == id);
            if (entity == null)
            {
                return null;
            }

            Department? department = null;
            if (entity.DeptId.HasValue)
            {
                var deptEntity = _context.Departments.FirstOrDefault(d => d.DeptId == entity.DeptId.Value);
                if (deptEntity != null)
                {
                    department = new Department(deptEntity.DeptId, deptEntity.DeptName);
                }
            }

            return new Employee(
                entity.EmpId,
                entity.EmpName,
                entity.Email,
                department);
        }
        catch (Exception e)
        {
            throw new InternalException(
                "指定された従業員Idの従業員を取得できませんでした。", e);
        }
    }
    public void Remove(Employee employee)
    {
        try
        {
            var entity = _adapter.Convert(employee);
            _context.Employees.Remove(entity);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new InternalException(
                "部署の削除に失敗しました。", e);
        }
    }
}