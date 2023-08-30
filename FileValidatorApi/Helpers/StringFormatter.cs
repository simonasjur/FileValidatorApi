
namespace FileValidatorApi.Helpers;

public static class StringFormatter
{
    public static string ListToFormattedString(List<string> words, char separator)
    {
        if (words.Count == 0)
        {
            return string.Empty;
        }
        
        words[0] = char.ToUpper(words[0][0]) + words[0].Substring(1);

        return string.Join(separator + " ", words);
    }
}