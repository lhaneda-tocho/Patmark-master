using System;
using System.Collections.Generic;
using System.IO;

namespace TemplateKit
{
    /// <summary>
    /// テンプレート処理用の初期化を行うクラス.
    /// プロジェクトの設定でカレントディレクトリをこのプロジェクトに設定すること.
    /// </summary>
    public static class InitClass
    {
        public static string        ProjectDir { get; private set; }
        public static string        TargetDir  { get; private set; }
        public static IList<string> CopyList { get; } = new List<string>{
            "Assembly.tt",
        };

        public static void Main(string[] args)
        {
            if(args.Length < 2) {
                Console.WriteLine("[Error] Missing Arguments");
                Console.WriteLine("Usage: <executable> ProjectDir TargetDir");
                return;
            } else {
                ProjectDir = args[0];
                TargetDir  = args[1];
            }
            Console.Write("ProjectDir: ");
            Console.WriteLine(ProjectDir);
            Console.Write("TargetDir : ");
            Console.WriteLine(TargetDir);
            Setup();
            Copy();
            Generate();
        }

        public static void Setup()
        {
            var category = TemplateCategory.Design;
            category.MakeRootDirectory();
            category.MakeIncludeDirectory();
        }

        public static void Copy()
        {
            var to   = TemplateCategory.Design.ResolveIncludeDirectory( TargetDir);
            var from = TemplateCategory.User  .ResolveIncludeDirectory(ProjectDir);
            foreach(var file in CopyList)
            {
                File.Copy(
                    Path.Combine(from, file),
                    Path.Combine(to  , file),
                    true
                );
            }
        }

        public static void Generate() 
        {
            var category = TemplateCategory.Design;

            using (var file = category.NewIncludeFile("ProjectDir.tt")) {
                file.WriteLine(string.Format("<#+ static readonly string ProjectDir = \"{0}\"; #>", TargetDir));
            }
        }
    }

    public interface ITemplateCategory
    {
        string RootDirectory    { get; }
    }

    public static class TemplateCategory
    {
        public static string ProjectDir => InitClass.ProjectDir;
        public static string TargetDir  => InitClass.TargetDir;
        public static ITemplateCategory User   = new UserImpl();
        public static ITemplateCategory Design = new DesignImpl();


        class UserImpl: ITemplateCategory
        {
            public string RootDirectory { get; } = "Template";
        }

        class DesignImpl: ITemplateCategory
        {
            public string RootDirectory { get; } = "DesignTemplate";
        }

        public static void MakeRootDirectory(this ITemplateCategory category)
        {
            string root = Path.Combine(TargetDir, category.RootDirectory);
            Directory.CreateDirectory(root);
        }

        public static void MakeIncludeDirectory(this ITemplateCategory category)
        {
            string dir = Path.Combine(TargetDir, category.RootDirectory, "Include");
            Directory.CreateDirectory(dir);
        }

        public static StreamWriter NewIncludeFile(this ITemplateCategory category, string name) {
            string dir = Path.Combine(TargetDir, category.RootDirectory, "Include", name);
            return File.CreateText(dir);
        }

        public static string ResolveIncludeDirectory(this ITemplateCategory category, string projectDir)
        {
            return Path.Combine(projectDir, category.RootDirectory, "Include");
        }
    }
}
