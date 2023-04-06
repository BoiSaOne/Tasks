using System.ComponentModel.DataAnnotations.Schema;

namespace TaskOne.Models
{
    /// <summary>
    /// Сотрудник
    /// </summary>
    [Table(nameof(Employee))]
    public class Employee
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id отдела
        /// </summary>
        public int DepartmentId { get; set; }
        /// <summary>
        /// Отдел
        /// </summary>
        public Department Department { get; set; } = null!;
        /// <summary>
        /// Id родителя
        /// </summary>
        public int? ChiefId { get; set; }
        /// <summary>
        /// Родитель
        /// </summary>
        public Employee? Chief { get; set; }
        /// <summary>
        /// Имя сотрудника
        /// </summary>
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;
        /// <summary>
        /// Зарплата
        /// </summary>
        public int Salary { get; set; }
        /// <summary>
        /// Список потомков
        /// </summary>
        public ICollection<Employee>? Subordinates { get; set; }
    }
}
