using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace WebApplication1.Handler;

public class BasicAuthenticationHandler: AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            string authorizationHeader = Request.Headers["Authorization"];
            if (authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                string base64Credentials = authorizationHeader.Substring("Basic ".Length).Trim();
                byte[] credentialBytes = Convert.FromBase64String(base64Credentials);
                string[] credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                string username = credentials[0];
                string password = credentials[1];

                if (IsAuthenticated(username, password))
                {
                    var claims = new[] {
                        new Claim(ClaimTypes.Name, username),
                    };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
                else
                {
                    return AuthenticateResult.Fail("Invalid username or password");
                }
            }

            return AuthenticateResult.Fail("Invalid Authorization Header");
        }

        private bool IsAuthenticated(string username, string password)
        {
            return username == "your_username" && password == "your_password";
        }
    }
