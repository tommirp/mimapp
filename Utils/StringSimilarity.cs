namespace MimApp.Utils
{
    public static class StringSimilarity
    {
        public static int Levenshtein(string source, string target)
        {
            if (string.IsNullOrEmpty(source)) return target?.Length ?? 0;
            if (string.IsNullOrEmpty(target)) return source.Length;

            var lengthA = source.Length;
            var lengthB = target.Length;
            var matrix = new int[lengthA + 1, lengthB + 1];

            for (var i = 0; i <= lengthA; matrix[i, 0] = i++) { }
            for (var j = 0; j <= lengthB; matrix[0, j] = j++) { }

            for (var i = 1; i <= lengthA; i++)
            {
                for (var j = 1; j <= lengthB; j++)
                {
                    var cost = target[j - 1] == source[i - 1] ? 0 : 1;
                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost
                    );
                }
            }

            return matrix[lengthA, lengthB];
        }
    }

}
