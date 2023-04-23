using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KiCADParserLibrary.Tree;

namespace KiCADParserLibrary.Symbols.Models;

/// <summary>
/// Representation of a KiCAD schematic symbol library
/// </summary>
public class Library
{
   #region Local Props
   public string Name { get; set; } = null!;
   public string Version { get; set; } = null!;
   public string Generator { get; set; } = null!;

   public Node RawTree { get; set; } = null!;

   public List<Symbol> Symbols { get; set; } = new();
   #endregion

   #region Constructors
   public Library() { }
   #endregion

   #region Methods
   public override string ToString() => $"Symbol Library - Ver: {Version} Gen: {Generator}";

   public void SyncTree()
   {
      if (RawTree is null)
         return;
      if (RawTree.Children is null)
         return;
      foreach (var sym in Symbols)
      {
         sym.SyncNode();
      }
   }
   #endregion

   #region Full Props

   #endregion

}
