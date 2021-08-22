using Schlechtums.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Schlechtums.Core.Common
{
	/// <summary>
	/// Extended Dictionary class which returns information about missing or duplicate keys.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	public class DictionaryEx<TKey, TValue> : Dictionary<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>
	{
		#region <<< Constructors >>>
		public DictionaryEx()
		{ }

		public DictionaryEx(int capacity)
			: base(capacity)
		{ }

		public DictionaryEx(IEqualityComparer<TKey> comparer)
			: base(comparer)
		{ }

		public DictionaryEx(IDictionary<TKey, TValue> dictionary)
			: base(dictionary)
		{ }

		public DictionaryEx(int capacity, IEqualityComparer<TKey> comparer)
			: base(capacity, comparer)
		{ }

		public DictionaryEx(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }

		public DictionaryEx(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
			: base(dictionary, comparer)
		{ }
		#endregion

		public new TValue this[TKey key]
		{
			get
			{
				try
				{
					return base[key];
				}
				catch (KeyNotFoundException)
				{
					var e = new KeyNotFoundException($"The given key '{key}' was not present in the dictionary.");
					e.HelpLink = key.ToString();
					throw e;
				}
			}
			set
			{
				base[key] = value;
			}
		}

		public new void Add(TKey key, TValue value)
		{
			try
			{
				base.Add(key, value);
			}
			catch (ArgumentException ae)
			{
				if (ae.GetHResult() == -2147024809)
				{
					throw new ArgumentException($"An item with the key '{key}' has already been added");
				}
			}
		}
	}
}