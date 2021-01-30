using System;

namespace Ravid.Utilities
{
    public static class RandomKey
    {
        public static string GeneratePassword(int numSmallLetters, int numNumbers, int numCapitalLetters)
        {
            var rng = new Random();
            var strPass = "";


            const string alphaSmall = "abcdefghijklmnopqrstuvwxyz";

            for (var i = 0; i < numSmallLetters; i++)
            {
                strPass += alphaSmall[rng.Next(alphaSmall.Length)];
            }


            const string num = "0123456789";
            for (var i = 0; i < numNumbers; i++)
            {

                strPass += rng.Next(num.Length);
            }



            const string alphaCapital = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            for (var i = 0; i < numCapitalLetters; i++)
            {
                strPass += alphaCapital[rng.Next(alphaCapital.Length)];
            }



            return Shuffle(strPass);
        }

        public static string Shuffle(this string str)
        {
            char[] array = str.ToCharArray();
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }

    }
}
