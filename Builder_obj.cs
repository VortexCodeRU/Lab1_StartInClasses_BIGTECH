using System;
public class PlaylistSpec
{
    public string Name { get; }
    public string GenreFilter { get; }
    public int MaxDurationSec { get; }
    public string Order { get; }
    public string Author { get; }

    internal PlaylistSpec(string name, string genre, int maxDur, string order, string author)
    {
        Name = name;
        GenreFilter = genre;
        MaxDurationSec = maxDur;
        Order = order;
        Author = author;
    }
}

public class PlaylistSpecBuilder
{
    private string name;
    private string genreFilter = "";
    private int maxDurationSec = 3600;
    private string order = "byKey";
    private string author = "Unknown";

    public PlaylistSpecBuilder SetName(string str_name) { name = str_name; return this; }
    public PlaylistSpecBuilder SetGenre(string g) { genreFilter = g; return this; }
    public PlaylistSpecBuilder SetMaxDuration(int sec) { maxDurationSec = sec; return this; }
    public PlaylistSpecBuilder SetOrder(string o) { order = o; return this; }
    public PlaylistSpecBuilder SetAuthor(string a) { author = a; return this; }

    public PlaylistSpec Build()
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Подборка без названия не создаётся");
        if (maxDurationSec < 1)
            throw new ArgumentException("Лимит длительности должен быть ≥ 1");

        return new PlaylistSpec(name, genreFilter, maxDurationSec, order, author);
    }
}