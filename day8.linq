<Query Kind="Program" />

int day = 8;
bool trace = false;
void Main()
{
	var current = Path.GetDirectoryName(Util.CurrentQueryPath);
	var rawSample = File.ReadAllLines(Path.Combine(current, $"day{day}.sample.dat"));
	var rawInput = File.ReadAllLines(Path.Combine(current, $"day{day}.dat"));

	var sample = ToMatrix(rawSample).ToArray();

	var sampleCount = CountVisible(sample).Dump("Sample");
	if (trace)
	{
		PrettyPrint(CreateVisbilityMask(sample)).Dump("Sample");
	}
	if (sampleCount != 21)
	{
		throw new Exception($"Expected 21 instead {sampleCount} detected");
	}

	var input = ToMatrix(rawInput).ToArray();
	if (trace)
	{
		PrettyPrint(CreateVisbilityMask(input)).Dump("Input");
	}
	CountVisible(input).Dump("Input");

	var sampleScore = ScoreVisbility(sample);
	if (trace)
	{
		PrettyPrint(sampleScore).Dump("Sample");
	}
	if (sampleScore[1][2] != 4)
	{
		throw new Exception($"Expected 4 instead {sampleScore[1][2]} detected");
	}
	if (sampleScore[3][2] != 8)
	{
		throw new Exception($"Expected 4 instead {sampleScore[3][2]} detected");
	}
	
	var inputScore = ScoreVisbility(input);
	MaxScore(inputScore).Dump("Input");
}

public IEnumerable<int[]> ToMatrix(IEnumerable<string> lines)
{
	foreach (var line in lines)
	{
		yield return line.Select(c => int.Parse(c.ToString())).ToArray();
	}
}

public int[][] CreateVisbilityMask(int[][] matrix)
{
	int rows = matrix.Length;
	int columns = matrix[0].Length;
	var mask = new int[rows][];
	for (int i = 0; i < rows; i++)
	{
		mask[i] = Enumerable.Repeat(1, columns).ToArray();
		mask[i][0] = matrix[i][0];
		mask[i][^1] = matrix[i][^1];
	}
	for (int i = 1; i < columns - 2; i++)
	{
		mask[0][i] = matrix[0][i];
		mask[^1][i] = matrix[^1][i];
	}

	for (int r = 1; r < rows - 1; r++)
	{
		for (int c = 1; c < columns - 1; c++)
		{
			var height = matrix[r][c];

			bool FromLeft()
			{
				var isVisible = true;
				for (int n = 0; n < c && isVisible; n++)
				{
					var neighbor = matrix[r][n];
					isVisible &= height > neighbor;
				}
				return isVisible;
			}

			bool FromRight()
			{
				var isVisible = true;
				for (int n = columns - 1; n > c && isVisible; n--)
				{
					var neighbor = matrix[r][n];
					isVisible &= height > neighbor;
				}
				return isVisible;
			}


			bool FromTop()
			{
				var isVisible = true;
				for (int n = 0; n < r && isVisible; n++)
				{
					var neighbor = matrix[n][c];
					isVisible &= height > neighbor;
				}
				return isVisible;
			}

			bool FromBottom()
			{
				var isVisible = true;
				for (int n = rows - 1; n > r && isVisible; n--)
				{
					var neighbor = matrix[n][c];
					isVisible &= height > neighbor;
				}
				return isVisible;
			}


			mask[r][c] = FromLeft() || FromRight() || FromTop() || FromBottom() ? height : 0;
		}
	}



	return mask;
}

public string PrettyPrint(int[][] matrix)
{
	var sb = new StringBuilder();
	foreach (var row in matrix)
	{
		foreach (var column in row)
		{
			sb.Append(column);
		}
		sb.AppendLine();
	}

	return sb.ToString();
}

public int[][] ScoreVisbility(int[][] matrix)
{
	var rows = matrix.Length;
	var columns = matrix[0].Length;
	var visiblity = new int[rows][];
	for (int i = 0; i < columns; i++)
	{
		visiblity[i] = new int[columns];
	}

	for (int r = 0; r < rows - 1; r++)
	{
		for (int c =0; c < columns - 1; c++)
		{
			var height = matrix[r][c];

			int ToLeft()
			{
				var isVisible = true;
				int count = 0;
				for (int n = c - 1; n > -1 && isVisible; n--)
				{
					var neighbor = matrix[r][n];
					isVisible &= height > neighbor;
					count++;
				}
				return count;
			}

			int ToRight()
			{
				var isVisible = true;
				int count = 0;
				for (int n = c + 1; n < columns && isVisible; n++)
				{
					var neighbor = matrix[r][n];
					isVisible &= height > neighbor;
					count++;
				}
				return count;
			}


			int ToTop()
			{
				var isVisible = true;
				int count = 0;
				for (int n = r - 1; n > -1 && isVisible; n--)
				{
					var neighbor = matrix[n][c];
					isVisible &= height > neighbor;
					count++;
				}
				return count;
			}

			int ToBottom()
			{
				var isVisible = true;
				int count = 0;
				for (int n = r + 1; n < rows && isVisible; n++)
				{
					var neighbor = matrix[n][c];
					isVisible &= height > neighbor;
					count++;
				}
				return count;
			}


			visiblity[r][c] = ToLeft() * ToRight() * ToTop() * ToBottom();
		}
	}

	return visiblity;

}

public int MaxScore(int[][] matrix)
{
	int max = 0;
	var rows = matrix.Length;
	var columns = matrix[0].Length;

	for (int r = 1; r < rows - 1; r++)
	{
		for (int c = 1; c < rows - 1; c++)
		{
			max = Math.Max(max, matrix[r][c]);
		}
	}

	return max;

}

public int CountVisible(int[][] matrix)
{
	int rows = matrix.Length;
	int columns = matrix[0].Length;
	int visible = columns * 2 + (rows - 2) * 2;

	for (int r = 1; r < rows - 1; r++)
	{
		for (int c = 1; c < columns - 1; c++)
		{
			var height = matrix[r][c];


			bool FromLeft()
			{
				var isVisible = true;
				for (int n = 0; n < c && isVisible; n++)
				{
					var neighbor = matrix[r][n];
					isVisible &= height > neighbor;
				}
				return isVisible;
			}

			bool FromRight()
			{
				var isVisible = true;
				for (int n = columns - 1; n > c && isVisible; n--)
				{
					var neighbor = matrix[r][n];
					isVisible &= height > neighbor;
				}
				return isVisible;
			}


			bool FromTop()
			{
				var isVisible = true;
				for (int n = 0; n < r && isVisible; n++)
				{
					var neighbor = matrix[n][c];
					isVisible &= height > neighbor;
				}
				return isVisible;
			}

			bool FromBottom()
			{
				var isVisible = true;
				for (int n = rows - 1; n > r && isVisible; n--)
				{
					var neighbor = matrix[n][c];
					isVisible &= height > neighbor;
				}
				return isVisible;
			}


			visible += FromLeft() || FromRight() || FromTop() || FromBottom() ? 1 : 0;
		}
	}

	return visible;
}