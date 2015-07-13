namespace EsentManagementStudio.Extensions
{
    public static class StringExt
    {
        public static string With(this string s, string pattern, string value)
        {
            return string.IsNullOrEmpty(value)
                ? s
                : s.Replace($"{{{pattern}}}", value);
        } 
    }
}
