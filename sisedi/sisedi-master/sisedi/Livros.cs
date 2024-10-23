using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisedi
{
    internal class Livros
    {
        private List<Livro> bancoLivros = new List<Livro>();
        string caminhoBanco = ConfigurationManager.AppSettings["caminhoBanco"];
        string nomeBancoLivros = ConfigurationManager.AppSettings["nomeBancoLivros"];

        // Salvar Livros
        public void SalvarLivrosEmArquivoCsv()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(caminhoBanco + nomeBancoLivros))
                {
                    writer.WriteLine("id,nome,anopublicacao,isbn,observacoes,ideditora");
                    foreach (var livro in bancoLivros)
                    {
                        writer.WriteLine(
                            $"{livro.livid}," +
                            $"{livro.livnome}," +
                            $"{livro.livanopublicacao}," +
                            $"{livro.livisbn}," +
                            $"{livro.livobservacoes}," +
                            $"{livro.ediid},"
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao salvar a lista de livros em  CSV: {ex.Message}");
            }
        }

        // Carregar Livros
        public List<Livro> CarregarLivrosDeArquivoCsv()
        {
            var bancoLivros = new List<Livro>();
            try
            {
                if (File.Exists(caminhoBanco + nomeBancoLivros) == true)
                {
                    using (StreamReader reader = new StreamReader(caminhoBanco + nomeBancoLivros))
                    {
                        string linha = reader.ReadLine();
                        while ((linha = reader.ReadLine()) != null)
                        {
                            var partes = linha.Split(',');
                            if (partes.Length == 6)
                            {
                                int codigo = int.Parse(partes[0]);
                                string nome = partes[1];
                                int ano = int.Parse(partes[2]);
                                int isbn = int.Parse(partes[3]);
                                string observacao = partes[4];
                                int ideditora = int.Parse(partes[5]);
                                bancoLivros.Add(new Livro
                                {
                                    livid = codigo,
                                    livnome = nome,
                                    livanopublicacao = ano,
                                    livisbn = isbn,
                                    livobservacoes = observacao,
                                    ediid = ideditora
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao carregar a lista de livros de CSV: {ex.Message}");
            }
            return bancoLivros;
        }

        public Livros()
        {
            bancoLivros = CarregarLivrosDeArquivoCsv();
        }

        public void inserir(Livro Livro)
        {
            bancoLivros.Add(Livro);
            Console.WriteLine("Livro inserida com sucesso");
        }

        public void alterar(int isbn, Livro livAlterado)
        {
            foreach (var livro in bancoLivros)
            {
                if (livro.livisbn == isbn)
                {
                    livro.livnome = livAlterado.livnome;
                    livro.livanopublicacao = livAlterado.livanopublicacao;
                    livro.livisbn = livAlterado.livisbn;
                    livro.livobservacoes = livAlterado.livobservacoes;
                    livro.ediid = livAlterado.ediid;
                    break;
                }
            }
        }

        public void excluir(int isbn)
        {
            foreach (var livro in bancoLivros)
            {
                if (livro.livisbn == isbn)
                {
                    bancoLivros.Remove(livro);
                    break;
                }
            }
        }

        public void pesquisar(int isbn)
        {
            foreach (var livPes in bancoLivros)
            {
                if (livPes.livisbn == isbn)
                {
                    Console.WriteLine(
                        livPes.livid + " - " +
                        livPes.livnome + " - " +
                        livPes.livanopublicacao + " - " +
                        livPes.livisbn + " - " +
                        livPes.livobservacoes + " - " +
                        livPes.ediid

                    );
                }
            }
        }

        public void exibirTodos()
        {
            foreach (var livExi in bancoLivros)
            {
                Console.WriteLine(
                    livExi.livid + " - " +
                    livExi.livnome + " - " +
                    livExi.livanopublicacao + " - " +
                    livExi.livisbn + " - " +
                    livExi.livobservacoes + " - " +
                    livExi.ediid
                );
            }
        }
    }
}
