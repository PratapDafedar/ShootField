using System;
using UnityEngine;
using System.Collections;

public static class EaseUtility
{
	const float piOver2 = Mathf.PI * 0.5f;
	const float twoPi = Mathf.PI * 2;
	
	public enum Ease
	{
		Unset, // Used to let TweenParams know that the ease was not set and apply it differently if used on Tweeners or Sequences
		Linear,
		CLerp,
		InSine,
		OutSine,
		InOutSine,
		InQuad,
		OutQuad,
		InOutQuad,
		InCubic,
		OutCubic,
		InOutCubic,
		InQuart,
		OutQuart,
		InOutQuart,
		InQuint,
		OutQuint,
		InOutQuint,
		InExpo,
		OutExpo,
		InOutExpo,
		InCirc,
		OutCirc,
		InOutCirc,
		InElastic,
		OutElastic,
		InOutElastic,
		InBack,
		OutBack,
		InOutBack,
		InBounce,
		OutBounce,
		InOutBounce,
		Spring,
		Punch,
		/// <summary>
		/// Don't assign this! It's assigned automatically when creating 0 duration tweens
		/// </summary>
		INTERNAL_Zero,
		/// <summary>
		/// Don't assign this! It's assigned automatically when setting the ease to an AnimationCurve or to a custom ease function
		/// </summary>
		INTERNAL_Custom
	}
	
	public static float EaseConstant (Ease easeType, float time, float duration, float overshootOrAmplitude = 0, float period = 0)
	{
		switch (easeType) 
		{
		case Ease.Linear:
			return time / duration;
		case Ease.InSine:
			return -(float)Math.Cos(time / duration * piOver2) + 1;
		case Ease.OutSine:
			return (float)Math.Sin(time / duration * piOver2);
		case Ease.InOutSine:
			return -0.5f * ((float)Math.Cos(Mathf.PI * time / duration) - 1);
		case Ease.InQuad:
			return (time /= duration) * time;
		case Ease.OutQuad:
			return -(time /= duration) * (time - 2);
		case Ease.InOutQuad:
			if ((time /= duration * 0.5f) < 1) return 0.5f * time * time;
			return -0.5f * ((--time) * (time - 2) - 1);
		case Ease.InCubic:
			return (time /= duration) * time * time;
		case Ease.OutCubic:
			return ((time = time / duration - 1) * time * time + 1);
		case Ease.InOutCubic:
			if ((time /= duration * 0.5f) < 1) return 0.5f * time * time * time;
			return 0.5f * ((time -= 2) * time * time + 2);
		case Ease.InQuart:
			return (time /= duration) * time * time * time;
		case Ease.OutQuart:
			return -((time = time / duration - 1) * time * time * time - 1);
		case Ease.InOutQuart:
			if ((time /= duration * 0.5f) < 1) return 0.5f * time * time * time * time;
			return -0.5f * ((time -= 2) * time * time * time - 2);
		case Ease.InQuint:
			return (time /= duration) * time * time * time * time;
		case Ease.OutQuint:
			return ((time = time / duration - 1) * time * time * time * time + 1);
		case Ease.InOutQuint:
			if ((time /= duration * 0.5f) < 1) return 0.5f * time * time * time * time * time;
			return 0.5f * ((time -= 2) * time * time * time * time + 2);
		case Ease.InExpo:
			return (time == 0) ? 0 : (float)Math.Pow(2, 10 * (time / duration - 1));
		case Ease.OutExpo:
			if (time == duration) return 1;
			return (-(float)Math.Pow(2, -10 * time / duration) + 1);
		case Ease.InOutExpo:
			if (time == 0) return 0;
			if (time == duration) return 1;
			if ((time /= duration * 0.5f) < 1) return 0.5f * (float)Math.Pow(2, 10 * (time - 1));
			return 0.5f * (-(float)Math.Pow(2, -10 * --time) + 2);
		case Ease.InCirc:
			return -((float)Math.Sqrt(1 - (time /= duration) * time) - 1);
		case Ease.OutCirc:
			return (float)Math.Sqrt(1 - (time = time / duration - 1) * time);
		case Ease.InOutCirc:
			if ((time /= duration * 0.5f) < 1) return -0.5f * ((float)Math.Sqrt(1 - time * time) - 1);
			return 0.5f * ((float)Math.Sqrt(1 - (time -= 2) * time) + 1);
		case Ease.InElastic:
			float s0;
			if (time == 0) return 0;
			if ((time /= duration) == 1) return 1;
			if (period == 0) period = duration * 0.3f;
			if (overshootOrAmplitude < 1) {
				overshootOrAmplitude = 1;
				s0 = period / 4;
			} else s0 = period / twoPi * (float)Math.Asin(1 / overshootOrAmplitude);
			return -(overshootOrAmplitude * (float)Math.Pow(2, 10 * (time -= 1)) * (float)Math.Sin((time * duration - s0) * twoPi / period));
		case Ease.OutElastic:
			float s1;
			if (time == 0) return 0;
			if ((time /= duration) == 1) return 1;
			if (period == 0) period = duration * 0.3f;
			if (overshootOrAmplitude < 1) {
				overshootOrAmplitude = 1;
				s1 = period / 4;
			} else s1 = period / twoPi * (float)Math.Asin(1 / overshootOrAmplitude);
			return (overshootOrAmplitude * (float)Math.Pow(2, -10 * time) * (float)Math.Sin((time * duration - s1) * twoPi / period) + 1);
		case Ease.InOutElastic:
			float s;
			if (time == 0) return 0;
			if ((time /= duration * 0.5f) == 2) return 1;
			if (period == 0) period = duration * (0.3f * 1.5f);
			if (overshootOrAmplitude < 1) {
				overshootOrAmplitude = 1;
				s = period / 4;
			} else s = period / twoPi * (float)Math.Asin(1 / overshootOrAmplitude);
			if (time < 1) return -0.5f * (overshootOrAmplitude * (float)Math.Pow(2, 10 * (time -= 1)) * (float)Math.Sin((time * duration - s) * twoPi / period));
			return overshootOrAmplitude * (float)Math.Pow(2, -10 * (time -= 1)) * (float)Math.Sin((time * duration - s) * twoPi / period) * 0.5f + 1;
		case Ease.InBack:
			return (time /= duration) * time * ((overshootOrAmplitude + 1) * time - overshootOrAmplitude);
		case Ease.OutBack:
			return ((time = time / duration - 1) * time * ((overshootOrAmplitude + 1) * time + overshootOrAmplitude) + 1);
		case Ease.InOutBack:
			if ((time /= duration * 0.5f) < 1) return 0.5f * (time * time * (((overshootOrAmplitude *= (1.525f)) + 1) * time - overshootOrAmplitude));
			return 0.5f * ((time -= 2) * time * (((overshootOrAmplitude *= (1.525f)) + 1) * time + overshootOrAmplitude) + 2);
		case Ease.InBounce:
			return Bounce.EaseIn(time, duration, overshootOrAmplitude, period);
		case Ease.OutBounce:
			return Bounce.EaseOut(time, duration, overshootOrAmplitude, period);
		case Ease.InOutBounce:
			return Bounce.EaseInOut(time, duration, overshootOrAmplitude, period);
		default:
			// OutQuad
			return -(time /= duration) * (time - 2);
		}
	}
	
	public static float EaseValue (Ease easeType, float start, float end, float value)
	{
		switch (easeType)
		{
			case Ease.CLerp:
				{
					float min = 0.0f;
					float max = 360.0f;
					float half = Mathf.Abs((max - min) * 0.5f);
					float retval = 0.0f;
					float diff = 0.0f;
					if ((end - start) < -half)
					{
						diff = ((max - start) + end) * value;
						retval = start + diff;
					}
					else if ((end - start) > half)
					{
						diff = -((max - end) + start) * value;
						retval = start + diff;
					}
					else retval = start + (end - start) * value;
					return retval;
				}

			case Ease.InSine:
				end -= start;
				return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
			case Ease.OutSine:
				end -= start;
				return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
			case Ease.InOutSine:
				end -= start;
				return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
			case Ease.InQuad:
				end -= start;
				return end * value * value + start;
			case Ease.OutQuad:
				end -= start;
				return -end * value * (value - 2) + start;
			case Ease.InOutQuad:
				value /= .5f;
				end -= start;
				if (value < 1) return end * 0.5f * value * value + start;
				value--;
				return -end * 0.5f * (value * (value - 2) - 1) + start;
			case Ease.InCubic:
				end -= start;
				return end * value * value * value + start;
			case Ease.OutCubic:
				value--;
				end -= start;
				return end * (value * value * value + 1) + start;
			case Ease.InOutCubic:
				value /= .5f;
				end -= start;
				if (value < 1) return end * 0.5f * value * value * value + start;
				value -= 2;
				return end * 0.5f * (value * value * value + 2) + start;
			case Ease.InQuart:
				end -= start;
				return end * value * value * value * value + start;
			case Ease.OutQuart:
				value--;
				end -= start;
				return -end * (value * value * value * value - 1) + start;
			case Ease.InOutQuart:
				value /= .5f;
				end -= start;
				if (value < 1) return end * 0.5f * value * value * value * value + start;
				value -= 2;
				return -end * 0.5f * (value * value * value * value - 2) + start;
			case Ease.InQuint:
				end -= start;
				return end * value * value * value * value * value + start;
			case Ease.OutQuint:
				value--;
				end -= start;
				return end * (value * value * value * value * value + 1) + start;
			case Ease.InOutQuint:
				value /= .5f;
				end -= start;
				if (value < 1) return end * 0.5f * value * value * value * value * value + start;
				value -= 2;
				return end * 0.5f * (value * value * value * value * value + 2) + start;
			case Ease.InExpo:
				end -= start;
				return end * Mathf.Pow(2, 10 * (value - 1)) + start;
			case Ease.OutExpo:
				end -= start;
				return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
			case Ease.InOutExpo:
				{
					value /= .5f;
					end -= start;
					if (value < 1) return end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;
					value--;
					return end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;
				}
			case Ease.InElastic:
				{
					end -= start;

					float d = 1f;
					float p = d * .3f;
					float s = 0;
					float a = 0;

					if (value == 0) return start;

					if ((value /= d) == 1) return start + end;

					if (a == 0f || a < Mathf.Abs(end))
					{
						a = end;
						s = p / 4;
					}
					else
					{
						s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
					}

					return -(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
				}
			case Ease.OutElastic:
				{
					end -= start;

					float d = 1f;
					float p = d * .3f;
					float s = 0;
					float a = 0;

					if (value == 0) return start;

					if ((value /= d) == 1) return start + end;

					if (a == 0f || a < Mathf.Abs(end))
					{
						a = end;
						s = p * 0.25f;
					}
					else
					{
						s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
					}

					return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
				}
			case Ease.InOutElastic:
				{
					end -= start;

					float d = 1f;
					float p = d * .3f;
					float s = 0;
					float a = 0;

					if (value == 0) return start;

					if ((value /= d * 0.5f) == 2) return start + end;

					if (a == 0f || a < Mathf.Abs(end))
					{
						a = end;
						s = p / 4;
					}
					else
					{
						s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
					}

					if (value < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
					return a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
				}
			case Ease.InCirc:
				end -= start;
				return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
			case Ease.OutCirc:
				value--;
				end -= start;
				return end * Mathf.Sqrt(1 - value * value) + start;
			case Ease.InOutCirc:
				value /= .5f;
				end -= start;
				if (value < 1) return -end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start;
				value -= 2;
				return end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start;
			case Ease.InBack:
				{
					end -= start;
					value /= 1;
					float s = 1.70158f;
					return end * (value) * value * ((s + 1) * value - s) + start;
				}
			case Ease.OutBack:
				{
					float s = 1.70158f;
					end -= start;
					value = (value) - 1;
					return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
				}
			case Ease.InOutBack:
				{
					float s = 1.70158f;
					end -= start;
					value /= .5f;
					if ((value) < 1)
					{
						s *= (1.525f);
						return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
					}
					value -= 2;
					s *= (1.525f);
					return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
				}
			case Ease.InBounce:
				{
					end -= start;
					float d = 1f;
					return end - easeOutBounce(0, end, d - value) + start;
				}
			case Ease.OutBounce:
				{
					value /= 1f;
					end -= start;
					if (value < (1 / 2.75f))
					{
						return end * (7.5625f * value * value) + start;
					}
					else if (value < (2 / 2.75f))
					{
						value -= (1.5f / 2.75f);
						return end * (7.5625f * (value) * value + .75f) + start;
					}
					else if (value < (2.5 / 2.75))
					{
						value -= (2.25f / 2.75f);
						return end * (7.5625f * (value) * value + .9375f) + start;
					}
					else
					{
						value -= (2.625f / 2.75f);
						return end * (7.5625f * (value) * value + .984375f) + start;
					}
				}
			case Ease.InOutBounce:
				{
					end -= start;
					float d = 1f;
					if (value < d * 0.5f) return easeInBounce(0, end, value * 2) * 0.5f + start;
					else return easeOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
				}
			case Ease.Spring:
				value = Mathf.Clamp01(value);
				value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
				return start + (end - start) * value;
			case Ease.Punch:
				{
					float c = 9;
					if (value == 0)
					{
						return 0;
					}
					else if (value == 1)
					{
						return 0;
					}
					float period = 1 * 0.3f;
					c = period / (2 * Mathf.PI) * Mathf.Asin(0);
					return (end * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 1 - c) * (2 * Mathf.PI) / period));
				}
			case Ease.Linear:
				return Mathf.Lerp(start, end, value);
			default:
				return Mathf.Lerp(start, end, value);
		}
	}

	private static float easeOutBounce(float start, float end, float value){
		value /= 1f;
		end -= start;
		if (value < (1 / 2.75f)){
			return end * (7.5625f * value * value) + start;
		}else if (value < (2 / 2.75f)){
			value -= (1.5f / 2.75f);
			return end * (7.5625f * (value) * value + .75f) + start;
		}else if (value < (2.5 / 2.75)){
			value -= (2.25f / 2.75f);
			return end * (7.5625f * (value) * value + .9375f) + start;
		}else{
			value -= (2.625f / 2.75f);
			return end * (7.5625f * (value) * value + .984375f) + start;
		}
	}

	private static float easeInBounce(float start, float end, float value){
		end -= start;
		float d = 1f;
		return end - easeOutBounce(0, end, d-value) + start;
	}
}

public static class Bounce
{
	/// <summary>
	/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in: accelerating from zero velocity.
	/// </summary>
	/// <param name="time">
	/// Current time (in frames or seconds).
	/// </param>
	/// <param name="duration">
	/// Expected easing duration (in frames or seconds).
	/// </param>
	/// <param name="unusedOvershootOrAmplitude">Unused: here to keep same delegate for all ease types.</param>
	/// <param name="unusedPeriod">Unused: here to keep same delegate for all ease types.</param>
	/// <returns>
	/// The eased value.
	/// </returns>
	public static float EaseIn(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
	{
		return 1 - EaseOut(duration - time, duration, -1, -1);
	}
	
	/// <summary>
	/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out: decelerating from zero velocity.
	/// </summary>
	/// <param name="time">
	/// Current time (in frames or seconds).
	/// </param>
	/// <param name="duration">
	/// Expected easing duration (in frames or seconds).
	/// </param>
	/// <param name="unusedOvershootOrAmplitude">Unused: here to keep same delegate for all ease types.</param>
	/// <param name="unusedPeriod">Unused: here to keep same delegate for all ease types.</param>
	/// <returns>
	/// The eased value.
	/// </returns>
	public static float EaseOut(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
	{
		if ((time /= duration) < (1 / 2.75f)) {
			return (7.5625f * time * time);
		}
		if (time < (2 / 2.75f)) {
			return (7.5625f * (time -= (1.5f / 2.75f)) * time + 0.75f);
		}
		if (time < (2.5f / 2.75f)) {
			return (7.5625f * (time -= (2.25f / 2.75f)) * time + 0.9375f);
		}
		return (7.5625f * (time -= (2.625f / 2.75f)) * time + 0.984375f);
	}
	
	/// <summary>
	/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out: acceleration until halfway, then deceleration.
	/// </summary>
	/// <param name="time">
	/// Current time (in frames or seconds).
	/// </param>
	/// <param name="duration">
	/// Expected easing duration (in frames or seconds).
	/// </param>
	/// <param name="unusedOvershootOrAmplitude">Unused: here to keep same delegate for all ease types.</param>
	/// <param name="unusedPeriod">Unused: here to keep same delegate for all ease types.</param>
	/// <returns>
	/// The eased value.
	/// </returns>
	public static float EaseInOut(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
	{
		if (time < duration*0.5f)
		{
			return EaseIn(time*2, duration, -1, -1)*0.5f;
		}
		return EaseOut(time*2 - duration, duration, -1, -1)*0.5f + 0.5f;
	}
}

public class Temp
{
	#region Easing Curves
	
	private float linear(float start, float end, float value){
		return Mathf.Lerp(start, end, value);
	}
	
	private float clerp(float start, float end, float value){
		float min = 0.0f;
		float max = 360.0f;
		float half = Mathf.Abs((max - min) * 0.5f);
		float retval = 0.0f;
		float diff = 0.0f;
		if ((end - start) < -half){
			diff = ((max - start) + end) * value;
			retval = start + diff;
		}else if ((end - start) > half){
			diff = -((max - end) + start) * value;
			retval = start + diff;
		}else retval = start + (end - start) * value;
		return retval;
	}
	
	private float spring(float start, float end, float value){
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
		return start + (end - start) * value;
	}
	
	private float easeInQuad(float start, float end, float value){
		end -= start;
		return end * value * value + start;
	}
	
	private float easeOutQuad(float start, float end, float value){
		end -= start;
		return -end * value * (value - 2) + start;
	}
	
	private float easeInOutQuad(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return end * 0.5f * value * value + start;
		value--;
		return -end * 0.5f * (value * (value - 2) - 1) + start;
	}
	
	private float easeInCubic(float start, float end, float value){
		end -= start;
		return end * value * value * value + start;
	}
	
	private float easeOutCubic(float start, float end, float value){
		value--;
		end -= start;
		return end * (value * value * value + 1) + start;
	}
	
	private float easeInOutCubic(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return end * 0.5f * value * value * value + start;
		value -= 2;
		return end * 0.5f * (value * value * value + 2) + start;
	}
	
	private float easeInQuart(float start, float end, float value){
		end -= start;
		return end * value * value * value * value + start;
	}
	
	private float easeOutQuart(float start, float end, float value){
		value--;
		end -= start;
		return -end * (value * value * value * value - 1) + start;
	}
	
	private float easeInOutQuart(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return end * 0.5f * value * value * value * value + start;
		value -= 2;
		return -end * 0.5f * (value * value * value * value - 2) + start;
	}
	
	private float easeInQuint(float start, float end, float value){
		end -= start;
		return end * value * value * value * value * value + start;
	}
	
	private float easeOutQuint(float start, float end, float value){
		value--;
		end -= start;
		return end * (value * value * value * value * value + 1) + start;
	}
	
	private float easeInOutQuint(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return end * 0.5f * value * value * value * value * value + start;
		value -= 2;
		return end * 0.5f * (value * value * value * value * value + 2) + start;
	}
	
	private float easeInSine(float start, float end, float value){
		end -= start;
		return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
	}
	
	private float easeOutSine(float start, float end, float value){
		end -= start;
		return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
	}
	
	private float easeInOutSine(float start, float end, float value){
		end -= start;
		return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
	}
	
	private float easeInExpo(float start, float end, float value){
		end -= start;
		return end * Mathf.Pow(2, 10 * (value - 1)) + start;
	}
	
	private float easeOutExpo(float start, float end, float value){
		end -= start;
		return end * (-Mathf.Pow(2, -10 * value ) + 1) + start;
	}
	
	private float easeInOutExpo(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;
		value--;
		return end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;
	}
	
	private float easeInCirc(float start, float end, float value){
		end -= start;
		return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
	}
	
	private float easeOutCirc(float start, float end, float value){
		value--;
		end -= start;
		return end * Mathf.Sqrt(1 - value * value) + start;
	}
	
	private float easeInOutCirc(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return -end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start;
		value -= 2;
		return end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start;
	}
	
	/* GFX47 MOD START */
	private float easeInBounce(float start, float end, float value){
		end -= start;
		float d = 1f;
		return end - easeOutBounce(0, end, d-value) + start;
	}
	/* GFX47 MOD END */
	
	/* GFX47 MOD START */
	//private float bounce(float start, float end, float value){
	private static float easeOutBounce(float start, float end, float value){
		value /= 1f;
		end -= start;
		if (value < (1 / 2.75f)){
			return end * (7.5625f * value * value) + start;
		}else if (value < (2 / 2.75f)){
			value -= (1.5f / 2.75f);
			return end * (7.5625f * (value) * value + .75f) + start;
		}else if (value < (2.5 / 2.75)){
			value -= (2.25f / 2.75f);
			return end * (7.5625f * (value) * value + .9375f) + start;
		}else{
			value -= (2.625f / 2.75f);
			return end * (7.5625f * (value) * value + .984375f) + start;
		}
	}
	/* GFX47 MOD END */
	
	/* GFX47 MOD START */
	private float easeInOutBounce(float start, float end, float value){
		end -= start;
		float d = 1f;
		if (value < d* 0.5f) return easeInBounce(0, end, value*2) * 0.5f + start;
		else return easeOutBounce(0, end, value*2-d) * 0.5f + end*0.5f + start;
	}
	/* GFX47 MOD END */
	
	private float easeInBack(float start, float end, float value){
		end -= start;
		value /= 1;
		float s = 1.70158f;
		return end * (value) * value * ((s + 1) * value - s) + start;
	}
	
	private float easeOutBack(float start, float end, float value){
		float s = 1.70158f;
		end -= start;
		value = (value) - 1;
		return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
	}
	
	private float easeInOutBack(float start, float end, float value){
		float s = 1.70158f;
		end -= start;
		value /= .5f;
		if ((value) < 1){
			s *= (1.525f);
			return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
		}
		value -= 2;
		s *= (1.525f);
		return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
	}
	
	private float punch(float amplitude, float value){
		float s = 9;
		if (value == 0){
			return 0;
		}
		else if (value == 1){
			return 0;
		}
		float period = 1 * 0.3f;
		s = period / (2 * Mathf.PI) * Mathf.Asin(0);
		return (amplitude * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 1 - s) * (2 * Mathf.PI) / period));
	}
	
	/* GFX47 MOD START */
	private float easeInElastic(float start, float end, float value){
		end -= start;
		
		float d = 1f;
		float p = d * .3f;
		float s = 0;
		float a = 0;
		
		if (value == 0) return start;
		
		if ((value /= d) == 1) return start + end;
		
		if (a == 0f || a < Mathf.Abs(end)){
			a = end;
			s = p / 4;
		}else{
			s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
		}
		
		return -(a * Mathf.Pow(2, 10 * (value-=1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
	}		
	/* GFX47 MOD END */
	
	/* GFX47 MOD START */
	//private float elastic(float start, float end, float value){
	private float easeOutElastic(float start, float end, float value){
		/* GFX47 MOD END */
		//Thank you to rafael.marteleto for fixing this as a port over from Pedro's UnityTween
		end -= start;
		
		float d = 1f;
		float p = d * .3f;
		float s = 0;
		float a = 0;
		
		if (value == 0) return start;
		
		if ((value /= d) == 1) return start + end;
		
		if (a == 0f || a < Mathf.Abs(end)){
			a = end;
			s = p * 0.25f;
		}else{
			s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
		}
		
		return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
	}		
	
	/* GFX47 MOD START */
	private float easeInOutElastic(float start, float end, float value){
		end -= start;
		
		float d = 1f;
		float p = d * .3f;
		float s = 0;
		float a = 0;
		
		if (value == 0) return start;
		
		if ((value /= d*0.5f) == 2) return start + end;
		
		if (a == 0f || a < Mathf.Abs(end)){
			a = end;
			s = p / 4;
		}else{
			s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
		}
		
		if (value < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (value-=1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
		return a * Mathf.Pow(2, -10 * (value-=1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
	}		
	/* GFX47 MOD END */
	
	#endregion	
}