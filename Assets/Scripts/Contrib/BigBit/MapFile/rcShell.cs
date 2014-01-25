using UnityEngine;
using System.Diagnostics;

// Pretty much taken from here: http://wiki.unity3d.com/index.php?title=Shell with the addition of taking aworking directory -cm
public class rcShell 
{	
	static Process StartShellProcess(string filename, string args, string workingDir = null)
	{
		var p = new Process();
		p.StartInfo.CreateNoWindow = true;
		p.StartInfo.UseShellExecute = false;
		p.StartInfo.RedirectStandardOutput = true;
		p.StartInfo.RedirectStandardInput = true;
		p.StartInfo.RedirectStandardError = true;
		if(!string.IsNullOrEmpty(workingDir))
			p.StartInfo.WorkingDirectory = workingDir;
		p.StartInfo.FileName = filename;
		p.StartInfo.Arguments = args;
		p.Start();
		return p;
	}
	
	public static string Execute( string filename, string args, string workingDir = null)
	{
		Process p = StartShellProcess(filename, args, workingDir);
		string output = p.StandardOutput.ReadToEnd();
		p.WaitForExit();
		return output;
	}
}
