using ProjetoUsuario.Models;

public interface IUserServices
{
    public int Insert(User user);
    public bool Update(User user);
    public bool Delete(User user);
}