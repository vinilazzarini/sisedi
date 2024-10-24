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

        static void Main(string[] args)
        {

            int opc = 0;
            int opcsub = 0;

            Editoras editoras = new Editoras();
            Livros livros = new Livros();
            Autores autores = new Autores();

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

                                editoras.inserir(itemIns);

                                break;

                            // Alterar Editora
                            case 11:
                                Console.WriteLine("Sigla da editora para alterar: ");
                                string siglaAlt = Console.ReadLine();

                                Editora itemAlt = new Editora();
                                Console.WriteLine("Id editora: ");
                                itemAlt.ediid = int.Parse(Console.ReadLine());
                                Console.WriteLine("Nome editora: ");
                                itemAlt.edinome = Console.ReadLine();
                                Console.WriteLine("Sigla editora: ");
                                itemAlt.edisigla = Console.ReadLine();
                                Console.WriteLine("Observações editora: ");
                                itemAlt.ediobservacoes = Console.ReadLine();

                                editoras.alterar(siglaAlt, itemAlt);

                                break;

                            // Excluir Editora
                            case 12:
                                Console.WriteLine("Sigla da editora para excluir: ");
                                string siglaExc = Console.ReadLine();

                                editoras.excluir(siglaExc);

                                break;

                            // Pesquisar Editora
                            case 13:
                                Console.WriteLine("Sigla da editora para pesquisar: ");
                                string siglaPes = Console.ReadLine();

                                editoras.pesquisar(siglaPes);
                                break;

                            // Exibir Editoras
                            case 14:
                                editoras.exibirTodos();
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

                                livros.inserir(itemIns);

                                break;

                            // Alterar Livro
                            case 11:
                                Console.WriteLine("Isbn do livro para alterar: ");
                                int isnbAlt = int.Parse(Console.ReadLine());
 
                                Livro livAlt = new Livro();
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
                                
                                livros.alterar(isnbAlt, livAlt);
                                break;

                            // Excluir Livro
                            case 12:
                                Console.WriteLine("Isbn do livro para excluir: ");
                                int isnbExc = int.Parse(Console.ReadLine());
                                
                                livros.excluir(isnbExc);

                                break;

                            // Pesquisar Livro
                            case 13:
                                Console.WriteLine("Isbn do livro para pesquisar: ");
                                int isnbPes = int.Parse(Console.ReadLine());
                                
                                livros.pesquisar(isnbPes);

                                break;

                            // Exibir Livros
                            case 14:
                                livros.exibirTodos();
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

                                autores.inserir(itemIns);

                                break;

                            // Alterar Autor
                            case 11:
                                Console.WriteLine("Id do autor para alterar: ");
                                int idAlt = int.Parse(Console.ReadLine());

                                Autor autAlt = new Autor();
                                Console.WriteLine("Nome autor: ");
                                autAlt.autnome = Console.ReadLine();
                                Console.WriteLine("Pseudonimo do autor: ");
                                autAlt.autpseudonimo = Console.ReadLine();
                                Console.WriteLine("Observações autor: ");
                                autAlt.autobservacoes = Console.ReadLine();

                                autores.alterar(idAlt, autAlt);
                                break;


                            // Excluir Autor
                            case 12:
                                Console.WriteLine("Id do autor para excluir: ");
                                int idExc = int.Parse(Console.ReadLine());
                                
                                autores.excluir(idExc);

                                break;

                            // Pesquisar Autor
                            case 13:
                                Console.WriteLine("Id do autor para pesquisar: ");
                                int idPes = int.Parse(Console.ReadLine());

                                autores.pesquisar(idPes);
                                break;

                            // Exibir Autores
                            case 14:
                                autores.exibirTodos();
                                break;
                        }
                    }
                }
            }

            // Salva dados no nos bancos para persistência em disco
            editoras.SalvarEditorasEmArquivoCsv();
            livros.SalvarLivrosEmArquivoCsv();
            autores.SalvarAutoresEmArquivoCsv();
        }
    }
}

