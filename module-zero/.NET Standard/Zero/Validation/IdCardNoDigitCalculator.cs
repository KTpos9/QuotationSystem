namespace Zero.Validation
{
    internal class IdCardNoDigitCalculator
    {
        private const int Priority = 14;

        public int Calculate(int digit, int sequence)
        {
            int multiplier = Priority - sequence;
            return digit * multiplier;
        }
    }
}