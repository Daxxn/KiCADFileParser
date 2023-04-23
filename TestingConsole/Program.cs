using KiCADParserLibrary;
using KiCADParserLibrary.Symbols.Models;

namespace TestingConsole;

internal class Program
{
   static void Main(string[] args)
   {
      Console.WriteLine("KiCAD Parsing Library Testing");

      string symbolsFolder = "F:\\Electrical\\KiCad\\Libraries\\Symbols";
      string testSymbolsFolder = "F:\\Electrical\\KiCad\\Libraries\\TestingSymbols";
      string fileName = "Daxxn_Buttons.kicad_sym";
      string testFileName = "Daxxn_Buttons_Test.kicad_sym";
      string readPath = Path.Combine(symbolsFolder, fileName);
      string writePath = Path.Combine(testSymbolsFolder, testFileName);

      // Parse the file
      SymbolParser parser = new();
      var library = parser.ParseFile(readPath);

      // Change some properties and stuff
      library.Symbols[1].Name += " Test";
      var pn = library.Symbols[0].GetProperty("PartNumber");
      if (pn != null)
      {
         pn.Coords.Y = 1;
         pn.Value = "9999-9999";
      }

      // Save the file
      SymbolWriter writer = new();
      writer.Write(writePath, library);
   }
}