using Microsoft.Templates.Core.Packaging;
using System;
using System.IO;

namespace TemplatesSourceTool
{
    internal class PackageWorker
    {
        internal static void Create(string sourcePath, string certThumbprint, StreamWriter output, StreamWriter error)
        {
            try
            {
                if (!Directory.Exists(sourcePath))
                {
                    output.WriteLine($"## Creating template package from folder {sourcePath}.");
                    if (!string.IsNullOrWhiteSpace(certThumbprint))
                    {
                        output.WriteLine($"The template package will be signed using the cert matching {certThumbprint} as thumbprint...");
                        TemplatePackage.PackAndSign(sourcePath, certThumbprint);
                    }
                    else
                    {
                        output.WriteLine($"The template package will not be signed. No cert thumbprint provided...");
                        TemplatePackage.Pack(sourcePath);
                    }
                    output.WriteLine("Done!");
                }
                else
                {
                    error.WriteLine($"{sourcePath} is not a valid folder to create a Templates Package.");
                }
            }
            catch(Exception ex)
            {
                error.WriteException(ex, "Unexpected exception creating templates package.");
            }
        }

        internal static void Extract(string sourceFile, StreamWriter output, StreamWriter error)
        {
            try
            {
                var outpath = Path.Combine(Path.GetDirectoryName(sourceFile), Path.GetFileNameWithoutExtension(sourceFile));
                output.WriteLine($"## Extracting {sourceFile} to {output}...");
                TemplatePackage.Extract(sourceFile, outpath, true);
                output.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                error.WriteException(ex, "Unexpected exception extracting templates package.");
            }
        }

        internal static void GetInfo(string targetPath, StreamWriter output, StreamWriter error)
        {
            try
            {
                output.WriteLine($"## Templates package {targetPath} information:");
                if (File.Exists(targetPath))
                {
                    output.WriteLine($"Is signed: {TemplatePackage.IsSigned(targetPath).ToString()}");
                    var certsInfo = TemplatePackage.GetCertsInfo(targetPath);
                    output.WriteLine($"Found {certsInfo.Count} certificates in the package.");
                    foreach (var info in certsInfo)
                    {
                        output.WriteLine($"> Cert Subject: {info.cert.Subject}");
                        output.WriteLine($"> Cert Issuer: {info.cert.IssuerName}");
                        output.WriteLine($"> Cert PubKey: {info.cert.GetPublicKeyString()}");
                        output.WriteLine($"> Cert Pin: {info.pin}");
                        output.WriteLine($"> Cert Status Flag: {info.status.ToString()}");
                        output.WriteLine();
                    }
                }
            }
            catch(Exception ex)
            {
                error.WriteException(ex, $"Unexpected error getting template package information.");
            }
        }
    }
}
