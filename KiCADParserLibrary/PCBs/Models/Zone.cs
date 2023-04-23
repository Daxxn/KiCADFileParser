using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.PCBs.Models;
public class Zone
{
   #region Local Props
   public Net Net { get; set; } = Net.EmptyNet;
   public Layer Layer { get; set; } = Layer.None;
   public List<Point> PolygonPoints { get; set; } = new();
   public List<Point> FilledPolygonPoints { get; set; } = new();
   #endregion

   #region Constructors
   public Zone() { }
   #endregion

   #region Methods

   #endregion

   #region Full Props

   #endregion
}
