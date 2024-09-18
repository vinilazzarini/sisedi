using sisedi;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace sisedi
{
    internal class Program
    {
        // Salvar Editoras
        static void SalvarEditorasEmArquivoCsv(List<Editora> bancoEditoras, string caminhoDoArquivo)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(caminhoDoArquivo))
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
        static List<Editora> CarregarEditorasDeArquivoCsv(string caminhoDoArquivo)
        {
            var bancoEditoras = new List<Editora>();
            try
            {
                if (File.Exists(caminhoDoArquivo) == true)
                {
                    using (StreamReader reader = new StreamReader(caminhoDoArquivo))
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
                                bancoEditoras.Add(new Editora{
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

        // Salvar Livros
        static void SalvarLivrosEmArquivoCsv(List<Livro> bancoLivros, string caminhoDoArquivo)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(caminhoDoArquivo))
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
        static List<Livro> CarregarLivrosDeArquivoCsv(string caminhoDoArquivo)
        {
            var bancoLivros = new List<Livro>();
            try
            {
                if (File.Exists(caminhoDoArquivo) == true)
                {
                    using (StreamReader reader = new StreamReader(caminhoDoArquivo))
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

        // Salvar Autores
        static void SalvarAutoresEmArquivoCsv(List<Autor> bancoAutores, string caminhoDoArquivo)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(caminhoDoArquivo))
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
        static List<Autor> CarregarAutoresDeArquivoCsv(string caminhoDoArquivo)
        {
            var bancoAutores = new List<Autor>();
            try
            {
                if (File.Exists(caminhoDoArquivo) == true)
                {
                    using (StreamReader reader = new StreamReader(caminhoDoArquivo))
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

        static void Main(string[] args)
        {

            int opc = 0;
            int opcsub = 0;

            // Inicialização bancos RAM
            List<Editora> bancoEditoras = new List<Editora>();
            List<Livro> bancoLivros = new List<Livro>();
            List<Autor> bancoAutores = new List<Autor>();
            
            // Caminho Banco
            string caminhoBanco = ConfigurationManager.AppSettings["caminhoBanco"];
            string nomeBancoEditoras = ConfigurationManager.AppSettings["nomeBancoEditoras"];
            string nomeBancoAutores = ConfigurationManager.AppSettings["nomeBancoAutores"];
            string nomeBancoLivros = ConfigurationManager.AppSettings["nomeBancoLivros"];

            // Carregar dados dos bancos para persistência em RAM
            bancoEditoras = CarregarEditorasDeArquivoCsv(caminhoBanco + nomeBancoEditoras);
            bancoLivros = CarregarLivrosDeArquivoCsv(caminhoBanco + nomeBancoLivros);
            bancoAutores = CarregarAutoresDeArquivoCsv(caminhoBanco + nomeBancoAutores);

            // Menu principal
            while (opc != 9)
            {
                Console.WriteLine("SISEDI");
                Console.WriteLine("1.Editoras");
                Console.WriteLine("2.Livros");
                Console.WriteLine("3.Autores");
                Console.WriteLine("9.Sair");
                Console.WriteLine("Digite a opção: ");

                opc = int.Parse(Console.ReadLine());

                opcsub = 0;
                // Operações Editoras
                if (opc == 1)
                {
                    // Menu Editoras
                    while (opcsub != 19)
                    {
                        Console.WriteLine("--------------");
                        Console.WriteLine("Editoras");
                        Console.WriteLine("10.Inserir");
                        Console.WriteLine("11.Alterar");
                        Console.WriteLine("12.Excluir");
                        Console.WriteLine("13.Pesquisar");
                        Console.WriteLine("14.Exibir");
                        Console.WriteLine("19. Sair");
                        Console.WriteLine("Digite a opção: ");
                        opcsub = int.Parse(Console.ReadLine());
                        Console.WriteLine("--------------");

                        switch (opcsub)
                        {

                            // Inserir Editora
                            case 10:
                                Editora itemIns = new Editora();
                                Console.WriteLine("Id editora: ");
                                itemIns.ediid = int.Parse(Console.ReadLine());
                                Console.WriteLine("Nome editora: ");
                                itemIns.edinome = Console.ReadLine();
                                Console.WriteLine("Sigla editora: ");
                                itemIns.edisigla = Console.ReadLine();
                                Console.WriteLine("Observações editora: ");
                                itemIns.ediobservacoes = Console.ReadLine();

                                bancoEditoras.Add(itemIns);

                                break;

                            // Alterar Editora
                            case 11:
                                Console.WriteLine("Sigla da editora para alterar: ");
                                string varSiglaAlt = Console.ReadLine();
                                foreach (var ediAlt in bancoEditoras)
                                {
                                    if (ediAlt.edisigla == varSiglaAlt)
                                    {
                                        Console.WriteLine("Nome editora: ");
                                        ediAlt.edinome = Console.ReadLine();
                                        Console.WriteLine("Sigla editora: ");
                                        ediAlt.edisigla = Console.ReadLine();
                                        Console.WriteLine("Observações editora: ");
                                        ediAlt.ediobservacoes = Console.ReadLine();
                                        break;
                                    }
                                }
                                break;

                            // Excluir Editora
                            case 12:
                                Console.WriteLine("Sigla da editora para excluir: ");
                                string varSiglaExc = Console.ReadLine();
                                foreach (var ediExc in bancoEditoras)
                                {
                                    if (ediExc.edisigla == varSiglaExc)
                                    {
                                        bancoEditoras.Remove(ediExc);
                                        break;
                                    }
                                }

                                break;

                            // Pesquisar Editora
                            case 13:
                                Console.WriteLine("Sigla da editora para pesquisar: ");
                                string varSiglaPes = Console.ReadLine();
                                foreach (var ediPes in bancoEditoras)
                                {
                                    if (ediPes.edisigla == varSiglaPes)
                                    {
                                        Console.WriteLine(
                                            ediPes.ediid + " - " +
                                            ediPes.edinome + " - " +
                                            ediPes.edisigla + " - " +
                                            ediPes.ediobservacoes
                                        );
                                    }
                                }

                                break;

                            // Exibir Editoras
                            case 14:
                                foreach (var ediExi in bancoEditoras)
                                {
                                    Console.WriteLine(
                                        ediExi.ediid + " - " +
                                        ediExi.edinome + " - " +
                                        ediExi.edisigla + " - " +
                                        ediExi.ediobservacoes
                                    );
                                }
                                break;
                        }
                    }
                }
                // Operações Livros
                else if (opc == 2)
                {
                    // Menu Livros
                    while (opcsub != 19)
                    {
                        Console.WriteLine("--------------");
                        Console.WriteLine("Livros");
                        Console.WriteLine("10.Inserir");
                        Console.WriteLine("11.Alterar");
                        Console.WriteLine("12.Excluir");
                        Console.WriteLine("13.Pesquisar");
                        Console.WriteLine("14.Exibir");
                        Console.WriteLine("19. Sair");
                        Console.WriteLine("Digite a opção: ");
                        opcsub = int.Parse(Console.ReadLine());
                        Console.WriteLine("--------------");

                        switch (opcsub)
                        {

                            // Inserir Livro
                            case 10:
                                Livro itemIns = new Livro();
                                Console.WriteLine("Id livro: ");
                                itemIns.livid = int.Parse(Console.ReadLine());
                                Console.WriteLine("Nome livro: ");
                                itemIns.livnome = Console.ReadLine();
                                Console.WriteLine("Ano de publicação livro: ");
                                itemIns.livanopublicacao = int.Parse(Console.ReadLine());
                                Console.WriteLine("Isbn do livro: ");
                                itemIns.livisbn = int.Parse(Console.ReadLine());
                                Console.WriteLine("Observações livro: ");
                                itemIns.livobservacoes = Console.ReadLine();
                                Console.WriteLine("Id editora livro: ");
                                itemIns.ediid = int.Parse(Console.ReadLine());

                                bancoLivros.Add(itemIns);

                                break;

                            // Alterar Livro
                            case 11:
                                Console.WriteLine("Isbn do livro para alterar: ");
                                int varIsnbAlt = int.Parse(Console.ReadLine());
                                foreach (var livAlt in bancoLivros)
                                {
                                    if (livAlt.livisbn == varIsnbAlt)
                                    {
                                        Console.WriteLine("Nome livro: ");
                                        livAlt.livnome = Console.ReadLine();
                                        Console.WriteLine("Ano publicação livro: ");
                                        livAlt.livanopublicacao = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Isbn do livro: ");
                                        livAlt.livisbn = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Observações livro: ");
                                        livAlt.livobservacoes = Console.ReadLine();
                                        Console.WriteLine("Id editora livro: ");
                                        livAlt.ediid = int.Parse(Console.ReadLine());
                                        break;
                                    }
                                }
                                break;

                            // Excluir Livro
                            case 12:
                                Console.WriteLine("Isbn do livro para excluir: ");
                                int varIsnbExc = int.Parse(Console.ReadLine());
                                foreach (var livExc in bancoLivros)
                                {
                                    if (livExc.livisbn == varIsnbExc)
                                    {
                                        bancoLivros.Remove(livExc);
                                        break;
                                    }
                                }

                                break;

                            // Pesquisar Livro
                            case 13:
                                Console.WriteLine("Isbn do livro para pesquisar: ");
                                int varIsnbPes = int.Parse(Console.ReadLine());
                                foreach (var livPes in bancoLivros) { 
                                    if (livPes.livisbn == varIsnbPes)
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

                                break;

                            // Exibir Livros
                            case 14:
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
                                break;
                        }
                    }
                }
                // Operações Autores
                else if (opc == 3)
                {
                    // Menu Autores
                    while (opcsub != 19)
                    {
                        Console.WriteLine("--------------");
                        Console.WriteLine("Autores");
                        Console.WriteLine("10.Inserir");
                        Console.WriteLine("11.Alterar");
                        Console.WriteLine("12.Excluir");
                        Console.WriteLine("13.Pesquisar");
                        Console.WriteLine("14.Exibir");
                        Console.WriteLine("19. Sair");
                        Console.WriteLine("Digite a opção: ");
                        opcsub = int.Parse(Console.ReadLine());
                        Console.WriteLine("--------------");


                        switch (opcsub)
                        {

                            // Inserir Autor
                            case 10:
                                Autor itemIns = new Autor();
                                Console.WriteLine("Id autor: ");
                                itemIns.autid = int.Parse(Console.ReadLine());
                                Console.WriteLine("Nome autor: ");
                                itemIns.autnome = Console.ReadLine();
                                Console.WriteLine("Pseudonimo do autor: ");
                                itemIns.autpseudonimo = Console.ReadLine();
                                Console.WriteLine("Observações autor: ");
                                itemIns.autobservacoes = Console.ReadLine();

                                bancoAutores.Add(itemIns);

                                break;

                            // Alterar Autor
                            case 11:
                                Console.WriteLine("Id do autor para alterar: ");
                                int varIdAlt = int.Parse(Console.ReadLine());
                                foreach (var autAlt in bancoAutores)
                                {
                                    if (autAlt.autid == varIdAlt)
                                    {
                                        Console.WriteLine("Nome autor: ");
                                        autAlt.autnome = Console.ReadLine();
                                        Console.WriteLine("Pseudonimo do autor: ");
                                        autAlt.autpseudonimo = Console.ReadLine();
                                        Console.WriteLine("Observações autor: ");
                                        autAlt.autobservacoes = Console.ReadLine();
                                        break;
                                    }
                                }
                                break;

                            // Excluir Autor
                            case 12:
                                Console.WriteLine("Id do autor para excluir: ");
                                int varIdExc = int.Parse(Console.ReadLine());
                                foreach (var autExc in bancoAutores)
                                {
                                    if (autExc.autid == varIdExc)
                                    {
                                        bancoAutores.Remove(autExc);
                                        break;
                                    }
                                }

                                break;

                            // Pesquisar Autor
                            case 13:
                                Console.WriteLine("Id do autor para pesquisar: ");
                                int varIdPes = int.Parse(Console.ReadLine());
                                foreach (var autPes in bancoAutores)
                                {
                                    if (autPes.autid == varIdPes)
                                    {
                                        Console.WriteLine(
                                            autPes.autid + " - " +
                                            autPes.autnome + " - " +
                                            autPes.autpseudonimo + " - " +
                                            autPes.autobservacoes

                                        );
                                    }
                                }

                                break;

                            // Exibir Autores
                            case 14:
                                foreach (var autExi in bancoAutores)
                                {
                                    Console.WriteLine(
                                        autExi.autid + " - " +
                                        autExi.autnome + " - " +
                                        autExi.autpseudonimo + " - " +
                                        autExi.autobservacoes
                                    );
                                }
                                break;
                        }
                    }
                }
            }

            // Salva dados no nos bancos para persistência em disco
            SalvarEditorasEmArquivoCsv(bancoEditoras, caminhoBanco + nomeBancoEditoras);
            SalvarLivrosEmArquivoCsv(bancoLivros, caminhoBanco + nomeBancoLivros);
            SalvarAutoresEmArquivoCsv(bancoAutores, caminhoBanco + nomeBancoAutores);
        }
    }
}

