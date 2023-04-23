using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KiCADParserLibrary.PCBs.Enums;

namespace KiCADParserLibrary.PCBs.Models.Shapes;
public struct Line : IShape
{
   #region Local Props
   public Point Start { get; set; } = new();
   public Point End { get; set; } = new();
   public double Stroke { get; set; } = 0;
   public GraphicsType Type { get; set; } = GraphicsType.Default;
   public Layer Layer { get; set; } = Layer.None;
   #endregion

   #region Constructors
   public Line() { }
   #endregion

   #region Methods

   #endregion

   #region Full Props

   #endregion
}
