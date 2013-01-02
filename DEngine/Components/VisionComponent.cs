using System;
using System.Collections.Generic;
using DEngine.Actor;
using DEngine.Core;
using DEngine.Entity;

namespace DEngine.Components {
	public class VisionComponent : EntityComponent {
		public int SightRadius { get; set; }
		public bool Dirty { get; set; }

		private Dictionary<Point, bool> visiblePoints;

		public bool this[Point location] {
			get { return visiblePoints.ContainsKey(location); }
			set {
				if (value)
					visiblePoints.Add(location, value);
				else
					visiblePoints.Remove(location);
			}
		}

		public VisionComponent() {
			visiblePoints = new Dictionary<Point, bool>();
			Dirty = false;
			SightRadius = 1;
		}

		public bool HasLineOfSight(AbstractActor target) {
			return HasLineOfSight(target.Position);
		}

		public bool HasLineOfSight(Point position) {
			return this[position];
		}

		public bool CanSpot(AbstractActor target) {
			throw new NotImplementedException();
		}
	}

	public class FieldOfViewSystem {
		private FilteredCollection entities;

		public FieldOfViewSystem(EntityManager entityManager) {
			entities = entityManager.Get(typeof(VisionComponent), typeof(Location));
		}

		public void Update() {
			foreach (var entity in entities) {
				var vision = entity.As<VisionComponent>();
				if (vision.Dirty) {
					var location = entity.As<Location>();
					location.Level.CalculateFOV(location.Position, vision.SightRadius);
					ShadowCastingFOV.ComputeRecursiveShadowcasting();
				}
			}
		}

		private static int[,] mult = new int[4, 8] {{1,0,0,-1,-1,0,0,1},
													{0,1,-1,0,0,-1,1,0},
													{0,1,1,0,0,-1,-1,0},
													{1,0,0,1,-1,0,0,-1}};

		private static void CastLight(List<Point> visibles, FovMap map, int cx, int cy, int row, float start, float end, int radius, int r2, int xx, int xy, int yx, int yy, int id, bool light_walls) {			 
			float new_start = 0.0f;
			if (start < end)
				return;

			for (int j = row; j < radius + 1; j++) {
				int dx = -j - 1;
				int dy = -j;
				bool blocked = false;
				while (dx <= 0) {
					dx++;
					int X = cx + dx * xx + dy * xy;
					int Y = cy + dx * yx + dy * yy;
					if ((uint)X < (uint)map.Width && (uint)Y < (uint)map.Height) {
						int offset = X + Y * map.Width;
						float l_slope = (dx - 0.5f) / (dy + 0.5f);
						float r_slope = (dx + 0.5f) / (dy - 0.5f);
						if (start < r_slope)
							continue;
						else if (end > l_slope)
							break;
						if (dx * dx + dy * dy <= r2 && (light_walls || map.Cells[X, Y].Transparent))
							visibles.Add(new Point(X, Y));
						if (blocked) {
							if (!map.Cells[X, Y].Transparent) {
								new_start = r_slope;
								continue;
							} else {
								blocked = false;
								start = new_start;
							}
						} else {
							if (!map.Cells[X, Y].Transparent && j < radius) {
								blocked = true;
								CastLight(visibles, map, cx, cy, j + 1, start, l_slope, radius, r2, xx, xy, yx, yy, id + 1, light_walls);
								new_start = r_slope;
							}
						}
					}
				}
				if (blocked)
					break;
			}
		}

		private static List<Point> ComputeRecursiveShadowcasting(FovMap map, int playerX, int playerY, int maxRadius, bool lightWalls) {
			List<Point> visibles = new List<Point>();

			map.ClearVisibility();

			if (maxRadius == 0) {
				int max_radius_x = map.Width - playerX;
				int max_radius_y = map.Height - playerY;
				max_radius_x = Math.Max(max_radius_x, playerX);
				max_radius_y = Math.Max(max_radius_y, playerY);
				maxRadius = (int)(Math.Sqrt(max_radius_x * max_radius_x + max_radius_y * max_radius_y)) + 1;
			}
			int r2 = maxRadius * maxRadius;
			/* recursive shadow casting */
			for (int oct = 0; oct < 8; oct++) {
				CastLight(visibles, map, playerX, playerY, 1, 1.0f, 0.0f, maxRadius, r2, mult[0, oct], mult[1, oct], mult[2, oct], mult[3, oct], 0, lightWalls);
			}
			map.Cells[playerX, playerY].Visible = true;
		}
	}
}