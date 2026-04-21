using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BeritaDlanggu.Helpers
{
    public static class StringExtensions
    {
        public static string GenerateSlug(this string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return "";

            // to lowercase
            string slug = title.ToLowerInvariant();

            // remove accents (é → e)
            slug = RemoveDiacritics(slug);

            // remove invalid chars
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // convert multiple spaces into one
            slug = Regex.Replace(slug, @"\s+", " ").Trim();

            // replace spaces with dash
            slug = slug.Replace(" ", "-");

            // remove multiple dashes
            slug = Regex.Replace(slug, @"-+", "-");

            return slug;
        }

        public static string RemoveDiacritics(this string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = Char.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
