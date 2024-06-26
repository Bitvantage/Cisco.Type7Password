# Bitvantage.Cisco.Type7Password
Encrypts, decrypts, and re-salts Cisco type 7 passwords. Type 7 passwords are typically used in configuration files for Cisco's routers and switches.

## Installing via NuGet Package Manager
```
PM> NuGet\Install-Package Bitvantage.Cisco.Type7Password
```

## Quickstart
``` csharp
var type7Password = Type7Password.Encrypt("Not very secure");
Console.WriteLine(type7Password.Password);
Console.WriteLine(type7Password.EncryptedPassword);
```