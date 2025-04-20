using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

//using EnhancementAPI.Hubs;
//using EnhancementAPI.Models;
//using EnhancementAPI.Models.Repository.LoginRepository;
//using EnhancementAPI.Utility;
using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using EnhancementAPI.Models;
using EnhancementAPI.Models.Repository.LoginRepository;
using EnhancementAPI.Utility;
using EnhancementAPI.Hubs;

namespace EnhancementAPI.Controllers
{
    [Route("api/[controller]")]

    public class TokenAuthController : Controller
    {
        public IConfiguration _iconfiguration;
        public TokenAuthController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;


        }
        string ModelName = "TokenAuth";
      

        [HttpPost("GetDBToken")]
        public IActionResult GetTokenForADF([FromBody] string userName, [FromBody] string password)
        {
            LoginRepository repository = new LoginRepository(_iconfiguration);
            DataSet ds = new DataSet();
            ds = repository.getEncToken(userName, password);
            return Json(new RequestResult
            {
                Results = new { Status = RequestState.Success, Msg = "ok" },
                Data = ds
            });
        }
        async Task<JsonResult> checkDBAuth_User(User user)
        {

            //var ADHubUrl = _iconfiguration.GetValue<string>("ADHubServer:ServerUrl");
            string UsernameEncrypt = HttpUtility.UrlEncode(Encrypt(user.Username.Trim()));
            string PasswordEncrypt = HttpUtility.UrlEncode(Encrypt(user.Password.Trim()));

            //var url = ADHubUrl + "/api/ADLogin?userId=" + UsernameEncrypt + "&userPassword=" + PasswordEncrypt;
            var AuthType = _iconfiguration.GetValue<string>("ConnectionStrings:AuthType");

            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri(url);
            //    client.DefaultRequestHeaders.Accept.Clear();

            //    var response = await client.GetStringAsync(url);
            //    if (response == "true")
            //    {
            User existUser = user;
            string Password = EncrPassword(user.Password);

            Login objlogin = new Login();

            JObject JLoginObj = new JObject();
            JLoginObj.Add("userName", user.Username);
            JLoginObj.Add("Password", Password);
            JLoginObj.Add("authType", AuthType);
            JLoginObj.Add("AuthSuccess", "N");

            LoginRepository repository = new LoginRepository(_iconfiguration);
            DataSet UserData = repository.SelectLoginDetails(JLoginObj.ToString());
            UserStatus userLoginstatus = new UserStatus();

            if (UserData.Tables["UserLogin"] != null)
            {
                if (AuthType == "AD")
                {
                    var Samepassword = UserData.Tables["UserLogin"].Rows[0]["LoginPassword"].ToString();
                    userLoginstatus = getLoginUserStatus(UserData, Samepassword);
                }
                else
                {
                    userLoginstatus = getLoginUserStatus(UserData, Password);
                }

            }
            else
            {
                return Json(new RequestResult
                {
                    Results = new { Status = RequestState.Failed, Msg = "User Not Exist in CrisMac System" },
                });
            }

            if (userLoginstatus.IsLogin)
            {
                var requestAt = DateTime.Now;
                var expiresIn = requestAt + TokenAuthOption.ExpiresSpan;
                var token = GenerateToken(existUser, expiresIn);

                var strGroups = UserData.Tables["UserLogin"].Rows[0]["DeptGroupCode"].ToString();
                var userGroups = strGroups.Split(',');
                var userFullName = UserData.Tables["UserLogin"].Rows[0].ItemArray[2];
                var userrole = UserData.Tables["UserLogin"].Rows[0]["UserRoleAlt_Key"].ToString();
                var isChecker = UserData.Tables["UserLogin"].Rows[0]["IsChecker"].ToString();
                var departmentnamepara = UserData.Tables["DepartmentName"].Rows[0]["DeptGroupName"].ToString();
                var currdatetemp = UserData.Tables["QtrDate"].Rows[0]["QtrDate"].ToString();
                var AuditStatus = UserData.Tables["AuditStatus"].Rows[0]["AuditStatus"].ToString();
                var AuditStatusForExceptionalCurrQTRDate = UserData.Tables["AuditStatusForExceptional"].Rows[0]["CurQtrDate"].ToString();
                var AuditStatusForExceptionaLastQTRDate = UserData.Tables["AuditStatusForExceptional"].Rows[0]["LastQtrDate"].ToString();
                var AuditStatusForExceptionalAuditStatus = UserData.Tables["AuditStatusForExceptional"].Rows[0]["AuditStatus"].ToString();

                GroupModel.UserID = existUser.Username;
                GroupModel.grouplist = new List<GroupsList>();
                List<GroupsList> grplist = new List<GroupsList>();

                foreach (var grpName in userGroups)
                {
                    grplist.Add(new GroupsList()
                    {
                        GroupName = grpName
                    });
                }

                if (grplist.Count() > 1)
                {
                    GroupModel.grouplist = grplist;
                }

                return Json(new RequestResult
                {
                    Results = new { Status = RequestState.Success, Msg = "ok" },
                    Data = new
                    {
                        requertAt = requestAt,
                        expiresIn = TokenAuthOption.ExpiresSpan.TotalSeconds,
                        tokeyType = TokenAuthOption.TokenType,
                        accessToken = token,
                        currdate = currdatetemp,
                        departmentname = departmentnamepara,
                        auditstatus = AuditStatus,
                        auditstatusforexcptionalcurqtrdate = AuditStatusForExceptionalCurrQTRDate,
                        auditstatusforexcptionalLastQtrDate = AuditStatusForExceptionaLastQTRDate,
                        auditstatusforexcptionalAuditStaus = AuditStatusForExceptionalAuditStatus,
                        Userinfo = new
                        {
                            UserID = existUser.Username,
                            username = userFullName,
                            userRole = userrole,
                            isChecker = isChecker

                        }
                    }
                });
            }
            else
            {
                return Json(new RequestResult
                {
                    Results = new { Status = RequestState.Failed, Msg = userLoginstatus.Msg },
                });
            }

            //    }
            //    else
            //    {
            //        return Json(new RequestResult
            //        {
            //            Results = new { Status = RequestState.Failed, Msg = "Username or password is invalid" },
            //        });
            //    }
            //}
        }


        
        
        [HttpPost("token")]
        public async Task<JsonResult> token([FromBody] User user)
        {
            try
            {
                var AuthType = _iconfiguration.GetValue<string>("ConnectionStrings:AuthType");
               
                {
                    var dataAD = await checkDBAuth_User(user);
                    return dataAD;
                }

                //var dataAD = await checkADAuth_User(user);
                //return dataAD;
            }
            catch (Exception ex)
            {
               
               // writeerrorlog(ex, "TokenAuth");
                return Json(new RequestResult
                {
                    Results = new { Status = RequestState.Failed, Msg = "Somthing went wrong, please try again" },
                });
            }

        }

        
        private UserStatus getLoginUserStatus(DataSet userData, string Password)
        {
            UserStatus userLoginstatus = new UserStatus();

            var IsSUSPEND = userData.Tables["UserLogin"].Rows[0]["SUSPEND"].ToString();
            var IsExpiredUser = userData.Tables["UserLogin"].Rows[0]["ExpiredUser"].ToString();
            var isActivate = userData.Tables["UserLogin"].Rows[0]["Activate"].ToString();
            var IsLoginPassword = userData.Tables["UserLogin"].Rows[0]["LoginPassword"].ToString();

            if (IsSUSPEND == "SUSPEND")
            {
                userLoginstatus.IsLogin = false;
                userLoginstatus.Msg = "User has been Suspended";
                return userLoginstatus;
            }
            else if (IsExpiredUser == "ExpiredUser")
            {
                userLoginstatus.IsLogin = false;
                userLoginstatus.Msg = "User has been Expired";
                return userLoginstatus;
            }
            else if (isActivate != "Y")
            {
                userLoginstatus.IsLogin = false;
                userLoginstatus.Msg = "User is Not Active";
                return userLoginstatus;
            }
            else if (IsLoginPassword != Password)
            {
                userLoginstatus.IsLogin = false;
                userLoginstatus.Msg = "Invalid Password";
                return userLoginstatus;
            }
            else
            {
                userLoginstatus.IsLogin = true;
                userLoginstatus.Msg = "Login Successfully";
                return userLoginstatus;
            }

        }

        private string GenerateToken(User user, DateTime expires)
        {
            var handler = new JwtSecurityTokenHandler();
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.Username), new[] { new Claim("ID", user.ID.ToString()) }
            );

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = TokenAuthOption.Issuer,
                Audience = TokenAuthOption.Audience,
                SigningCredentials = TokenAuthOption.SigningCredentials,
                Subject = identity,
                Expires = expires
            });
            return handler.WriteToken(securityToken);
        }
        private string EncrPassword(string _password)
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            Byte[] bytProducts = uEncode.GetBytes(_password);
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] hash = sha1.ComputeHash(bytProducts);
            return Convert.ToBase64String(hash);
        }


        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }


    }

    public class HelpController : Controller
    {
        public IActionResult HelpPage()
        {
            return View();
        }
    }
}
