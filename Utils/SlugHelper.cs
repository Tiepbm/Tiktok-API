using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace TikTokAPI.Utils;

public static class SlugHelper
{
    public static string ToSlug(string phrase)
    {
        string str = phrase.ToLowerInvariant();

        // Remove diacritics (tiếng Việt)
        str = RemoveDiacritics(str);

        // Replace invalid chars
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

        // Convert multiple spaces to single hyphen
        str = Regex.Replace(str, @"\s+", "-").Trim();

        // Trim hyphens
        str = Regex.Replace(str, @"-+", "-");

        return str;
    }

    public static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var ch in normalized)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                sb.Append(ch);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}