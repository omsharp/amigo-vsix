using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace VSIX.Package.Utils
{
   class DteUtils
   {
      public static string GetSolutionName(DTE2 app)
      {
         ThreadHelper.ThrowIfNotOnUIThread();

         if (app == null || app.Solution == null || string.IsNullOrEmpty(app.Solution.FullName)) return "";
         return Path.GetFileNameWithoutExtension(app.Solution.FullName);
      }

      public static string[] FindSolutionDirectories(DTE2 app)
      {
         ThreadHelper.ThrowIfNotOnUIThread();

         var basePaths = new List<string>();

         if (app.Solution != null)
         {
            for (var i = 1; i <= app.Solution.Projects.Count; i++)
            {
               var projectItem = app.Solution.Projects.Item(i);
               AddPathFromProjectItem(basePaths, projectItem);
            }

            return basePaths.ToArray();
         }

         app.StatusBar.Text = "No solution or project is identified. app.Solution is " +
             app.Solution?.GetType().Name ?? "NULL";

         //  App.DTE = (DTE2)Package.GetGlobalService(typeof(SDTE));

         return null;
      }


      public static List<Project> GetProjects(DTE2 dteServiceProvider)
      {
         ThreadHelper.ThrowIfNotOnUIThread();

         var projects = dteServiceProvider.Solution.Projects;
         var result = new List<Project>();
         var item = projects.GetEnumerator();

         while (item.MoveNext())
         {
            var project = item.Current as Project;
            if (project == null) continue;

            if (project.Kind == ProjectKinds.vsProjectKindSolutionFolder)
               result.AddRange(GetSolutionFolderProjects(project));

            else result.Add(project);
         }

         return result;
      }

      static void AddPathFromProjectItem(List<string> basePaths, Project projectItem)
      {
         ThreadHelper.ThrowIfNotOnUIThread();

         if (projectItem == null) return;

         try
         {
            // Project
            var projectFileName = projectItem.FileName;

            if (!string.IsNullOrWhiteSpace(projectFileName))
            {
               if (projectItem.Properties.Item("FullPath").Value is string fullPath)
                  basePaths.Add(fullPath);
            }
            else
            {
               // Folder
               for (var i = 1; i <= projectItem.ProjectItems.Count; i++)
                  AddPathFromProjectItem(basePaths, projectItem.ProjectItems.Item(i).Object as Project);
            }
         }
         //An unloaded project
         catch (NotImplementedException ex)
         {
            Debug.WriteLine(ex);
         }
         catch (Exception err)
         {
            //todo handle error
         }
      }

      static IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder)
      {
         try
         {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (solutionFolder.ProjectItems == null) return null;

            var result = new List<Project>();

            for (var i = 1; i <= solutionFolder.ProjectItems.Count; i++)
            {
               var subProject = solutionFolder.ProjectItems.Item(i).SubProject;
               if (subProject == null) continue;

               // another solution folder
               if (subProject.Kind == ProjectKinds.vsProjectKindSolutionFolder)
               {
                  var solutionFolderProjects = GetSolutionFolderProjects(subProject);

                  if (solutionFolder != null)
                     result.AddRange(GetSolutionFolderProjects(subProject));
               }

               else result.Add(subProject);
            }

            return result;
         }
         catch (Exception e)
         {
            //todo Handle error
            return null;
         }
      }
   }
}
