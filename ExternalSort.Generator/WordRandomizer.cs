namespace ExternalSort.Generator;

public class WordRandomizer
{
    private readonly string[] _nouns = File.ReadAllLines("Dictionaries/nouns.txt");
    private readonly string[] _adjectives = File.ReadAllLines("Dictionaries/adjectives.txt");

    public string NextNoun()
    {
        return _nouns[Random.Shared.Next(_nouns.Length)];
    }

    public string NextAdjective()
    {
        return _adjectives[Random.Shared.Next(_adjectives.Length)];
    }
}