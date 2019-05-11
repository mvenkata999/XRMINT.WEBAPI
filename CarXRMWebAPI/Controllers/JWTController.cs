using CarXRMWebAPI.Factory;
using CarXRMWebAPI.Models;
using CarXRMWebAPI.Models.Helpers;
using CarXRMWebAPI.Models.Security;
using CarXRMWebAPI.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;


namespace CarXRMWebAPI.Controllers
{

    //[Authorize(Policy = "AuthorizationPolicy")]    
    [ApiVersion("1.0")]
    [Area("Security")]
    [Route("api/{version:apiVersion}/[area]/[controller]")]
    public class JWTController : Controller
    {
        private readonly IJwtFactory _jwtFactory = null;
        private readonly JwtIssuerOptions _jwtOptions;        
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly string _connectionString;
        private readonly IJwtToken _jwtToken = null;
        private readonly ILogger _logger;

        public JWTController(IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, ILoggerFactory loggerFactory, IJwtToken jwtToken)
        {
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _logger = loggerFactory.CreateLogger<JWTController>();            
            _jwtToken = jwtToken;

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            _connectionString = Startup.ConnectionString;
            
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginUser loginUser)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Begin Login", loginUser.UserName + ":" + loginUser.Password);
            if (!ModelState.IsValid)
            {
                _logger.LogInformation(LoggingEvents.GetItem, "Model state is invalid", loginUser.UserName + loginUser.Password);
                return BadRequest(ModelState);
            }

            var identity = await _jwtFactory.GetClaimsIdentity(loginUser);
            if (identity == null)
            {
                _logger.LogInformation(LoggingEvents.GetItem, "Identity is null", loginUser.UserName + loginUser.Password);
                _logger.LogInformation($"Invalid username ({loginUser.UserName}) or password ({loginUser.Password})");
                return BadRequest("Invalid credentials");
            }

            //  var claims = new[]
            //    {
            //  new Claim("UserID",user.UserID.ToString()),
            //  new Claim("UserName",user.Username),
            //  new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            //  new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
            //  new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64), identity.FindFirst("Test")
            //};

            //  // Create the JWT security token and encode it.
            //  var jwt = new JwtSecurityToken(
            //      issuer: _jwtOptions.Issuer,
            //      audience: _jwtOptions.Audience,
            //      claims: claims,
            //      notBefore: _jwtOptions.NotBefore,
            //      expires: _jwtOptions.Expiration,
            //      signingCredentials: _jwtOptions.SigningCredentials);

            //  var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //  // Serialize and return the response
            //  var response = new
            //  {
            //      access_token = encodedJwt,
            //      expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            //  };

            else
            {
                _logger.LogInformation(LoggingEvents.GenerateItems, "Begin generating Jwt token", loginUser.UserName + loginUser.Password);
                var jwt = await _jwtToken.GenerateJwt(identity, _jwtFactory, identity.Name, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
                _logger.LogInformation(LoggingEvents.GenerateItems, "End generating Jwt token", loginUser.UserName + loginUser.Password);
                return new OkObjectResult(jwt);
            }

            //var json = JsonConvert.SerializeObject(response, _serializerSettings);
            //return new OkObjectResult(json);
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>



        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        //    private static long ToUnixEpochDate(DateTime date)
        //      => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        //    /// <summary>
        //    /// IMAGINE BIG RED WARNING SIGNS HERE!
        //    /// You'd want to retrieve claims through your claims provider
        //    /// in whatever way suits you, the below is purely for demo purposes!
        //    /// </summary>
        //    private static Task<ClaimsIdentity> GetClaimsIdentity(User user)
        //    {
        //        if (user == null)
        //        {
        //            // Credentials are invalid, or account doesn't exist
        //            return Task.FromResult<ClaimsIdentity>(null);
        //        }
        //        if (user.UserID == 0)
        //        {
        //            return Task.FromResult(new ClaimsIdentity(new GenericIdentity(user.Username, "Test"),
        //                new Claim[] { }));
        //        }
        //        return Task.FromResult(new ClaimsIdentity(new GenericIdentity(user.Username, "Test"),
        //          new[]
        //          {
        //        new Claim("Test", "Test123")
        //          }));
        //    }

        //    private User GetUser(string UserName, string password)
        //    {
        //        User user = new User();
        //        user.Username = UserName;
        //        user.UserID = 1234;

        //        return user;
        //    }
        //}
        //public class User
        //{
        //    private int _UserID = 0;
        //    private string _Username = string.Empty;

        //    public int UserID { get => _UserID; set => _UserID = value; }
        //    public string Username { get => _Username; set => _Username = value; }
        //}
    }
}

