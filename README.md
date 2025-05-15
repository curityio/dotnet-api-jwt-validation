# Secure a .NET API with JWT Access Tokens

[![Quality](https://img.shields.io/badge/quality-demo-red)](https://curity.io/resources/code-examples/status/)
[![Availability](https://img.shields.io/badge/availability-source-blue)](https://curity.io/resources/code-examples/status/)

A demo API to show how to use JWTs for authorization in .NET APIs.\
The code uses the [JWT Bearer Middleware](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/configure-jwt-bearer-authentication) and [Policy Based Authorization](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/policies).

## Configure the API

The API uses an `appSettings.json` file to configure its expected issuer, audience and JWT signing algorithm:

```json
{
  "Authorization": {
    "Issuer": "https://idsvr.example.com/oauth/v2/oauth-anonymous",
    "Audience": "demo-api",
    "Algorithm": "RS256"
  }
}
```

## Run the Example

Ensure that an up to date [.NET SDK](https://dotnet.microsoft.com/en-us/download) is installed, then run the example:

```bash
export ASPNETCORE_ENVIRONMENT='Development'
dotnet build
dotnet run
```

## Run a Deployed API

To run the API in a [Docker](https://docs.docker.com/engine/install/) container, execute the deployment script:

```bash
./deployment/run.sh
```

## Further Information

- See the [API Tutorial](https://curity.io/resources/learn/dotnet-api) for further details on the example API's security behavior.
- See the [JWT Best Practices](https://curity.io/resources/learn/jwt-best-practices/) article for further information on using JWTs correctly.
