using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using sisedimodel;

// Passo 1
using System.Data;
using System.Data.SqlClient;

namespace sisedicontroller
{
    public class Livros
    {
        private List<Livro> bancoLivros = new List<Livro>();
        private string caminhoBanco = ConfigurationManager.AppSettings["caminhoBanco"];
        private string nomeBancoLivros = ConfigurationManager.AppSettings["nomeBancoLivros"];

        // Passo 2
        string connectionString = "Server=BRJND02L\\MSSQLSERVER01; Database=SYSEDIDB; Integrated Security=True;";

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

            
            if (caminhoBanco == null )
            {
                caminhoBanco = AppDomain.CurrentDomain.BaseDirectory;
            }

            

            if (string.IsNullOrEmpty(nomeBancoLivros) == false)
            {
                nomeBancoLivros = "livros.csv";
            }
            bancoLivros = CarregarLivrosDeArquivoCsv();
        }

        // Passo 3
        /*
        public void inserir(Livro Livro)
        {
            bancoLivros.Add(Livro);
            Console.WriteLine("Livro inserida com sucesso");
        }
        */
        public void inserir(Livro livro)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO LIVROS (LIVID,LIVNOME, LIVANOPUBLICACAO, LIVISBN, LIVOBSERVACOES, EDIID) " +
                               "VALUES (@livid,@livnome, @livanopublicacao, @livisbn, @livobservacoes, @ediid)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@livid", livro.livid);
                    command.Parameters.AddWithValue("@livnome", livro.livnome);
                    command.Parameters.AddWithValue("@livanopublicacao", livro.livanopublicacao);
                    command.Parameters.AddWithValue("@livisbn", livro.livisbn);
                    command.Parameters.AddWithValue("@livobservacoes", livro.livobservacoes);
                    command.Parameters.AddWithValue("@ediid", livro.ediid);  // A editora do livro
                    command.ExecuteNonQuery();
                }
            }
        }

        /*
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
        */
        public void alterar(int isbn, Livro livAlterado)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE LIVROS SET LIVNOME = @livnome, LIVANOPUBLICACAO = @livanopublicacao, " +
                               "LIVISBN = @livisbn, LIVOBSERVACOES = @livobservacoes, EDIID = @ediid WHERE LIVISBN = @isbn";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@isbn", isbn);
                    command.Parameters.AddWithValue("@livnome", livAlterado.livnome);
                    command.Parameters.AddWithValue("@livanopublicacao", livAlterado.livanopublicacao);
                    command.Parameters.AddWithValue("@livisbn", livAlterado.livisbn); 
                    command.Parameters.AddWithValue("@livobservacoes", livAlterado.livobservacoes);
                    command.Parameters.AddWithValue("@ediid", livAlterado.ediid);
                    command.ExecuteNonQuery();
                }
            }
        }

        /*
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
        */
        public void excluir(int isbn)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM LIVROS WHERE LIVISBN = @livisbn";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@livisbn", isbn);
                    command.ExecuteNonQuery();
                }
            }
        }

        /*
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
        */
        public void pesquisar(int isbn)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT LIVID, LIVNOME, LIVANOPUBLICACAO, LIVISBN, LIVOBSERVACOES, EDIID FROM LIVROS WHERE LIVISBN = @livisbn";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@livisbn", isbn);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["LIVID"]} - {reader["LIVNOME"]} - {reader["LIVANOPUBLICACAO"]} - " +
                                                  $"{reader["LIVISBN"]} - {reader["LIVOBSERVACOES"]} - {reader["EDIID"]}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nenhum registro encontrado com o ISBN informado.");
                        }
                    }
                }
            }
        }

        /*
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
        */
        public void exibirTodos()
        {
            string query = "SELECT LIVID, LIVNOME, LIVANOPUBLICACAO, LIVISBN, LIVOBSERVACOES, EDIID FROM LIVROS";

            // Cria a conexão com o banco de dados
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Abre a conexão
                    connection.Open();

                    // Cria o comando SQL
                    SqlCommand command = new SqlCommand(query, connection);

                    // Executa o comando e obtém os dados
                    SqlDataReader reader = command.ExecuteReader();

                    // Exibe os dados, se houver registros
                    if (reader.HasRows)
                    {
                        Console.WriteLine("ID\tLIVNOME\tLIVANOPUBLICACAO\tLIVISBN\tLIVOBSERVACOES\tEDIID");
                        Console.WriteLine("---------------------------------------------------------------");

                        // Lê os registros e exibe na tela
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["LIVID"]}\t{reader["LIVNOME"]}\t{reader["LIVANOPUBLICACAO"]}\t\t\t{reader["LIVISBN"]}\t{reader["LIVOBSERVACOES"]}\t\t{reader["EDIID"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nenhum registro encontrado.");
                    }
                }
                catch (Exception ex)
                {
                    // Em caso de erro, exibe a mensagem de erro
                    Console.WriteLine($"Erro: {ex.Message}");
                }
                finally
                {
                    // Fecha a conexão
                    connection.Close();
                }
            }
        }
    }
}
