using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KiCADParserLibrary.PCBs.Models.Shapes;

namespace KiCADParserLibrary.PCBs.Models;
public class PCB
{
   #region Local Props
   public List<Footprint> Footprints { get; set; } = new();
   public TraceCollection Traces { get; set; } = new();
   public List<Via> Vias { get; set; } = new();
   public LayerCollection Layers { get; set; } = new();
   public Stackup Stackup { get; set; } = new();
   public List<IShape> Graphics { get; set; } = new();
   #endregion

   #region Constructors
   public PCB() { }
   #endregion

   #region Methods
   public void GetItemByLayer(string layer)
   {

   }
   public void GetByLayer(int layerIndex)
   {

   }

   public List<IShape> GetPCBEdges()
   {
      List<IShape> edges = new();
      foreach (var shape in Graphics)
      {
         if (shape.Layer.Name == "Edge.Cuts")
         {
            edges.Add(shape);
         }
      }
      return edges;
   }
   #endregion

   #region Full Props

   #endregion
}
