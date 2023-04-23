using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.PCBs.Models;
public struct Point
{
   #region Local Props
   public double X { get; set; }
   public double Y { get; set; }
   #endregion

   #region Constructors
   public Point()
   {
      X = 0;
      Y = 0;
   }
   public Point(double x, double y)
   {
      X = x;
      Y = y;
   }
   #endregion

   #region Methods
   public static Point Parse(IEnumerable<string> data)
   {
      Point newPoint = new();
      if (data.Count() == 2)
      {
         var d = data.ToArray();
         if (double.TryParse(d[0], out double x))
         {
            newPoint.X = x;
         }
         if (double.TryParse(d[1], out double y))
         {
            newPoint.Y = y;
         }
      }
      return newPoint;
   }
   #endregion

   #region Full Props

   #endregion
}
