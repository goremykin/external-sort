namespace ExternalSort.CustomSorter;

public class LineComparer : IComparer<string>
{
    private readonly string _separator = ". ";
    
    public int Compare(string? x, string? y)
    {
        if (x == null && y == null)
        {
            return 0;
        }

        if (x == null)
        {
            return -1;
        }

        if (y == null)
        {
            return 1;
        }
        
        var xSpan = x.AsSpan();
        var ySpan = y.AsSpan();

        var xPieces = xSpan.Split(_separator);
        var yPieces = ySpan.Split(_separator);

        xPieces.MoveNext();
        yPieces.MoveNext();

        var xNumberRange = xPieces.Current;
        var yNumberRange = yPieces.Current;
        
        xPieces.MoveNext();
        yPieces.MoveNext();
        
        var xTextRange = xPieces.Current;
        var yTextRange = yPieces.Current;
        
        var textComparisonResult = xSpan[xTextRange].CompareTo(ySpan[yTextRange], StringComparison.Ordinal);

        if (textComparisonResult != 0)
        {
            return textComparisonResult;
        }
        
        var xNumber = int.Parse(xSpan[xNumberRange]);
        var yNumber = int.Parse(ySpan[yNumberRange]);
        
        return xNumber.CompareTo(yNumber);
    }
}