public class FakeDatabase
{
    public List<object> Registros { get; } = new();

    public void Salvar(object dado)
    {
        Registros.Add(dado);
    }
}
