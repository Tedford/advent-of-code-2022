<Query Kind="Program" />

int day = 4;
void Main()
{
	var current = Path.GetDirectoryName(Util.CurrentQueryPath);
	var rawSample = File.ReadAllLines(Path.Combine(current, $"day{day}.sample.dat"));
	var rawInput = File.ReadAllLines(Path.Combine(current, $"day{day}.dat"));

	var overlap = FindTotalOverlap(rawSample.Select(Parse));
	overlap.Count().Dump("Sample Overlap");

	overlap = FindTotalOverlap(rawInput.Select(Parse));
	overlap.Count().Dump("Input Overlap");

	overlap = FindAnyOverlap(rawSample.Select(Parse));
	overlap.Count().Dump("Sample Any Overlap");

	overlap = FindAnyOverlap(rawInput.Select(Parse));
	overlap.Count().Dump("Input Any Overlap");

}

public IEnumerable<(Range, Range)> FindTotalOverlap(IEnumerable<(Range, Range)> data)
{
	return data.Where(tuple => tuple.Item1.Min >= tuple.Item2.Min && tuple.Item1.Max <= tuple.Item2.Max ||
	tuple.Item2.Min >= tuple.Item1.Min && tuple.Item2.Max <= tuple.Item1.Max).ToArray();
}

public IEnumerable<(Range, Range)> FindAnyOverlap(IEnumerable<(Range, Range)> data)
{
	return data.Where(tuple => tuple.Item1.Min >= tuple.Item2.Min && tuple.Item1.Min <= tuple.Item2.Max ||
	tuple.Item2.Min >= tuple.Item1.Min && tuple.Item2.Min <= tuple.Item1.Max).ToArray();
}

public class Range
{
	public int Min { get; set; }
	public int Max { get; set; }
	public Range()
	{

	}
	public Range(int min, int max)
	{
		Min = min;
		Max = max;
	}
}

public (Range, Range) Parse(string line)
{
	var parts = line.Split(',');
	var nums = parts[0].Split('-');
	var first = new Range(int.Parse(nums[0]), int.Parse(nums[1]));
	nums = parts[1].Split('-');
	var second = new Range(int.Parse(nums[0]), int.Parse(nums[1]));
	return (first, second);
}

// You can define other methods, fields, classes and namespaces here