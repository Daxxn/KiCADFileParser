using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KiCADParserLibrary.PCBs.Enums;

namespace KiCADParserLibrary.PCBs.Models;
public class Pad
{
   #region Local Props
   public string Name { get; set; } = string.Empty;
   public string Pin { get; set; } = string.Empty;
   public PadTech PadTech { get; set; }
   public PadType Type { get; set; }
   public Net Net { get; set; } = Net.EmptyNet;
   public Point Position { get; set; }
   public Size Size { get; set; }
   public List<string> Layers { get; set; } = new();
   public double DrillDiameter { get; set; }
   #endregion

   #region Constructors
   public Pad() { }
   #endregion

   #region Methods

   #endregion

   #region Full Props

   #endregion
}
