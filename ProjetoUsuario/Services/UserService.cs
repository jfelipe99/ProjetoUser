using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using ProjetoUsuario.Data;
using ProjetoUsuario.Models;

namespace ProjetoUsuario.Services
{
    public class UserService : GenericServices, IUserServices
    {
        public UserService(UserDbContext context,IMemoryCache memoryCache)
        : base(context, memoryCache) {

        }

        public int Insert(User user)
        {
            try
            {
                var userRequest = _context.Users
                    .Where(a => a.CPF == user.CPF)
                    .Select(x => new
                    {
                        Id = x.Id,
                    })
                    .FirstOrDefault();

                if(userRequest == null)
                {
                    bool cpfIsValid = IsCpf(user.CPF);
                    if(cpfIsValid == false)
                        new Exception("CPF Invalido");
                    if (!(Regex.Match(user.Telefone, @"^(\+[0-9])$").Success))
                        new Exception("");
                    
                    var userInsertRequest = new User {
                        CPF = user.CPF,
                        Nome = user.Nome,
                        Telefone = user.Telefone,
                        Email = user.Email,
                        Sexo = user.Sexo,
                        DataNascimento = user.DataNascimento
                    };

                    var result =_context.Users.Add(userInsertRequest);
                    _context.SaveChanges();

                    return userInsertRequest.Id;
                }
            }catch(Exception ex)
            {
                return 0;
            }

            return user.Id;

        }

        public bool Update(User user)
        {
            try{

                var userRequest = _context.Users
                    .Where(a => a.CPF == user.CPF)
                    .Select(x => new
                    {
                        Id = x.Id,
                    })
                    .FirstOrDefault();

                if(userRequest != null)
                {
                    //Valida CPF
                    bool cpfIsValid = IsCpf(user.CPF);
                    if(cpfIsValid == false)
                        new Exception("CPF Invalido");
                    //Valida Telefone
                    if (!(Regex.Match(user.Telefone, @"^(\+[0-9])$").Success))
                        new Exception("");
                    
                    var userentity = _context.Users.FirstOrDefault(a => a.Id == userRequest.Id);
                    if(userentity != null)
                    {
                        userentity.Nome = user.Nome;
                        userentity.Email = user.Email;
                        userentity.DataNascimento = user.DataNascimento;
                        userentity.Sexo = user.Sexo;
                        userentity.Telefone = user.Telefone;
                    }
                    _context.SaveChanges();
                    return true;
                }
                else {
                    return false;
                }
            }catch(Exception ex)
            {
                return false;
            }
        }

        public bool Delete(User user)
        {
            var userRequest = _context.Users
                    .Where(a => a.CPF == user.CPF)
                    .Select(x => new
                    {
                        Id = x.Id,
                    })
                    .FirstOrDefault();

                if(userRequest != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    return true;
                }else {
                    return false;
                }
        }

        private static bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;

            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                    return false;

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}