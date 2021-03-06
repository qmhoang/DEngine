﻿using System;
using System.Diagnostics.Contracts;
using DEngine.Core;

namespace DEngine.Level {
	/// Credit to https://code.google.com/p/magecrawl/source/browse/Trunk/GameEngine/Physics/ShadowCastingFOV.cs
	/// 
	/// This code is taken almost directly from fov_recursive_shadowcasting.c
	/// in libtcod, with formatting changes and a few interface changes
	/// It is excempt from most any code formatting rules, since I try to
	/// keep the internals as close to C as possible to make porting changes easy

	// ReSharper disable InconsistentNaming
	public static class ShadowCastingFOV
	{
		private static readonly int [,] mult = new int[4,8] {{1,0,0,-1,-1,0,0,1},
															{0,1,-1,0,0,-1,1,0},
															{0,1,1,0,0,-1,-1,0},
															{1,0,0,1,-1,0,0,-1}};

		private static void CastLight(VisionMap vision, AbstractLevel level, int cx, int cy, int row, float start, float end, int radius, int r2, int xx, int xy, int yx, int yy, int id, bool light_walls) 
		{
			Contract.Requires<ArgumentNullException>(vision != null, "vision");
			Contract.Requires<ArgumentNullException>(level != null, "level");

			float new_start = 0.0f;
			if (start < end)
				return;
		
			for (int j = row; j < radius + 1; j++) 
			{
				int dx = -j - 1;
				int dy = -j;
				bool blocked = false;
				while (dx <= 0)
				{
					dx++;
					int X = cx + dx * xx + dy * xy;
					int Y = cy + dx * yx + dy * yy;
					if ((uint)X < (uint)level.Width && (uint)Y < (uint)level.Height) 
					{
//						int offset = X + Y * level.Width;
						float l_slope = (dx - 0.5f) / (dy + 0.5f);
						float r_slope = (dx + 0.5f) / (dy - 0.5f);
						if (start < r_slope)
							continue;
						else if(end > l_slope)
							break;
//						if (dx * dx + dy * dy <= r2 && (light_walls || level.Cells[X, Y].Transparent))
						if (dx * dx + dy * dy <= r2 && (light_walls || level.IsTransparent(X, Y)))
//							map.Cells[X, Y].Visible = true;
							vision.SetVisibility(X, Y, true);
						if ( blocked ) 
						{
//							if (!level.Cells[X, Y].Transparent)
							if (!level.IsTransparent(X, Y))
							{
								new_start = r_slope;
								continue;
							}
							else 
							{
								blocked = false;
								start = new_start;
							}
						} 
						else 
						{
//							if (!level.Cells[X, Y].Transparent && j < radius)
							if (!level.IsTransparent(X, Y) && j < radius)
							{
								blocked = true;
								CastLight(vision, level, cx, cy, j + 1, start, l_slope, radius, r2, xx, xy, yx, yy, id+1, light_walls);
								new_start = r_slope;
							}
						}
					}
				}
				if (blocked)
					break;
			}
		}

		public static void ComputeRecursiveShadowcasting(VisionMap vision, AbstractLevel level, int playerX, int playerY, int maxRadius, bool lightWalls)
		{
			Contract.Requires<ArgumentNullException>(level != null, "level");
			Contract.Requires<ArgumentNullException>(vision != null, "vision");
//			map.ClearVisibility();
			vision.ClearVisibility();

			if ( maxRadius == 0 ) 
			{
				int max_radius_x = level.Width - playerX;
				int max_radius_y = level.Height - playerY;
				max_radius_x = Math.Max(max_radius_x, playerX);
				max_radius_y = Math.Max(max_radius_y, playerY);
				maxRadius = (int)(Math.Sqrt(max_radius_x * max_radius_x + max_radius_y * max_radius_y)) + 1;
			}
			int r2 = maxRadius * maxRadius;
			/* recursive shadow casting */
			for (int oct = 0; oct < 8; oct++)
			{
				CastLight(vision, level, playerX, playerY, 1, 1.0f, 0.0f, maxRadius, r2, mult[0,oct], mult[1,oct], mult[2,oct], mult[3,oct], 0, lightWalls);
			}
//			map.Cells[playerX, playerY].Visible = true;
			vision.SetVisibility(playerX, playerY, true);
		}
	}
	// ReSharper restore InconsistentNaming
}