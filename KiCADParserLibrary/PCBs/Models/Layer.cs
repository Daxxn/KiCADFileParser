using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KiCADParserLibrary.PCBs.Enums;

namespace KiCADParserLibrary.PCBs.Models;
public class Layer
{
   #region Local Props
   public static readonly Layer None = new();
   public int Index { get; set; } = -1;
   public string Name { get; set; } = string.Empty;
   public LayerType Type { get; set; }
   #endregion

   #region Constructors
   public Layer() { }
   #endregion

   #region Methods
   public override string ToString() => $"{Index} - {Name} - {Type}";
   public override bool Equals(object? obj)
   {
      if (obj is Layer l)
      {
         if (l.Name == Name || l.Index == Index)
         {
            return true;
         }
      }
      return false;
   }

   public override int GetHashCode() => base.GetHashCode();
   #endregion

   #region Full Props

   #endregion
}
