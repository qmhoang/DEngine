using System;
using System.Collections.Generic;
using DEngine.Actor;
using DEngine.Core;
using DEngine.Entity;

namespace DEngine.Components {
//	public class VisionComponent : EntityComponent {
//		public int SightRadius { get; set; }
//		public bool Dirty { get; set; }
//
//		private Dictionary<Point, bool> visiblePoints;
//
//		public bool this[Point location] {
//			get { return visiblePoints.ContainsKey(location); }
//			set {
//				if (value)
//					visiblePoints.Add(location, value);
//				else
//					visiblePoints.Remove(location);
//			}
//		}
//
//		public VisionComponent() {
//			visiblePoints = new Dictionary<Point, bool>();
//			Dirty = false;
//			SightRadius = 1;
//		}
//
//		public bool HasLineOfSight(AbstractActor target) {
//			return HasLineOfSight(target.Position);
//		}
//
//		public bool HasLineOfSight(Point position) {
//			return this[position];
//		}
//
//		public bool CanSpot(AbstractActor target) {
//			throw new NotImplementedException();
//		}
//	
//	}
}