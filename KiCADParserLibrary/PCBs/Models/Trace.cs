using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KiCADParserLibrary.PCBs.Models.Shapes;

namespace KiCADParserLibrary.PCBs.Models;
public class Trace
{
   #region Local Props
   public double Width { get; set; }
   public Layer Layer { get; set; } = Layer.None;
   public Net Net { get; set; } = Net.EmptyNet;
   public List<TraceSegment> Segments { get; set; } = new();
   #endregion

   #region Constructors
   public Trace() { }
   #endregion

   #region Methods

   #endregion

   #region Full Props

   #endregion
}
