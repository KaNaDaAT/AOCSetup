using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Lib {
	public static class Utils {

		public static Dictionary<TKey, TValue> CloneDictionary<TKey, TValue>(Dictionary<TKey, TValue> original)
				where TValue : ICloneable {
			Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count,
																	original.Comparer);
			foreach(KeyValuePair<TKey, TValue> entry in original) {
				ret.Add(entry.Key, (TValue) entry.Value.Clone());
			}
			return ret;
		}

		public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey fromKey, TKey toKey) {
			TValue value = dic[fromKey];
			dic.Remove(fromKey);
			dic[toKey] = value;
		}

		public static void AddToDictionary<T>(this Dictionary<T, long> dict, T key, long value = 1) {
			if(dict.ContainsKey(key)) {
				dict[key] += value;
			} else {
				dict[key] = value;
			}
		}

		/// <see cref="https://stackoverflow.com/questions/4269737/function-convert-hex-string-to-bitarray-c-sharp"/>
		public static BitArray ConvertHexToBitArray(string hexData) {
			if(hexData == null)
				return null;
			BitArray ba = new BitArray(4 * hexData.Length);
			for(int i = 0; i < hexData.Length; i++) {
				byte b = byte.Parse(hexData[i].ToString(), NumberStyles.HexNumber);
				for(int j = 0; j < 4; j++) {
					ba.Set(i * 4 + j, (b & (1 << (3 - j))) != 0);
				}
			}
			return ba;
		}

	}
}
