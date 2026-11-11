namespace ProjetoMedicamentos.Models;

public class Medicamento
{
    private int _id;
    private string _nome = string.Empty;
    private string _laboratorio = string.Empty;
    private readonly Queue<Lote> _lotes = new();

    public Medicamento() { }

    public Medicamento(int id, string nome, string laboratorio)
    {
        if (id <= 0) throw new ArgumentException("O id deve ser maior que zero.");
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("O nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(laboratorio)) throw new ArgumentException("O laboratório é obrigatório.");
        _id = id;
        _nome = nome.Trim();
        _laboratorio = laboratorio.Trim();
    }

    public int Id => _id;
    public string Nome => _nome;
    public string Laboratorio => _laboratorio;
    public IReadOnlyCollection<Lote> Lotes => _lotes.ToArray();
    public bool Vazio => _id == 0;

    public int QtdeDisponivel() => _lotes.Sum(lote => lote.Qtde);

    public void Comprar(Lote lote)
    {
        ArgumentNullException.ThrowIfNull(lote);
        _lotes.Enqueue(lote);
    }

    public bool Vender(int qtde)
    {
        if (qtde <= 0 || qtde > QtdeDisponivel()) return false;
        var restante = qtde;
        while (restante > 0 && _lotes.Count > 0)
        {
            var lote = _lotes.Peek();
            var retirada = Math.Min(restante, lote.Qtde);
            lote.Retirar(retirada);
            restante -= retirada;
            if (lote.Qtde == 0) _lotes.Dequeue();
        }
        return true;
    }

    public override string ToString() => $"{Id} - {Nome} - {Laboratorio} - disponível: {QtdeDisponivel()}";
    public override bool Equals(object? obj) => obj is Medicamento outro && Id == outro.Id;
    public override int GetHashCode() => Id.GetHashCode();
}
