using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
   private ObservableCollection<Property> _props = new();
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
         }
         else if (child.Type == "on_board")
         {
            newSymbol.OnBoard = child.Value == "yes";
         }
      }
      return newSymbol;
   }

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
   #endregion
}
