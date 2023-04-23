using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.PCBs.Models.Shapes;
public class Polygon
{
   #region Local Props
   public List<Point> Points { get; set; } = new();
   public double Thickness { get; set; }
   public bool Fill { get; set; }
   public Layer Layer { get; set; } = Layer.None;
   #endregion

   #region Constructors
   public Polygon() { }
   #endregion

   #region Methods

   #endregion

   #region Full Props

   #endregion
}
