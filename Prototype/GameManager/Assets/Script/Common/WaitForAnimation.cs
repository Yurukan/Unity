using UnityEngine;

namespace Assets.Script.Common
{
	public class WaitForAnimation : CustomYieldInstruction
	{
		Animator _animator;
		int _lastStateHash = 0;
		int _layerNo = 0;

		public WaitForAnimation(Animator animator, int layerNo)
		{
			Init(animator, layerNo, 
				animator.GetCurrentAnimatorStateInfo(layerNo).fullPathHash);
		}

		void Init(Animator animator, int layerNo, int hash)
		{
			_layerNo = layerNo;
			_animator = animator;
			_lastStateHash = hash;
		}

		public override bool keepWaiting
		{
			get
			{
				var currentAnimatorState = _animator
					.GetCurrentAnimatorStateInfo(_layerNo);

				return currentAnimatorState.fullPathHash == _lastStateHash &&
					(currentAnimatorState.normalizedTime < 1);
			}
		}
	}
}
