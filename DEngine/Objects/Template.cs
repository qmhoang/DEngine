using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEngine.Entity {
	public class Template : IEnumerable<EntityComponent> {
		readonly Dictionary<Type, EntityComponent> components = new Dictionary<Type, EntityComponent>();

		public Template() { }

		public Template(params EntityComponent[] components)
			: this() {
			if (components == null)
				return;

			Add(components);
		}

		/// <summary>
		/// Add a component to the template
		/// </summary>
		/// <param name="component"></param>
		public void Add(EntityComponent component) {
			if (components.ContainsKey(component.GetType())) {
				components[component.GetType()] = component;
			} else {
				components.Add(component.GetType(), component);
			}
		}

		/// <summary>
		/// Add a collection of components
		/// </summary>
		/// <param name="components"></param>
		public void Add(params EntityComponent[] components) {
			foreach (var component in components) {
				Add(component);
			}
		}

		/// <summary>
		/// Get a component of a given type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T As<T>() where T : EntityComponent {
			EntityComponent o;
			components.TryGetValue(typeof(T), out o);
			return (T)o;
		}

		/// <summary>
		/// Check if the template contains a given type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public bool Is<T>() where T : EntityComponent {
			return components.ContainsKey(typeof(T));
		}

		#region IEnumerable

		public IEnumerator<EntityComponent> GetEnumerator() {
			return components.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		#endregion
	}
}
