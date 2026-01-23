using PairUp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairUp
{
	public class DataCache<T> 
	{
		private Dictionary<string, T> dataCache = new Dictionary<string, T>();

		public void AddData(string key, T data)
		{
			if (dataCache.ContainsKey(key))
			{
				throw new Exception($"{typeof(T)} with key '{key}' already exists.");
			}
			dataCache[key] = data;
		}

		public void AddData(string key, Func<T> data)
		{
			if (dataCache.ContainsKey(key))
			{
				throw new Exception($"{typeof(T)} with key '{key}' already exists.");
			}
			dataCache[key] = data.Invoke(); ;
		}

		public void RemoveData(string key)
		{
			if (!dataCache.ContainsKey(key))
			{
				throw new Exception($"{typeof(T)} with key '{key}' not found.");
			}

			T dataToRemove = GetData(key);
			if (dataToRemove is IDisposable disposable)
			{
				disposable.Dispose();
			}
			dataCache.Remove(key);	
		}

		public T GetData(string key)
		{
			if (dataCache.TryGetValue(key, out T data))
			{
				return data;
			}

			throw new Exception($"{typeof(T)} with key '{key}' not found.");
		}

		public void SetData(string key, T data)
		{
			if (dataCache.ContainsKey(key))
			{
				dataCache[key] = data;
			}
		}


	}
}
