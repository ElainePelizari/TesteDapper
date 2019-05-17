using System;
using System.Collections.Generic;
using Dapper;

namespace DapperTesteMay
{
    class Program
    {
        public static void Main(string[] args)
        {
            string nome = string.Empty;
            do
            {
                Console.WriteLine("Digite o nome:");
                nome = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nome))
                {
                    Console.WriteLine("Digite o sobrenome:");
                    var sobrenome = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(sobrenome))
                    {
                        var pessoa = new Pessoa()
                        {
                            Nome = nome,
                            Sobrenome = sobrenome
                        };
                        CadastrarPessoaNoBanco(pessoa);
                    }
                }

                ListarPessoas();
            }
            while (!string.IsNullOrWhiteSpace(nome));

            Console.ReadLine();
        }

        private static void ListarPessoas()
        {
            Console.WriteLine("======= Listagem de pessoas =======");

            var pessoas = CarregarPessoasDoBanco();
            foreach (var pessoa in pessoas)
            {
                Console.WriteLine("Id: {0}, Nome: {1}, Sobrenome: {2}", pessoa.Id, pessoa.Nome, pessoa.Sobrenome);
            }

            Console.WriteLine("===================================");
        }

        private static IEnumerable<Pessoa> CarregarPessoasDoBanco()
        {
            IEnumerable<Pessoa> pessoas = null;

            using (var conn = new System.Data.SQLite.SQLiteConnection("Data Source=db.db;Version=3;"))
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    pessoas = conn.Query<Pessoa>("SELECT * FROM Pessoa");
                }
            }

            return pessoas;
        }

        private static void CadastrarPessoaNoBanco(Pessoa pessoa)
        {
            using (var conn = new System.Data.SQLite.SQLiteConnection("Data Source=db.db;Version=3;"))
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Execute(
                        "INSERT INTO Pessoa (Nome, Sobrenome) VALUES (@Nome, @Sobrenome)",
                        new { Nome = pessoa.Nome, Sobrenome = pessoa.Sobrenome });
                }
            }
        }
    }
}
