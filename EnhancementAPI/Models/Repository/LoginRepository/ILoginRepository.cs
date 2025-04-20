using System.Data;

namespace EnhancementAPI.Models.Repository.LoginRepository
{
    public interface ILoginRepository
    {
        DataSet SelectLoginDetails(string paramString);

        DataSet getEncToken(string userID, string password);
    }

}