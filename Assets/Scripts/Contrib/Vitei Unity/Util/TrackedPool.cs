using System;
using System.Collections.Generic;

[System.Serializable]
public class TrackedPool<T>
{
	public List<T> m_available { get; private set; }
	public List<T> m_unavailable { get; private set; }
	Random m_random;
	
	public int AvailableCount { get { return m_available.Count; } }
	public int UnavailableCount { get { return m_unavailable.Count; } }
	public int TotalCount { get { return (m_available.Count + m_unavailable.Count); } }
	
	public TrackedPool(int startingCapacity = 0)
	{
		m_available = new List<T>(startingCapacity);
		m_unavailable = new List<T>(startingCapacity);
		m_random = new Random();
	}
	
	public TrackedPool(T[] startingArray)
	{
		m_available = new List<T>(startingArray);
		m_unavailable = new List<T>(startingArray.Length);
		m_random = new Random();
	}
	
	public TrackedPool(List<T> startingList)
	{
		m_available = new List<T>(startingList);
		m_unavailable = new List<T>(startingList.Count);
		m_random = new Random();
	}
	
	public void Add(T item)
	{
		m_available.Add(item);
	}

	public void Add(List<T> items)
	{
		for(int i = 0; i < items.Count; i++)
			m_available.Add(items[i]);
	}
	
	public void Remove(T item)
	{
		m_available.Remove(item);
		m_unavailable.Remove(item);
	}
	
	public T GetNextAvailable()
	{
		if(m_available.Count == 0)
			return default(T);
		
		T a = m_available[ 0 ];
		m_available.Remove(a);
		m_unavailable.Add(a);
		return a;
	}
	
	public T GetRandomAvailable()
	{
		if(m_available.Count == 0)
			return default(T);
		
		T a = m_available[ m_random.Next(0, m_available.Count) ];
		m_available.Remove(a);
		m_unavailable.Add(a);
		return a;
	}
	
	public void SetUnavailable(T item)
	{
		if(m_available.Contains(item))
		{
			m_unavailable.Add(item);
			m_available.Remove(item);
		}
		else
		{
			throw new SystemException("Trying to use item that doesn't belong in this TrackedPool");
		}
	}
	
	public void SetAvailable(T u)
	{
		if(m_unavailable.Contains(u))
		{
			m_unavailable.Remove(u);
			m_available.Add(u);
		}
		else
		{
			throw new SystemException("Trying to return item that doesn't belong in this TrackedPool");
		}
	}
	
	public void SetAllAvailable()
	{
		for(int i = 0; i < m_unavailable.Count; i++)
		{
			m_available.Add(m_unavailable[i]);
		}
		m_unavailable.Clear();
	}
}
