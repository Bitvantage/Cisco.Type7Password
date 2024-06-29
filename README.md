# Bitvantage.Cisco.Type7Password
Encrypts, decrypts, and re-salts Cisco type 7 passwords. Type 7 passwords are typically used in configuration files for Cisco's routers and switches as a way to obfuscate clear text passwords.

## Installing via NuGet Package Manager
```
PM> NuGet\Install-Package Bitvantage.Cisco.Type7Password
```

## Quickstart
``` csharp
var type7Password = Type7Password.Encrypt("Not very secure");
Console.WriteLine(type7Password.Password);                      // Not very secure
Console.WriteLine(type7Password.EncryptedPassword);             // 080F435A490F00050B4B1F01293E362D
```

## Background
Type 7 passwords are used in the configuration files for Cisco routers and switches. They are NOT considered secure as they use a simple substitution cipher with a well-known key, are easily reversible, and exist more as a way to obfuscate clear text passwords from casual observation than to provide any level of security.

Many existing tools exist to encrypt and decrypt type 7 passwords, inclusive of the built-in key-chain commands provided by Cisco on Cisco's routers and switches.

Cisco recommends that type 7 passwords should not be used, but then goes on to require them in numerous contexts making following their recommendation nearly impossible.