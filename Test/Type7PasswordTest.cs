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

using Bitvantage.Cisco;

namespace Test
{
    internal class Type7PasswordTest
    {
        [Test]
        public void KnowValuesTest01()
        {
            Type7Password type7Password;
            Type7Password decryptedPassword;

            type7Password = Type7Password.Encrypt("password", 0);
            Assert.That(type7Password.EncryptedPassword, Is.EqualTo("00141215174C04140B"));

            decryptedPassword = Type7Password.Decrypt(type7Password.EncryptedPassword);
            Assert.That(decryptedPassword.Password, Is.EqualTo("password"));

            type7Password = Type7Password.Encrypt("password", 5);
            Assert.That(type7Password.EncryptedPassword, Is.EqualTo("051B071C325B411B1D"));

            decryptedPassword = Type7Password.Decrypt(type7Password.EncryptedPassword);
            Assert.That(decryptedPassword.Password, Is.EqualTo("password"));

            type7Password = Type7Password.Encrypt("password", 15);
            Assert.That(type7Password.EncryptedPassword, Is.EqualTo("15020A1F173D24362C"));

            decryptedPassword = Type7Password.Decrypt(type7Password.EncryptedPassword);
            Assert.That(decryptedPassword.Password, Is.EqualTo("password"));
        }

        [Test]
        public void InstanceTest01()
        {
            var password1 = Type7Password.Encrypt("this is a test", 3);
            var password2 = Type7Password.Encrypt("this is a test", 10);

            Assert.That(password1, Is.EqualTo(password2));
            Assert.That(password1.GetHashCode(), Is.EqualTo(password2.GetHashCode()));

            Assert.That(password1.Password, Is.EqualTo("this is a test"));
            Assert.That(password2.Password, Is.EqualTo("this is a test"));

            Assert.That(password1.EncryptedPassword, Is.Not.EqualTo(password2.EncryptedPassword));
        }

        [Test]
        public void Example01()
        {
            var type7Password = Type7Password.Encrypt("Not very secure");
            Console.WriteLine(type7Password.Password);                      // Not very secure
            Console.WriteLine(type7Password.EncryptedPassword);             // 080F435A490F00050B4B1F01293E362D
        }
    }
}