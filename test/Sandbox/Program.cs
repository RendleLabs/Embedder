using System;
using System.Text;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = TestFiles.StringFile_txt.Utf8ToString();
            Console.WriteLine(p);

        }
    }
}
