using UnityEngine;
using UnityEngine.UI;
namespace Imoet.Unity.U2D {
	public class ButtonX : Button
	{
		protected override void DoStateTransition(SelectionState state, bool instant)
		{
            var targetColor =
                state == SelectionState.Disabled ? colors.disabledColor :
                state == SelectionState.Highlighted ? colors.highlightedColor :
                state == SelectionState.Normal ? colors.normalColor :

#if UNITY2017_OR_NEWER
                state == SelectionState.Pressed ? colors.pressedColor :
                : state == SelectionState.Selected ? colors.selectedColor : Color.white;
#else  
                state == SelectionState.Pressed ? colors.pressedColor : Color.white;
#endif

            foreach (var graphic in GetComponentsInChildren<Graphic>()) {
				graphic.CrossFadeColor(targetColor, instant ? 0.001f : colors.fadeDuration, true, true);
			}
		}
	}
}
