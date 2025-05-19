namespace GameBoxSdk.Runtime.Utils
{
    public static class MathfExtensions
    {
        public static int Mod(int number, int divisor)
        {
            return (number % divisor + divisor) % divisor;
        }
    }
}

