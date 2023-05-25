namespace ErrorCorrectingCoding;

public class Application
{
    private static readonly byte[] InitialCode = new byte[]
    {
        0, 1, 0, 0, 
        0, 1, 0, 0, 
        0, 0, 1, 1, 
        1, 1, 0, 1
    };

    private const int HammingAdditionalCodeCount = 5;

    private static readonly byte[] HammingCodeWithError = new byte[]
    {
        1, 0, 0, 1, 
        1, 0, 0, 0,
        1, 1, 0, 0,
        0, 0, 1, 0, 
        1, 1, 1, 0, 1
    };

    private static readonly IErrorCorrectingEncoder Encoder = new HammingEncoder();

    public static void Main()
    {
        EncodeData();
        FindErrorInCode();
    }

    private static void EncodeData()
    {
        var code = string.Join("", Encoder.Encode(InitialCode));
        Console.WriteLine(code);
    }

    private static void FindErrorInCode()
    {
        var errorNumber = Encoder.FindError(HammingCodeWithError, HammingAdditionalCodeCount);
        var text = errorNumber == -1 ? "There was no error in the code." : $"An error was found in bit number {errorNumber}.";
        Console.WriteLine(text);
    }
}