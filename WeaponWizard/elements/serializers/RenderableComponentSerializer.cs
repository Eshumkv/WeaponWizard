using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeaponWizard.Elements.Components;
using System.Linq;
using Microsoft.Xna.Framework;

namespace WeaponWizard.Elements.Serializers
{
	public class RenderableComponentSerializer : JsonConverter
	{
		public override bool CanRead {
			get {
				return true;
			}
		}

		public override bool CanWrite {
			get {
				return true;
			}
		}

		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			var component = value as RenderableComponent;

			writer.WriteStartObject ();
			writer.WritePropertyName ("Texture");
			serializer.Serialize (writer, component.Texture.Name);
			writer.WritePropertyName ("Z");
			serializer.Serialize (writer, component.Z);
			writer.WritePropertyName ("Color");
			serializer.Serialize (writer, component.Color);
			writer.WritePropertyName ("Rect");
			serializer.Serialize (writer, component.SourceRect);
			writer.WriteEndObject ();
		}

		public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var jobj = JObject.Load (reader);
			var properties = jobj.Properties ().ToList ();

			return null;
		}

		public override bool CanConvert (Type objectType)
		{
			throw new NotImplementedException ();
		}
	}
}

