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
    public class Autores
    {

        private List<Autor> bancoAutores = new List<Autor>();
        private string caminhoBanco = ConfigurationManager.AppSettings["caminhoBanco"];
        private string nomeBancoAutores = ConfigurationManager.AppSettings["nomeBancoAutores"];

        // Passo 2
        string connectionString = "Server=BRJND02L\\MSSQLSERVER01; Database=SYSEDIDB; Integrated Security=True;";

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
           

            if (caminhoBanco == null)
            {
                caminhoBanco = AppDomain.CurrentDomain.BaseDirectory;
            }

            if (string.IsNullOrEmpty(nomeBancoAutores) == false)
            {
                nomeBancoAutores = "autores.csv";
            }

            bancoAutores = CarregarAutoresDeArquivoCsv();
        }

        /*
        public void inserir(Autor autor)
            {
                bancoAutores.Add(autor);
                Console.WriteLine("Autor inserida com sucesso");
            }
        */
        public void inserir(Autor autor)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO AUTORES (AUTID, AUTNOME, AUTPSEUDONIMO, AUTOBSERVACOES) " +
                               "VALUES (@autid, @autnome, @autpseudonimo, @autobservacoes)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@autid", autor.autid);
                    command.Parameters.AddWithValue("@autnome", autor.autnome);
                    command.Parameters.AddWithValue("@autpseudonimo", autor.autpseudonimo);
                    command.Parameters.AddWithValue("@autobservacoes", autor.autobservacoes);
                    command.ExecuteNonQuery();
                }
            }
        }

        /*
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
        */
        public void alterar(int id, Autor autAlterado)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE AUTORES " +
                                   "SET AUTNOME = @autnome, AUTPSEUDONIMO = @autpseudonimo, AUTOBSERVACOES = @autobservacoes " +
                                   "WHERE AUTID = @id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@autnome", autAlterado.autnome);
                        command.Parameters.AddWithValue("@autpseudonimo", autAlterado.autpseudonimo);
                        command.Parameters.AddWithValue("@autobservacoes", autAlterado.autobservacoes);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Autor atualizado com sucesso.");
                        }
                        else
                        {
                            Console.WriteLine("Nenhum autor encontrado com o ID especificado.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao alterar o autor: {ex.Message}");
            }
        }

        /*
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
        */
        public void excluir(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM AUTORES WHERE AUTID = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Autor excluído com sucesso.");
                        }
                        else
                        {
                            Console.WriteLine("Nenhum autor encontrado com o ID especificado.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir o autor: {ex.Message}");
            }
        }

        /*
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
        */
        public void pesquisar(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT AUTID, AUTNOME, AUTPSEUDONIMO, AUTOBSERVACOES FROM AUTORES WHERE AUTID = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine($"ID: {reader["AUTID"]}");
                                    Console.WriteLine($"Nome: {reader["AUTNOME"]}");
                                    Console.WriteLine($"Pseudônimo: {reader["AUTPSEUDONIMO"]}");
                                    Console.WriteLine($"Observações: {reader["AUTOBSERVACOES"]}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nenhum autor encontrado com o ID especificado.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao pesquisar o autor: {ex.Message}");
            }
        }

        /* 
        
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
        */
        public void exibirTodos()
        {
            string query = "SELECT AUTID, AUTNOME, AUTPSEUDONIMO, AUTOBSERVACOES FROM AUTORES";

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
                        Console.WriteLine("ID\tAUTNOME\tAUTPSEUDONIMO\tAUTOBSERVACOES");
                        Console.WriteLine("--------------------------------------------");

                        // Lê os registros e exibe na tela
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["AUTID"]}\t{reader["AUTNOME"]}\t{reader["AUTPSEUDONIMO"]}\t\t{reader["AUTOBSERVACOES"]}");
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
