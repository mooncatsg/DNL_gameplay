using System;
using System.Linq;
using System.Numerics;
using System.Text;

public static class BigIntegerHelper
{
    private static readonly BigInteger Ten = new BigInteger(10);
    public static BigInteger ToBigInteger(this ulong ul)
    {
        return new BigInteger(ul);
    }
    public static BigInteger ToBigInteger(this long ul)
    {
        return new BigInteger((ulong)ul);
    }
    public static BigInteger ToBigInteger(this int ul)
    {
        return new BigInteger((ulong)ul);
    }
    public static BigInteger ToBigInteger(this uint ul)
    {
        return new BigInteger((ulong)ul);
    }
    public static BigInteger Sqrt(this BigInteger number)
    {
        BigInteger n = 0, p = 0;
        if (number == BigInteger.Zero)
            return BigInteger.Zero;
        var high = number >> 1;
        var low = BigInteger.Zero;
        while (high > low + 1)
        {
            n = (high + low) >> 1;
            p = n * n;
            if (number < p)
                high = n;
            else if (number > p)
                low = n;
            else
                break;
        }

        return number == p ? n : low;
    }
    /// <summary>
    ///     Creates a new BigInteger from a binary (Base2) string
    /// </summary>
    public static BigInteger NewBigInteger2(this string binaryValue)
    {
        BigInteger res = 0;
        if (binaryValue.Count(b => b == '1') + binaryValue.Count(b => b == '0') != binaryValue.Length) return res;
        foreach (var c in binaryValue)
        {
            res <<= 1;
            res += c == '1' ? 1 : 0;
        }

        return res;
    }
    /// <summary>
    ///     Get the bitwidth of this biginteger n
    /// </summary>
    public static int GetBitwidth(this BigInteger n)
    {
        return n.ToByteArray().Length << 3;
    }
    ///// <summary>
    /////     Get the Maxvalue for a biginteger of this bitlength
    ///// </summary>
    //public static BigInteger GetMaxValue(int bitlength)
    //{
    //    var buffer = "";
    //    if (!bitlength.IsEven())
    //        buffer = "7f";
    //    var ByteLength = bitlength >> 3;
    //    for (var i = 0; i < ByteLength; ++i)
    //        buffer += "ff";
    //    return ToBigInteger16(buffer);
    //}
    ///// <summary>
    /////     Converts a hex number (0xABCDEF or ABCDEF) into a BigInteger
    ///// </summary>
    //public static BigInteger ToBigInteger16(this string hexNumber)
    //{
    //    if (string.IsNullOrEmpty(hexNumber))
    //        throw new Exception("Error: hexNumber cannot be either null or have a length of zero.");
    //    if (!hexNumber.Equals("0123456789abcdefABCDEFxX"))
    //        throw new Exception("Error: hexNumber cannot contain characters other than 0-9,a-f,A-F, or xX");
    //    hexNumber = hexNumber.ToUpper();
    //    if (hexNumber.IndexOf("0X", StringComparison.OrdinalIgnoreCase) != -1)
    //        hexNumber = hexNumber.Substring(2);
    //    var bytes = Enumerable.Range(0, hexNumber.Length)
    //        .Where(x => x % 2 == 0)
    //        .Select(x => Convert.ToByte(hexNumber.Substring(x, 2), 16))
    //        .ToArray();
    //    return new BigInteger(bytes.Concat(new byte[] { 0 }).ToArray());
    //}
    /// <summary>
    ///     Creates a new BigInteger from a BigInteger
    /// </summary>
    public static BigInteger NewBigInteger(this BigInteger value)
    {
        return new BigInteger(value.ToByteArray());
    }
    /// <summary>
    ///     Creates a new BigInteger from a hex (Base16) string
    /// </summary>
    public static BigInteger NewBigInteger16(this string hexValue)
    {
        return new BigInteger(HexToByteArray(hexValue).Concat(new byte[] { 0 }).ToArray());
    }
    /// <summary>
    ///     Creates a new BigInteger from a number (Base10) string
    /// </summary>
    public static BigInteger NewBigInteger10(this string str)
    {
        if (str[0] == '-')
            throw new Exception("Invalid numeric string. Only positive numbers are allowed.");
        var number = new BigInteger();
        int i;
        for (i = 0; i < str.Length; i++)
            if (str[i] >= '0' && str[i] <= '9')
                number = number * Ten + long.Parse(str[i].ToString());
        return number;
    }
    public static BigInteger ToBigIntegerBase10(this string str)
    {
        if (str[0] == '-')
            throw new Exception("Invalid numeric string. Only positive numbers are allowed.");
        var number = new BigInteger();
        int i;
        for (i = 0; i < str.Length; i++)
            if (str[i] >= '0' && str[i] <= '9')
                number = number * Ten + long.Parse(str[i].ToString());
        return number;
    }
    /// <summary>
    ///     Return a byte array that represents this hex string
    /// </summary>
    private static byte[] HexToByteArray(string hex)
    {
        return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
    }
    /// <summary>
    ///     Ensures that the BigInteger value will be a positive number. BigInteger is Big-endian
    ///     (the most significant byte is in the [0] position)
    /// </summary>
    public static byte[] EnsurePositiveNumber(this byte[] ba)
    {
        return ba.Concat(new byte[] { 0 }).ToArray();
    }
    /// <summary>
    ///     Converts from a BigInteger to a binary string.
    /// </summary>
    public static string ToBinaryString(this BigInteger bigint)
    {
        var bytes = bigint.ToByteArray();
        var index = bytes.Length - 1;
        var base2 = new StringBuilder(bytes.Length * 8);
        var binary = Convert.ToString(bytes[index], 2);
        if (binary[0] != '0' && bigint.Sign == 1) base2.Append('0');
        base2.Append(binary);
        for (index--; index >= 0; index--)
            base2.Append(Convert.ToString(bytes[index], 2).PadLeft(8, '0'));
        return base2.ToString();
    }
    /// <summary>
    ///     Converts from a BigInteger to a hexadecimal string.
    /// </summary>
    public static string ToHexString(this BigInteger bi)
    {
        var bytes = bi.ToByteArray();
        var sb = new StringBuilder();
        foreach (var b in bytes)
        {
            var hex = b.ToString("X2");
            sb.Append(hex);
        }

        return sb.ToString();
    }
    /// <summary>
    ///     Converts from a BigInteger to a octal string.
    /// </summary>
    public static string ToOctalString(this BigInteger bigint)
    {
        var bytes = bigint.ToByteArray();
        var index = bytes.Length - 1;
        var base8 = new StringBuilder((bytes.Length / 3 + 1) * 8);
        var rem = bytes.Length % 3;
        if (rem == 0) rem = 3;
        var base0 = 0;
        while (rem != 0)
        {
            base0 <<= 8;
            base0 += bytes[index--];
            rem--;
        }

        var octal = Convert.ToString(base0, 8);
        if (octal[0] != '0' && bigint.Sign == 1) base8.Append('0');
        base8.Append(octal);
        while (index >= 0)
        {
            base0 = (bytes[index] << 16) + (bytes[index - 1] << 8) + bytes[index - 2];
            base8.Append(Convert.ToString(base0, 8).PadLeft(8, '0'));
            index -= 3;
        }

        return base8.ToString();
    }
}