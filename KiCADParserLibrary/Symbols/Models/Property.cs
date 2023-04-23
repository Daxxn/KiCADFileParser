using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KiCADParserLibrary.Tree;

namespace KiCADParserLibrary.Symbols.Models;

/// <summary>
/// <see cref="Symbol"/> property model
/// </summary>
public class Property
{
   #region Local Props
   public int Id { get; set; }
   public Node TreeNode { get; set; } = null!;
   public Node IdNode { get; set; } = null!;
   public string Name { get; set; } = null!;
   public string Value { get; set; } = null!;
   public Coordinates Coords { get; set; } = null!;
   #endregion

   #region Constructors
   public Property() { }
   #endregion

   #region Methods
   public static Property Create(Node node)
   {
      Property newProp = new()
      {
         TreeNode = node,
      };

      if (node.GetNode("id") is Node idNode)
      {
         newProp.IdNode = idNode;
         if (int.TryParse(idNode.Value.Trim(), out int id))
         {
            newProp.Id = id;
         }
      }
      if (node.GetNode("at") is Node coordsNode)
      {
         newProp.Coords = Coordinates.ParseString(coordsNode.Value, coordsNode);
      }

      if (node.Properties is null)
      {
         throw new Exception("No properties found.");
      }
      newProp.Name = node.Properties[0].Key ?? string.Empty;
      newProp.Value = node.Properties[0].Value ?? string.Empty;

      return newProp;
   }

   public void SyncNode()
   {
      if (TreeNode.GetProperty(Name) is NodeProperty prop)
      {
         prop.Value = Value;
      }
      //TreeNode.Value = Value;
      IdNode.Value = $"{Id}";
   }
   #endregion

   #region Full Props

   #endregion
}
