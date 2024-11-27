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
    public class Editoras
    {

        private List<Editora> bancoEditoras = new List<Editora>();

        private string caminhoBanco = ConfigurationManager.AppSettings["caminhoBanco"];
        private string nomeBancoEditoras = ConfigurationManager.AppSettings["nomeBancoEditoras"];

        // Passo 2
        string connectionString = "Server=BRJND02L\\MSSQLSERVER01; Database=SYSEDIDB; Integrated Security=True;";


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
 
            if (caminhoBanco == null)
            {
                caminhoBanco = AppDomain.CurrentDomain.BaseDirectory;
            }


            if (string.IsNullOrEmpty(nomeBancoEditoras) == false)
            {
                nomeBancoEditoras = "editoras.csv";
            }

            bancoEditoras = CarregarEditorasDeArquivoCsv();

        }

        // Passo 3
        /*
        public void inserir(Editora editora)
        {
            bancoEditoras.Add(editora);
            Console.WriteLine("Editora inserida com sucesso");
        }
        */
        public void inserir(Editora editora)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO EDITORAS (EDIID,EDINOME, EDISIGLA, EDIOBSERVACOES) " +
                               "VALUES (@ediid,@edinome, @edisigla, @ediobservacoes)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ediid", editora.ediid);
                    command.Parameters.AddWithValue("@edinome", editora.edinome);
                    command.Parameters.AddWithValue("@edisigla", editora.edisigla);
                    command.Parameters.AddWithValue("@ediobservacoes", editora.ediobservacoes);
                    command.ExecuteNonQuery();
                }
            }
        }

        /*
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
        */
        public void alterar(string sigla, Editora ediAlterada)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE EDITORAS SET EDIID = @ediid,EDINOME = @edinome, EDIOBSERVACOES = @ediobservacoes WHERE EDISIGLA = @sigla";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@sigla", sigla);
                        command.Parameters.AddWithValue("@ediid", ediAlterada.ediid);
                        command.Parameters.AddWithValue("@edinome", ediAlterada.edinome);
                        command.Parameters.AddWithValue("@edisigla", ediAlterada.edisigla);
                        command.Parameters.AddWithValue("@ediobservacoes", ediAlterada.ediobservacoes);
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Editora atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar editora: {ex.Message}");
            }
        }

        /*
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
        */
        public void excluir(string sigla)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM EDITORAS WHERE EDISIGLA = @edisigla";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@edisigla", sigla);
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Editora excluída com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir editora: {ex.Message}");
            }
        }

        /*
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
        */
        public void pesquisar(string sigla)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT EDIID, EDINOME, EDISIGLA, EDIOBSERVACOES FROM EDITORAS WHERE EDISIGLA = @edisigla";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@edisigla", sigla);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                Console.WriteLine("ID\tNOME\tSIGLA\tOBSERVACOES");
                                while (reader.Read())
                                {
                                    Console.WriteLine($"{reader["EDIID"]}\t{reader["EDINOME"]}\t{reader["EDISIGLA"]}\t{reader["EDIOBSERVACOES"]}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nenhuma editora encontrada com a sigla especificada.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao pesquisar editora: {ex.Message}");
            }
        }

        /*
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
        */
        public void exibirTodos()
        {
            string query = "SELECT EDIID,EDINOME,EDISIGLA,EDIOBSERVACOES FROM EDITORAS";

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
                        Console.WriteLine("ID\tNOME\tSIGLA\tOBSERVACOES");
                        Console.WriteLine("------------------------------------");

                        // Lê os registros e exibe na tela
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["EDIID"]}\t{reader["EDINOME"]}\t{reader["EDISIGLA"]}\t{reader["EDIOBSERVACOES"]}");
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
