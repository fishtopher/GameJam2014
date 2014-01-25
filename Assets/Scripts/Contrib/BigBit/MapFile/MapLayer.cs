using UnityEngine;
using System;
using System.IO;

[System.Serializable]
public class MapLayer 
{
	public String layerName;
	public bool checkedOut;
	public String user;
	private GameObject root;

    public MapLayer()
    {
        layerName = "";
        checkedOut = false;
        user = "";
        root = null;
    }

	public MapLayer(String n)
	{
		layerName = n;
		checkedOut = false;
		user = "";
		root = GetRoot();
	}

    public MapLayer(String n, GameObject rootGameObject)
    {
        layerName = n;
        checkedOut = false;
        user = "";
        root = rootGameObject;
    }

    public bool IsRootValid()
	{
		return root != null;
	}
	
	public GameObject GetRoot()
	{
		if(root == null)
		{
			root = GameObject.Find(layerName);
		}
		
		return root;
	}
	
	String GetLockfile()
	{
		String lockfile = MapData.GetVcsPath(layerName);
		return lockfile;
	}
	
	public void Refresh()
	{
		user = null;
		String lockfile = GetLockfile();
		if(File.Exists(lockfile)) 
		{
			checkedOut = true;
			using (StreamReader sr = File.OpenText(lockfile)) 
	        {
				user = sr.ReadLine();
	        }
		}
		else
		{
			checkedOut = false;
			user = null;
		}
	}

    public String CheckOut()
	{
        String result = null;
        
        Refresh();
		
		if(!checkedOut)
		{
			String lockfile = GetLockfile();

            try
            {
                using (StreamWriter sw = File.CreateText(lockfile))
                {
                    sw.WriteLine(MapData.userName);
                }

				// Make sure everyone and anyone can edit the file if they have to
				rcShell.Execute("chmod", "777 " + lockfile);

                checkedOut = true;
                user = MapData.userName;

                //V MapData.Log("Locking: ", layerName, user);
            }
            catch (Exception error)
            {
                MapData.Log("Error Locking: ", layerName, user);
                result = error.Message;
            }
		}
		else
		{
			MapData.Log("Lock Failed: ", layerName, user);
		}

        return result;
	}
	
	public String CheckIn()
	{
        String result = null;

		Refresh();
		
		if(checkedOut && user == MapData.userName)
		{
			String lockfile = GetLockfile();

            try
            {
                File.Delete(lockfile);

                checkedOut = false;
                user = null;

				//V MapData.Log("Unlocked: ", layerName, MapData.userName);
            }
            catch (Exception error)
            {
				MapData.Log("Error Unlocking: ", layerName, MapData.userName);
                result = error.Message;
            }
		}
        return result;
	}
	
	public void OnDestroy()
	{
		Debug.Log("MapLayer.OnDestroy()");	
	}
	
};

