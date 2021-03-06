﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using DEngine.Components;
using log4net;

namespace DEngine.Entities {
	public class IllegalInheritanceException : Exception {
		public string IllegalInheritanceId { get; private set; }
		public string TemplateId { get; private set; }

		public IllegalInheritanceException(string illegalInheritanceId, string templateId) : base("Cannot find template to inherit from.") {
			IllegalInheritanceId = illegalInheritanceId;
			TemplateId = templateId;
		}
	}

	public class EntityFactory {
		public class Template : IEnumerable<Component> {
			private readonly Dictionary<Type, Component> _components;

			public Template() {
				_components = new Dictionary<Type, Component>();
			}

			public Template(IEnumerable<Component> components)
				: this(components.ToArray()) { }

			public Template(params Component[] components)
				: this() {
				if (components == null)
					return;

				Add(components);
			}

			/// <summary>
			/// Add a component to the template
			/// </summary>
			/// <param name="component"></param>
			public Template Add(Component component) {
				if (_components.ContainsKey(component.GetType())) {
					_components[component.GetType()] = component;
				} else {
					_components.Add(component.GetType(), component);
				}
				return this;
			}

			/// <summary>
			/// Add a collection of components
			/// </summary>
			/// <param name="comps"></param>
			public Template Add(IEnumerable<Component> comps) {
				foreach (var component in comps) {
					Add(component);
				}
				return this;
			}

			/// <summary>
			/// Add a collection of components
			/// </summary>
			/// <param name="comps"></param>
			public Template Add(params Component[] comps) {
				return Add(comps as IEnumerable<Component>);

			}

			/// <summary>
			/// Get a component of a given type
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <returns></returns>
			[Pure]
			public T Get<T>() where T : Component {
				Component o;
				_components.TryGetValue(typeof(T), out o);
				return (T)o;
			}

			/// <summary>
			/// Check if the template contains a given type
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <returns></returns>
			[Pure]
			public bool Has<T>() where T : Component {
				return _components.ContainsKey(typeof(T));
			}

			#region IEnumerable

			public IEnumerator<Component> GetEnumerator() {
				return _components.Values.GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
				return GetEnumerator();
			}

			#endregion
		}

		private readonly Dictionary<string, Template> _compiledTemplates;
		private readonly Dictionary<string, Tuple<string, Template>> _inheritanceTemplates;
		private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public EntityFactory() {
			_compiledTemplates = new Dictionary<string, Template>();
			_inheritanceTemplates = new Dictionary<string, Tuple<string, Template>>();
		}

		public void Compile() {
			foreach (var t in _inheritanceTemplates) {
				if (!(_inheritanceTemplates.ContainsKey(t.Value.Item1) || _compiledTemplates.ContainsKey(t.Value.Item1))) {
					throw new IllegalInheritanceException(t.Value.Item1, t.Key);
				}
			}

			Queue<string> queues = new Queue<string>();
			foreach (var t in _inheritanceTemplates) {
				queues.Enqueue(t.Key);
			}

			while (queues.Count > 0) {
				string id = queues.Dequeue();
				var baseId = _inheritanceTemplates[id].Item1;

				Logger.DebugFormat("Dequequeing {0}{1}.", id, String.IsNullOrEmpty(baseId) ? "" : string.Format(" which inherits from {0}", baseId));

				if (String.IsNullOrEmpty(baseId)) {
					Logger.DebugFormat("No inheritance, compiling {0}", id);
					_compiledTemplates.Add(id, _inheritanceTemplates[id].Item2);
				} else if (_compiledTemplates.ContainsKey(baseId)) {
					Logger.DebugFormat("Inherited class found, compiling {0}", id);
					Template template = new Template(_compiledTemplates[baseId]);
					template.Add(_inheritanceTemplates[id].Item2);

					_compiledTemplates.Add(id, template);
				} else {
					Logger.DebugFormat("No inherited class found, requequeing {0}", id);
					queues.Enqueue(id);
				}
			}

			_inheritanceTemplates.Clear();
		}

		public IEnumerable<Component> Get(string id) {
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(id));
			return _compiledTemplates[id].Select(c => c.Copy());
		}

		public void Add(string refId, params Component[] comps) {
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(refId));
			var t = new Template(comps) {new ReferenceId(refId)};
			Add(t);
		}

		public void Add(Template template) {
			Contract.Requires<ArgumentNullException>(template != null, "entity");			
			Contract.Requires<ArgumentException>(template.Has<ReferenceId>());
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(template.Get<ReferenceId>().RefId));

			_compiledTemplates.Add(template.Get<ReferenceId>().RefId, template);
		}

		public void Inherits(string refId, string baseEntity, params Component[] comps) {
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(baseEntity));
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(refId));

			var t = new Template(comps) {new ReferenceId(refId)};
			Inherits(baseEntity, t);
		}

		public void Inherits(string baseEntity, Template template) {
			Contract.Requires<ArgumentNullException>(template != null, "entity");
			Contract.Requires<ArgumentException>(template.Has<ReferenceId>());
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(template.Get<ReferenceId>().RefId));
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(baseEntity));

			_inheritanceTemplates.Add(template.Get<ReferenceId>().RefId, new Tuple<string, Template>(baseEntity, template));
		}

		public Entity Create(string refId, EntityManager em) {
			return em.Create(Get(refId));
		}
	}
}