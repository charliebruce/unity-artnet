using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ArtNet : MonoBehaviour
{

    private byte sequence = 0;
    private IPEndPoint ep;
    private UdpClient client;
    private double lastUpdate = 0.0;

    // Start is called before the first frame update
    void Start()
    {
        client = new UdpClient();
        client.EnableBroadcast = true;
        ep = new IPEndPoint(IPAddress.Broadcast, 6454);

        Debug.Log("ArtNet starting...");
        lastUpdate = Time.fixedTimeAsDouble;

        byte[] poll = ArtNetPacket.PollPacket();
        client.SendAsync(poll, poll.Length, ep);
    }

    // Update is called once per frame
    void Update()
    {
        // Limit the ArtNet framerate to 40 Hz
        double now = Time.fixedTimeAsDouble;
        if((now - lastUpdate) < (1.0 / 40.0)) {
            return;
        }
        lastUpdate = now;

        // Increment the sequence value by 1 each time. Note that 0 is not a valid value (used to indicate that sequence is unused)
        sequence++;
        if (sequence == 0)
            sequence++;

        byte[] channels = { 255, sequence, sequence, sequence };

        byte[] pkt = ArtNetPacket.DmxPacket(channels, 0, sequence);

        // The transmission is over UDP, and host PC should be able to deal with 530 bytes at a rate of 40/sec.
        client.SendAsync(pkt, pkt.Length, ep);

    }
}
