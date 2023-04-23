using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using KiCADParserLibrary.Tree;

using MVVMLibrary;

namespace KiCADParserLibrary.Symbols.VModels;

/// <summary>
/// Representation of a KiCAD schematic symbol.
/// </summary>
public class Symbol : Model
{
   #region Local Props
   private string _name = null!;
   private bool _onBoard;
   private bool _inBOM;
   private Node _node = null!;
   private ObservableCollection<Property> _props = new();
   #endregion

   #region Constructors
   /// <inheritdoc/>
   public Symbol() { }
   #endregion

   #region Methods
   /// <summary>
   /// Creates a symbol from the KiCAD file node.
   /// </summary>
   /// <param name="node">KiCAD tree node</param>
   /// <returns>Newly created symbol</returns>
   public static Symbol Create(Node node)
   {
      Symbol newSymbol = new()
      {
         Node = node,
      };
      if (node.Props?.Any() == true)
      {
         newSymbol.Name = node.Props[0];
      }
      if (node.Children is null)
      {
         return newSymbol;
      }
      var inBomNode = node.Search("in_bom");
      if (inBomNode != null)
      {
         newSymbol.InBOM = inBomNode.Props![0] == "yes";
      }
      var onBoardNode = node.Search("on_board");
      if (onBoardNode != null)
      {
         newSymbol.OnBoard = onBoardNode.Props![0] == "yes";
      }
      var propNodes = node.GetNodes("property");
      foreach (var child in propNodes)
      {
         newSymbol.Properties.Add(Property.Create(child));
      }
      return newSymbol;
   }

   /// <summary>
   /// Add a property to <see langword="this"/> symbol.
   /// </summary>
   /// <param name="name">Property Name (ID)</param>
   /// <param name="value">Property Value</param>
   public void AddProperty(string name, string value)
   {
      Property newProp = new()
      {
         Name = name,
         Value = value,
      };
      if (Node.Children is null) return;
      foreach (var child in Node.Children)
      {
         if (child.Props.Count == 2)
         {
            if (child.Props[0] == "ki_keywords")
            {
               newProp.Node = child.ShallowCopy();
               newProp.Node.Props[0] = name;
               newProp.Node.Props[1] = value;
            }
         }
      }
      Properties.Add(newProp);
   }

   /// <inheritdoc/>
   public override string ToString() => $"Symbol - {Name} Props: {Properties.Count} {(OnBoard ? " On Board" : " ")} {(InBOM ? " In BOM" : "")}";

   public Property? GetProperty(string name) => Properties.FirstOrDefault(p => p.Name == name);
   #endregion

   #region Full Props
   public string Name
   {
      get => _name;
      set
      {
         _name = value;
         OnPropertyChanged();
      }
   }

   public bool OnBoard
   {
      get => _onBoard;
      set
      {
         _onBoard = value;
         OnPropertyChanged();
      }
   }

   public bool InBOM
   {
      get => _inBOM;
      set
      {
         _inBOM = value;
         OnPropertyChanged();
      }
   }

   public ObservableCollection<Property> Properties
   {
      get => _props;
      set
      {
         _props = value;
         OnPropertyChanged();
      }
   }

   public Node Node
   {
      get => _node;
      set
      {
         _node = value;
         OnPropertyChanged();
      }
   }
   #endregion
}
