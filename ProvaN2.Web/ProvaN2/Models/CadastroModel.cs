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

        public string  Email{ get; set; }

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

        /* public int Salvar()
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
                         comando.CommandText = "insert into municipio (nome, codigo, uf) values (@nome, @codigo, @uf); select convert(int, scope_identity())";

                         comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                         comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
                         comando.Parameters.Add("@id_grupo_produto", SqlDbType.Int).Value = this.IdGrupoProduto;

                         ret = (int)comando.ExecuteScalar();
                     }
                     else
                     {
                         comando.CommandText = "update municipio set nome=@nome, codigo=@codigo, uf=@uf where id = @id";

                         comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;
                         comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                         comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
                         comando.Parameters.Add("@id_grupo_produto", SqlDbType.Int).Value = this.IdGrupoProduto;


                         if (comando.ExecuteNonQuery() > 0)
                         {
                             ret = this.Id;
                         }
                     }
                 }
             }

             return ret;
         }*/
    }
}