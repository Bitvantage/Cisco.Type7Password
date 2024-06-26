/*
   Bitvantage.Cisco.Type7Password
   Copyright (C) 2024 Michael Crino
   
   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU Affero General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.
   
   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU Affero General Public License for more details.
   
   You should have received a copy of the GNU Affero General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.Text;

namespace Bitvantage.Cisco;

public class Type7Password : IEquatable<Type7Password>
{
    private static readonly byte[] Key =
    {
        0x64, 0x73, 0x66, 0x64, 0x3B, 0x6B, 0x66, 0x6F, 0x41, 0x2C,
        0x2E, 0x69, 0x79, 0x65, 0x77, 0x72, 0x6B, 0x6C, 0x64, 0x4A,
        0x4B, 0x44, 0x48, 0x53, 0x55, 0x42, 0x73, 0x67, 0x76, 0x63,
        0x61, 0x36, 0x39, 0x38, 0x33, 0x34, 0x6E, 0x63, 0x78, 0x76,
        0x39, 0x38, 0x37, 0x33, 0x32, 0x35, 0x34, 0x6B, 0x3B, 0x66,
        0x67, 0x38, 0x37
    };

    private static readonly Random Random = new();
    public string EncryptedPassword { get; }
    public string Password { get; }

    public int Salt { get; }

    private Type7Password(string password, string encryptedPassword, int salt)
    {
        Password = password;
        EncryptedPassword = encryptedPassword;
        Salt = salt;
    }

    public Type7Password(string encryptedPassword)
    {
        var type7Password = Decrypt(encryptedPassword);
        Password = type7Password.Password;
        EncryptedPassword = type7Password.EncryptedPassword;
        Salt = type7Password.Salt;
    }

    public bool Equals(Type7Password? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Password == other.Password;
    }

    public static Type7Password Decrypt(string encryptedPassword)
    {
        if (encryptedPassword.Length < 4 || encryptedPassword.Length > 52 || encryptedPassword.Length % 2 != 0)
            throw new ArgumentException("Bad password length");

        var salt = int.Parse(encryptedPassword.Substring(0, 2));
        if (salt >= 16)
            throw new ArgumentException("Bad key offset");

        var encrypted = encryptedPassword.Substring(2);
        var decrypted = new StringBuilder();

        for (var counter = 0; counter < encrypted.Length; counter += 2)
        {
            var encryptedHexByte = encrypted.Substring(counter, 2);
            var encryptedInteger = Convert.ToInt32(encryptedHexByte, 16);
            int keyChar = Key[(counter / 2 + salt) % 53];
            var decChar = encryptedInteger ^ keyChar;

            decrypted.Append((char)decChar);
        }

        return new Type7Password(decrypted.ToString(), encryptedPassword, salt);
    }

    public Type7Password Encrypt(int newSalt)
    {
        var newPassword = Encrypt(Password, newSalt);

        return newPassword;
    }

    public static Type7Password Encrypt(string value)
    {
        return Encrypt(value, Random.Next(16));
    }

    public static Type7Password Encrypt(string value, int salt)
    {
        if (value.Length is < 1 or > 25)
            throw new ArgumentException("Bad password length");

        if (salt is < 0 or > 15)
            throw new ArgumentException("Salt must be between 0 and 15");

        var encrypted = new StringBuilder(salt.ToString("D2"));

        for (var counter = 0; counter < value.Length; counter++)
        {
            int decChar = value[counter];
            int keyChar = Key[(counter + salt) % 53];
            var encChar = decChar ^ keyChar;
            encrypted.Append(encChar.ToString("X2"));
        }

        return new Type7Password(value, encrypted.ToString(), salt);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (obj.GetType() != GetType())
            return false;

        return Equals((Type7Password)obj);
    }

    public override int GetHashCode()
    {
        return Password.GetHashCode();
    }

    public static bool operator ==(Type7Password? left, Type7Password? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Type7Password? left, Type7Password? right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return Password;
    }
}