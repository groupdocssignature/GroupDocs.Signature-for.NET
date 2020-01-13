﻿using System;
using System.IO;
using System.Collections.Generic;

namespace GroupDocs.Signature.Examples.CSharp.AdvancedUsage
{
    using GroupDocs.Signature;
    using GroupDocs.Signature.Domain;
    using GroupDocs.Signature.Options;

    public class DeleteMultipleAdvanced
    {
        /// <summary>
        /// Delete multiple signatures in the document over known Signature Id property
        /// </summary>
        public static void Run()
        {
            // The path to the documents directory.
            string filePath = Constants.SAMPLE_SPREADSHEET_SIGNED;
            // copy source file since Delete method works with same Document
            string fileName = Path.GetFileName(filePath);
            string outputFilePath = Path.Combine(Constants.OutputPath, "DeleteMultipleAdvanced", fileName);
            Constants.CheckDir(outputFilePath);
            File.Copy(filePath, outputFilePath, true);
            // initialize Signature instance
            using (Signature signature = new Signature(outputFilePath))
            {
                // define few search options
                BarcodeSearchOptions barcodeOptions = new BarcodeSearchOptions();
                QrCodeSearchOptions qrCodeOptions = new QrCodeSearchOptions();
                // add options to list
                List<SearchOptions> listOptions = new List<SearchOptions>();
                listOptions.Add(barcodeOptions);
                listOptions.Add(qrCodeOptions);

                // search for signatures in document
                SearchResult result = signature.Search(listOptions);
                if (result.Signatures.Count > 0)
                {
                    Console.WriteLine("Trying to delete signatures...");
                    List<BaseSignature> signaturesToDelete = new List<BaseSignature>();
                    // collect image signatures to delete
                    foreach (BaseSignature temp in result.Signatures)
                    {
                        if (temp.SignatureType == SignatureType.Image)
                        {
                            signaturesToDelete.Add(temp);
                        }
                    }
                    // delete signatures
                    DeleteResult deleteResult = signature.Delete(signaturesToDelete);
                    if (deleteResult.Succeeded.Count == signaturesToDelete.Count)
                    {
                        Console.WriteLine("All signatures were successfully deleted!");
                    }
                    else
                    {
                        Console.WriteLine($"Successfully deleted signatures : {deleteResult.Succeeded.Count}");
                        Console.WriteLine($"Not deleted signatures : {deleteResult.Failed.Count}");
                    }
                    Console.WriteLine("List of deleted signatures:");
                    foreach (BaseSignature temp in deleteResult.Succeeded)
                    {
                        Console.WriteLine($"Signature# Id:{temp.SignatureId}, Location: {temp.Left}x{temp.Top}. Size: {temp.Width}x{temp.Height}");
                    }
                }
                else
                {
                    Console.WriteLine("No one signature was found.");
                }
            }
        }
    }
}