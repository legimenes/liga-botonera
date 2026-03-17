namespace LigaBotonera.Entities;
public class Club(
    Guid id,
    string name,
    string fullName,
    int cityId)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string FullName { get; set; } = fullName;
    public int CityId { get; set; } = cityId;

    public virtual City City { get; set; } = null!;
}