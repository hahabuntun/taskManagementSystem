using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Entities.Worker;

namespace Vkr.Domain.Entities.Organization
{
    public class Organizations
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; } = null!;
        
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Идентификатор владельца организации
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// Владелец организации
        /// </summary>
        public Workers Owner { get; set; } = null!;

        public ICollection<Projects> Projects { get; set; } = new List<Projects>();
        
        public ICollection<Workers> Workers { get; set; } = new List<Workers>();
    }
}