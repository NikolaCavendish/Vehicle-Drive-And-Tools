using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager 
{
	private EventManager() { }
	private static EventManager instance;
	public static EventManager Instance
	{
		get
		{
			if (instance == null)
				instance = new EventManager();
			return instance;
		}
	}

	/// <summary>
	/// 委托字典
	/// </summary>
	private Dictionary<string, IEvent> eventMap = new Dictionary<string, IEvent>();

	/// <summary>
	/// 委托接口与基础委托类型
	/// </summary>
	private interface IEvent {  }
	private class BaseEvent:IEvent
	{
		public Action OnEvent;
	}
	private class BaseEvent<T>:IEvent
	{
		public Action<T> OnEvent;
	}

	/// <summary>
	/// 注册事件
	/// </summary>
	/// <param name="name"></param>
	/// <param name="onEvent"></param>
	public void Register(string name,Action onEvent)
	{
		if (eventMap.TryGetValue(name, out IEvent e))
		{
			(e as BaseEvent).OnEvent += onEvent;
		}
		else
			eventMap.Add(name, new BaseEvent() { OnEvent = onEvent });
	}

	public void Register<T>(string name,Action<T> onEvent)
	{
		if (eventMap.TryGetValue(name, out IEvent e))
		{
			(e as BaseEvent<T>).OnEvent += onEvent;
		}
		else
			eventMap.Add(name, new BaseEvent<T>() { OnEvent = onEvent });
	}

	/// <summary>
	/// 注销事件
	/// </summary>
	/// <param name="name"></param>
	/// <param name="onEvent"></param>
	public void UnRegister(string name, Action onEvent)
	{
		if (!eventMap.TryGetValue(name, out IEvent e))
		{
			return;
		}
		(e as BaseEvent).OnEvent -= onEvent;

	}

	public void UnRegister<T>(string name, Action<T> onEvent)
	{
		if (!eventMap.TryGetValue(name, out IEvent e))
		{
			return;
		}
		(e as BaseEvent<T>).OnEvent -= onEvent;
	}

	/// <summary>
	/// 触发方法
	/// </summary>
	/// <param name="name"></param>
	/// <param name="onEvent"></param>
	public void Trigger(string name)
	{
		if (!eventMap.TryGetValue(name, out IEvent e))
		{
			return;
		}
		(e as BaseEvent).OnEvent?.Invoke();

	}

	public void Trigger<T>(string name, T data)
	{
		if (!eventMap.TryGetValue(name, out IEvent e))
		{
			return;
		}
		(e as BaseEvent<T>).OnEvent?.Invoke(data);
	}
}
