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

		private Dictionary<string, AnimationJsonFile> _tempStore;

		public string Path {
			get {
				return _path;
			}
		}

		public List<FrameInfo> this [string key] {
			get {
				return _store [key];
			}
		}

		public AnimationDataStore (GameEngine engine, string path)
		{
			_store = new Dictionary<string, List<FrameInfo>> ();
			_tempStore = new Dictionary<string, AnimationJsonFile> ();
			_path = path;
			_engine = engine;
		}

		public void Load ()
		{
			var files = Helper.GetAllFiles (_path);

			// Load all the files
			foreach (var file in files) {
				using (var sr = new StreamReader (file)) {
					try {
						var animationfile = JsonConvert.DeserializeObject<AnimationJsonFile> (sr.ReadToEnd ());
						_tempStore.Add (animationfile.Id, animationfile);

					} catch (Exception e) {
						Helper.ShowMsgBox ("ERROR: <file: " + file + ">" + e.Message);
					}
				}
			}

			// done!
		}

		public void SetTextures ()
		{
			foreach (var animationfile in _tempStore) {
				foreach (var animation in animationfile.Value.Animations) {
					_store.Add (animationfile.Key + "." + animation.Key, animationfile.Value.Load (_engine, animation.Key));
				}
			}
		}

		public IDictionary<string, string> GetImagePaths ()
		{
			var dict = new Dictionary<string, string> ();

			foreach (var animationfile in _tempStore.Values) {
				dict.Add (animationfile.ImageName, animationfile.Image);
			}

			return dict;
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

