using UnityEngine;
using ChatSharp;
using System;

public class TwitchController {

	static IrcClient client;
	static bool isConnected = false;

	public static Action<string, string> onMessageReceived;

	public static void ConnectToTwitchChat(string room) {

		// NOTE: Will be unable to support Twitch chat in web player.  Grr....

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
			client.JoinChannel("#"+room);
		};
		client.UserJoinedChannel += (sender, e) => {
			isConnected = true;
			client.SendMessage("MAN vs TWITCH! Prepare yourselves to battle your streamer in the arena!", "#"+room);
		};

		client.ConnectAsync ();
	}
	
}
