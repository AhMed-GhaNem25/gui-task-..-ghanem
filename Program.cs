using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_encrypt_project
{
    class Program
    {
        static void Main(string[] args)
        {
            ICipher cipher = new SubstitutionCipher();

            run(cipher);

            Console.ReadKey();
        }


        private static void run(ICipher cipher)
        {
            try
            {
                encrypt(cipher);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR -- {ex.Message}");
                Console.WriteLine(Environment.NewLine);
                encrypt(cipher);
            }
        }

        private static void encrypt(ICipher cipher)
        {
            Console.Write("Enter a key of 10 digits: ");
            var key = Console.ReadLine();

            Console.Write("Enter message to encrypt: ");
            var value = Console.ReadLine();

            try
            {
                Console.WriteLine(cipher.Encrypt(value, key));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR -- {ex.Message}");
                Console.WriteLine(Environment.NewLine);
                run(cipher);
            }
        }
    }

    class SubstitutionCipher : ICipher
    {
        private readonly StringBuilder _builder;
        private string _value;
        private long _key;

        public SubstitutionCipher()
        {
            _builder = new StringBuilder();
        }

        public void SetKey(string key)
        {
            if (!Checks.IsNumber(key))
                throw new KeyLengthException();

            if (Checks.HaveDuplicates(key))
                throw new KeyFormatException();

            _key = long.Parse(key);
        }

        public void SetInput(string value)
        {
            if (!Checks.IsNumber(value))
                throw new InputFormatException();
            else
                _value = value;
        }

        public string Encrypt(string value, string key)
        {
            SetKey(key);
            SetInput(value);

            for (int i = 0; i < _value.Length; i++)
            {
                char ch = (char)(((long)_value[i] +
                                _key - 48) % 10 + 48); // x = ( value + key - index ) % 10 + index
                _builder.Append(ch);
            }
            return _builder.ToString();
        }
    }

    public interface ICipher
    {
        void SetKey(string key);
        void SetInput(string input);

        string Encrypt(string input, string key);
    }

    public static class Checks
    {
        // the sum of 10 none duplicate digits is always 45
        public static bool HaveDuplicates(string key)
        {
            int digitsSum = 0;

            foreach (var digitChar in key.ToCharArray())
            {
                var digit = int.Parse(digitChar.ToString());
                digitsSum = digitsSum + digit;
            }

            if (digitsSum == 45)
                return false;
            else
                return true;
        }

        // Check if the key characters are anywhere between 0 and 9
        public static bool IsNumber(string key)
        {
            foreach (char c in key)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }

    [Serializable]
    public class InputFormatException : FormatException
    {
        public InputFormatException()
            : base("Input must be a number") { }
    }

    [Serializable]
    public class KeyFormatException : FormatException
    {
        public KeyFormatException()
            : base("Key Must not have duplicates and be a number") { }
    }

    [Serializable]
    public class KeyLengthException : ApplicationException
    {
        public KeyLengthException()
            : base("Key Must be 10 digits") { }
    }
}
