namespace LigaBotonera.Entities;
public class City(
    int id,
    string name,
    int stateId)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public int StateId { get; set; } = stateId;

    public virtual State State { get; set; } = null!;
}