using System;
using System.ComponentModel.DataAnnotations;
namespace Shop.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo é Obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        public string Titulo { get; set; }

        [MaxLength(1024, ErrorMessage = "Este campo deve conter ate 1024 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Este campo é Obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
        public decimal Valor { get; set; }


        [Required(ErrorMessage = "Este campo é Obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Categoria Invalida")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}