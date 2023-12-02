using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib {
	public class NullableDictionary<TKey, TValue> : Dictionary<TKey, TValue> {
		public NullableDictionary() : base() { }
		public NullableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
		: base(collection.ToDictionary(pair => pair.Key, pair => pair.Value))
		{
		}
		public NullableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }

		public new TValue this[TKey key]
		{
			get {
				return ContainsKey(key) ? base[key] : default;
			}
			set {
				base[key] = value;
			}
		}
	}

	public static class NullableDictionaryExtensions {

		public static NullableDictionary<TKey, TSource> ToNullableDictionary<TSource, TKey>(
			this IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector
		) => ToNullableDictionary(source, keySelector, null);


		public static NullableDictionary<TKey, TSource> ToNullableDictionary<TSource, TKey>(
			this IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector,
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
			IEqualityComparer<TKey>? comparer
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
		) =>
			new NullableDictionary<TKey, TSource>(source.ToDictionary(keySelector, comparer));


		public static NullableDictionary<TKey, TElement> ToNullableDictionary<TSource, TKey, TElement>(
			this IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector,
			Func<TSource, TElement> elementSelector
		) => ToNullableDictionary(source, keySelector, elementSelector, null);


		public static NullableDictionary<TKey, TElement> ToNullableDictionary<TSource, TKey, TElement>(
			this IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector,
			Func<TSource, TElement> elementSelector,
			IEqualityComparer<TKey> comparer
		) =>
			new NullableDictionary<TKey, TElement>(source.ToDictionary(keySelector, elementSelector, comparer));

	}
}
