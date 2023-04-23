using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.PCBs.Models;
public class TraceCollection : IDictionary<Net, Trace>
{
   #region Local Props
   private Dictionary<Net, Trace> _traces = new();
   public ICollection<Net> Keys => _traces.Keys;
   public ICollection<Trace> Values => _traces.Values;
   public int Count => _traces.Count;
   public bool IsReadOnly => false;

   public Trace this[Net key] { get => _traces[key]; set => _traces[key] = value; }
   public Net this[int index] => Keys.FirstOrDefault(n => n.Id == index) ?? new();
   #endregion

   #region Constructors
   public TraceCollection() { }
   #endregion

   #region Methods
   public Trace? GetTrace(string netName)
   {
      var foundNet = _traces.Keys.FirstOrDefault(n => n.Name == netName);
      if (foundNet != null)
      {
         return _traces[foundNet];
      }
      return null;
   }
   public void Add(Net key, Trace value) => _traces.Add(key, value);
   public void Add(KeyValuePair<Net, Trace> item) => _traces.Add(item.Key, item.Value);
   public void Clear() => _traces.Clear();
   public bool Contains(KeyValuePair<Net, Trace> item) => _traces.Contains(item);
   public bool ContainsKey(Net key) => _traces.ContainsKey(key);
   public void CopyTo(KeyValuePair<Net, Trace>[] array, int arrayIndex) => throw new NotImplementedException();
   public IEnumerator<KeyValuePair<Net, Trace>> GetEnumerator() => _traces.GetEnumerator();
   public bool Remove(Net key) => _traces.Remove(key);
   public bool Remove(KeyValuePair<Net, Trace> item) => _traces.Remove(item.Key);
   public bool TryGetValue(Net key, [MaybeNullWhen(false)] out Trace value) => _traces.TryGetValue(key, out value);
   IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
   #endregion

   #region Full Props

   #endregion
}
