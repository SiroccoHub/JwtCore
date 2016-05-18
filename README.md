# Json Web Token (JWT) / JWS Implementation for .NET Core

This library supports generating and decoding [JSON Web Tokens](http://tools.ietf.org/html/draft-jones-json-web-token-10). 

## Features
* Support .NET Core / (ASP.NET Core).
* Two Extention Methods for Converting Unix Timestamp between .NET DateTime. 
* Simple usage.

## Installation
At first, You need to install Newtonsoft.Json.  [FYI](http://www.newtonsoft.com/json).  
and, Please  download and compile JwtCore yourself  or Install by NuGet,

```console
PM> Install-Package JwtCore
```
NuGet repo is [here](https://www.nuget.org/packages/JwtCore/).

## Usage
### Creating Tokens

```csharp
var payload = new Dictionary<string, object>()
{
    { "claim1", 0 },
    { "claim2", "claim2-value" }
};
var secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
string token = JwtCore.JsonWebToken.Encode(payload, secretKey, JwtCore.JwtHashAlgorithm.HS256);
Console.WriteLine(token);
```

Output will be:
    eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJjbGFpbTEiOjAsImNsYWltMiI6ImNsYWltMi12YWx1ZSJ9.8pwBI_HtXqI3UgQHQ_rDRnSQRxFL1SR8fbQoS-5kM5s

### Verifying and Decoding Tokens

```csharp
var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJjbGFpbTEiOjAsImNsYWltMiI6ImNsYWltMi12YWx1ZSJ9.8pwBI_HtXqI3UgQHQ_rDRnSQRxFL1SR8fbQoS-5kM5s";
var secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
try
{
    string jsonPayload = JwtCore.JsonWebToken.Decode(token, secretKey);
    Console.WriteLine(jsonPayload);
}
catch (JwtCore.SignatureVerificationException)
{
    Console.WriteLine("Invalid token!");
}
```

Output will be:

    {"claim1":0,"claim2":"claim2-value"}

You can also deserialize the JSON payload directly to a .Net object with DecodeToObject:

```csharp
var payload = JwtCore.JsonWebToken.DecodeToObject(token, secretKey) as IDictionary<string, object>;
Console.WriteLine(payload["claim2"]);
```

which will output:
    
    claim2-value

#### exp claim

As described in the [JWT RFC](https://tools.ietf.org/html/draft-ietf-oauth-json-web-token-32#section-4.1.4) the `exp` "claim identifies the expiration time on or after which the JWT MUST NOT be accepted for processing." If an `exp` claim is present and is prior to the current time the token will fail verification. The exp (expiry) value must be specified as the number of seconds since 1/1/1970 UTC.

```csharp
var now = DateTime.UtcNow.ToUnixTimeSeconds();
var payload = new Dictionary<string, object>()
{
    { "exp", now }
};
var secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
string token = JwtCore.JsonWebToken.Encode(payload, secretKey, JwtCore.JwtHashAlgorithm.HS256);
string jsonPayload = JwtCore.JsonWebToken.Decode(token, secretKey);
```

if you will decode  json that has invalid Unix Timestamp, you'll get some exception.

```csharp
string jsonPayload = JwtCore.JsonWebToken.Decode(token, secretKey); // JwtCore.SignatureVerificationException!
```

### Configure JSON Serialization

By default JSON Serialization is done by Newtonsoft.Json.  To configure a different one first implement the IJsonSerializer interface.

```csharp
public class CustomJsonSerializer : IJsonSerializer
{
    public string Serialize(object obj)
    {
        // Implement using favorite JSON Serializer
    }

    public T Deserialize<T>(string json)
    {
        // Implement using favorite JSON Serializer
    }
}
```

Next configure this serializer as the JsonSerializer.
```cs
JsonWebToken.JsonSerializer = new CustomJsonSerializer();
```
