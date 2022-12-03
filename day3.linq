<Query Kind="Program" />

int day = 3;

void Main()
{
	var current = Path.GetDirectoryName(Util.CurrentQueryPath);
	var rawSample = File.ReadAllLines(Path.Combine(current, $"day{day}.sample.dat"));
	var rawInput = File.ReadAllLines(Path.Combine(current, $"day{day}.dat"));

	int EvalPriorities(IEnumerable<string> rucksacks)
	{
		return rucksacks.Select(i => (i.Substring(0, i.Length / 2), i.Substring(i.Length / 2)))
		.Select(FindCommon)
		.Select(Score)
		.Sum();
	}

	EvalPriorities(rawSample).Dump("Sample - Prorities ");
	EvalPriorities(rawInput).Dump("Input- Priorities");

	int EvalBadges(IEnumerable<string> rucksacks)
	{
		return rucksacks
				.Chunk(3)
				.Select(FindCommon)
				.Select(Score)
				.Sum();
	}

	EvalBadges(rawSample).Dump("Sample - Badges");
	EvalBadges(rawInput).Dump("Input - Badges");


}

public char FindCommon((string first, string second) t) => t.first.First(c => t.second.IndexOf(c) > -1);

public char FindCommon(IEnumerable<string> slice) {
	var first = slice.First();
	var second = slice.ElementAt(1);
	var third = slice.ElementAt(2);
	
	return first.First(c => second.IndexOf(c) > -1 && third.IndexOf(c) > -1  );
}

public int Score(char c) => Char.IsUpper(c) ? c - 'A' + 27 : c - 'a' + 1;

