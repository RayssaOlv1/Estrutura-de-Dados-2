namespace ProjetoMedicamentos.Models;

public class Lote
{
    private int _id;
    private int _qtde;
    private DateTime _venc;

    public Lote() { }

    public Lote(int id, int qtde, DateTime venc)
    {
        if (id <= 0) throw new ArgumentException("O id do lote deve ser maior que zero.");
        if (qtde <= 0) throw new ArgumentException("A quantidade do lote deve ser maior que zero.");
        _id = id;
        _qtde = qtde;
        _venc = venc.Date;
    }

    public int Id => _id;
    public int Qtde => _qtde;
    public DateTime Venc => _venc;
    public void Retirar(int quantidade) { if (quantidade < 0 || quantidade > _qtde) throw new ArgumentException("Quantidade inválida."); _qtde -= quantidade; }
    public override string ToString() => $"Lote {_id} - quantidade: {_qtde} - vencimento: {_venc:dd/MM/yyyy}";
}
