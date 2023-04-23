using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.PCBs.Models;
public class LayerCollection : IDictionary<int, Layer>
{
   #region Local Props
   private Dictionary<int, Layer> _layers = new();
   public ICollection<int> Keys => _layers.Keys;
   public ICollection<Layer> Values => _layers.Values;
   public int Count => _layers.Count;
   public bool IsReadOnly => false;
   public double BoardThickness { get; set; }
   public int CopperLayerCount { get; set; }

   public Layer this[int key] { get => _layers[key]; set => _layers[key] = value; }
   #endregion

   #region Constructors
   public LayerCollection() { }
   #endregion

   #region Methods
   public Layer GetLayer(string name)
   {
      if (_layers.Count > 0)
      {
         foreach (var layer in Values)
         {
            if (layer.Name == name) return layer;
         }
      }
      return Layer.None;
   }
   public void Add(int key, Layer value) => _layers.Add(key, value);
   public void Add(KeyValuePair<int, Layer> item) => _layers.Add(item.Key, item.Value);
   public bool ContainsKey(int key) => _layers.ContainsKey(key);
   public bool Contains(KeyValuePair<int, Layer> item) => _layers.Contains(item);
   public bool TryGetValue(int key, [MaybeNullWhen(false)] out Layer value) => _layers.TryGetValue(key, out value);
   public bool Remove(int key) => _layers.Remove(key);
   public bool Remove(KeyValuePair<int, Layer> item) => _layers.Remove(item.Key);
   public void Clear() => _layers.Clear();
   public void CopyTo(KeyValuePair<int, Layer>[] array, int arrayIndex) => throw new NotImplementedException();
   public IEnumerator<KeyValuePair<int, Layer>> GetEnumerator() => _layers.GetEnumerator();
   IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
   #endregion

   #region Full Props

   #endregion
}
