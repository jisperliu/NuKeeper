﻿using System.Linq;
using System.Text;
using NuKeeper.RepositoryInspection;

namespace NuKeeper.Engine
{
    public static class CommitReport
    { 
        public static string MakeCommitMessage(PackageUpdateSet updates)
        {
            return $"Automatic update of {updates.PackageId} to {updates.NewVersion}";
        }

        public static string MakeCommitDetails(PackageUpdateSet updates)
        {
            var oldVersions = updates.CurrentPackages
                .Select(u => CodeQuote(u.Version.ToString()))
                .Distinct()
                .ToList();

            var oldVersionsString = string.Join(",", oldVersions);
            var newVersion = CodeQuote(updates.NewVersion.ToString());
            var packageId = CodeQuote(updates.PackageId);

            var builder = new StringBuilder();


            if (oldVersions.Count == 1)
            {
                builder.AppendLine($"NuKeeper has generated an update of {packageId} to {newVersion} from {oldVersionsString}");
            }
            else
            {
                builder.AppendLine($"NuKeeper has generated an update of {packageId} to {newVersion}");
                builder.AppendLine($"{oldVersions.Count} versions of {packageId} were found in use: {oldVersionsString}");
            }

            if (updates.CurrentPackages.Count == 1)
            {
                builder.AppendLine("1 project update:");
            }
            else
            {
                builder.AppendLine($"{updates.CurrentPackages.Count} project updates:");
            }

            foreach (var current in updates.CurrentPackages)
            {
                var line = $"Updated {CodeQuote(current.Path.RelativePath)} to {packageId} {CodeQuote(updates.NewVersion.ToString())} from {CodeQuote(current.Version.ToString())}";
                builder.AppendLine(line);
            }

            builder.AppendLine("This is an automated update. Merge only if it passes tests");
            builder.AppendLine("");
            builder.AppendLine("**NuKeeper**: https://github.com/NuKeeperDotNet/NuKeeper");
            return builder.ToString();
        }

        private static string CodeQuote(string value)
        {
            return "`" + value + "`";
        }
    }
}
