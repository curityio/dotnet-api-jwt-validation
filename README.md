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
    "Issuer": "https://login.example.com/oauth/v2/oauth-anonymous",
    "Audience": "demo-api",
    "Algorithm": "RS256"
  }
}
```

## Configure the Curity Identity Server

Before running the app you need to configure an authorization server like a local Docker instance of the Curity Identity Server:

- [Run a local Docker instance](https://curity.io/resources/learn/run-curity-docker/).
- [Use the token designer to configure scopes and claims](https://curity.io/resources/learn/token-designer/).
- [Create a client that gets an access token to send to the API](https://curity.io/resources/learn/configure-client/).

## Run the Example

Ensure that an up to date [.NET SDK](https://dotnet.microsoft.com/en-us/download) is installed, then run the example.\
Use developer-specific settings if required, such as the use of HTTP OAuth URLs.

```bash
export ASPNETCORE_ENVIRONMENT='Development'
dotnet build
dotnet run
```

The configuration uses a local example domain for the authorization server.\
To use such a domain, add the following entry to your local computer's hosts file:

```text
127.0.0.1 login.example.com
```

## Call the API

You can then act as an OAuth client to get an access token and call the API.\
The following endpoint returns normal sensitivity data and requires a `read` scope:

```bash
curl -i http://localhost:5000/demo/data -H "Authorization: Bearer $ACCESS_TOKEN"
```

The following endpoint return higher sensitivity data and also requires a custom `risk` claim with a value below 50.\
Such a claim might originate from an external system like a risk engine.

```bash
curl -i http://localhost:5000/demo/highworthdata -H "Authorization: Bearer $ACCESS_TOKEN"
```

## Run a Deployed API

To run the API in a [Docker](https://docs.docker.com/engine/install/) container, execute the deployment script:

```bash
./deployment/run.sh
```

## Further Information

- See the [.NET API Tutorial](https://curity.io/resources/learn/dotnet-api) for further details on the example API's security behavior.
- See the [JWT Best Practices](https://curity.io/resources/learn/jwt-best-practices/) article for further information on using JWTs securely.
