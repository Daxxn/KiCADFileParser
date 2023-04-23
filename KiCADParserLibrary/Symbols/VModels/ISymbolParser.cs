using KiCADParserLibrary.Tree;

namespace KiCADParserLibrary.Symbols.VModels;
public interface ISymbolParser
{
   TreeBuilder Builder { get; set; }
   TreeBuilderOptions Options { get; }

   Library ParseFile(string path);
   IEnumerable<Library> ParseAllFiles(string dir, bool recursiveSearch = false);
}