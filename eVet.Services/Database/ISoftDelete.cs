namespace eVet.Services.Database;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }

    DateTime? VrijemeBrisanja { get; set; }
}
