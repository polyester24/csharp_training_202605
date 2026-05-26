using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using csharp_training_202605.Applications.Domains;
using csharp_training_202605.Infrastructures.Adapters;
using csharp_training_202605.Infrastructures.Context;
using csharp_training_202605.Infrastructures.Repositories;
using Microsoft.VisualBasic;

namespace csharp_training_202605.Test.Infrastructures.Repositories;

[DoNotParallelize]
[TestClass]
public class DepartmentRepositoryTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=csharp_training_202605;Username=postgres;Password=training;";

    private DepartmentRepository _repository = null!;
    private AppDbContext _context = null!;

    [TestInitialize]
    public void Setup()
    {
        var adapter = new DepartmentEntityAdapter();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        _context = new AppDbContext(options);

        var path = Path.Combine(AppContext.BaseDirectory, "sql", "init.sql");
        var sql = File.ReadAllText(path);
        _context.Database.ExecuteSqlRaw(sql);

        _repository = new DepartmentRepository(_context, adapter);
    }

    [TestMethod]
    public void FindAll_Result()
    {
        var actual = _repository.FindAll();

        AreEqual(5, actual.Count);
        IsTrue(actual.Any(c => c.Equals(new Department(1, "総務部"))));
        IsTrue(actual.Any(c => c.Equals(new Department(2, "経理部"))));
        IsTrue(actual.Any(c => c.Equals(new Department(3, "人事部"))));
        IsTrue(actual.Any(c => c.Equals(new Department(4, "開発部"))));
        IsTrue(actual.Any(c => c.Equals(new Department(5, "営業部"))));

    }

    [TestMethod]
    public void FindById_WhenIdCorrect()
    {
        var expected = new Department(1, "総務部");
        var actual = _repository.FindById(1);

        AreEqual(expected, actual);
        AreEqual("総務部", actual?.Name);
    }

    [TestMethod]
    public void FindById_WhenIdNotFound()
    {
        var actual = _repository.FindById(999);
        IsNull(actual);
    }

    [TestMethod]
    public void Create_WhenCorrect()
    {
        var beforeCount = _context.Departments.Count();
        var department = new Department("aaa");

        _repository.Create(department);

        var afterCount = _context.Departments.Count();
        AreEqual(beforeCount + 1, afterCount);

    }
    [TestMethod]
    public void Create_Correct()
    {
        var department = new Department("aaa");

        _repository.Create(department);
        //var result = _repository.FindAll();
        var actual = _repository.FindById(6);
        //var find = result.Where(d => d.Name == "aaa");
        AreEqual("aaa", actual?.Name);

    }
}
