using System;

// Methods used to parse individual values from the input and read them into a specific datatype

public class APIItemData
{
	private byte[][] geometryField;
	private byte[][] textureField;
	string nameField;

	public byte[][] geometry
	{
		get { return geometryField; }
		set { geometryField = value; }
	}

	public byte[][] texture
	{
		get { return textureField; }
		set { textureField = value; }
	}
	
	public string name
	{
		get { return nameField; }
		set { nameField = value; }
	}
}

class TGXMUtils
{
	public static double unormalize(byte value, int bits) 
	{
		double max = Math.Pow(2, bits) - 1;
		return value/max;
	}
	
	public static double unormalize(short value, int bits) 
	{
		double max = Math.Pow(2, bits) - 1;
		return value/max;
	}
	
	public static double unormalize(int value, int bits) 
	{
		double max = Math.Pow(2, bits) - 1;
		return value/max;
	}

	public static double normalize(int value, int bits) 
	{
		double max = Math.Pow(2, bits-1) - 1;
		return Math.Max(value/max, -1);
	}

	public static byte Sbyte(byte[] data, int offset) 
	{
		return (byte)decodeSigned(data, offset, 1);
	}

	public static byte Ubyte(byte[] data, int offset) 
	{
		return (byte)decodeUnsigned(data, offset, 1);
	}

	public static short Sshort(byte[] data, int offset) 
	{
		return (short)(int)decodeSigned(data, offset, 2);
	}

	public static short Ushort(byte[] data, int offset) 
	{
		return (short)decodeUnsigned(data, offset, 2);
	}

	public static int Sint(byte[] data, int offset) 
	{
		return (int)decodeSigned(data, offset, 4);
	}

	public static int Uint(byte[] data, int offset) 
	{
		return (int)decodeUnsigned(data, offset, 4);
	}

	public static float Sfloat(byte[] data, int offset) 
	{
		return decodeFloat(Bytes(data, offset, 4), 1, 8, 23, -126, 127);
	}

	public static byte[] Bytes(byte[] data, int offset, int length) 
	{
		byte[] bytes = new byte[length];
		for (int i=0; i<length; i++) {
			bytes[i] = Ubyte(data, offset+i);
		}
		return bytes;
	}

	public static string String(byte[] data, int offset, int length) 
	{
		string str = "";
		if (offset == 0) offset = 0;
		if (length == 0) length = data.Length-offset;
		for (int i=0; i<length; i++) 
		{
			byte chr = data[offset+i];
			if (chr == 0) continue;
			str += (char) chr;
		}
		//var str = data.substr(offset, length);
		//if (str.indexOf("\0") != -1) str = str.substr(0, str.indexOf("\0"));
		return str;
	}

	public static string bits(int value, int length) 
	{
		string str = "";
		for (var i=0; i<length; i++) 
		{
			str = ((value >> i) & 0x1)+str;
		}
		return str;
	}

	public static double radianToDegrees(double value) 
	{
		return value * 180 / Math.PI;
	}

	public static double degreesToRadian(double value) 
	{
		return value * Math.PI / 180;
	}

	//public static String padNum(byte[] num, int length) 
	//{
	//    num = num.toString();
	//    while(num.length < length) 
	//    {
	//        num = "0"+num;
	//    }
	//    return num;
	//}

	//public static String decodeHex(byte[] data, int offset, int length) 
	//{
	//    var hex = "";

	//    if (data instanceof Number) 
	//    {
	//        length = offset != undefined ? offset : 4;
	//        for (var i=0; i<length; i++) 
	//        {
	//            var u8 = (data >> (i*8)) & 0xFF;
	//            hex = padNum(u8.toString(16), 2).toUpperCase() + hex;
	//        }
	//        return "0x"+hex;
	//    }

	//    if (offset == undefined) offset = 0;
	//    if (length == undefined) length = data.length;
	//    for (var i=0; i<length; i++) 
	//    {
	//        hex = padNum(data.charCodeAt(offset+i).toString(16).toUpperCase(), 2) + hex;
	//    }
	//    return "0x"+hex;
	//}

	public static uint decodeUnsigned(byte[] data, int offset, int length) 
	{
		uint integer = 0;
		for (byte i=0; i<length; i++) 
		{
			integer |= (uint)data[offset+i] << (i*8);
		}
		return integer;
	}
	
	//public static ushort decodeUnsigned(short[] data, int offset, int length) 
	//{
	//    ushort integer = 0;
	//    for (int i=0; i<length; i++) 
	//    {
	//        integer |= (data[offset+i] << (i*8));
	//    }
	//    return integer;
	//}

	//public static uint decodeUnsigned(int[] data, int offset, int length) 
	//{
	//    uint integer = 0;
	//    for (uint i=0; i<length; i++) 
	//    {
	//        integer |= data[offset+i] << (i*8);
	//    }
	//    return integer;
	//}

	public static Object decodeSigned(byte[] bytes, int offset, int length) 
	{
		int data = (int)decodeUnsigned(bytes, offset, length);
		int bits = length * 8;
		int max = (1 << bits) - 1;
		if ((data & (1 << (bits - 1))) != 0) 
		{
			data = (data & max) - max;
		}
		return data;
	}

	public static int decodeSigned(byte bytes, int offset, int length) 
	{
		length = offset;
		int data = bytes;
		int bits = length * 8;
		int max = (1 << bits) - 1;
		if ((data & (1 << (bits - 1))) != 0) 
		{
			data = (data & max) - max;
		}
		return data;
	}

	public static float decodeFloat(byte[] bytes, int signBits, int exponentBits, int fractionBits, int eMin, int eMax/*, boolean littleEndian*/) 
	{
		//if (littleEndian == undefined) littleEndian = true;
		bool littleEndian = true;
		int totalBits = (signBits + exponentBits + fractionBits);

		string binary = "";
		for (int i = 0, l = bytes.Length; i < l; i++) 
		{
			int singleByte = bytes[i];
			string bits = "" + Convert.ToString(bytes[i],2).PadLeft(8,'0');
			while (bits.Length < 8)
				bits = "0" + bits;

			if (littleEndian)
				binary = bits + binary;
			else
				binary += bits;
		}

		int sign = (binary[0] == '1')?-1:1;
		var exponent = Convert.ToInt32(binary.Substring(signBits, exponentBits), 2) - eMax;
		var significandBase = binary.Substring(signBits + exponentBits, fractionBits);
		String significandBin = "1"+significandBase;
		//int i = 0;
		int val = 1;
		int significand = 0;

		if (exponent == -eMax) 
		{
			if (significandBase.IndexOf('1') == -1)
				return 0;
			else 
			{
				exponent = eMin;
				significandBin = '0'+significandBase;
			}
		}

		for (int i=0;i<significandBin.Length;i++) //while (i < significandBin.Length)
		{
			significand += val * Convert.ToInt32(significandBin[i]);
			val = val / 2;
			//i++;
		}

		return (float) (sign * significand * Math.Pow(2, exponent));
	}    
}