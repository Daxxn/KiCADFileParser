using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.PCBs.Models;
public class Net
{
   #region Local Props
   public static readonly Net EmptyNet = new() { Id = -1, Name = "None" };
   public int Id { get; set; } = -1;
   public string Name { get; set; } = "None";
   #endregion

   #region Constructors
   public Net() { }
   #endregion

   #region Methods

   #endregion

   #region Full Props

   #endregion
}
