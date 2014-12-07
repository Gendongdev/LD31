using UnityEngine;
using ChatSharp;
using System;
using System.Threading;

public class TwitchController {

	static IrcClient client;
	static string connectedRoom;

	public static bool isConnected = false;
	public static Action<string, string> onMessageReceived;
	public static Action onConnectedToServer;

	public static void ConnectToTwitchChat(string room) {

		// NOTE: Will be unable to support Twitch chat in web player.  Grr....

		Debug.Log ("Attempting to connect to twitch channel: " + room);
		client = new IrcClient ("irc.twitch.tv:6667", new IrcUser ("ld31_manvstwitch", "ld31_manvstwitch", "oauth:nvxf2uriculudnyvieo8lgvghbivtuq"));
		client.NetworkError += (s, e) => Debug.Log ("Error: " + e.SocketError);
		client.RawMessageRecieved += (s, e) => Debug.Log ("<< " + e.Message);
		client.RawMessageSent += (s, e) => Debug.Log (">> " + e.Message);
		client.ChannelMessageRecieved += (s, e) => {
			if(onMessageReceived != null){
				PlanetUnityGameObject.ScheduleTask(new Task(delegate
					{
						onMessageReceived(e.PrivateMessage.User.Nick, e.PrivateMessage.Message);
					}));
			}
		};
		client.ConnectionComplete += (s, e) =>  {
			Debug.Log("Connected to Twitch IRC");
			connectedRoom = room;
			client.JoinChannel("#"+room);
		};
		client.UserJoinedChannel += (sender, e) => {
			if(!isConnected){
				if(onConnectedToServer != null){
					PlanetUnityGameObject.ScheduleTask(new Task(delegate
						{
							onConnectedToServer();
						}));
				}
			}
			isConnected = true;
		};

		isConnected = false;

		client.ConnectAsync ();
	}

	public static void SendRoomMessage(string msg) {
		client.SendMessage(msg, "#"+connectedRoom);
	}






	public static void DemoOnThread() {

		System.Random random = new System.Random ();

		int sleepTimeMax = 2500;

		while (true) {

			string[] comments = {
				"Someone pass the !chicken?",
				"How does a !chicken bake a cake? ",
				"!chicken",
				"!trap",
				"!knife",
				"!boulder",
				"I really want some !chicken for dinner!",
				"I feel bad for you, have some !rageaid.",
			};

			string[] people = {
				"CuriousBystander",
				"InterruptiveCow",
				"FeralCat",
				"LazyDeveloper",
				"LuckyMusician",
				"ShinyNight",
				"TrickySandwhich",
			};
				
			int c = random.Next() % comments.Length;
			int p = random.Next() % people.Length;
			SimulateUserComment (people [p], comments [c]);

			Thread.Sleep(random.Next() % sleepTimeMax + 50);

			sleepTimeMax -= 30;
			if (sleepTimeMax < 500) {
				sleepTimeMax = 500;
			}
		}
	}

	public static void SimulateUserComment(string name, string msg) {
		PlanetUnityGameObject.ScheduleTask(new Task(delegate
			{
				onMessageReceived(name, msg);
			}));
	}

	public static void BeginDemoPlay() {
		// This method will simiulate a IRC population sending obstacle commands...
		Thread t = new Thread(new ThreadStart(DemoOnThread));
		t.Start();

	}
	
}
