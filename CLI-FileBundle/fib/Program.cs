
//fib bundle --output rootCommand.txt

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography;



string[] getLanguagesArr(string language)
{
    string[] languages = { "cs", "cpp", "py", "js" };
    if (language == "all")
        return languages;
    string[] languagesArr = language.Split('-');
    string languagesStr = "";
    foreach (string item in languagesArr)
    {
        if (languages.Contains(item))
            languagesStr += item + '-';
    }
    return languagesStr.Split('-');

}


var bundleOption = new Option<FileInfo>("--output", "File path and name");
var languageOption = new Option<string>("--language") { IsRequired = true };
var noteOption = new Option<bool>("--note");
var sortOption = new Option<string>("--sort");
var removeOption = new Option<bool>("--remove-empty-lines");
var authorOption = new Option<string>("--author");

var bundleCommand = new Command("bundle", "Bundle code file files to a single file");


bundleCommand.AddOption(bundleOption);
bundleCommand.AddOption(languageOption);
bundleCommand.AddOption(noteOption);
bundleCommand.AddOption(sortOption);
bundleCommand.AddOption(removeOption);
bundleCommand.AddOption(authorOption);

bundleCommand.SetHandler((output, language, note, sort, remove, author) =>
{
    try{ 
    if (sort == null)
        sort = "name";
    string[] languages = getLanguagesArr(language);

    //create a bundle file
    try
    {
            var myFile=File.Create(output.FullName);
            myFile.Close();
            Console.WriteLine("File  was created");
    }
    catch (DirectoryNotFoundException ex)
    {
        Console.WriteLine("Error: File path  is invalid");

    }


    string bundleFilePath = output.FullName;
    if (author != null)
    {
        File.AppendAllText(bundleFilePath, "//author:" + author + "\n");
    }


    string[] directories = Directory.GetDirectories("./");

    foreach (string i in directories)
    {


        string[] files = Directory.GetFiles("./" + i);

        if (sort == "name")
        {
            //sort the files array by name
        }
        if (sort == "kind")
        {
            //sort the files array by kind
        }


        foreach (string file in files)
        {

            string fileType = file.Substring(file.LastIndexOf(".") + 1);
            if (languages.Contains(fileType))
            {
                if (note)
                {
                    string sourceCode = "//sourceCode: " + Directory.GetCurrentDirectory() + "\\" + file.Substring(4);
                    File.AppendAllText(bundleFilePath, sourceCode + "\n");
                
                }

                //add file to the bundel file
                string[] lines = File.ReadAllLines(file);

                if (remove)
                {
                    foreach (string line in lines)
                    {
                        if (line != "")
                            File.AppendAllText(bundleFilePath, line + "\n");
                    }

                }
                else
                {
                        File.AppendAllLines(bundleFilePath, lines);
                }

            }

        };

    }
    }
    catch (Exception ex)
    {
        
        Console.WriteLine(ex);
    }

}, bundleOption, languageOption, noteOption, sortOption, removeOption, authorOption);




//create 
var createRspCommand = new Command("create-rsp", "Bundle code file files to a single file");

createRspCommand.SetHandler(() =>
{

    string command = "fib\nbundle\n";
    //fib bundle --output bunb.txt  --language all --note true --author "tzofia ben zion" --remove-empty-lines true
    Console.WriteLine("enter path for the bandle file");
    string output = Console.ReadLine();
    Console.WriteLine("enter language");
    string language = Console.ReadLine();
    Console.WriteLine("enter path to the author name");
    string author = Console.ReadLine();
    Console.WriteLine("enter if remove empty lines");
    string remove= Console.ReadLine();
    Console.WriteLine("enter if write note");
    string note = Console.ReadLine();
    command += "--output\n" + output + "\n--language\n" + language + "\n--note\n" + note + "\n--author\n" + author + "\n--remove-empty-lines\n" + remove;
    Console.WriteLine(command);
    try
    {
        var myFile = File.Create("responseFile.rsp");
        myFile.Close();
        File.AppendAllText("responseFile.rsp", command);
    }
    catch (DirectoryNotFoundException ex)
    {
        Console.WriteLine("Error: File path  is invalid");

    }
    //
    //responseFile.rsp
});

//fib

var rootCommand = new RootCommand("root command for File Bundler CLI");
rootCommand.AddCommand(bundleCommand);
rootCommand.AddCommand(createRspCommand);
rootCommand.InvokeAsync(args);

