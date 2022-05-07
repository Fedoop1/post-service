using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace PostService.Common.Jwt.Attributes
{
    public class JwtAuthAttribute : AuthorizeAttribute
    {
        public JwtAuthAttribute(string policy = "") : base(policy)
        {
            this.AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
