using System.Collections.Generic;
using System.Data;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json.Linq;

namespace EnhancementAPI.Models.Repository.LoginRepository
{
    public class LoginRepository : ILoginRepository
    {
        public IConfiguration _iconfiguration;
        LoginModel loginModel;
        public LoginRepository(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }


        public DataSet getEncToken(string userID, string password)
        {
            return loginModel.GetEncToken(userID, password).GetTableName();
        }

        public DataSet SelectLoginDetails(string paramString)
        {
            loginModel = new LoginModel(_iconfiguration);
            JObject JData = JObject.Parse(paramString);
            loginModel.userName = JData["userName"].ToString();
            loginModel.Password = JData["Password"].ToString();
            loginModel.authType = JData["authType"].ToString(); //Auth Type can be AD or DB
            loginModel.AuthSuccess = JData["AuthSuccess"].ToString(); //Added to Know AD login is Successful or Not
            return loginModel.Select_LoginDetails().GetTableName();
        }

       
    }

}

