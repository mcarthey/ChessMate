using Xunit.Abstractions;

namespace ChessMate.Tests;

public class CustomTestOutputHelper : ITestOutputHelper
{
    private readonly ITestOutputHelper _output;
    private readonly StringWriter _buffer = new StringWriter();

    public CustomTestOutputHelper(ITestOutputHelper output)
    {
        _output = output;
    }

    public void Write(string message)
    {
        _buffer.Write(message);
    }

    public void WriteLine(string message)
    {
        _buffer.WriteLine(message);
    }

    public void WriteLine(string format, params object[] args)
    {
        _buffer.WriteLine(format, args);
    }

    public void Flush()
    {
        // Write the buffered content to the output
        _output.WriteLine(_buffer.ToString());

        // Clear the buffer
        _buffer.GetStringBuilder().Clear();
    }
}