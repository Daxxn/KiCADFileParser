using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KiCADParserLibrary.Tree;
using MVVMLibrary;

namespace KiCADParserLibrary.Symbols.VModels;

/// <summary>
/// <see cref="Symbol"/> property model
/// </summary>
public class Property : Model
{
   #region Local Props
   private int _id;
   private string _name = null!;
   private string _value = null!;
   private Coordinates _coords = null!;
   private Node _node = null!;
   #endregion

   #region Constructors
   public Property() { }
   #endregion

   #region Methods
   public static Property Create(Node node)
   {
      Property newProp = new()
      {
         Node = node,
      };

      if (node.Props?.Count > 0)
      {
         newProp.Name = node.Props[0];
         if (node.Props?.Count > 1)
         {
            newProp.Value = node.Props[1];
         }
      }

      if (node.Search("at") is Node coordsNode)
      {
         newProp.Coords = Coordinates.ParseString(coordsNode.Props.ToArray());
      }

      return newProp;
   }
   #endregion

   #region Full Props
   public int Id
   {
      get => _id;
      set
      {
         _id = value;
         OnPropertyChanged();
      }
   }

   public string Name
   {
      get => _name;
      set
      {
         _name = value;
         OnPropertyChanged();
      }
   }

   public string Value
   {
      get => _value;
      set
      {
         _value = value;
         OnPropertyChanged();
      }
   }

   public Coordinates Coords
   {
      get => _coords;
      set
      {
         _coords = value;
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
