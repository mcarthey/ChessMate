namespace ChessMate.Utilities;

public static class ChessNotationUtility
{
    /// <summary>
    /// Converts row/col indices to chess notation (e.g., (0, 0) -> "a8").
    /// </summary>
    /// <param name="position">The row/col indices as a tuple.</param>
    /// <returns>The position in chess notation.</returns>
    public static string ToChessNotation((int Row, int Col) position)
    {
        char col = (char)('a' + position.Col); // Convert column index to letter
        int row = 8 - position.Row; // Flip row index for chess notation
        return $"{col}{row}";
    }

    /// <summary>
    /// Converts chess notation (e.g., "a8") to row/col indices.
    /// </summary>
    /// <param name="notation">The chess notation (e.g., "a8").</param>
    /// <returns>The row/col indices as a tuple.</returns>
    public static (int Row, int Col) FromChessNotation(string notation)
    {
        if (notation.Length != 2)
            throw new ArgumentException("Invalid chess notation.");

        char colChar = notation[0];
        int rowNumber = int.Parse(notation[1].ToString());

        if (colChar < 'a' || colChar > 'h' || rowNumber < 1 || rowNumber > 8)
            throw new ArgumentException("Invalid chess notation.");

        int col = colChar - 'a'; // Convert letter to column index
        int row = 8 - rowNumber; // Flip row for internal representation
        return (row, col);
    }

    /// <summary>
    /// Validates whether a given chess notation is valid.
    /// </summary>
    /// <param name="notation">The chess notation to validate.</param>
    /// <returns>True if valid, false otherwise.</returns>
    public static bool IsValidChessNotation(string notation)
    {
        if (notation.Length != 2)
            return false;

        char colChar = notation[0];
        if (colChar < 'a' || colChar > 'h')
            return false;

        char rowChar = notation[1];
        if (rowChar < '1' || rowChar > '8')
            return false;

        return true;
    }
}