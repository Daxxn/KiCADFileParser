using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MVVMLibrary;

namespace KiCADParserLibrary.Symbols.VModels;

/// <summary>
/// Grid coordinates of a parent model.
/// </summary>
public class Coordinates : Model
{
   private double _x;
   private double _y;
   private double _z;

   public Coordinates() { }
   public Coordinates(double x, double y, double z)
   {
      X = x;
      Y = y;
      Z = z;
   }

   public static Coordinates ParseString(string coords)
   {
      var coordsSplit = coords.Split(' ');
      if (coordsSplit.Length == 3)
      {
         _ = double.TryParse(coordsSplit[0], out double x);
         _ = double.TryParse(coordsSplit[1], out double y);
         _ = double.TryParse(coordsSplit[2], out double z);
         return new Coordinates(x, y, z);
      }
      throw new Exception("Unable to parse coordinates.");
   }

   public double X
   {
      get => _x;
      set
      {
         _x = value;
         OnPropertyChanged();
      }
   }

   public double Y
   {
      get => _y;
      set
      {
         _y = value;
         OnPropertyChanged();
      }
   }

   public double Z
   {
      get => _z;
      set
      {
         _z = value;
         OnPropertyChanged();
      }
   }
}
