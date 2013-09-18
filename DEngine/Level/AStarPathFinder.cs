using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DEngine.Level {
	// ReSharper disable InconsistentNaming
	// ReSharper disable FieldCanBeMadeReadOnly.Local

	/// Credit to https://code.google.com/p/magecrawl/source/browse/Trunk/GameEngine/Physics/AStarPathFinder.cs
	/// 
	/// This code is taken almost directly from path_c.c
	/// in libtcod, with formatting changes and a few interface changes
	/// It is excempt from most any code formatting rules, since I try to
	/// keep the internals as close to C as possible to make porting changes easy
	public class AStarPathFinder {
		private enum PathFindingDirection {
			NORTH_WEST,
			NORTH,
			NORTH_EAST,
			WEST,
			NONE,
			EAST,
			SOUTH_WEST,
			SOUTH,
			SOUTH_EAST
		};

		/* convert dir_t to dx,dy */
		static int[] dirx={-1,0,1,-1,0,1,-1,0,1};
		static int[] diry={-1,-1,-1,0,0,0,1,1,1};

		private static PathFindingDirection[] invdir = {
		                                               		PathFindingDirection.SOUTH_EAST,
		                                               		PathFindingDirection.SOUTH, 
															PathFindingDirection.SOUTH_WEST, 
															PathFindingDirection.EAST, 
															PathFindingDirection.NONE,
		                                               		PathFindingDirection.WEST, 
															PathFindingDirection.NORTH_EAST, 
															PathFindingDirection.NORTH, 
															PathFindingDirection.NORTH_WEST
		                                               };

		private int ox, oy; /* coordinates of the creature position */
		private int dx, dy; /* coordinates of the creature's destination */


		private List<PathFindingDirection> path;

		private int w, h; /* map size */

		private float[] grid; /* wxh djikstra distance grid (covered distance) */
		private float[] heur;
		private PathFindingDirection[] prev;

		private float diagonalCost;

		private List<uint> heap;

		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(grid != null);
			Contract.Invariant(heur != null);
			Contract.Invariant(heap != null);
			Contract.Invariant(path != null);
			Contract.Invariant(prev != null);			
		}

		private AbstractLevel level;

		#region Heap Functions

		// libtcod's version uses a list that these functions turn into a heap
		/* small layer on top of TCOD_list_t to implement a binary heap (min_heap) */
		private void heap_sift_down() {
			/* sift-down : move the first element of the heap down to its right place */
			int cur = 0;
			int end = heap.Count - 1;
			int child = 1;

			while (child <= end) {
				int toSwap = cur;
				uint off_cur = heap[cur];
				float cur_dist = heur[off_cur];
				float swapValue = cur_dist;
				uint off_child = heap[child];
				float child_dist = heur[off_child];
				if (child_dist < cur_dist) {
					toSwap = child;
					swapValue = child_dist;
				}
				if (child < end) {
					/* get the min between child and child+1 */
					uint off_child2 = heap[child + 1];
					float child2_dist = heur[off_child2];
					if (swapValue > child2_dist) {
						toSwap = child + 1;
						swapValue = child2_dist;
					}
				}
				if (toSwap != cur) {
					/* get down one level */
					uint tmp = heap[toSwap];
					heap[toSwap] = heap[cur];
					heap[cur] = tmp;
					cur = toSwap;
				} else
					return;
				child = cur * 2 + 1;
			}
		}

		private void heap_sift_up() {
			/* sift-up : move the last element of the heap up to its right place */
			int end = heap.Count - 1;
			int child = end;
			while (child > 0) {
				uint off_child = heap[child];
				float child_dist = heur[off_child];
				int parent = (child - 1) / 2;
				uint off_parent = heap[parent];
				float parent_dist = heur[off_parent];
				if (parent_dist > child_dist) {
					/* get up one level */
					uint tmp = heap[child];
					heap[child] = heap[parent];
					heap[parent] = tmp;
					child = parent;
				} else
					return;
			}
		}

		/* add a coordinate pair in the heap so that the heap root always contains the minimum A* score */

		private void heap_add(int x, int y) {
			/* append the new value to the end of the heap */
			uint off = (uint) (x + y * w);
			heap.Add(off);
			/* bubble the value up to its real position */
			heap_sift_up();
		}

		/* get the coordinate pair with the minimum A* score from the heap */

		private uint heap_get() {
			/* return the first value of the heap (minimum score) */
			int end = heap.Count() - 1;
			uint off = heap[0];
			/* take the last element and put it at first position (heap root) */
			heap[0] = heap[end];
			heap.RemoveAt(end);
			/* and bubble it down to its real position */
			heap_sift_down();
			return off;
		}

		/* this is the slow part, when we change the heuristic of a cell already in the heap */

		private void heap_reorder(uint offset) {
			int end = heap.Count;
			int cur = 0;
			uint off_idx = 0;
			float value;
			int idx = 0;
			int heap_size = heap.Count();

			while (cur != end) {
				if (heap[cur] == offset) 
					break;
				cur++;idx++;
			}

			if (cur == end) return;

			off_idx = heap[idx];
			value = heur[off_idx];
			if (idx > 0) {
				int parent = (idx - 1) / 2;
				/* compare to its parent */
				uint off_parent = heap[parent];
				float parent_value = heur[off_parent];
				if (value < parent_value) {
					/* smaller. bubble it up */
					while (idx > 0 && value < parent_value) {
						/* swap with parent */
						heap[parent] = off_idx;
						heap[idx] = off_parent;
						idx = parent;
						if (idx > 0) {
							parent = (idx - 1) / 2;
							off_parent = heap[parent];
							parent_value = heur[off_parent];
						}
					}
					return;
				}
			}
			/* compare to its sons */
			while (idx * 2 + 1 < heap_size) {
				int child = idx * 2 + 1;
				Contract.Assume(child < heap.Count);
				uint off_child = heap[child];
				int toSwap = idx;
				int child2;
				float swapValue = value;
				if (heur[off_child] < value) {
					/* swap with son1 ? */
					toSwap = child;
					swapValue = heur[off_child];
				}
				child2 = child + 1;
				if (child2 < heap_size) {
					Contract.Assume(child2 < heap.Count);
					uint off_child2 = heap[child2];
					if (heur[off_child2] < swapValue) /* swap with son2 */
						toSwap = child2;
				}
				if (toSwap != idx) {
					/* bigger. bubble it down */
					uint tmp = heap[toSwap];
					heap[toSwap] = heap[idx];
					heap[idx] = tmp;
					idx = toSwap;
				} else
					return;
			}
		}

		#endregion

		public AStarPathFinder(AbstractLevel level, float diagonalCost) {
			Contract.Requires<ArgumentNullException>(level != null, "level");
			w = level.Width;
			h = level.Height;

			grid = new float[h * w];
			heur = new float[h * w];
			prev = new PathFindingDirection[h * w];
			path = new List<PathFindingDirection>();
			heap = new List<uint>();

			this.level = level;
			this.diagonalCost = diagonalCost;
		}

		public bool Compute(int ox, int oy, int dx, int dy) {
			this.ox = ox;
			this.oy = oy;
			this.dx = dx;
			this.dy = dy;
			path.Clear();
			heap.Clear();

			if (ox == dx && oy == dy) return true; /* trivial case */

			/* check that origin and destination are inside the map */
			if (!((uint) ox < (uint) w && (uint) oy < (uint) h)) 
				return false;
			if (!((uint) dx < (uint) w && (uint) dy < (uint) h)) 
				return false;

			Array.Clear(grid, 0, w * h);
			for (int i = 0; i < w * h; i++) {
				prev[i] = PathFindingDirection.NONE;
			}
			heur[ox + oy * w] = 1.0f;

			TCOD_path_push_cell(ox, oy); /* put the origin cell as a bootstrap */
			/* fill the djikstra grid until we reach dx,dy */
			TCOD_path_set_cells();

			if (grid[dx + dy * w] == 0)
				return false; /* no path found */

			/* there is a path. retrieve it */
			do {
				/* walk from destination to origin, using the 'prev' array */
				PathFindingDirection step = prev[dx + dy * w];
				path.Add(step);
				dx -= dirx[(int) step];
				dy -= diry[(int) step];
			} while (dx != ox || dy != oy);
			return true;
		}

		public void Reverse() {
			int tmp, i;

			tmp = ox;
			ox = dx;
			dx = tmp;
			tmp = oy;
			oy = dy;
			dy = tmp;

			for (i = 0; i < path.Count; i++) {
				PathFindingDirection d = path[i];
				d = invdir[(int) d];
				path[i] = d;				
			}
		}

		public bool Walk(ref int x, ref int y, bool recalculate_when_needed) {
			if (path.Count == 0)
				return false;
			PathFindingDirection d = path[path.Count - 1];
			path.RemoveAt(path.Count - 1);			

			int newx = ox + dirx[(int) d];
			int newy = oy + diry[(int) d];

			/* check if the path is still valid */
			if (!level.IsWalkable(newx, newy)) {
				if (!recalculate_when_needed)
					return false; /* don't walk */
				/* calculate a new path */
				if (!Compute(ox, oy, dx, dy))
					return false; /* cannot find a new path */

				return Walk(ref x, ref y, true); /* walk along the new path */
			}

			x = newx;
			y = newy;
			ox = newx;
			oy = newy;
			return true;
		}

		public bool IsEmpty() {
			return path.Count == 0;
		}

		public int Size() {
			return path.Count;
		}

		public void GetPathElement(int index, out int x, out int y) {
			int pos;
			x = ox;
			y = oy;
			pos = path.Count - 1;
			do {
				Contract.Assert(index >= -1);
				Contract.Assert(pos >= 0);
				PathFindingDirection step = path[pos];
				x += dirx[(int) step];
				y += diry[(int) step];
				pos--;
				index--;
			} while (index >= 0);
		}

		/* private stuff */
		/* add a new unvisited cells to the cells-to-treat list
		 * the list is in fact a min_heap. Cell at index i has its sons at 2*i+1 and 2*i+2
		 */

		private void TCOD_path_push_cell(int x, int y) {
			heap_add(x, y);
		}

		/* get the best cell from the heap */

		private void TCOD_path_get_cell(out int x, out int y, out float distance) {
			uint offset = heap_get();
			x = (int) (offset % w);
			y = (int) (offset / w);
			distance = grid[offset];
		}

		private float TCOD_path_walk_cost(int xFrom, int yFrom, int xTo, int yTo) {
			return level.IsWalkable(xTo, yTo) ? 1.0f : 0.0f;
		}

		private void TCOD_path_get_origin(out int x, out int y) {
			x = ox;
			y = oy;
		}

		private void TCOD_path_get_destination(out int x, out int y) {
			x = dx;
			y = dy;
		}

		private int[] idirx = {0, -1, 1, 0, -1, 1, -1, 1};
		private int[] idiry = {-1, 0, 0, 1, -1, -1, 1, 1};

		private PathFindingDirection[] prevdirs = {
		                                          		PathFindingDirection.NORTH, PathFindingDirection.WEST, PathFindingDirection.EAST, PathFindingDirection.SOUTH,
		                                          		PathFindingDirection.NORTH_WEST, PathFindingDirection.NORTH_EAST, PathFindingDirection.SOUTH_WEST,
		                                          		PathFindingDirection.SOUTH_EAST
		                                          };

		/* fill the grid, starting from the origin until we reach the destination */

		private void TCOD_path_set_cells() {
			while (grid[dx + dy * w] == 0 && !(heap.Count == 0)) {
				int x, y, i, imax;
				float distance;
				TCOD_path_get_cell(out x, out y, out distance);

				imax = (diagonalCost == 0.0f ? 4 : 8);
				for (i = 0; i < imax; i++) {
					/* convert i to dx,dy */					
					/* convert i to direction */
					/* coordinate of the adjacent cell */
					int cx = x + idirx[i];
					int cy = y + idiry[i];
					if (cx >= 0 && cy >= 0 && cx < w && cy < h) {
						float walk_cost = TCOD_path_walk_cost(x, y, cx, cy);
						if (walk_cost > 0.0f) {
							/* in of the map and walkable */
							float covered = distance + walk_cost * (i >= 4 ? diagonalCost : 1.0f);
							float previousCovered = grid[cx + cy * w];
							if (previousCovered == 0) {
								/* put a new cell in the heap */
								int offset = cx + cy * w;
								/* A* heuristic : remaining distance */
								float remaining = (float) Math.Sqrt((cx - dx) * (cx - dx) + (cy - dy) * (cy - dy));
								grid[offset] = covered;
								heur[offset] = covered + remaining;
								prev[offset] = prevdirs[i];
								TCOD_path_push_cell(cx, cy);
							} else if (previousCovered > covered) {
								/* we found a better path to a cell already in the heap */
								int offset = cx + cy * w;
								grid[offset] = covered;
								heur[offset] -= (previousCovered - covered); /* fix the A* score */
								prev[offset] = prevdirs[i];
								/* reorder the heap */
								heap_reorder((uint)offset);
							}
						}
					}
				}
			}
		}
	}

	// ReSharper restore InconsistentNaming
	// ReSharper restore FieldCanBeMadeReadOnly.Local
}