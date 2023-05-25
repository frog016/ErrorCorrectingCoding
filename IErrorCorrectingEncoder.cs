namespace ErrorCorrectingCoding;

public interface IErrorCorrectingEncoder
{
    byte[] Encode(byte[] data);
    int FindError(byte[] data, int count);
}