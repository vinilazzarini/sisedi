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
    public class Autores
    {
        List<Autor> bancoAutores = new List<Autor>();
        // Caminho Banco
        string caminhoBanco = ConfigurationManager.AppSettings["caminhoBanco"];
        if (caminhoBanco == null )
        {
            caminhoBanco = AppDomain.CurrentDomain.BaseDirectory;
        }

        string nomeBancoAutores = ConfigurationManager.AppSettings["nomeBancoAutores"];

        if (string.IsNullOrEmpty(nomeBancoAutores) == false)
        {
            nomeBancoAutores = "autores.csv";
        }
        

        // Salvar Autores
        public void SalvarAutoresEmArquivoCsv()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(caminhoBanco + nomeBancoAutores))
                {
                    writer.WriteLine("codigo,nome,sigla");
                    foreach (var autor in bancoAutores)
                    {
                        writer.WriteLine(
                            $"{autor.autid}," +
                            $"{autor.autnome}," +
                            $"{autor.autpseudonimo}," +
                            $"{autor.autobservacoes}"
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao salvar a lista de autores em  CSV: {ex.Message}");
            }
        }

        // Carregar Autores
        public List<Autor> CarregarAutoresDeArquivoCsv()
        {
            var bancoAutores = new List<Autor>();
            try
            {
                if (File.Exists(caminhoBanco + nomeBancoAutores) == true)
                {
                    using (StreamReader reader = new StreamReader(caminhoBanco + nomeBancoAutores))
                    {
                        string linha = reader.ReadLine();
                        while ((linha = reader.ReadLine()) != null)
                        {
                            var partes = linha.Split(',');
                            if (partes.Length == 4)
                            {
                                int codigo = int.Parse(partes[0]);
                                string nome = partes[1];
                                string pseudonimo = partes[2];
                                string observacao = partes[3];
                                bancoAutores.Add(new Autor
                                {
                                    autid = codigo,
                                    autnome = nome,
                                    autpseudonimo = pseudonimo,
                                    autobservacoes = observacao,
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
            return bancoAutores;
        }

        public Autores()
        {
            bancoAutores = CarregarAutoresDeArquivoCsv();
        }


        public void inserir(Autor autor)
        {
            bancoAutores.Add(autor);
            Console.WriteLine("Autor inserida com sucesso");
        }

        public void alterar(int id, Autor autAlterado)
        {
            foreach (var autor in bancoAutores)
            {
                if (autor.autid == id)
                {
                    autor.autnome = autAlterado.autnome;
                    autor.autpseudonimo = autAlterado.autpseudonimo;
                    autor.autobservacoes = autAlterado.autobservacoes;
                    break;
                }
            }
        }

        public void excluir(int id)
        {
            foreach (var autor in bancoAutores)
            {
                if (autor.autid == id)
                {
                    bancoAutores.Remove(autor);
                    break;
                }
            }
        }

        public void pesquisar(int id)
        {
            foreach (var autor in bancoAutores)
            {
                if (autor.autid == id)
                {
                    Console.WriteLine(
                        autor.autid + " - " +
                        autor.autnome + " - " +
                        autor.autpseudonimo + " - " +
                        autor.autobservacoes

                    );
                }
            }
        }

        public void exibirTodos()
        {
            foreach (var autExi in bancoAutores)
            {
                Console.WriteLine(
                    autExi.autid + " - " +
                    autExi.autnome + " - " +
                    autExi.autpseudonimo + " - " +
                    autExi.autobservacoes
                );
            }
        }

    }
}
