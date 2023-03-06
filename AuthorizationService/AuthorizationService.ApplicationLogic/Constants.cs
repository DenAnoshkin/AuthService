using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthorizationService.ApplicationLogic
{
    public class Constants
    {

        public const string Issuer = "AuthorizationService";
        public const string Audience = "OtherService";

        public static SymmetricSecurityKey Key => new(Encoding.UTF8.GetBytes("SomeSecterKey99!"));

        internal static SymmetricSecurityKey KeyForRefreshToken => new(Encoding.UTF8.GetBytes("SomeSecterRefreshKey99!"));
    }
}
