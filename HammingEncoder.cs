namespace ErrorCorrectingCoding;

public class HammingEncoder : IErrorCorrectingEncoder
{
    public byte[] Encode(byte[] data)
    {
        var table = CreateDataWithAdditionalCode(data);
        var controlBytesCount = table.Length - data.Length;
        var result = HammingMethod(controlBytesCount, table);

        return result;
    }

    public int FindError(byte[] data, int count)
    {
        var initialCode = ClearControlBytes(data);
        var table = HammingMethod(count, initialCode);

        var errorCodeIndex = -1;
        for (var index = 1; index <= data.Length; index++)
            if (data[index - 1] != table[index - 1])
                errorCodeIndex += index;

        return errorCodeIndex != -1 ? errorCodeIndex + 1 : -1;
    }

    private static byte[] HammingMethod(int controlBytesCount, byte[] table)
    {
        var copy = table.ToArray();
        for (var byteDegree = 0; byteDegree < controlBytesCount; byteDegree++)
        {
            var value = GetControlByteValue(copy, byteDegree);
            var byteIndex = (int)Math.Pow(2, byteDegree) - 1;
            copy[byteIndex] = value;
        }

        return copy;
    }

    private static byte[] CreateDataWithAdditionalCode(byte[] data)
    {
        var controlBytesCount = GetControlBytesCount(data.Length);
        var resultLength = data.Length + controlBytesCount;

        var table = new byte[resultLength];
        var dataIndex = 0;
        for (var index = 1; index <= table.Length; index++)
        {
            var code = IsDegreeOfTwo(index) ? (byte)0 : data[dataIndex++];
            table[index - 1] = code;
        }

        return table;
    }

    private static byte GetControlByteValue(byte[] table, int byteDegree)
    {
        var controlByte = (int)Math.Pow(2, byteDegree);
        var startIndex = controlByte - 1;

        var result = 0;
        for (var index = startIndex; index < table.Length; index += 2 * controlByte)
        {
            var endIndex = Math.Clamp(index + controlByte, 0, table.Length);
            var arrayWindow = table[index..endIndex];
            result += arrayWindow.Sum(value => value);
        }

        return (byte)(result % 2);
    }

    private static int GetControlBytesCount(int dataLength)
    {
        var count = Math.Log2(dataLength) + 1;
        return (int)Math.Ceiling(count);
    }

    private static byte[] ClearControlBytes(byte[] code)
    {
        var copy = code.ToArray();
        for (var byteDegree = 0;; byteDegree++)
        {
            var byteIndex = (int)Math.Pow(2, byteDegree) - 1;
            if (byteIndex >= code.Length)
                break;

            copy[byteIndex] = 0;
        }

        return copy;
    }

    private static bool IsDegreeOfTwo(int number)
    {
        return (number & (number - 1)) == 0;
    }
}