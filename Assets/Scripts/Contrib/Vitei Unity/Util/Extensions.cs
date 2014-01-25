using UnityEngine;
using System.Collections;

public static class Extensions
{
    public static Transform FindChildRecursive(this Transform target, string name)
    {
        if (target.name == name) 
			return target;
 
        for (int i = 0; i < target.childCount; ++i)
        {
			Transform result = FindChildRecursive(target.GetChild(i), name);
            if (result != null) 
				return result;
        }
        return null;
    }
	
	public static bool IsChildOf(this GameObject go, System.Type componentType)
    {
		return go.transform.IsChildOf(componentType);
	}
	public static bool IsChildOf(this Transform target, System.Type componentType)
    {
		if(target.parent == null)
			return false;
		else if (target.parent.GetComponent( componentType ) )
			return true;
		else
			return target.parent.IsChildOf(componentType);
    }
	    
	public static void SetLayerRecursive(this GameObject target, int layer)
    {
		Transform[] trs = target.GetComponentsInChildren<Transform>();	
		for(int i = 0; i < trs.Length; i++)

        {
            trs[i].gameObject.layer = layer;
        }
    }
	
	public static Vector3 GetCenter(this GameObject me)
    {
		Vector3 c = Vector3.zero;
		
		Renderer[] rs = me.GetComponentsInChildren<Renderer>();
		if(rs.Length > 0)
		{				
			for(int i = 0; i < rs.Length; i++)
			{
				c += rs[i].transform.position;
			}
			
			c/= (float)rs.Length;
		}
		else
		{
			c = me.transform.position;
		}
		
		return c;
    }
	
	public static Rect Expand(this Rect r, Vector2 amt)
	{
		r.x -= amt.x * 0.5f;
		r.y -= amt.y * 0.5f;
		r.width += amt.x;
		r.height += amt.y;
		return r;
	}

	// Return a random vector in the range ( (-x -> x), (-y -> y), (-z -> z) )
	public static Vector3 Randomized(this Vector3 v)
	{
		return new Vector3(
			Random.Range(-v.x, v.x),
			Random.Range(-v.y, v.y),
			Random.Range(-v.z, v.z));
	}
}