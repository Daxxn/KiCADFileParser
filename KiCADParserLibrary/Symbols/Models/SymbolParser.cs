using KiCADParserLibrary.Tree;

namespace KiCADParserLibrary.Symbols.Models;

/// <summary>
/// Parses a KiCAD symbol library file.
/// </summary>
public class SymbolParser
{
   #region Local Props
   public TreeBuilder Builder { get; set; }
   public TreeBuilderOptions Options => Builder.Options;
   #endregion

   #region Constructors
   public SymbolParser() => Builder = new();
   public SymbolParser(TreeBuilderOptions options) => Builder = new(options);
   #endregion

   #region Methods
   public IEnumerable<Library> ParseAllFiles(string dir)
   {
      EnumerationOptions opt = new()
      {
         IgnoreInaccessible = true,
         RecurseSubdirectories = true,
         MaxRecursionDepth = 3,
         MatchCasing = MatchCasing.CaseInsensitive,
      };
      var paths = Directory.GetFiles(dir, "*.kicad_sym", opt);
      List<Library> results = new();
      List<Exception> errors = new();
      foreach (var path in paths)
      {
         try
         {
            results.Add(ParseFile(path));
         }
         catch (Exception e)
         {
            errors.Add(e);
         }
      }
      return errors.Count > 0 ? throw new AggregateException("Parsing errors occured.", errors) : results;
   }

   public Library ParseFile(string path)
   {
      Library lib = new();
      lib.Name = Path.GetFileNameWithoutExtension(path);
      var rootNode = Builder.ParseFile(path);
      lib.RawTree = rootNode;
      var versionNode = rootNode.GetNode("version");
      var genNode = rootNode.GetNode("generator");
      if (versionNode != null)
      {
         lib.Version = versionNode.Value;
      }
      if (genNode != null)
      {
         lib.Generator = genNode.Value;
      }

      if (rootNode.Data.StartsWith("kicad_symbol_lib"))
      {
         if (rootNode.Children is null)
         {
            throw new Exception("Library is either empty or parsing failed.");
         }
         foreach (var node in rootNode.Children)
         {
            if (node.Type == "symbol")
            {
               lib.Symbols.Add(Symbol.Create(node));
            }
         }
      }

      return lib;
   }
   #endregion

   #region Full Props

   #endregion
}
