using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TaskOne.DataBase;
using TaskOne.Models;

var builder = WebApplication.CreateBuilder(args);
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseDefaultFiles();

//1
app.MapGet("/maxSalaryEF", (AppDbContext db) =>
    db.Employees.Where(e => e.Salary == db.Employees.Max(e => e.Salary))
        .Select(m => new { empleyeeId = m.Id, employeeName = m.Name, m.Salary }).FirstOrDefault());

app.MapGet("/maxSalarySql", () =>
{
    using (IDbConnection db = new SqlConnection(connectionString))
    {
        return db.Query("SELECT TOP (1) E.Id EmpleyeeId, E.Name EmployeeName, E.Salary FROM Employee E ORDER BY E.Salary DESC").FirstOrDefault();
    }
});

//2
app.MapGet("/countSQL", () => 
{
    using (IDbConnection db = new SqlConnection(connectionString))
    {
        var query = @"WITH Recursives AS (
            SELECT  E.Id, E.ChiefId, 0 CountStep FROM Employee E WHERE ChiefId IS NULL
            UNION ALL
            SELECT T.Id, T.ChiefId, R.CountStep +1 FROM Employee T INNER JOIN Recursives R ON T.ChiefId = R.Id)
            SELECT  Max(CountStep) MaxStep FROM Recursives";
        return db.Query(query).FirstOrDefault();
    }
});

//3
app.MapGet("/departmentMaxSumSalaryEF", (AppDbContext db) =>
    db.Employees.Include(e => e.Department).GroupBy(m => m.Department)
        .Select(m => new { Department = m.Key, SumSalary = m.Sum( s => s.Salary)})
        .OrderByDescending(m => m.SumSalary)
        .Select(m => new { departmentId = m.Department.Id, departmentName = m.Department.Name, sum = m.SumSalary })
        .FirstOrDefault());

app.MapGet("/departmentMaxSumSalarySQL", () =>
{
    using (IDbConnection db = new SqlConnection(connectionString))
    {
        return db.Query(@"SELECT TOP (1) D.Id DepartmentId, D.Name DepartmentName, SUM(E.Salary) Sum FROM Department D 
            LEFT JOIN Employee E ON E.DepartmentId = D.Id GROUP BY D.Id, D.Name ORDER BY SUM(E.Salary) DESC").FirstOrDefault();
    }
});

//4
app.MapGet("/employeeEF", (AppDbContext db) =>
    db.Employees.Where(e => EF.Functions.Like(e.Name, "Ð%í"))
        .Select(m => new { empleyeeId = m.Id, employeeName = m.Name }).FirstOrDefault());

app.MapGet("/employeeSQL", () =>
{
    using (IDbConnection db = new SqlConnection(connectionString))
    {
        return db.Query("SELECT TOP (1) E.Id EmpleyeeId, E.Name EmployeeName FROM Employee E WHERE E.Name LIKE 'Ð%í'").FirstOrDefault();
    }
});

app.Run();
