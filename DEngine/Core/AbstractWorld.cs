using System;
using DEngine.Entities;

namespace DEngine.Core {
	public abstract class AbstractWorld {
		/// <summary>
		/// default speed of entities, an entity with 2x speed gains AP 2x as fast
		/// </summary>
		public const int DEFAULT_SPEED = 100; // 

		public const int TURN_LENGTH_IN_SECONDS = 1;	// how long is a turn in seconds
		public const int TURN_LENGTH_IN_AP = DEFAULT_SPEED;	// how long is a turn in seconds
		public const int MEAN = 50;						// what is the mean score for an attribute
		public const int STANDARD_DEVIATION = 15;		// what is the stddev for an attribute score
		public const double TILE_LENGTH_IN_METER = 1f;	// length of 1 square tile
		public TagManager<string> TagManager { get; private set; }
		public GroupManager<string> GroupManager { get; private set; }
		public EntityFactory EntityFactory { get; private set; }
		public abstract Entity Player { get; set; }
		public EntityManager EntityManager { get; private set; }
		public Log Log { get; private set; }

		public static int SecondsToActionPoints(double seconds) {
			return  (int) Math.Round((seconds * DEFAULT_SPEED) / TURN_LENGTH_IN_SECONDS);
		}

		public static double ActionPointsToSeconds(int ap) {
			return (double) (ap * TURN_LENGTH_IN_SECONDS) / DEFAULT_SPEED;
		}

		public static int SpeedToActionPoints(double speed) {
			return SecondsToActionPoints(SpeedToSeconds(speed));
		}

		public static double ActionPointsToSpeed(int ap) {
			return SecondsToSpeed(ActionPointsToSeconds(ap));
		}

		public static double SpeedToSeconds(double speed) {
			return (DEFAULT_SPEED * TURN_LENGTH_IN_SECONDS) / speed;
		}

		/// <summary>
		/// Convert how fast an action in seconds to its speed, where speed represents how fast an action is
		/// </summary>
		public static double SecondsToSpeed(double seconds) {
			return ((DEFAULT_SPEED * TURN_LENGTH_IN_SECONDS) / seconds);
		}

		protected AbstractWorld(TagManager<string> tagManager, GroupManager<string> groupManager, EntityFactory entityFactory, EntityManager entityManager, Log log) {
			TagManager = tagManager;
			GroupManager = groupManager;
			EntityFactory = entityFactory;
			EntityManager = entityManager;
			Log = log;
		}
	}
}