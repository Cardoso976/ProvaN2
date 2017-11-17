using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProvaN2.Models
{
    public class CadastroModel
    {

        public int Id { get; set; }

        public string CPF { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        public string Nome { get; set; }

        public int Idade { get; set; }

        public string Sexo { get; set; }

        public string Email { get; set; }

        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select count(*) from pessoa";
                    ret = (int)comando.ExecuteScalar();
                }
            }

            return ret;
        }

        public static List<CadastroModel> RecuperarLista(int pagina, int tamPagina)
        {
            var ret = new List<CadastroModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    var pos = (pagina - 1) * tamPagina;

                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                        "select * from pessoa order by nome offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new CadastroModel
                        {
                            Id = (int)reader["id"],
                            CPF = (string)reader["cpf"],
                            Nome = (string)reader["nome"],
                            Email = (string)reader["email"],
                            Idade = (int)reader["idade"],
                            Sexo = (string)reader["sexo"]
                        });
                    }
                }
            }

            return ret;
        }

        public static CadastroModel RecuperarPeloId(int id)
        {
            CadastroModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from pessoa where (id = @id)";

                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new CadastroModel
                        {
                            Id = (int)reader["id"],
                            CPF = (string)reader["cpf"],
                            Nome = (string)reader["nome"],
                            Email = (string)reader["email"],
                            Idade = (int)reader["idade"],
                            Sexo = (string)reader["sexo"]
                        };
                    }
                }
            }

            return ret;
        }

        public static bool ExcluirPeloId(int id)
        {
            var ret = false;

            if (RecuperarPeloId(id) != null)
            {
                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                    conexao.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexao;
                        comando.CommandText = "delete from pessoa where (id = @id)";

                        comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                        ret = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }

            return ret;
        }

        public int Salvar()
        {
            var ret = 0;

            var model = RecuperarPeloId(this.Id);

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    if (model == null)
                    {
                        comando.CommandText = "insert into pessoa (nome, email, cpf, sexo, idade) values (@nome, @email, @cpf, @sexo, @idade); select convert(int, scope_identity())";

                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@email", SqlDbType.VarChar).Value = this.Email;
                        comando.Parameters.Add("@cpf", SqlDbType.VarChar).Value = this.CPF;
                        comando.Parameters.Add("@sexo", SqlDbType.VarChar).Value = this.Sexo;
                        comando.Parameters.Add("@idade", SqlDbType.Int).Value = this.Idade;

                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = "update pessoa set cpf=@cpf, nome=@nome, idade=@idade, sexo=@sexo, email=@email where id = @id";

                        comando.Parameters.Add("@cpf", SqlDbType.VarChar).Value = this.CPF;
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@idade", SqlDbType.Int).Value = this.Idade;
                        comando.Parameters.Add("@sexo", SqlDbType.VarChar).Value = this.Sexo;
                        comando.Parameters.Add("@email", SqlDbType.VarChar).Value = this.Email;                        
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;


                        if (comando.ExecuteNonQuery() > 0)
                        {
                            ret = this.Id;
                        }
                    }
                }
            }

            return ret;
        }
    }
}