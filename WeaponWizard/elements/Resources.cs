using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace WeaponWizard.Elements
{
	public class Resources<T>
	{
		private Dictionary<string, string> _assetNames;
		private Dictionary<string, T> _resources;

		private bool _isLoaded;

		public Resources (IDictionary<string, string> namesAndAssets)
		{
			_assetNames = new Dictionary<string, string> (namesAndAssets);
			_resources = new Dictionary<string, T> ();
			_isLoaded = false;
		}

		public T this [string name] {
			get {
				if (!_isLoaded)
					throw new Exception ("Content has not been loaded yet. (Have you called Load() ?)");

				return _resources [name];
			}
		}

		public void Load (ContentManager c)
		{
			if (_isLoaded)
				return;
			
			foreach (var kv in _assetNames) {
				_resources.Add (kv.Key, c.Load<T> (kv.Value));
			}
			_isLoaded = true;
		}

		public void AddAsset (string name, string path)
		{
			_assetNames.Add (name, path);
		}

		public T AddResource (string name, T resource)
		{
			_resources.Add (name, resource);
			return resource;
		}

		public bool RemoveResource (string name)
		{
			return _resources.Remove (name);
		}
	}
}

