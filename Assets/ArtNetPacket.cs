using System;
using UnityEngine;

public class ArtNetPacket
{

    public static byte[] DmxPacket(byte[] channels, int universe, byte seq)
    {

        byte[] data = new byte[512 + 18];

        if (channels.Length < 2)
        {
            Debug.LogWarning("Must have at least 2 channels");
            return data;
        }

        if (channels.Length > 512)
        {
            Debug.LogWarning("May not have more than 512 channels");
            return data;
        }

        byte physical = 0;

        data[0] = (byte)'A';
        data[1] = (byte)'r';
        data[2] = (byte)'t';
        data[3] = (byte)'-';
        data[4] = (byte)'N';
        data[5] = (byte)'e';
        data[6] = (byte)'t';
        data[7] = 0;
        data[8] = 0x00; // Opcode low
        data[9] = 0x50; // Opcode high

        data[10] = 0;
        data[11] = 14;

        data[12] = seq;
        data[13] = physical;

        data[14] = ((byte)(universe & 0xFF));
        data[15] = ((byte)((universe >> 8) & 0xFF));

        data[16] = ((byte)(channels.Length >> 8));
        data[17] = ((byte)(channels.Length & 0xFF));

        for (int i = 0; i < channels.Length; i++)
        {
            data[i + 18] = channels[i];
        }

        return data;
    }

    public static byte[] PollPacket()
    {
        byte[] data = new byte[14];
        data[0] = (byte)'A';
        data[1] = (byte)'r';
        data[2] = (byte)'t';
        data[3] = (byte)'-';
        data[4] = (byte)'N';
        data[5] = (byte)'e';
        data[6] = (byte)'t';
        data[7] = 0;
        data[8] = 0x00; // Opcode low
        data[9] = 0x20; // Opcode high

        data[10] = 0;
        data[11] = 14;

        byte flags = 0; // See https://art-net.org.uk/how-it-works/discovery-packets/artpoll/
        byte prio = 0;

        data[12] = flags;
        data[13] = prio;

        return data;
    }
}
