<Query Kind="Program" />

int day = 2;

void Main()
{
	var current = Path.GetDirectoryName(Util.CurrentQueryPath);
	var sample = File.ReadAllLines(Path.Combine(current, $"day{day}.sample.dat")).Select(Parse);
	var raw = File.ReadAllLines(Path.Combine(current, $"day{day}.dat")).Select(Parse);
	
	sample.Dump();
}

enum Action {
	Rock  = 1,
	Paper = 2,
	Sissors = 3
}

(Action Opponent, Action Player) Parse(string raw) {
	var parts = raw.Split(" ");
	var opponent =  parts[0] switch {
		"A" => Action.Rock,
		"B" => Action.Paper,
		"C" => Action.Sissors
	};
	var player = parts[1] switch
	{
		"X" => Action.Rock,
		"Y" => Action.Paper,
		"Z" => Action.Sissors
	};
	return (opponent, player);
}

int ScoreRound((Action opponent, Action player) round) {
	var score = 0;
	if( round.opponent == round.player) {
		score = 3;
	} else if (round.opponent == Action.Rock && round.player == Action.Paper 
				|| round.opponent==Action.Paper && round.player == Action.Sissors 
				|| round.opponent == Action.Sissors && round.player == Action.Rock) {
		score = 6;	
	}
	score += (int)round.player;
	
	return score;
}

int Score(IEnumerable<(Action Opponent, Action Player)> rounds) => rounds.Select(ScoreRound).Sum();
