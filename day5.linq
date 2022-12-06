<Query Kind="Program" />

int day = 5;
void Main()
{
	var current = Path.GetDirectoryName(Util.CurrentQueryPath);
	var rawSample = File.ReadAllLines(Path.Combine(current, $"day{day}.sample.dat"));
	var rawInput = File.ReadAllLines(Path.Combine(current, $"day{day}.dat"));

	var game = Game.Init(rawSample);
	game.GetTop().Dump("Sample - Init");
	game.MoveSingleton();
	game.GetTop().Dump("Sample - singleton");

	game = Game.Init(rawInput);
	game.GetTop().Dump("input - Init");
	game.MoveSingleton();
	game.GetTop().Dump("input - singleton");

	game = Game.Init(rawSample);
	game.GetTop().Dump("Sample - Init");
	game.MoveBatch();
	game.GetTop().Dump("Sample - batch");

	game = Game.Init(rawInput);
	game.GetTop().Dump("input - Init");
	game.MoveBatch();
	game.GetTop().Dump("input - batch");
}

public class Game
{
	public Stack[] Towers { get; set; }
	public IEnumerable<Move> Moves { get; set; }

	public static Game Init(string[] spec)
	{
		var index = spec.Select((i, c) => new { Index = c, Item = i }).First(i => string.IsNullOrWhiteSpace(i.Item)).Index;

		var towerCount = spec[index - 1].Where(c => Char.IsDigit(c)).Count();
		var towers = Enumerable.Range(0, towerCount).Select(_ => new Stack()).ToArray();

		for (var r = index - 2; r > -1; r--)
		{
			var row = spec[r];
			for (int c = 1, column = 0; c < 4 * towerCount; c += 4, column++)
			{
				if (Char.IsLetter(row[c]))
				{
					towers[column].Push(row[c]);
				}
			}

		}

		return new Game { Moves = spec.Skip(index + 1).Select(Move.Parse).ToArray(), Towers = towers };
	}

	public void MoveSingleton()
	{
		foreach (var move in Moves)
		{
			foreach (var item in Enumerable.Range(0, move.Count).Select(_ => Towers[move.Source-1].Pop()))
			{
				Towers[move.Target-1].Push(item);
			}
		}
	}

	public void MoveBatch()
	{
		foreach (var move in Moves)
		{
			foreach (var item in Enumerable.Range(0, move.Count).Select(_ => Towers[move.Source - 1].Pop()).Reverse())
			{
				Towers[move.Target - 1].Push(item);
			}
		}
	}


	public string GetTop() => string.Join("", Towers.Select(i => i.Peek()));
}

public class Move
{
	private static Regex Linex = new Regex("move (?<count>\\d+) from (?<source>\\d) to (?<target>\\d)", RegexOptions.Compiled);

	public int Count { get; set; }
	public int Source { get; set; }
	public int Target { get; set; }

	public Move()
	{
	}

	public Move(int count, int source, int target)
	{
		Count = count;
		Source = source;
		Target = target;
	}

	public static Move Parse(string line)
	{
		var m = Linex.Match(line);
		var count = int.Parse(m.Groups["count"].Value);
		var source = int.Parse(m.Groups["source"].Value);
		var target = int.Parse(m.Groups["target"].Value);
		return new Move(count, source, target);
	}
}