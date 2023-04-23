using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MVVMLibrary;

namespace KiCADParserLibrary.Symbols.VModels;

/// <summary>
/// Representation of a KiCAD schematic symbol library
/// </summary>
public class Library : Model
{
	private string _name = null!;
	private string _version = null!;
   private string _generator = null!;
   private ObservableCollection<Symbol> _symbols = new();

   public override string ToString() => $"Symbol Library {Name} - Ver: {Version} Gen: {Generator}";

	public string Name
	{
		get => _name;
		set
		{
			_name = value;
			OnPropertyChanged();
		}
	}

	public string Version
	{
		get => _version;
		set
		{
			_version = value;
			OnPropertyChanged();
		}
	}

	public string Generator
	{
		get => _generator;
		set
		{
			_generator = value;
			OnPropertyChanged();
		}
	}

	public ObservableCollection<Symbol> Symbols
	{
		get => _symbols;
		set
		{
			_symbols = value;
			OnPropertyChanged();
		}
	}
}
