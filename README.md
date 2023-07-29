# .NET 6 Web API Read JWT Authorization Claims of a User (from a JSON Web Token)

## Jwt & Role Based Authorization

- add packages
```
Microsoft.AspNetCore.Authentication.JwtBearer
Microsoft.IdentityModel.Tokens
Swashbuckle.AspNetCore
Swashbuckle.AspNetCore.Filters
System.IdentityModel.Tokens.Jwt
```

### Read Claims in the controller

- register a new user
<img src="/pictures/register.png" title="register a new user"  width="900">

- login with that user
<img src="/pictures/login.png" title="register a new user"  width="900">

- authenticate with token
<img src="/pictures/auth_with_token.png" title="authenticate with token"  width="900">

- authenticate without token
<img src="/pictures/auth.png" title="authenticate without token"  width="900">

- verify signature
<img src="/pictures/verify.png" title="verify signature"  width="900">

## Refresh tokens