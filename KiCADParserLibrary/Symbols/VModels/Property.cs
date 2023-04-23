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
   #endregion

   #region Constructors
   public Property() { }
   #endregion

   #region Methods
   public static Property Create(Node node)
   {
      Property newProp = new();

      if (node.GetNode("id") is Node idNode)
      {
         if (int.TryParse(idNode.Value.Trim(), out int id))
         {
            newProp.Id = id;
         }
      }
      if (node.GetNode("at") is Node coordsNode)
      {
         newProp.Coords = Coordinates.ParseString(coordsNode.Value);
      }

      if (node.Properties is null)
      {
         throw new Exception("No properties found.");
      }
      newProp.Name = node.Properties[0].Key ?? string.Empty;
      newProp.Value = node.Properties[0].Value ?? string.Empty;

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
   #endregion
}
