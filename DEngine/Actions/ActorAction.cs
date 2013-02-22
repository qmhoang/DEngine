using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using DEngine.Actor;
using DEngine.Core;
using DEngine.Entities;

namespace DEngine.Actions {
	public enum PromptRequired {
		None,
		Boolean,
		Number,
		Location,
		Direction,
		Options,
		PlayerInput
	}
	public interface IAction {
		int APCost { get; }
		PromptRequired RequiresPrompt { get; }
		ActionResult OnProcess();		
	}

	public interface IBooleanAction : IAction {
		string Message { get; }
		bool SetBoolean(bool b);
	}

	public interface INumberAction : IAction {
		string Message { get; }
		bool SetNumber(int i);
	}

	public interface ILocationAction : IAction {
		string Message { get; }
		bool SetLocation(Point p);
	}

	public interface IDirectionAction : IAction {
		string Message { get; }
		bool SetDirection(Direction d);
	}

	public interface IOptionsAction : IAction {
		string Message { get; }
		bool SetOption(string o);
		IEnumerable<string> Options { get; }
	}

	public interface IPlayerInputAction : IAction {
		bool SetFinished();
	}

	public class RequiresPlayerInputAction : IPlayerInputAction {
		public int APCost {
			get { return 1; }
		}

		public PromptRequired RequiresPrompt { get; private set; }

		public ActionResult OnProcess() {
			return ActionResult.SuccessNoTime;
		}

		public bool SetFinished() {
			RequiresPrompt = PromptRequired.None;
			return true;
		}

		public RequiresPlayerInputAction() {
			RequiresPrompt = PromptRequired.PlayerInput;
			
		}
	}

	public abstract class ActorAction : IAction {
		public Entity Entity { get; private set; }
		public abstract int APCost { get; }

		public virtual PromptRequired RequiresPrompt { get { return PromptRequired.None; } }
		
		protected ActorAction(Entity entity) {
			Contract.Requires<ArgumentNullException>(entity != null, "entity");			
			Contract.Requires<ArgumentException>(entity.Has<Components.ActorComponent>());
			Entity = entity;			
		}
		
		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(Entity != null);
		}

		public abstract ActionResult OnProcess();
	}
}
