using System;
using System.Collections.Generic;
using _Framework.Singleton;
using UnityEngine;

namespace _Framework.Event.Scripts
{
    public class EventManager : Singleton<EventManager>
    {
        #region Fields

        private Dictionary<EventID, Action<object>> _listeners = new Dictionary<EventID, Action<object>>();
		
		#endregion
		
		#region Add Listeners, Post events, Remove listener
		
		public void RegisterListener (EventID eventID, Action<object> callback)
		{
			if (_listeners.ContainsKey(eventID))
			{
				_listeners[eventID] += callback;
			}
			else
			{
				_listeners.Add(eventID, null);
				_listeners[eventID] += callback;
			}
		}
		
		public void PostEvent (EventID eventID, object param = null)
		{
			if (!_listeners.ContainsKey(eventID))
			{
				Common.Log("No listeners for this event : {0}", eventID);
				return;
			}

			var callbacks = _listeners[eventID];
			
			if (callbacks != null)
			{
				callbacks(param);
			}
			else
			{
				Common.Log("PostEvent {0}, but no listener remain, Remove this key", eventID);
				_listeners.Remove(eventID);
			}
		}
		
		public void RemoveListener (EventID eventID, Action<object> callback)
		{
			Common.Assert(callback != null, "RemoveListener, event {0}, callback = null !!", eventID.ToString());
			Common.Assert(eventID != EventID.None, "AddListener, event = None !!");

			if (_listeners.ContainsKey(eventID))
			{
				_listeners[eventID] -= callback;
			}
			else
			{
				Common.Warning(false, "RemoveListener, not found key : " + eventID);
			}
		}
		
		public void ClearAllListener ()
		{
			_listeners.Clear();
		}

		#endregion
	}


	#region Extension class
	// This is "shortcut" for using EventDispatcher easier
	public static class EventDispatcher
	{
		public static void RegisterListener (this MonoBehaviour listener, EventID eventID, Action<object> callback)
		{
			EventManager.Instance.RegisterListener(eventID, callback);
		}
		
		public static void PostEvent (this MonoBehaviour listener, EventID eventID, object param)
		{
			EventManager.Instance.PostEvent(eventID, param);
		}
		
		public static void PostEvent (this MonoBehaviour sender, EventID eventID)
		{
			EventManager.Instance.PostEvent(eventID, null);
		}
		
		public static void RemoveListener (this MonoBehaviour listener, EventID eventID, Action<object> callback)
		{
			EventManager.Instance.RemoveListener(eventID, callback);
		}
		
		public static void ClearAllListener (this MonoBehaviour listener)
		{
			EventManager.Instance.ClearAllListener();
		}
	}
	
	#endregion
}
