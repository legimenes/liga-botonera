namespace LigaBotonera.Entities;
public class City(
    int id,
    string name,
    string state)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string State { get; set; } = state;
}