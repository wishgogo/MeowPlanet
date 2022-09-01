using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeowPlanet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Net.Mail;
using Google.Apis.Auth;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace MeowPlanet.Controllers
{
    public class LoginController : Controller
    {
        private readonly endtermContext _context;

        public LoginController(endtermContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Password()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        // 登入判定Email及密碼是否正確
        [HttpGet]
        public ActionResult LoginCheck(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var count = _context.Members.Count(x => x.Email == email);

                if (count > 0)
                {
                    var count2 = _context.Members.Count(x => x.Email == email && x.Password == password);

                    if (count2 > 0)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return Content("密碼錯誤");
                    }
                }
                else
                {
                    return Content("此Email不存在");
                }


            }
            return NoContent();

        }

        // 登入
        [HttpPost]
        public ActionResult Login(Member member, bool rememberme)
        {
            var count = _context.Members.Count(p => p.Email == member.Email && p.Password == member.Password);

            if (count > 0)
            {
                var LoginInfo = _context.Members.FirstOrDefault(p => p.Email == member.Email); //整筆資料取出

                var LoginId = LoginInfo.MemberId;

                var LoginName = LoginInfo.Name;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, LoginId.ToString()),
                    new Claim(ClaimTypes.Name, LoginName),
                    new Claim(ClaimTypes.Role, "member")

                };

                //cookie驗證
                var claimsIdentity = new ClaimsIdentity(
                           claims, CookieAuthenticationDefaults.AuthenticationScheme);

                //記住我
                var properties = new AuthenticationProperties
                {
                    IsPersistent = rememberme,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
                };

                //登入驗證存進cookie
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                        new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Member");
            }
            else
            {
                return NoContent();
            }

        }

        // 登出
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        // 寄送註冊驗證碼
        [HttpGet]
        public ActionResult SendRegistMail(string email)
        {

            Random random = new Random();
            int verifycode = random.Next(10000, 99999);

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.To.Add(email);

            msg.From = new MailAddress("meowplanet04@gmail.com", "喵屋星球", System.Text.Encoding.UTF8);

            msg.Subject = "MeowPlanet 註冊驗證碼";
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = "這是 MeowPlanet 喵屋星球 的註冊驗證信，若你不曾要求註冊，請忽略這封信<br />" + "驗證碼:" + verifycode;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("meowplanet04@gmail.com", "vxzqhwksdippafpq");
            client.Host = "smtp.gmail.com";
            client.Port = 25;
            client.EnableSsl = true;
            client.Send(msg);
            client.Dispose();
            msg.Dispose();

            return Content(verifycode.ToString());
        }

        // 註冊判定Email是否可使用
        [HttpGet]
        public ActionResult EmailCheck(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var count = _context.Members.Count(x => x.Email == email);
                
                if (count > 0)
                {
                    return Content("此信箱已註冊");
                }
                else
                {
                    return Content("此信箱可使用");
                }
            }

            return NoContent();
        }

        // 註冊
        [HttpPost]
        public async Task<IActionResult> AddMember(Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Members.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Register");
        }

        // 寄密碼重置信
        [HttpGet]
        public ActionResult SendEmailMsg(string Email)
        {
            // 加密

            string sVerify = Email;
            string SecretKey = "meow";

            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] buf = Encoding.UTF8.GetBytes(SecretKey);
            byte[] result = md5.ComputeHash(buf);
            string md5Key = BitConverter.ToString(result).Replace("-", "").ToLower().Substring(0, 24);
            DES.Key = UTF8Encoding.UTF8.GetBytes(md5Key);
            DES.Mode = CipherMode.ECB;
            ICryptoTransform DESEncrypt = DES.CreateEncryptor();
            byte[] Buffer = UTF8Encoding.UTF8.GetBytes(sVerify);
            sVerify = Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));

            // 建立MailMessage物件，編寫信件內容
            MailMessage msg = new MailMessage();
            // 收件信箱
            msg.To.Add(Email);
            // 寄件者、編碼
            msg.From = new MailAddress("meowplanet04@gmail.com", "喵屋星球", System.Text.Encoding.UTF8);
            // 標題
            msg.Subject = "重新設定您在 MeowPlanet 的密碼";
            // 標題編碼
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            // 內容
            msg.Body = "這是 MeowPlanet 喵屋星球 的密碼重置信，若你不曾要求重設密碼，請忽略這封信<br />" +
                "<a href='" + "https://meowplanet.lol/Login/ResetPassword" + "?verify=" + sVerify + "'>請點擊此連結重置密碼</a>" + "<br /><br />MeowPlanet 喵屋星球";
            // 內容編碼
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            // 內容為html格式
            msg.IsBodyHtml = true;

            // 建立SmtpClient物件，設定發信的帳號、smtp主機跟port
            SmtpClient client = new SmtpClient();
            // 設定發信帳號及密碼取得 Smtp 伺服器的憑證
            client.Credentials = new System.Net.NetworkCredential("meowplanet04@gmail.com", "vxzqhwksdippafpq");
            // 設定smtp主機
            client.Host = "smtp.gmail.com";
            // 設定port
            client.Port = 25;
            // 啟用ssl (Gmail需使用)
            client.EnableSsl = true;
            // 寄出信件
            client.Send(msg);
            // 結束連接
            client.Dispose();
            msg.Dispose();

            return NoContent();
        }

        // 重置密碼
        [HttpGet]
        public async Task<IActionResult> DoPwdReset(string email, string password)
        {
            // 解密
            string SecretKey = "meow";
            TripleDESCryptoServiceProvider DES2 = new TripleDESCryptoServiceProvider();
            MD5 md6 = new MD5CryptoServiceProvider();
            byte[] buf2 = Encoding.UTF8.GetBytes(SecretKey);
            byte[] md5result = md6.ComputeHash(buf2);
            string md5Key2 = BitConverter.ToString(md5result).Replace("-", "").ToLower().Substring(0, 24);
            DES2.Key = UTF8Encoding.UTF8.GetBytes(md5Key2);
            DES2.Mode = CipherMode.ECB;
            DES2.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ICryptoTransform DESDecrypt2 = DES2.CreateDecryptor();
            byte[] Buffer2 = Convert.FromBase64String(email);
            string deCode = UTF8Encoding.UTF8.GetString(DESDecrypt2.TransformFinalBlock(Buffer2, 0, Buffer2.Length));

            
            var UserInfo = _context.Members.FirstOrDefault(x => x.Email == deCode);
            if (UserInfo != null)
            {
                UserInfo.Password = password;
                await _context.SaveChangesAsync();
                return Content("修改完成");
            }
            else
            {
                return NoContent();
            }

        }

        [HttpGet]
        public ActionResult ForgetPassword(string ForgetEmail)
        {
            var count = _context.Members.Count(x => x.Email == ForgetEmail);

            if (count > 0)
            {
                return Content("重置信已寄出");
            }
            else
            {
                return Content("此信箱不存在");
            }

        }



        // google登入
        public async Task<IActionResult> ValidGoogleLogin()
        {
            string? formCredential = Request.Form["credential"]; //回傳憑證
            string? formToken = Request.Form["g_csrf_token"]; //回傳令牌
            string? cookiesToken = Request.Cookies["g_csrf_token"]; //Cookie 令牌

            // 驗證 Google Token
            GoogleJsonWebSignature.Payload? payload = VerifyGoogleToken(formCredential, formToken, cookiesToken).Result;
            if (payload == null)
            {
                // 驗證失敗
                ViewData["Msg"] = "驗證 Google 授權失敗";
            }
            else
            {
                //驗證成功，取使用者資訊內容
                ViewData["Msg"] = "驗證 Google 授權成功" + "<br>";

                var count = _context.Members.Count(x => x.Email == payload.Email);

                if (count > 0)
                {
                    var LoginInfo = _context.Members.FirstOrDefault(p => p.Email == payload.Email); //整筆資料取出

                    var LoginId = LoginInfo.MemberId;

                    var LoginName = LoginInfo.Name;

                    //cookie驗證
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Sid, LoginId.ToString()),
                        new Claim(ClaimTypes.Name, LoginName),
                        new Claim(ClaimTypes.Role, "google")
                    };

                    var claimsIdentity = new ClaimsIdentity(
                               claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    //記住我
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
                    };

                    //登入驗證存進cookie
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                            new ClaimsPrincipal(claimsIdentity), properties);



                    return RedirectToAction("Index", "Member");

                }
                else
                {
                    // 建立google會員
                    Member member = new()
                    {
                        Email = payload.Email,
                        Name = payload.Name,
                        Password = "123456",
                        Phone = " "

                    };

                    await AddMember(member);


                    // 登入
                    var LoginInfo = _context.Members.FirstOrDefault(p => p.Email == payload.Email);

                    var LoginId = LoginInfo.MemberId;

                    var LoginName = LoginInfo.Name;

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Sid, LoginId.ToString()),
                        new Claim(ClaimTypes.Name, LoginName),
                        new Claim(ClaimTypes.Role, "google")
                    };

                    var claimsIdentity = new ClaimsIdentity(
                               claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
                    };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                            new ClaimsPrincipal(claimsIdentity), properties);


                    return RedirectToAction("Index", "Member");
                }
            }



            return NoContent();
        }

        public async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string? formCredential, string? formToken, string? cookiesToken)
        {
            // 檢查空值
            if (formCredential == null || formToken == null && cookiesToken == null)
            {
                return null;
            }

            GoogleJsonWebSignature.Payload? payload;
            try
            {
                // 驗證 token
                if (formToken != cookiesToken)
                {
                    return null;
                }

                // 驗證憑證
                IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
                string GoogleApiClientId = Config.GetSection("GoogleApiClientId").Value;
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { GoogleApiClientId }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(formCredential, settings);
                if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
                {
                    return null;
                }
                if (payload.ExpirationTimeSeconds == null)
                {
                    return null;
                }
                else
                {
                    DateTime now = DateTime.Now.ToUniversalTime();
                    DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
                    if (now > expiration)
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
            return payload;
        }


    }
}
