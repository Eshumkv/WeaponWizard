using System;
using System.Collections.Generic;
using WeaponWizard.Elements.Components;

namespace WeaponWizard.Elements
{
	public class Entity
	{
		/// <summary>
		/// The list of components.
		/// </summary>
		public Dictionary<Type, Component> Components;

		/// <summary>
		/// Initializes a new instance of the <see cref="WeaponWizard.Elements.Entity"/> class.
		/// </summary>
		public Entity ()
		{
			Components = new Dictionary<Type, Component> ();
		}

		/// <summary>
		/// Adds the component.
		/// </summary>
		/// <returns>The component.</returns>
		/// <param name="component">Component.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public Entity AddComponent<T> (T component) where T : Component
		{
			Components.Add (typeof(T), component);

			return this;
		}

		/// <summary>
		/// Removes the component.
		/// </summary>
		/// <returns>The component.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T RemoveComponent<T> () where T : Component
		{
			T component = Components [typeof(T)] as T;
			Components.Remove (typeof(T));

			return component;
		}

		/// <summary>
		/// Get a component.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Get<T> () where T : Component
		{
			return Components [typeof(T)] as T;
		}

		/// <summary>
		/// Determines whether this instance has a component of type T.
		/// </summary>
		/// <returns><c>true</c> if this instance has component; otherwise, <c>false</c>.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public bool HasComponent<T> () where T : Component
		{
			try {
				return (Components [typeof(T)] as T) != null;
			} catch (KeyNotFoundException) {
				return false;
			}
		}

		/// <summary>
		/// Determines whether this instance has a component of the specified type;
		/// </summary>
		/// <returns><c>true</c> if this instance has component the specified t; otherwise, <c>false</c>.</returns>
		/// <param name="t">T.</param>
		public bool HasComponent (Type t)
		{ 
			try {
				var component = Components [t];

				return true;
			} catch (KeyNotFoundException) {
				return false;
			}
		}
	}
}

