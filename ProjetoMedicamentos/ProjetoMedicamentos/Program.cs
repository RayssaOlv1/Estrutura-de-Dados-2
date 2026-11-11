using ProjetoMedicamentos.Models;

namespace ProjetoMedicamentos;

internal static class Program
{
    private static readonly Medicamentos Catalogo = new();
    private static int _proximoIdMedicamento = 1;
    private static int _proximoIdLote = 1;

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var sair = false;
        while (!sair)
        {
            ExibirMenu();
            switch (LerInteiro("Escolha uma opção: "))
            {
                case 0: sair = true; break;
                case 1: CadastrarMedicamento(); break;
                case 2: ConsultarSintetico(); break;
                case 3: ConsultarAnalitico(); break;
                case 4: ComprarMedicamento(); break;
                case 5: VenderMedicamento(); break;
                case 6: ListarMedicamentos(); break;
                default: Avisar("Opção inválida."); break;
            }
            if (!sair) Pausar();
        }
        Console.WriteLine("Processo finalizado.");
    }

    private static void ExibirMenu()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("       CONTROLE DE MEDICAMENTOS");
        Console.WriteLine("========================================");
        Console.WriteLine("0. Finalizar processo");
        Console.WriteLine("1. Cadastrar medicamento");
        Console.WriteLine("2. Consultar medicamento (sintético)");
        Console.WriteLine("3. Consultar medicamento (analítico)");
        Console.WriteLine("4. Comprar medicamento (cadastrar lote)");
        Console.WriteLine("5. Vender medicamento");
        Console.WriteLine("6. Listar medicamentos");
        Console.WriteLine("========================================");
    }

    private static void CadastrarMedicamento()
    {
        Cabecalho("CADASTRAR MEDICAMENTO");
        try
        {
            var medicamento = new Medicamento(_proximoIdMedicamento++, LerTexto("Nome: "), LerTexto("Laboratório: "));
            Catalogo.Adicionar(medicamento);
            Informar($"Medicamento cadastrado com id {medicamento.Id}.");
        }
        catch (ArgumentException ex) { Avisar(ex.Message); }
    }

    private static Medicamento? LocalizarMedicamento()
    {
        var id = LerInteiro("Id do medicamento: ");
        if (id <= 0) { Avisar("O id deve ser maior que zero."); return null; }
        var medicamento = Catalogo.Pesquisar(new Medicamento(id, "consulta", "consulta"));
        if (medicamento.Vazio) { Avisar("Medicamento não encontrado."); return null; }
        return medicamento;
    }

    private static void ConsultarSintetico()
    {
        Cabecalho("CONSULTA SINTÉTICA");
        var medicamento = LocalizarMedicamento();
        if (medicamento is not null) Console.WriteLine(medicamento);
    }

    private static void ConsultarAnalitico()
    {
        Cabecalho("CONSULTA ANALÍTICA");
        var medicamento = LocalizarMedicamento();
        if (medicamento is null) return;
        Console.WriteLine(medicamento);
        if (medicamento.Lotes.Count == 0) Console.WriteLine("Nenhum lote cadastrado.");
        foreach (var lote in medicamento.Lotes) Console.WriteLine($"  - {lote}");
    }

    private static void ComprarMedicamento()
    {
        Cabecalho("COMPRAR MEDICAMENTO / CADASTRAR LOTE");
        var medicamento = LocalizarMedicamento();
        if (medicamento is null) return;
        try
        {
            var quantidade = LerInteiroPositivo("Quantidade do lote: ");
            var vencimento = LerData("Data de vencimento");
            var lote = new Lote(_proximoIdLote++, quantidade, vencimento);
            medicamento.Comprar(lote);
            Informar($"Lote {lote.Id} cadastrado para {medicamento.Nome}.");
        }
        catch (ArgumentException ex) { Avisar(ex.Message); }
    }

    private static void VenderMedicamento()
    {
        Cabecalho("VENDER MEDICAMENTO");
        var medicamento = LocalizarMedicamento();
        if (medicamento is null) return;
        var quantidade = LerInteiroPositivo("Quantidade para vender: ");
        if (medicamento.Vender(quantidade)) Informar($"Venda realizada. Estoque restante: {medicamento.QtdeDisponivel()}.");
        else Avisar($"Venda não realizada. Disponível: {medicamento.QtdeDisponivel()}.");
    }

    private static void ListarMedicamentos()
    {
        Cabecalho("LISTA DE MEDICAMENTOS");
        if (Catalogo.ListaMedicamentos.Count == 0) { Console.WriteLine("Nenhum medicamento cadastrado."); return; }
        foreach (var medicamento in Catalogo.ListaMedicamentos) Console.WriteLine(medicamento);
    }

    private static DateTime LerData(string rotulo)
    {
        while (true)
        {
            Console.WriteLine($"{rotulo}:");
            var dia = LerInteiro("Dia: "); var mes = LerInteiro("Mês: "); var ano = LerInteiro("Ano: ");
            try { return new DateTime(ano, mes, dia); }
            catch (ArgumentOutOfRangeException) { Avisar("Data inválida."); }
        }
    }

    private static int LerInteiroPositivo(string mensagem)
    {
        int valor;
        do { valor = LerInteiro(mensagem); if (valor <= 0) Avisar("Digite um valor maior que zero."); } while (valor <= 0);
        return valor;
    }

    private static int LerInteiro(string mensagem)
    {
        while (true) { Console.Write(mensagem); if (int.TryParse(Console.ReadLine(), out var valor)) return valor; Avisar("Digite um número inteiro válido."); }
    }

    private static string LerTexto(string mensagem)
    {
        while (true) { Console.Write(mensagem); var valor = Console.ReadLine()?.Trim(); if (!string.IsNullOrWhiteSpace(valor)) return valor; Avisar("Este campo é obrigatório."); }
    }

    private static void Cabecalho(string titulo) { Console.Clear(); Console.WriteLine($"--- {titulo} ---\n"); }
    private static void Avisar(string mensagem) => Console.WriteLine($"[Atenção] {mensagem}");
    private static void Informar(string mensagem) => Console.WriteLine($"[OK] {mensagem}");
    private static void Pausar() { Console.WriteLine("\nPressione ENTER para continuar..."); Console.ReadLine(); }
}
