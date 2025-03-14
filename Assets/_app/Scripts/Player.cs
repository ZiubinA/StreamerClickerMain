using SQLite4Unity3d;

public class Player
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public int AmountCoins { get; set; }
    public string Camera { get; set; }
    public string Keyboard { get; set; }
    public string Computer { get; set; }

    public override string ToString()
    {
        return string.Format("[Person: Id={0}, Name={1},  AmountCoins={2}, Camera={3},  Keyboard={4}, Computer={5}]", Id, Name, AmountCoins, Camera, Keyboard, Computer);
    }
}
