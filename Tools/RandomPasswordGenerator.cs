using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace OnlineConsulting.Tools
{

        public sealed class RandomPasswordGenerator : IDisposable
        {

            public enum CharType
            {
                Letter,
                Number,
                Special
            }

            private readonly PasswordOptions options;

            private readonly RNGCryptoServiceProvider rngCsp = new();

            private readonly Dictionary<CharType, string> randomChars = new()
            {
                { CharType.Letter, "abcdefghijklmnoprsuwxyz" },
                { CharType.Number, "0123456789" },
                { CharType.Special, "!@#$_-" }
            };

            public RandomPasswordGenerator(PasswordOptions options = null)
            {
                this.options = options ?? new PasswordOptions();
            }

            public string Generate()
            {
                var uniqueChars = new HashSet<char>(options.RequiredUniqueChars);
                var plainTextPassword = new StringBuilder(options.RequiredLength);
                int randomPlainTextIndex() => RandomIndex(options.RequiredLength);
                var usedIndexes = new List<int>(2);

                while (uniqueChars.Count != options.RequiredUniqueChars)
                {
                    uniqueChars.Add(randomChars[CharType.Letter][randomPlainTextIndex()]);
                }

                foreach (var singleChar in uniqueChars)
                {
                    plainTextPassword.Append(singleChar);
                }

                for (var i = options.RequiredUniqueChars; i < options.RequiredLength; ++i)
                {
                    plainTextPassword.Append(randomChars[CharType.Letter][randomPlainTextIndex()]);
                }

                if (options.RequireUppercase)
                {
                    var randomIndex = randomPlainTextIndex();
                    usedIndexes.Add(randomIndex);

                    plainTextPassword[randomIndex] = char.ToUpper(plainTextPassword[randomIndex]);
                }

                if (options.RequireDigit)
                {
                    var randomIndex = randomPlainTextIndex();
                    while (usedIndexes.Contains(randomIndex))
                    {
                        randomIndex = randomPlainTextIndex();
                    }
                    usedIndexes.Add(randomIndex);

                    plainTextPassword[randomIndex] = GetRandomNumber();
                }

                if (options.RequireNonAlphanumeric)
                {
                    var randomIndex = randomPlainTextIndex();
                    while (usedIndexes.Contains(randomIndex))
                    {
                        randomIndex = randomPlainTextIndex();
                    }

                    plainTextPassword[randomIndex] = GetRandomSpecialChar();
                }

                return plainTextPassword.ToString();
            }

            public void Dispose()
            {
                rngCsp.Dispose();
            }

            private char GetRandomNumber()
            {
                var randomIndex = RandomIndex(randomChars[CharType.Number].Length);
                return randomChars[CharType.Number][randomIndex];
            }

            private char GetRandomSpecialChar()
            {
                var randomIndex = RandomIndex(randomChars[CharType.Special].Length);
                return randomChars[CharType.Special][randomIndex];
            }

            private int RandomIndex(int maxValue)
            {
                var randomNumber = new byte[1];
                rngCsp.GetBytes(randomNumber);

                return randomNumber[0] % maxValue;
            }
        }
    
}
