// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

using Microsoft.Templates.Core.Packaging;

namespace WtsTool
{
    internal class PackageWorker
    {
        internal static void Create(string inputPath, string outfile, string certThumbprint, TextWriter output, TextWriter error)
        {
            try
            {
                if (Directory.Exists(inputPath))
                {
                    output.WriteCommandHeader($"Creating template package from folder {inputPath}.");
                    if (!string.IsNullOrWhiteSpace(certThumbprint))
                    {
                        output.WriteCommandText($"The template package will be signed using the cert matching {certThumbprint} as thumbprint.");
                        TemplatePackage.PackAndSign(inputPath, outfile, certThumbprint, "text/plain");
                        output.WriteCommandText($"Templates package file '{outfile}' successfully created.");
                    }
                    else
                    {
                        output.WriteCommandText($"No cert thumbprint provided, the template package will not be signed.");
                        TemplatePackage.Pack(inputPath, outfile, "text/plain");
                    }

                    output.WriteCommandText($"Templates package file '{outfile}' successfully created.");
                }
                else
                {
                    error.WriteCommandText($"{inputPath} is not a valid folder to create a Templates Package.");
                }
            }
            catch (Exception ex)
            {
                error.WriteException(ex, "Unexpected exception creating templates package.");
            }
        }

        internal static void Extract(string inputFile, string outPath, TextWriter output, TextWriter error)
        {
            try
            {
                if (File.Exists(inputFile))
                {
                    if (outPath == ".")
                    {
                        outPath = System.Environment.CurrentDirectory;
                    }

                    output.WriteCommandHeader($"Extracting {inputFile} to {outPath}...");
                    TemplatePackage.Extract(inputFile, outPath, true);
                }
                else
                {
                    error.WriteCommandText($"{inputFile} is not a valid folder to create a Templates Package.");
                }
            }
            catch (Exception ex)
            {
                error.WriteException(ex, "Unexpected exception extracting templates package.");
            }
        }

        internal static void GetInfo(string inputPath, TextWriter output, TextWriter error)
        {
            try
            {
                output.WriteCommandHeader($"Templates package {inputPath} information:");
                if (File.Exists(inputPath))
                {
                    output.WriteCommandText($"Is signed: {TemplatePackage.IsSigned(inputPath).ToString()}");
                    output.WriteLine();

                    WriteSignatureValidationsInfo(inputPath, output);

                    WriteCertificatesInfo(inputPath, output);
                }
            }
            catch (Exception ex)
            {
                error.WriteException(ex, $"Unexpected error getting template package information.");
            }
        }

        private static void WriteCertificatesInfo(string inputPath, TextWriter output)
        {
            var certsInfo = TemplatePackage.GetCertsInfo(inputPath);
            output.WriteCommandText($"Found {certsInfo.Count} certificates in the package.");
            foreach (var info in certsInfo)
            {
                output.WriteLine();
                output.WriteCommandText($" Cert Subject: {info.cert.Subject}");
                output.WriteLine();
                output.WriteCommandText($" Cert Issuer: {info.cert.IssuerName.Name}");
                output.WriteLine();
                output.WriteCommandText($" Cert Serial Number: {info.cert.SerialNumber}");
                output.WriteLine();
                output.WriteCommandText($" Cert Chain Status: {info.status.ToString()}");
                output.WriteLine();
                output.WriteCommandText($" Cert PubKey:");
                output.WriteCommandText($" {info.cert.GetPublicKeyString()}");
                output.WriteLine();
                output.WriteCommandText($" Cert Pin:");
                output.WriteCommandText($" {info.pin}");
                output.WriteLine();
                output.WriteCommandText("--");
                output.WriteLine();
            }
        }

        private static void WriteSignatureValidationsInfo(string inputPath, TextWriter output)
        {
            var allowedPins = Microsoft.Templates.Core.Configuration.Current.AllowedPublicKeysPins;
            output.WriteCommandText($"WtsTool certificate pins: {allowedPins.Count}");
            foreach (var pin in allowedPins)
            {
                output.WriteCommandText(pin);
            }

            output.WriteLine();
            output.WriteCommandText($"WTS is valid signature: {TemplatePackage.ValidateSignatures(inputPath)}");
            var validationType = "Cert Chain";
            if (allowedPins.Count > 0)
            {
                validationType = "Cert Pins";
            }

            output.WriteCommandText($"Signature validation type: {validationType}");
            output.WriteLine();
        }
    }
}
