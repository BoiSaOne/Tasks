using System.ComponentModel.DataAnnotations.Schema;

namespace TaskOne.Models
{
    /// <summary>
    /// Отдел
    /// </summary>
    [Table(nameof(Department))]
    public class Department
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;
        /// <summary>
        /// Список сотрудников
        /// </summary>
        ICollection<Employee>? Employees { get; set; }
    }
}
