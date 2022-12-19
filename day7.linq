<Query Kind="Program" />

int day = 7;
int MaxSize = 70000000;
int NeededSpace = 30000000;
bool trace = false;

void Main()
{
	var current = Path.GetDirectoryName(Util.CurrentQueryPath);
	var rawSample = File.ReadAllLines(Path.Combine(current, $"day{day}.sample.dat"));
	var rawInput = File.ReadAllLines(Path.Combine(current, $"day{day}.dat"));

	var sampleFS = Traverse(rawSample).Dump("Sample", 0);
	Predicate<IElfNode> dirLessThan100k = i => i.HasContents && i.Size < 100000;
	var filteredSamples = Filter(sampleFS, dirLessThan100k).Dump("Filtered Sample", 0);
	if (filteredSamples.Sum(i => i.Size).Dump("Sample") != 95437)
	{
		throw new Exception("Filtered size is incorrect");
	}

	var inputFS = Traverse(rawInput).Dump("Input", 0);
	var filteredInput = Filter(inputFS, dirLessThan100k).Dump("Filtered input", 0);
	filteredInput.Sum(i => i.Size).Dump("Part 1");

	var sampleTarget = DetermineSpaceRequirement(sampleFS);
	if (sampleTarget != 8381165)
	{
		throw new Exception($"Sample free space is incorrect, expected 8381165 but was {sampleTarget}");
	}

	Predicate<IElfNode> targetSize = i => i.HasContents && i.Size > sampleTarget;

	var sampleCleanup = Filter(sampleFS, targetSize).OrderBy(i => i.Size).Dump("Sample Cleanup");
	if (sampleCleanup.First().Name != "d")
	{
		throw new Exception($"Expected the direct d to be the cleanup directory, instead {sampleCleanup.First().Name} was identified");
	}
	
	var target = DetermineSpaceRequirement(inputFS);

	targetSize = i => i.HasContents && i.Size > target;
	var cleanup = Filter(inputFS, targetSize).OrderBy(i => i.Size).Dump("Cleanup");
	cleanup.Take(5).Dump();
}

public int DetermineSpaceRequirement(IElfNode fs) => NeededSpace - (MaxSize - fs.Size); 

public IEnumerable<IElfNode> Filter(IElfNode location, Predicate<IElfNode> selector)
{
	var nodes = new List<IElfNode>();
	if (selector(location))
	{
		nodes.Add(location);
	}
	if (location.HasContents)
	{
		var dir = (ElfDir)location;
		nodes.AddRange(dir.Files.SelectMany(i => Filter(i, selector)));
	}
	return nodes;
}

//public int CalculateSize(ElfDir dir) => dir.Files.Sum(i => i.HasContents ? CalculateSize((ElfDir)i) : i.Size);

public ElfDir Traverse(IEnumerable<string> lines)
{
	Dictionary<string, ElfDir> fs = new();
	var path = "/";
	fs[path] = new ElfDir(path);
	ElfDir current = null;

	foreach (var line in lines)
	{
		if (line.StartsWith("$"))
		{
			var parts = line.Substring(2).Split(" ");
			var command = parts[0];
			var args = parts.ElementAtOrDefault(1);

			if (command == "cd")
			{
				if (args == "/")
				{
					path = args;
					current = fs[args];
					if (trace)
					{
						Console.WriteLine($"[{path}] Moved to Root");
					}
				}
				else if (args == "..")
				{
					path = path.Substring(0, path[0..^1].LastIndexOf("/") + 1);
					current = fs[path];
					if (trace)
					{
						Console.WriteLine($"[{path}] Moved up a directory");
					}
				}
				else
				{
					path += args + "/";
					if (!fs.TryGetValue(path, out current))
					{
						current = new ElfDir(path);
						fs[path] = current;
					}
					if (trace)
					{
						Console.WriteLine($"[{path}] Descended a directory");
					}
				}
			}
			if (command == "ls")
			{
				// no-op
			}
		}
		else
		{
			var parts = line.Split(" ");
			if (int.TryParse(parts[0], System.Globalization.NumberStyles.None, null, out int size))
			{
				var file = new ElfFile(path, parts[1], size);
				current.Files.Add(file);
			}
			else
			{
				var dirpath = $"{path}{parts[1]}/";
				if (!fs.TryGetValue(dirpath, out ElfDir subdir))
				{
					subdir = new ElfDir(dirpath);
					fs[dirpath] = subdir;
				}
				if (!current.Files.Contains(subdir))
				{
					current.Files.Add(subdir);
				}
			}

		}
	}

	return fs["/"];
}

public interface IElfNode
{
	public string FullPath { get; set; }
	public string Name { get; set; }
	public bool HasContents { get; }
	public int Size { get; }
}

public class ElfFile : IElfNode
{
	public string FullPath { get; set; }
	public string Name { get; set; }
	public int Size { get; set; }
	public bool HasContents => false;

	public ElfFile(string path, string name, int size)
	{
		Size = size;
		Name = name;
		FullPath = $"{path}{name}";
	}
}

public class ElfDir : IElfNode
{
	public string FullPath { get; set; }
	public string Name { get; set; }
	public List<IElfNode> Files { get; } = new();
	public bool HasContents => true;
	public int Size => Files.Sum(i => i.Size);

	public ElfDir(string fullPath)
	{
		FullPath = fullPath;
		Name = fullPath.Split("/", StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? string.Empty;
	}
}