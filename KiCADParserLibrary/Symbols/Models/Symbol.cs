using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KiCADParserLibrary.Tree;

namespace KiCADParserLibrary.Symbols.Models
{
    /// <summary>
    /// Representation of a KiCAD schematic symbol.
    /// </summary>
    public class Symbol
   {
      #region Local Props
      public string Name { get; set; } = null!;
      public Node TreeNode { get; set; } = null!;
      public Node OnBoardNode { get; set; } = null!;
      public Node InBOMNode { get; set; } = null!;
      public bool OnBoard { get; set; }
      public bool InBOM { get; set; }
      public List<Property> Properties { get; set; } = new();
      #endregion

      #region Constructors
      public Symbol() { }
      #endregion

      #region Methods
      public static Symbol Create(Node node)
      {
         Symbol newSymbol = new()
         {
            Name = node.Value,
            TreeNode = node,
         };
         if (node.Children is null)
         {
            return newSymbol;
         }
         foreach (var child in node.Children)
         {
            if (child.Type == "property")
            {
               if (child.Properties != null)
               {
                  newSymbol.Properties.Add(Property.Create(child));
               }
            }
            else if (child.Type == "in_bom")
            {
               newSymbol.InBOM = child.Value == "yes";
               newSymbol.InBOMNode = child;
            }
            else if (child.Type == "on_board")
            {
               newSymbol.OnBoard = child.Value == "yes";
               newSymbol.OnBoardNode = child;
            }
         }
         return newSymbol;
      }

      public Property? GetProperty(string name) => Properties.FirstOrDefault(p => p.Name == name);

      public void SyncNode()
      {
         TreeNode.Value = Name;
         InBOMNode.Value = InBOM ? "yes" : "no";
         OnBoardNode.Value = OnBoard ? "yes" : "no";
         foreach (var prop in Properties)
         {
            prop.SyncNode();
         }
      }

      public override string ToString() => $"Symbol - {Name} Props: {Properties.Count} {(OnBoard ? " On Board" : " ")} {(InBOM ? " In BOM" : "")}";
      #endregion

      #region Full Props

      #endregion
   }
}
