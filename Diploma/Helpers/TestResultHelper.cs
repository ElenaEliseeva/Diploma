namespace Diploma.Helpers;

public class TestResultHelper
{
    public static string CreateWordFromTestResults(IReadOnlyList<char> testResults)
    {
        var dict = new Dictionary<char, int>
        {
            { 'E', 0 },
            { 'I', 0 },
            { 'S', 0 },
            { 'N', 0 },
            { 'T', 0 },
            { 'F', 0 },
            { 'J', 0 },
            { 'P', 0 }
        };

        for (int i = 0; i < testResults.Count; i++)
        {
            switch (i)
            {
                case 0 or 7 or 14 or 28 when testResults[i] == 'A':
                    dict['E']++;
                    break;
                case 0 or 7 or 14 or 28:
                    dict['I']++;
                    break;
                case 1 or 8 or 15 or 22 or 29 or 2 or 9 or 16 or 23 or 30 when testResults[i] == 'A':
                    dict['S']++;
                    break;
                case 1 or 8 or 15 or 22 or 29 or 2 or 9 or 16 or 23 or 30:
                    dict['N']++;
                    break;
                case 3 or 10 or 17 or 24 or 31 or 4 or 11 or 18 or 25 or 32 when testResults[i] == 'A':
                    dict['T']++;
                    break;
                case 3 or 10 or 17 or 24 or 31 or 4 or 11 or 18 or 25 or 32:
                    dict['F']++;
                    break;
                case 5 or 12 or 19 or 26 or 33 or 6 or 13 or 20 or 27 or 34 when testResults[i] == 'A':
                    dict['J']++;
                    break;
                case 5 or 12 or 19 or 26 or 33 or 6 or 13 or 20 or 27 or 34:
                    dict['P']++;
                    break;
            }
        }

        return string.Join("", new List<char>
        {
            dict.FirstOrDefault(x => x.Value == Math.Max(dict['E'], dict['I']) && x.Key is 'E' or 'I').Key,
            dict.FirstOrDefault(x => x.Value == Math.Max(dict['S'], dict['N']) && x.Key is 'S' or 'N').Key,
            dict.FirstOrDefault(x => x.Value == Math.Max(dict['T'], dict['F']) && x.Key is 'T' or 'F').Key,
            dict.FirstOrDefault(x => x.Value == Math.Max(dict['J'], dict['P']) && x.Key is 'J' or 'P').Key
        });
    }
}