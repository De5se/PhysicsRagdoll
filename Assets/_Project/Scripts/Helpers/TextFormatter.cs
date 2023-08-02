namespace _Scripts.Helpers
{
    public static class TextFormatter
    {
        private const string Format = "F1";
        
        public static string MoneyText(float value)
        {
            var result = value.ToString("F0");
            if (value >= 1e9)
            {
                result = (value / 1e9).ToString(Format) + 'B';
            }
            else if (value >= 1e6)
            {
                result = (value / 1e6).ToString(Format) + 'M';
            }
            else if (value >= 1e4)
            {
                result = (value / 1e3).ToString(Format) + 'k';
            }
            return result;
        }
    }
}