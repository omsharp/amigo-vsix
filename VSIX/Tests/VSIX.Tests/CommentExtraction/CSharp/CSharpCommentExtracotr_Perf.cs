using Configurations.Core.Comments;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSIX.Package.Comments.Parsers;
using Xunit;
using Xunit.Abstractions;

namespace VSIX.Tests.CommentExtraction.CSharp
{
   public class CSharpCommentExtracotr_Perf
   {

      ITestOutputHelper output;

      public CSharpCommentExtracotr_Perf(ITestOutputHelper output)
      {
         this.output = output;
      }

      [Fact(DisplayName = "Performance Test")]
      public void Test()
      {
         var configMock = new Mock<ICommentConfiguration>();

         configMock.Setup(c => c.Classifications).Returns(
            new ObservableCollection<Classification>
            {
               new Classification { Name = "Todo", Token = "todo"},
               new Classification { Name = "Hack", Token = "hack"},
               new Classification { Name = "Bug", Token = "bug"},

            });

         var extractor = new CSharpCommentExtractor(configMock.Object);
         var lines = File.ReadLines("TestData.txt").ToArray();

         //! No 1
         var w = Stopwatch.StartNew();
         extractor.Extract(lines);
         w.Stop();
         output.WriteLine($"Duration: {w.ElapsedMilliseconds} ms");

         //! No 2
         w.Start();
         extractor.Extract(lines);
         w.Stop();
         output.WriteLine($"Duration: {w.ElapsedMilliseconds} ms");

         //! No 3
         w.Start();
         extractor.Extract(lines);
         w.Stop();
         output.WriteLine($"Duration: {w.ElapsedMilliseconds} ms");

         //! No 4
         w.Start();
         extractor.Extract(lines);
         w.Stop();
         output.WriteLine($"Duration: {w.ElapsedMilliseconds} ms");

         //! No 5
         w.Start();
         extractor.Extract(lines);
         w.Stop();
         output.WriteLine($"Duration: {w.ElapsedMilliseconds} ms");

         Assert.True(true);
      }

   }
}
