
namespace Template.Backend.Model
{
    public interface IEntity
    {
        int ID { get; set; }
        int RowVersion { get; set; }
        DateTime? CreatedOn { get; set; }
    }
}
