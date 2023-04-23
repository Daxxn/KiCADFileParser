using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.PCBs.Models;
public class Stackup : IDictionary<string, StackLayer>
{
   #region Local Props
   private Dictionary<string, StackLayer> _stackup = new();
   public ICollection<string> Keys => _stackup.Keys;
   public ICollection<StackLayer> Values => _stackup.Values;
   public int Count => _stackup.Count;
   public bool IsReadOnly => false;
   public double BoardThickness { get; set; }
   public int CopperLayerCount { get; set; }

   public StackLayer this[string key] { get => _stackup[key]; set => _stackup[key] = value; }
   #endregion

   #region Constructors
   public Stackup() { }
   #endregion

   #region Methods
   public StackLayer? GetLayer(string name)
   {
      if (_stackup.Count > 0)
      {
         foreach (var layer in Values)
         {
            if (layer.Name == name)
               return layer;
         }
      }
      return null;
   }
   public void Add(string key, StackLayer value) => _stackup.Add(key, value);
   public void Add(KeyValuePair<string, StackLayer> item) => _stackup.Add(item.Key, item.Value);
   public bool ContainsKey(string key) => _stackup.ContainsKey(key);
   public bool Contains(KeyValuePair<string, StackLayer> item) => _stackup.Contains(item);
   public bool TryGetValue(string key, [MaybeNullWhen(false)] out StackLayer value) => _stackup.TryGetValue(key, out value);
   public bool Remove(string key) => _stackup.Remove(key);
   public bool Remove(KeyValuePair<string, StackLayer> item) => _stackup.Remove(item.Key);
   public void Clear() => _stackup.Clear();
   public void CopyTo(KeyValuePair<string, StackLayer>[] array, int arrayIndex) => throw new NotImplementedException();
   public IEnumerator<KeyValuePair<string, StackLayer>> GetEnumerator() => _stackup.GetEnumerator();
   IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
   #endregion

   #region Full Props

   #endregion
}
