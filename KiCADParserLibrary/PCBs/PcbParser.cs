using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using KiCADParserLibrary.PCBs.Enums;
using KiCADParserLibrary.PCBs.Models;
using KiCADParserLibrary.PCBs.Models.Shapes;
using KiCADParserLibrary.Tree;

namespace KiCADParserLibrary.PCBs;
public class PcbParser
{
   #region Local Props
   private TreeBuilder treeBuilder = new();
   public TreeBuilderOptions Options { get; set; } = new();
   #endregion

   #region Constructors
   public PcbParser() { }
   public PcbParser(TreeBuilderOptions options)
   {
      Options = options;
      treeBuilder.Options = options;
   }
   #endregion

   #region Methods
   public PCB Parse(string pcbPath)
   {
      PCB pcb = new PCB();
      var rootNode = treeBuilder.ParseFile(pcbPath);

      var stackupNode = rootNode.Search("setup>stackup");
      var layerNode = rootNode.Search("layers");
      if (stackupNode != null && layerNode != null)
      {
         ParseStackup(stackupNode, pcb);
         ParseLayers(layerNode, pcb);
      }
      ParseFootprints(rootNode, pcb);
      ParseTraces(rootNode, pcb);
      ParseVias(rootNode, pcb);
      ParseGraphics(rootNode, pcb);
      return pcb;
   }

   private void ParseStackup(Node stackupNode, PCB pcb)
   {
      if (stackupNode.Children is null)
         throw new Exception("No layers found. Something might be wrong with the save file.");
      Stackup stackup = new();
      foreach (var item in stackupNode.Children)
      {
         if (item.Type != "layer")
         {
            continue;
         }
         if (item.Props?.Count > 0)
         {
            StackLayer layer = new()
            {
               Name = item.Props[0],
            };
            var lType = item.Search("type");
            if (lType != null)
            {
               if (lType.Props != null)
               {
                  layer.Type = lType.Props[0];
                  if (layer.Type == "core" || layer.Type == "prepreg")
                  {
                     var lMaterial = item.Search("material");
                     if (lMaterial != null && lMaterial?.Props?.Count > 0)
                     {
                        layer.Material = lMaterial.Props[0];
                     }
                     var lDk = item.Search("epsilon_r");
                     if (lDk != null && lDk?.Props?.Count > 0)
                     {
                        if (double.TryParse(lDk.Props[0], out double dk))
                        {
                           layer.DielectricConstant = dk;
                        }
                     }
                     var lLoss = item.Search("loss_tangent");
                     if (lLoss != null && lLoss?.Props?.Count > 0)
                     {
                        if (double.TryParse(lLoss.Props[0], out double ls))
                        {
                           layer.LossTangent = ls;
                        }
                     }
                  }
               }
               var lThickness = item.Search("thickness");
               if (lThickness != null && lThickness?.Props?.Count > 0)
               {
                  if (double.TryParse(lThickness?.Props[0], out double thickness))
                  {
                     layer.Thickness = thickness;
                  }
               }
            }
            stackup.Add(layer.Name, layer);
         }
      }
      pcb.Stackup = stackup;
   }

   private void ParseLayers(Node layersNode, PCB pcb)
   {
      if (layersNode.Children is null)
         throw new Exception("No layers node found. Something might be wrong with the save file.");
      LayerCollection layers = new();

      foreach (var l in layersNode.Children)
      {
         if (l.Props?.Count > 0)
         {
            if (int.TryParse(l.Type, out int layerIndex))
            {
               layers.Add(layerIndex, new()
               {
                  Index = layerIndex,
                  Name = l.Props[1],
                  Type = l.Props[0] == "signal" ? Enums.LayerType.Signal : Enums.LayerType.User,
               });
            }
         }
      }
      pcb.Layers = layers;
   }

   private List<Net> ParseNets(Node rootNode)
   {
      List<Net> nets = new();
      var netNodes = rootNode.GetNodes("net");
      foreach (var netNode in netNodes)
      {
         if (netNode.Type == "net")
         {
            if (int.TryParse(netNode.Props[0], out int id))
            {
               if (netNode.Props.Count == 1)
               {
                  nets.Add(new() { Id = id, Name = "" });
               }
               else
               {
                  nets.Add(new() { Id = id, Name = netNode.Props[1] });
               }
            }
         }
      }
      return nets;
   }

   private void ParseFootprints(Node rootNode, PCB pcb)
   {
      List<Footprint> footprints = new();
      var footprintNodes = rootNode.GetNodes("footprint");
      if (footprintNodes is null)
      {
         return;
      }
      foreach (var fpNode in footprintNodes)
      {
         if (fpNode.Props != null)
         {
            Footprint newFp = new()
            {
               LibraryRef = fpNode.Props[0],
            };
            var props = fpNode.GetNodes("property");
            if (props != null)
            {
               foreach (var prop in props)
               {
                  if (prop.Props?.Count > 1)
                  {
                     if (prop.Props[0] == "PartNumber")
                     {
                        newFp.PartNumber = prop.Props[1];
                     }
                  }
               }
            }

            var descNode = fpNode.Search("descr");
            if (descNode != null)
            {
               if (descNode.Props != null)
               {
                  newFp.Description = descNode.Props[0];
               }
            }
            var textNodes = fpNode.GetNodes("fp_text");
            if (textNodes != null)
            {
               foreach (var node in textNodes)
               {
                  if (node.Props[0] == "reference")
                  {
                     newFp.Reference = node.Props[1];
                  }
                  else if (node.Props[0] == "value")
                  {
                     newFp.Value = node.Props[1];
                  }
               }
            }
            var padNodes = fpNode.GetNodes("pad");
            if (padNodes.Count > 0)
            {
               foreach (var node in padNodes)
               {
                  newFp.Pads.Add(ParsePad(node));
               }
            }
         }
      }
      pcb.Footprints = footprints;
   }

   private void ParseTraces(Node rootNode, PCB pcb)
   {
      var nets = ParseNets(rootNode);
      TraceCollection traces = new();
      foreach (var net in nets)
      {
         traces.Add(net, new Trace());
      }

      var traceNodes = rootNode.GetNodes("segment");

      foreach (var segmentNode in traceNodes)
      {
         Net? currentNet = null;
         var netNode = segmentNode.Search("net");
         if (netNode != null)
         {
            if (netNode.Props != null)
            {
               if (int.TryParse(netNode.Props[0], out int id))
               {
                  currentNet = traces[id];
               }
            }
         }
         if (currentNet != null)
         {
            TraceSegment line = new();
            var startNode = segmentNode.Search("start");
            if (startNode != null && startNode.Props != null)
            {
               line.Start = Point.Parse(startNode.Props);
            }
            var endNode = segmentNode.Search("end");
            if (endNode != null && endNode.Props != null)
            {
               line.End = Point.Parse(endNode.Props);
            }
            var widthNode = segmentNode.Search("width");
            if (widthNode != null && widthNode.Props != null)
            {
               if (double.TryParse(widthNode.Props[0], out double width))
               {
                  line.Width = width;
               }
            }
            var layerNode = segmentNode.Search("layer");
            if (layerNode != null && layerNode.Props != null)
            {
               var layer = pcb.Layers.GetLayer(layerNode.Props[0]);
               if (layer != null)
               {
                  line.Layer = layer;
               }
            }
            traces[currentNet].Segments.Add(line);
         }
      }
      pcb.Traces = traces;
   }

   private void ParseVias(Node rootNode, PCB pcb)
   {
      List<Via> vias = new();
      var viaNodes = rootNode.GetNodes("via");
      foreach (var viaNode in viaNodes)
      {
         Via via = new();
         var atNode = viaNode.Search("at");
         if (atNode != null && atNode.Props != null)
         {
            via.Position = Point.Parse(atNode.Props);
         }
         var sizeNode = viaNode.Search("size");
         if (sizeNode != null && sizeNode.Props != null)
         {
            if (double.TryParse(sizeNode.Props[0], out double size))
            {
               via.Size = size;
            }
         }
         var drillNode = viaNode.Search("drill");
         if (drillNode != null && drillNode.Props != null)
         {
            if (double.TryParse(drillNode.Props[0], out double drill))
            {
               via.Drill = drill;
            }
         }
         var layersNode = viaNode.Search("layers");
         if (layersNode != null && layersNode.Props != null)
         {
            foreach (var prop in layersNode.Props)
            {
               var foundLayer = pcb.Layers.GetLayer(prop);
               if (foundLayer != null)
               {
                  via.Layers.Add(foundLayer);
               }
            }
         }
      }
      pcb.Vias = vias;
   }

   private void ParseGraphics(Node rootNode, PCB pcb)
   {
      List<IShape> graphics = new List<IShape>();
      var graphicsNodes = rootNode.GetNodes("gr_");
      foreach (var grNode in graphicsNodes)
      {
         Point start = new();
         Point end = new();
         double stroke = 0;
         Layer layer = Layer.None;
         var startNode = grNode.Search("start");
         if (startNode != null && startNode.Props != null)
         {
            start = Point.Parse(startNode.Props);
         }
         var endNode = grNode.Search("end");
         if (endNode != null && endNode.Props != null)
         {
            end = Point.Parse(endNode.Props);
         }
         var layerNode = grNode.Search("layer");
         if (layerNode != null && layerNode.Props != null)
         {
            layer = pcb.Layers.GetLayer(layerNode.Props[0]);
         }
         var strokeNode = grNode.Search("stroke");
         if (strokeNode != null)
         {
            var widthNode = strokeNode.Search("width");
            if (widthNode != null && widthNode.Props != null)
            {
               if (double.TryParse(widthNode.Props[0], out double width))
               {
                  stroke = width;
               }
            }
            var typeNode = strokeNode.Search("type");
            if (typeNode != null && typeNode.Props != null)
            {

            }
         }
         if (grNode.Type == "gr_line")
         {
            Line newLine = new()
            {
               Start = start,
               End = end,
               Stroke = stroke,
               Layer= layer,
            };
            graphics.Add(newLine);
         }
         else if (grNode.Type == "gr_ark")
         {
            Ark newArk = new()
            {
               Start = start,
               End = end,
               Stroke = stroke,
               Layer= layer,
            };
            var midNode = grNode.Search("mid");
            if (midNode != null && midNode.Props != null)
            {
               newArk.Middle = Point.Parse(midNode.Props);
            }
            graphics.Add(newArk);
         }
         else if (grNode.Type == "gr_circle")
         {
            Circle circle = new()
            {
               Start = start,
               End = end,
               Stroke = stroke,
               Layer = layer,
            };
            var centerNode = grNode.Search("center");
            if (centerNode!= null && centerNode.Props != null)
            {
               circle.Start= Point.Parse(centerNode.Props);
            }
            graphics.Add(circle);
         }
      }
      pcb.Graphics = graphics;
   }

   private Pad ParsePad(Node node)
   {
      if (node.Props != null)
      {
         Pad newPad = new()
         {
            Pin = node.Props[2],
            Type = ParsePadType(node.Props[1]),
            PadTech = ParsePadTech(node.Props[0])
         };
         var atNode = node.Search("at");
         if (atNode != null && atNode?.Props != null)
         {
            newPad.Position = Point.Parse(atNode.Props);
         }
         var sizeNode = node.Search("size");
         if (sizeNode != null && sizeNode?.Props != null)
         {
            newPad.Size = Size.Parse(sizeNode.Props);
         }
         var drillNode = node.Search("drill");
         if (drillNode != null && drillNode?.Props != null)
         {
            if (double.TryParse(drillNode.Props[0], out double drill))
            {
               newPad.DrillDiameter = drill;
            }
         }
         return newPad;
      }
      return new();
   }

   private PadType ParsePadType(string input)
   {
      if (Enum.TryParse(input, true, out PadType padType))
      {
         return padType;
      }
      return PadType.Circle;
   }

   private PadTech ParsePadTech(string input)
   {
      if (input == "smd")
      {
         return PadTech.SMD;
      }
      else if (input == "thru-hole")
      {
         return PadTech.PTH;
      }
      return PadTech.MCH;
   }
   #endregion

   #region Full Props

   #endregion
}
