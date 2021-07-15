using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace VSIX.Package
{
   class Logger
   {

      static Logger()
      {
         AllocConsole();
      }

      [DllImport("kernel32.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      static extern bool AllocConsole();

      public static void Clear()
      {
#if DEBUG
         Console.Clear();
#endif

      }

      public static void Log(string str)
      {
#if DEBUG
         Console.WriteLine(str);
#endif
      }

      public static void Log(object obj)
      {
#if DEBUG
         Console.WriteLine(obj);
#endif
      }
   }
}
