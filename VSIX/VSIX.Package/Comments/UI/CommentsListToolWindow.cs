namespace VSIX.Package.Comments.UI
{
   using System;
   using System.Runtime.InteropServices;
   using Microsoft.VisualStudio.Shell;
   using VSIX.Package.Comments;

   /// <summary>
   /// This class implements the tool window exposed by this package and hosts a user control.
   /// </summary>
   /// <remarks>
   /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
   /// usually implemented by the package implementer.
   /// <para>
   /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
   /// implementation of the IVsUIElementPane interface.
   /// </para>
   /// </remarks>
   [Guid("0f5aa8f2-e097-4a0d-a212-2ca5aaeeca1a")]
   public class CommentsListToolWindow : ToolWindowPane
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="CommentsListToolWindow"/> class.
      /// </summary>
      public CommentsListToolWindow() : base(null)
      {
         Caption = "Bookmarks List";

         // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
         // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
         // the object returned by the Content property.
         
         Content = new CommentsListToolWindowControl();
      }
   }
}
