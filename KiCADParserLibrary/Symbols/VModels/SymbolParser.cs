﻿using KiCADParserLibrary.Tree;

namespace KiCADParserLibrary.Symbols.VModels;

/// <summary>
/// Parses a KiCAD symbol library file.
/// </summary>
public class SymbolParser : ISymbolParser
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
   public IEnumerable<Library> ParseAllFiles(string dir, bool recursiveSearch = false)
   {
      EnumerationOptions opt = new()
      {
         IgnoreInaccessible = true,
         RecurseSubdirectories = recursiveSearch,
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
      try
      {
         var rootNode = Builder.ParseFile(path);
         var genNode = rootNode.GetNode("generator");
         Library lib = new()
         {
            Name = Path.GetFileNameWithoutExtension(path),
            RootNode = rootNode
         };
         if (genNode != null)
         {
            lib.Generator = genNode.Value;
         }

         var libNode = rootNode.Search("kicad_symbol_lib");
         if (libNode != null)
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
      catch (Exception)
      {
         throw;
      }
   }
   #endregion

   #region Full Props

   #endregion
}
