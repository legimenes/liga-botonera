namespace LigaBotonera.Entities;
public class Club(
    Guid id,
    string name,
    string fullName,
    string city,
    string state)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string FullName { get; set; } = fullName;
    public string City { get; set; } = city;
    public string State { get; set; } = state;
}