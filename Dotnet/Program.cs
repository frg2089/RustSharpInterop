using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

struct Response
{
    public nint Data;
    public nuint Size;
}

internal unsafe partial class Program
{
    [LibraryImport("native", EntryPoint = "test")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static partial void Test(
        delegate* unmanaged[Cdecl]<
            byte*,
            nuint,
            Response
        > op);

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static Response Callback(byte* chars, nuint size)
    {
        Console.WriteLine(Encoding.UTF8.GetString(chars, (int)size));

        Console.WriteLine("Invoke from Rust");
        var data = Marshal.AllocHGlobal(3);
        var p = (byte*)data;

        p[0] = 1;
        p[1] = 2;
        p[2] = 3;

        return new()
        {
            Data = data,
            Size = 3
        };
    }

    private static void Main()
    {
        Console.WriteLine("Hello, World!");
        Test(&Callback);
    }
}
