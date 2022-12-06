<Query Kind="Program" />

int day = 6;
void Main()
{
	var current = Path.GetDirectoryName(Util.CurrentQueryPath);
	var rawSample = File.ReadAllLines(Path.Combine(current, $"day{day}.sample.dat"));
	var rawInput = File.ReadAllLines(Path.Combine(current, $"day{day}.dat"));

	var calibrations = new[]{
		("mjqjpqmgbljsphdztnvjfqwrcgsmlb",7),
		("bvwbjplbgvbhsrlpgdmjqwftvncz",5),
		("nppdvjthqldpwncqszvftbrmjlhg",6),
		("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg",10),
		("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw",11),
	};

	var width = 4;
	var calibrated = true;
	foreach (var calibration in calibrations)
	{
		var frame = FindSignalStart(calibration.Item1, width);
		Console.WriteLine($"[{(calibration.Item2 == frame ? "VALID" : "FAILED")}] Start frame for {calibration.Item1} is {frame}");
		calibrated &= frame == calibration.Item2;
	}

	if (calibrated)
	{
		FindSignalStart(rawInput[0], width).Dump($"Input - {width}-bit Signal Start");
	}

	calibrations = new[]{
		("mjqjpqmgbljsphdztnvjfqwrcgsmlb",19),
		("bvwbjplbgvbhsrlpgdmjqwftvncz",23),
		("nppdvjthqldpwncqszvftbrmjlhg",23),
		("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg",29),
		("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw",26),
	};

	width = 14;
	calibrated = true;
	foreach (var calibration in calibrations)
	{
		var frame = FindSignalStart(calibration.Item1, width);
		Console.WriteLine($"[{(calibration.Item2 == frame ? "VALID" : "FAILED")}] Start frame for {calibration.Item1} is {frame}");
		calibrated &= frame == calibration.Item2;
	}

	if (calibrated)
	{
		FindSignalStart(rawInput[0], width).Dump($"Input - {width}-bit Signal Start");
	}
}

public int FindSignalStart(string signal, int markerWidth)
{
	var found = false;
	var frame = -1;
	for (int i = markerWidth; i < signal.Length && !found; i++)
	{
		found = !signal[(i - markerWidth)..i].GroupBy(i => i).Any(i => i.Count() > 1);
		frame = i;
	}
	return frame;
}