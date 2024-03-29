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

### Refresh tokens

- get cookies
<img src="/pictures/cookies.png" title="cookies"  width="900">

- invalid refresh token
<img src="/pictures/invalid_refresh_token.png" title="invalid refresh token"  width="900">

- login again. Valid refresh token
<img src="/pictures/valid_refresh_token.png" title="valid refresh token"  width="900">



## ASP.NET CORE Web API - JWT Authentication (Refresh Token + Role Based + Entity Framework ) 

### Packages
```
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Tools
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.AspNetCore.Authentication.JwtBearer
Swashbuckle.AspNetCore
Swashbuckle.AspNetCore.Swagger
Swashbuckle.AspNetCore.SwaggerUI
```

### In the Package Manager Console
```
Add-Migration InitialCreate
Update-Database
```

### Use API

- register user
<img src="/pictures/register_user.png" title="register user"  width="900">

- authenticate
<img src="/pictures/authenticate.png" title="authenticate"  width="900">

- retrieve token
<img src="/pictures/token.png" title="retrieve token"  width="900">

- create customer
<img src="/pictures/customer.png" title="create customer"  width="900">

- get customer
<img src="/pictures/customer2.png" title="get customer"  width="900">
