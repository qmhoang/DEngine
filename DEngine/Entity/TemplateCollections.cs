using System;
using System.Collections.Generic;

namespace DEngine.Entity {
	/// <summary>
	/// Manager for template collections.  Provides public manipulations
	/// for template collections.
	/// </summary>
	public class TemplateCollections {
		readonly EntityManager manager;    // Entity manager
		readonly Dictionary<Type, TemplateCollection> templates;

		public TemplateCollections(EntityManager entityManager) {
			manager = entityManager;
			templates = new Dictionary<Type, TemplateCollection>();
		}

		/// <summary>
		/// Get a template collection for the given template type
		/// </summary>
		/// <typeparam name="TTemplate"></typeparam>
		/// <returns></returns>
		public TemplateCollection Get<TTemplate>()
				where TTemplate : Template {
			// Ensure that we have something to return
			if (!templates.ContainsKey(typeof(TTemplate))) {
				templates.Add(typeof(TTemplate), new TemplateCollection(manager, typeof(TTemplate)));
			}

			return templates[typeof(TTemplate)];
		}

		#region Internal Add/Remove

		/// <summary>
		/// Add a new entity to the appropriate template collection
		/// </summary>
		/// <param name="e"></param>
		public void Add(Entity e) {
			if (e.Template == null)
				return;

			// Create a collection for the template type
			if (!templates.ContainsKey(e.Template)) {
				templates.Add(e.Template, new TemplateCollection(manager, e.Template));
			}

			templates[e.Template].Add(e);
		}

		/// <summary>
		/// Remove an entity from the template collections
		/// </summary>
		/// <param name="e"></param>
		public void Remove(Entity e) {
			if (e.Template == null)
				return;

			templates[e.Template].Remove(e);
		}

		#endregion
	}
}