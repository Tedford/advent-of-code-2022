<Query Kind="Statements" />

var sample = File.ReadAllLines(@"C:\Projects\advent-of-code-2022\day1.sample.dat");
var input = File.ReadAllLines(@"C:\Projects\advent-of-code-2022\day1.dat");

IEnumerable<int> Bin(IEnumerable<string> data) {
	var bins = new List<int>();
	var index = 0;
	foreach (var l in data ) {
		if( string.IsNullOrWhiteSpace(l)) {
			index++;
		}
		else {
			if( bins.Count < index+1) {
				bins.Insert(index,0);
			}
			bins[index] += int.Parse(l);
		}
	}
	return bins;	
}


var binnedSample = Bin(sample).Select((i,index) => new { Elf = index + 1, Calories = i}).OrderByDescending(i => i.Calories);
binnedSample.Dump();
var binnedInput = Bin(input).Select((i,index) => new { Elf = index + 1, Calories = i}).OrderByDescending(i => i.Calories);
binnedInput.Dump();

binnedSample.Take(3).Dump("[Sample] Top 3");
binnedInput.Take(3).Dump("[Input] Top 3");
