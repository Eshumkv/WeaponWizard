using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using WeaponWizard.Elements.Animation;
using Newtonsoft.Json;
using WeaponWizard.Interfaces;
using WeaponWizard.Elements.Enums;

namespace WeaponWizard
{
	public class AnimationDataStore : ISystem
	{
		private string _path { get; set; }

		private GameEngine _engine { get; set; }

		private Dictionary<string, List<FrameInfo>> _store;

		public List<FrameInfo> this [string key] {
			get {
				return _store [key];
			}
		}

		public AnimationDataStore (GameEngine engine, string path)
		{
			_store = new Dictionary<string, List<FrameInfo>> ();
			_path = path;
			_engine = engine;
		}

		public void LoadAll ()
		{
			var files = new List<string> ();

			// Go through all the files in the main dir
			foreach (var file in Directory.GetFiles(_path)) {
				files.Add (file);
			}

			// Go through all the sub directories
			foreach (var dir in Directory.GetDirectories(_path)) {
				foreach (var file in Directory.GetFiles(dir)) {
					files.Add (file);
				}
			}

			// Load all the files
			foreach (var file in files) {
				using (var sr = new StreamReader (file)) {
					var animationfile = JsonConvert.DeserializeObject<AnimationJsonFile> (sr.ReadToEnd ());
					var key = animationfile.Id + ".";

					foreach (var animation in animationfile.Animations) {
						_store.Add (key + animation.Key, animationfile.Load (_engine, animation.Key));
					}
				}
			}

			// done!
		}

		public Dictionary<string, List<FrameInfo>> Get (string entity, params string[] keys)
		{
			var dict = new Dictionary<string, List<FrameInfo>> ();
			
			foreach (var key in keys) {
				dict.Add (key, _store [entity + "." + key]);
			}

			return dict;
		}

		public void HandleMsg (Message msg, object data)
		{
			switch (msg) {
			default:
				break;
			}
		}
	}
}

