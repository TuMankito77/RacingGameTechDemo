namespace GameBoxSdk.Runtime.Utils
{
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringUtil
    {
        /// <summary>
        /// Convert a given string into the variable format used for creating constants.
        /// All the characters are uppercase and words are separated by an underscore (_)
        /// </summary>
        /// <param name="source">String to modify</param>
        /// <returns>Constant formatted string</returns>
        public static string ToConstantFormat(this string source)
        {
            Regex nonAlphaNumeric = new Regex(@"\W|_");
            source = nonAlphaNumeric.Replace(source, string.Empty);

            Regex uppercaseMatch = new Regex(@"[A-Z]");
            MatchCollection matches = uppercaseMatch.Matches(source, 1);
            StringBuilder stringBuilder = new StringBuilder();
            int startIndex = 0;

            foreach (Match match in matches)
            {
                int capitalLeterIndex = match.Index;

                stringBuilder.Append(source.Substring(startIndex, capitalLeterIndex - startIndex).ToUpperInvariant());
                stringBuilder.Append('_');

                startIndex = match.Index;
            }

            stringBuilder.Append(source.Substring(startIndex, source.Length - startIndex).ToUpperInvariant());

            return stringBuilder.ToString();
        }
    }
}
