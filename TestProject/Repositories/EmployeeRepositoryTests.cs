
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using csharp_training_202605.Applications.Domains;
using csharp_training_202605.Infrastructures.Adapters;
using csharp_training_202605.Infrastructures.Context;
using csharp_training_202605.Infrastructures.Repositories;

namespace csharp_training_202605.Test.Infrastructures.Repositories;

[DoNotParallelize]
[TestClass]
public class EmployeeRepositoryTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=csharp_training_202605;Username=postgres;Password=training;";

    private EmployeeRepository _repository = null!;
    private DepartmentRepository _drepository = null!;

    private AppDbContext _context = null!;

    [TestInitialize]
    public void Setup()
    {
        var adapter = new EmployeeEntityAdapter();
        var dadapter = new DepartmentEntityAdapter();
        var itemAdapter = new EmployeeEntityAdapter();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        _context = new AppDbContext(options);

        var path = Path.Combine(AppContext.BaseDirectory, "sql", "init.sql");
        var sql = File.ReadAllText(path);
        _context.Database.ExecuteSqlRaw(sql);

        _repository = new EmployeeRepository(_context, adapter);
        _drepository = new DepartmentRepository(_context, dadapter);

    }



    [TestMethod]
    public void FindById_WhenIdCorrect()
    {
        var actual = _repository.FindById(1);

        IsNotNull(actual);
        AreEqual(1, actual.Id);
        AreEqual("田中太郎", actual.Name);
        AreEqual(2, actual.Department!.Id);
        AreEqual("tanakatarou@csharp.com", actual.Email);
    }

    [TestMethod]
    public void FindById_WhenIdNotFound()
    {
        var actual = _repository.FindById(999);
        IsNull(actual);
    }
    [TestMethod]
    public void FindAll_Result()
    {
        var department = new Department("a");
        var actual = _repository.FindAll();

        AreEqual(8, actual.Count);
        IsTrue(actual.Any(c => c.Equals(new Employee(1, "田中太郎", "tanakatarou@csharp.com", _drepository.FindById(2)))));
        IsTrue(actual.Any(c => c.Equals(new Employee(2, "鈴木三郎", "suzukisaburou@csharp.com", _drepository.FindById(1)))));
        IsTrue(actual.Any(c => c.Equals(new Employee(3, "佐藤花子", "sastouhanako@csharp.com", _drepository.FindById(4)))));
        IsTrue(actual.Any(c => c.Equals(new Employee(4, "中田彩子", "nakataayako@csharp.com", _drepository.FindById(5)))));
        IsTrue(actual.Any(c => c.Equals(new Employee(5, "加藤圭太", "katoukeita@csharp.com", _drepository.FindById(3)))));
        IsTrue(actual.Any(c => c.Equals(new Employee(6, "松本良太", "matumotoryouta@csharp.com", _drepository.FindById(4)))));
        IsTrue(actual.Any(c => c.Equals(new Employee(7, "山下孝輔", "yamasitakousuke@csharp.com", _drepository.FindById(5)))));
        IsTrue(actual.Any(c => c.Equals(new Employee(8, "渡辺大輔", "watanabedaisuke@csharp.com", _drepository.FindById(4)))));


    }
    [TestMethod]
    public void Create_WhenCorrect()
    {
        var beforeCount = _context.Employees.Count();
        var department = new Department("aaa");
        var employee = new Employee("a", "b", department);

        _repository.Create(employee);

        var afterCount = _context.Employees.Count();
        AreEqual(beforeCount + 1, afterCount);

    }
    [TestMethod]
    public void Create_Correct()
    {
        var employee = new Employee("a", "b", _drepository.FindById(1));

        _repository.Create(employee);
        //var result = _repository.FindAll();
        var actual = _repository.FindById(9);
        //var find = result.Where(d => d.Name == "aaa");
        AreEqual(("a", "b", _drepository.FindById(1)), (actual?.Name, actual?.Email, actual?.Department));

    }

}