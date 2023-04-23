using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.PCBs.Models;
public class TraceSegment
{
   #region Local Props
   public Point Start { get; set; } = new();
   public Point End { get; set; } = new();
   public double Width { get; set; } = 0;
   public Layer Layer { get; set; } = Layer.None;
   #endregion

   #region Constructors
   public TraceSegment() { }
   #endregion

   #region Methods

   #endregion

   #region Full Props

   #endregion
}
