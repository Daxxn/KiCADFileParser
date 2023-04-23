using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.PCBs.Models;
public class Via
{
   #region Local Props
   public Point Position { get; set; }
   public double Size { get; set; }
   public double Drill { get; set; }
   public Net Net { get; set; } = Net.EmptyNet;
   public List<Layer> Layers { get; set; } = new();
   #endregion

   #region Constructors
   public Via() { }
   #endregion

   #region Methods

   #endregion

   #region Full Props

   #endregion
}
