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
	public enum PromptType {
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
		PromptType RequiresPrompt { get; }
		ActionResult OnProcess();		
	}

	public interface IPromptAction : IAction {
		string Message { get; }
	}

	public interface IBooleanAction : IPromptAction {
		bool Default { get; }
		void SetBoolean(bool b);
	}

	public interface INumberAction : IPromptAction {
		int Mininum { get; }
		int Maximum { get; }
		int Initial { get; }
		void SetNumber(int i);
	}

	public interface ILocationAction : IPromptAction {
		void SetLocation(Point p);
	}

	public interface IDirectionAction : IPromptAction {
		void SetDirection(Direction d);
	}

	public interface IOptionsAction : IPromptAction {
		void Fail();
		void SetOption(string o);
		IEnumerable<string> Options { get; }
	}

	public interface IPlayerInputAction : IAction {
		bool SetFinished();
	}

	public class RequiresPlayerInputAction : IPlayerInputAction {
		public int APCost {
			get { return 1; }
		}

		public PromptType RequiresPrompt { get; private set; }

		public ActionResult OnProcess() {
			return ActionResult.SuccessNoTime;
		}

		public bool SetFinished() {
			RequiresPrompt = PromptType.None;
			return true;
		}

		public RequiresPlayerInputAction() {
			RequiresPrompt = PromptType.PlayerInput;
			
		}
	}

	public abstract class ActorAction : IAction {
		public Entity Entity { get; private set; }
		public abstract int APCost { get; }

		public virtual PromptType RequiresPrompt { get { return PromptType.None; } }
		
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
