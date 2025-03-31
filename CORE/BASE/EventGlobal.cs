using System;
using System.Collections.Generic;
using System.Linq;

namespace sELedit.CORE.BASE
{
	public class EventGlobal
	{
		private static readonly Dictionary<Type, List<Delegate>> subscriptions = new Dictionary<Type, List<Delegate>>();

		public static void Subscribe<TEvent>(Action<TEvent> handler)
		{
			if (!subscriptions.ContainsKey(typeof(TEvent)))
			{
				subscriptions[typeof(TEvent)] = new List<Delegate>();
			}
			subscriptions[typeof(TEvent)].Add(handler);
		}
		public static void Publish<TEvent>(TEvent evengArgs)
		{
			if (subscriptions.ContainsKey(typeof(TEvent)))
			{
				foreach (var handler in subscriptions[typeof(TEvent)].Cast<Action<TEvent>>())
				{
					handler?.Invoke(evengArgs);
				}
			}
		}
		public static void Unsubscribe<TEvent>(Action<TEvent> handler)
		{
			if (subscriptions.ContainsKey(typeof(TEvent)))
			{
				subscriptions[typeof(TEvent)].Remove(handler);


				if (!subscriptions[typeof(TEvent)].Any())
				{
					subscriptions.Remove(typeof(TEvent));
				}
			}
		}
	}
}
