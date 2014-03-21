using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private const string typeName = "ProjectBlobBattle";
	private const string gameName = "BLOB!!!";
	private const int port = 25000;
	private string ipAdress = "0.0.0.0";
	public string nickname = "MyName";
    private Vector2 scrollViewVector = Vector2.zero;
	
	private void StartServer()
	{
		Network.InitializeServer(8, port, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}

	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			ipAdress = GUI.TextField (new Rect (160, 150, 100, 30), ipAdress);

			nickname = GUI.TextField (new Rect (200, 100, 100, 30), nickname);

			GUI.Label(new Rect(200,60,100,30),"Nickname");

			if (GUI.Button(new Rect(100, 100, 100, 30), "Start Server"))
				StartServer();

			if (GUI.Button(new Rect(50, 150, 100, 30), "Connect To Ip"))
				Network.Connect(ipAdress,port);

			if (GUI.Button(new Rect(300, 100, 100, 30), "Refresh Rooms"))
				RefreshHostList();

			scrollViewVector = GUI.BeginScrollView (new Rect (250, 140, 100, 100), scrollViewVector, new Rect (0, 0, 400, 400));

			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(10, 10 + (40 * i), 100, 30), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}

			GUI.EndScrollView();
		}
	}

	private HostData[] hostList;
	
	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
}
