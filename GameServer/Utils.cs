﻿using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GameServer
{
    internal static class PacketUtils
{
	public const int SEND_BUFFER_LEN = 65535;

	public const int RECV_BUFFER_LEN = 65535;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte ReadByte(ref int ptr, ref byte spanRef, int len)
	{
		if (ptr + 1 > len)
		{
			throw new Exception("Receive buffer attempted to read out of bounds");
		}
		return Unsafe.ReadUnaligned<byte>(ref Unsafe.Add(ref spanRef, ptr++));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool ReadBool(ref int ptr, ref byte spanRef, int len)
	{
		if (ptr + 1 > len)
		{
			throw new Exception("Receive buffer attempted to read out of bounds");
		}
		return Unsafe.ReadUnaligned<bool>(ref Unsafe.Add(ref spanRef, ptr++));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static char ReadChar(ref int ptr, ref byte spanRef, int len)
	{
		if (ptr + 1 > len)
		{
			throw new Exception("Receive buffer attempted to read out of bounds");
		}
		return Unsafe.ReadUnaligned<char>(ref Unsafe.Add(ref spanRef, ptr++));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static short ReadShort(ref int ptr, ref byte spanRef, int len)
	{
		if (ptr + 2 > len)
		{
			throw new Exception("Receive buffer attempted to read out of bounds");
		}
		var i = Unsafe.ReadUnaligned<short>(ref Unsafe.Add(ref spanRef, ptr));
		ptr += 2;
		return i;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ushort ReadUShort(ref int ptr, ref byte spanRef, int len)
	{
		if (ptr + 2 > len)
		{
			throw new Exception("Receive buffer attempted to read out of bounds");
		}
		var i = Unsafe.ReadUnaligned<ushort>(ref Unsafe.Add(ref spanRef, ptr));
		ptr += 2;
		return i;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ReadInt(ref int ptr, ref byte spanRef, int len)
	{
		if (ptr + 4 > len)
		{
			throw new Exception("Receive buffer attempted to read out of bounds");
		}
		var i = Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref spanRef, ptr));
		ptr += 4;
		return i;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint ReadUInt(ref int ptr, ref byte spanRef, int len)
	{
		if (ptr + 4 > len)
		{
			throw new Exception("Receive buffer attempted to read out of bounds");
		}
		var i = Unsafe.ReadUnaligned<uint>(ref Unsafe.Add(ref spanRef, ptr));
		ptr += 4;
		return i;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float ReadFloat(ref int ptr, ref byte spanRef, int len)
	{
		if (ptr + 4 > len)
		{
			throw new Exception("Receive buffer attempted to read out of bounds");
		}
		var i = Unsafe.ReadUnaligned<float>(ref Unsafe.Add(ref spanRef, ptr));
		ptr += 4;
		return i;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string ReadString(ref int ptr, ref byte spanRef, int len)
	{
		if (ptr + 2 > len)
		{
			throw new Exception("Receive buffer attempted to read out of bounds");
		}
		var i = Unsafe.ReadUnaligned<ushort>(ref Unsafe.Add(ref spanRef, ptr));
		if (ptr + 2 + i > len)
		{
			throw new Exception("Receive buffer attempted to read out of bounds");
		}
		Span<byte> s = stackalloc byte[(int)i];
		Unsafe.CopyBlockUnaligned(ref MemoryMarshal.GetReference(s), ref Unsafe.Add(ref spanRef, ptr + 2), i);
		ptr += i + 2;
		return Encoding.UTF8.GetString(s);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool[] ReadBoolArray(ref int ptr, ref byte spanRef, int len)
	{
		if (ptr + 2 > len)
		{
			throw new Exception("Receive buffer attempted to read out of bounds");
		}
		var arrLen = Unsafe.ReadUnaligned<short>(ref Unsafe.Add(ref spanRef, ptr));
		if (ptr + 2 + arrLen > len)
		{
			throw new Exception("Receive buffer attempted to read out of bounds");
		}
		var ret = new bool[arrLen];
		for (var i = 0; i < arrLen; i++)
		{
			ret[i] = Unsafe.ReadUnaligned<bool>(ref Unsafe.Add(ref spanRef, ptr++));
		}
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TimedPosition[] ReadTimedPosArray(ref int ptr, ref byte spanRef, int len)
	{
		if (ptr + 2 > len)
			throw new Exception("Receive buffer attempted to read out of bounds");
		
		var arrLen = Unsafe.ReadUnaligned<short>(ref Unsafe.Add(ref spanRef, ptr));
		ptr += 2;
		if (ptr + arrLen > len)
			throw new Exception("Receive buffer attempted to read out of bounds");
		
		var ret = new TimedPosition[arrLen];
		for (var i = 0; i < arrLen; i++)
			ret[i] = new TimedPosition {
				Time = ReadInt(ref ptr, ref spanRef, len),
				X = ReadFloat(ref ptr, ref spanRef, len),
				Y = ReadFloat(ref ptr, ref spanRef, len)
			};
		
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteByte(ref int ptr, ref byte spanRef, byte i)
	{
		if (ptr + 1 > 65535)
		{
			throw new Exception("Send buffer attempted to write out of bounds");
		}
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref spanRef, ptr++), i);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteSByte(ref int ptr, ref byte spanRef, sbyte i)
	{
		if (ptr + 1 > 65535)
		{
			throw new Exception("Send buffer attempted to write out of bounds");
		}
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref spanRef, ptr++), i);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteBool(ref int ptr, ref byte spanRef, bool b)
	{
		if (ptr + 1 > 65535)
		{
			throw new Exception("Send buffer attempted to write out of bounds");
		}
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref spanRef, ptr++), b);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteChar(ref int ptr, ref byte spanRef, char i)
	{
		if (ptr + 1 > 65535)
		{
			throw new Exception("Send buffer attempted to write out of bounds");
		}
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref spanRef, ptr++), i);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteShort(ref int ptr, ref byte spanRef, short i)
	{
		if (ptr + 2 > 65535)
		{
			throw new Exception("Send buffer attempted to write out of bounds");
		}
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref spanRef, ptr), i);
		ptr += 2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteUShort(ref int ptr, ref byte spanRef, ushort i)
	{
		if (ptr + 2 > 65535)
		{
			throw new Exception("Send buffer attempted to write out of bounds");
		}
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref spanRef, ptr), i);
		ptr += 2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteInt(ref int ptr, ref byte spanRef, int i)
	{
		if (ptr + 4 > 65535)
		{
			throw new Exception("Send buffer attempted to write out of bounds");
		}
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref spanRef, ptr), i);
		ptr += 4;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteUInt(ref int ptr, ref byte spanRef, uint i)
	{
		if (ptr + 4 > 65535)
		{
			throw new Exception("Send buffer attempted to write out of bounds");
		}
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref spanRef, ptr), i);
		ptr += 4;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteFloat(ref int ptr, ref byte spanRef, float i)
	{
		if (ptr + 4 > 65535)
		{
			throw new Exception("Send buffer attempted to write out of bounds");
		}
		Unsafe.WriteUnaligned(ref Unsafe.Add(ref spanRef, ptr), i);
		ptr += 4;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteString(ref int ptr, ref byte spanRef, string s)
	{
		if (ptr + 2 + (ushort)s.Length > 65535)
		{
			throw new Exception("Send buffer attempted to write out of bounds");
		}
		WriteUShort(ref ptr, ref spanRef, (ushort)s.Length);
		foreach (var b in s)
		{
			WriteChar(ref ptr, ref spanRef, b);
		}
	}
}
    static class EnumerableUtils
    {
        public static T RandomElement<T>(this IEnumerable<T> source,
            Random rng)
        {
            var current = default(T);
            var count = 0;
            foreach (var element in source)
            {
                count++;
                if (rng.Next(count) == 0)
                {
                    current = element;
                }
            }

            if (count == 0)
            {
                throw new InvalidOperationException("Sequence was empty");
            }

            return current;
        }
    }

    static class MathsUtils
    {
        public static double Dist(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        public static double DistSqr(double x1, double y1, double x2, double y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }

        public static double DistSqr(int x1, int y1, int x2, int y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }

        // http://stackoverflow.com/questions/1042902/most-elegant-way-to-generate-prime-numbers/1072205#1072205
        public static List<int> GeneratePrimes(int n)
        {
            var limit = ApproximateNthPrime(n);
            var bits = SieveOfEratosthenes(limit);
            var primes = new List<int>(n);
            for (int i = 0, found = 0; i < limit && found < n; i++)
                if (bits[i])
                {
                    primes.Add(i);
                    found++;
                }

            return primes;
        }

        private static int ApproximateNthPrime(int nn)
        {
            var n = (double)nn;
            double p;
            if (nn >= 7022)
                p = n * Math.Log(n) + n * (Math.Log(Math.Log(n)) - 0.9385);
            else if (nn >= 6)
                p = n * Math.Log(n) + n * Math.Log(Math.Log(n));
            else if (nn > 0)
                p = new int[] { 2, 3, 5, 7, 11 }[nn - 1];
            else
                p = 0;

            return (int)p;
        }

        private static BitArray SieveOfEratosthenes(int limit)
        {
            var bits = new BitArray(limit + 1, true);
            bits[0] = false;
            bits[1] = false;
            for (var i = 0; i * i <= limit; i++)
                if (bits[i])
                    for (var j = i * i; j <= limit; j += i)
                        bits[j] = false;

            return bits;
        }
    }
}