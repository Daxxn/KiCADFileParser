using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KiCADParserLibrary.Tree;

namespace KiCADParserLibrary.Symbols.Models;

/// <summary>
/// Grid coordinates of a parent model.
/// </summary>
public class Coordinates
{
   public double X { get; set; } = 0;
   public double Y { get; set; } = 0;
   public double Z { get; set; } = 0;
   public Node TreeNode { get; set; } = null!;

   public Coordinates(double x, double y, double z, Node node)
   {
      X = x;
      Y = y;
      Z = z;
      TreeNode = node;
   }

   public static Coordinates ParseString(string coords, Node node)
   {
      var coordsSplit = coords.Split(' ');
      if (coordsSplit.Length == 3)
      {
         _ = double.TryParse(coordsSplit[0], out double x);
         _ = double.TryParse(coordsSplit[1], out double y);
         _ = double.TryParse(coordsSplit[2], out double z);
         return new Coordinates(x, y, z, node);
      }
      throw new Exception("Unable to parse coordinates.");
   }

   public void SyncNode() => TreeNode.Value = $"{X} {Y} {Z}";
}
