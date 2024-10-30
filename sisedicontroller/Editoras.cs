using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using sisedimodel;

namespace sisedicontroller
{
    public class Editoras
    {
        private List<Editora> bancoEditoras = new List<Editora>();
        string caminhoBanco = ConfigurationManager.AppSettings["caminhoBanco"];
        string nomeBancoEditoras = ConfigurationManager.AppSettings["nomeBancoEditoras"];

        // Salvar Editoras
        public void SalvarEditorasEmArquivoCsv()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(caminhoBanco + nomeBancoEditoras))
                {
                    writer.WriteLine("codigo,nome,sigla,observacoes");
                    foreach (var editora in bancoEditoras)
                    {
                        writer.WriteLine(
                            $"{editora.ediid}," +
                            $"{editora.edinome}," +
                            $"{editora.edisigla}," +
                            $"{editora.ediobservacoes}"
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao salvar a lista de editoras em  CSV: {ex.Message}");
            }
        }

        // Carregar editoras
        private List<Editora> CarregarEditorasDeArquivoCsv()
        {
            var bancoEditoras = new List<Editora>();
            try
            {
                if (File.Exists(caminhoBanco + nomeBancoEditoras) == true)
                {
                    using (StreamReader reader = new StreamReader(caminhoBanco + nomeBancoEditoras))
                    {
                        string linha = reader.ReadLine();
                        while ((linha = reader.ReadLine()) != null)
                        {
                            var partes = linha.Split(',');
                            if (partes.Length == 4)
                            {
                                int codigo = int.Parse(partes[0]);
                                string nome = partes[1];
                                string sigla = partes[2];
                                string observacao = partes[3];
                                bancoEditoras.Add(new Editora
                                {
                                    ediid = codigo,
                                    edinome = nome,
                                    edisigla = sigla,
                                    ediobservacoes = observacao
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao carregar a lista de editoras de CSV: {ex.Message}");
            }
            return bancoEditoras;
        }

        public Editoras()
        {
            bancoEditoras = CarregarEditorasDeArquivoCsv();
        }

        public void inserir(Editora editora)
        {
            bancoEditoras.Add(editora);
            Console.WriteLine("Editora inserida com sucesso");
        }

        public void alterar(string sigla, Editora ediAlterada)
        {
            foreach (var editora in bancoEditoras)
            {
                if (editora.edisigla == sigla)
                {
                    editora.ediid = ediAlterada.ediid;
                    editora.edinome = ediAlterada.edinome;
                    editora.edisigla = ediAlterada.edisigla;
                    editora.ediobservacoes = ediAlterada.ediobservacoes;

                    Console.WriteLine("Editora alterada com sucesso!");
                    break;
                }
            }
        }

        public void excluir(string sigla)
        {
            foreach (var editora in bancoEditoras)
            {
                if (editora.edisigla == sigla)
                {
                    bancoEditoras.Remove(editora);
                    break;
                }
            }
        }

        public void pesquisar(string sigla)
        {
            foreach (var editora in bancoEditoras)
            {
                if (editora.edisigla == sigla)
                {
                    Console.WriteLine(
                        editora.ediid + " - " +
                        editora.edinome + " - " +
                        editora.edisigla + " - " +
                        editora.ediobservacoes
                    );
                }
            }
        }

        public void exibirTodos()
        {
            foreach (var editora in bancoEditoras)
            {
                Console.WriteLine(
                    editora.ediid + " - " +
                    editora.edinome + " - " +
                    editora.edisigla + " - " +
                    editora.ediobservacoes
                );
            }
        }

    }
}
