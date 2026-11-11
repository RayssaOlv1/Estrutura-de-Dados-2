namespace ProjetoMedicamentos.Models;

public class Medicamentos
{
    private readonly List<Medicamento> _listaMedicamentos = new();
    public IReadOnlyList<Medicamento> ListaMedicamentos => _listaMedicamentos.AsReadOnly();

    public void Adicionar(Medicamento medicamento)
    {
        ArgumentNullException.ThrowIfNull(medicamento);
        if (Pesquisar(medicamento).Vazio) _listaMedicamentos.Add(medicamento);
        else throw new InvalidOperationException("Já existe um medicamento com este id.");
    }

    public bool Deletar(Medicamento medicamento)
    {
        ArgumentNullException.ThrowIfNull(medicamento);
        if (medicamento.QtdeDisponivel() != 0) return false;
        return _listaMedicamentos.Remove(medicamento);
    }

    public Medicamento Pesquisar(Medicamento medicamento)
    {
        ArgumentNullException.ThrowIfNull(medicamento);
        return _listaMedicamentos.FirstOrDefault(item => item.Id == medicamento.Id) ?? new Medicamento();
    }
}
