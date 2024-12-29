namespace ChessMate.Models;

public readonly struct Position : IEquatable<Position>
{
    public int Row { get; }
    public int Col { get; }

    // Computed property for Notation
    public string Notation => ToChessNotation();

    // Constructor accepting row and column
    public Position(int row, int col)
    {
        Row = row;
        Col = col;
    }

    // Constructor accepting notation
    public Position(string notation)
    {
        (Row, Col) = FromChessNotation(notation);
    }

    // Method to convert to chess notation
    public string ToChessNotation()
    {
        char colChar = (char)('a' + Col); // Convert column index to letter
        int rowNumber = 8 - Row; // Flip row index for chess notation
        return $"{colChar}{rowNumber}";
    }

    // Static method to parse notation
    public static (int Row, int Col) FromChessNotation(string notation)
    {
        if (string.IsNullOrWhiteSpace(notation) || notation.Length != 2)
            throw new ArgumentException("Invalid chess notation.");

        char colChar = notation[0];
        if (colChar < 'a' || colChar > 'h')
            throw new ArgumentException("Invalid column in chess notation.");

        if (!int.TryParse(notation[1].ToString(), out int rowNumber) || rowNumber < 1 || rowNumber > 8)
            throw new ArgumentException("Invalid row in chess notation.");

        int col = colChar - 'a';
        int row = 8 - rowNumber;
        return (row, col);
    }

    // Override ToString to return notation
    public override string ToString() => Notation;

    // Implement IEquatable<Position>
    public bool Equals(Position other)
    {
        return Row == other.Row && Col == other.Col;
    }

    public override bool Equals(object obj)
    {
        return obj is Position other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Col);
    }

    public static bool operator ==(Position left, Position right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Position left, Position right)
    {
        return !(left == right);
    }
}

