package com.aptima.ddd.networktools;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.InetAddress;
import java.nio.ByteBuffer;
import java.nio.charset.Charset;

public class NetMessage {

	// The numbering here is based on the order in which they appear in
	// NetworkTools.cs unless and until they are explicitly assigned numbers
	public enum NetMessageType {
		NONE(0), REGISTER_RESPONSE(2),REGISTER(1), SUBSCRIBE(3), EVENT(4), DISCONNECT(
				4), PING(6), PING_RESPONSE(7);

		private int toByte;

		NetMessageType(int b) {
			toByte = b;
		}

		public int toByte() {
			return toByte;
		}
		
		public static NetMessageType valueOf(int i) {
			for(NetMessageType t : values()) {
				if(t.toByte == i)
					return t;
			}
			return NONE;
		}
	}

	// A helper function to convert from one endianness to another.
	public final static int swabInt(int v) {
		return (v >>> 24) | (v << 24) | ((v << 8) & 0x00FF0000)
				| ((v >> 8) & 0x0000FF00);
	}

	public int clientID;
	public String msg;

	public String terminalID;;

	public NetMessageType type;

	public NetMessage() {
		type = NetMessageType.NONE;
		terminalID = "";
		clientID = -1;
		msg = "";
	}

	public NetMessage(NetMessageType type) {
		this();
		this.type = type;
	}

	public void receive(InputStream stream) throws IOException {

		receiveType(stream);
		switch (type) {
		case NONE:
			break;
		case REGISTER:
			clientID = receiveInt32(stream);
			terminalID = receiveString(stream);
			break;
		case REGISTER_RESPONSE:
			clientID = receiveInt32(stream);
			break;
		case SUBSCRIBE:
			clientID = receiveInt32(stream);
			msg = receiveString(stream);
			break;
		case EVENT:
			clientID = receiveInt32(stream);
			msg = receiveString(stream);
			break;
		}

	}

	private int receiveInt32(InputStream stream) throws IOException {
		int i = 0;
		byte[] data = new byte[Integer.SIZE / Byte.SIZE];
		stream.read(data, 0, data.length);
		i = (data[0] << 24) + ((data[1] & 0xFF) << 16)
				+ ((data[2] & 0xFF) << 8) + (data[3] & 0xFF);

		return swabInt(i);
	}

	private String receiveString(InputStream stream) throws IOException {
		String s = "";
		int bytes = receiveInt32(stream);
		byte[] data = new byte[bytes];

		int receivedBytes = 0;
		int chunk = 0;

		while (receivedBytes < bytes) {
			chunk = stream.read(data, receivedBytes, bytes - receivedBytes);
			receivedBytes += chunk;
		}
		s = Charset.forName("US-ASCII").decode(ByteBuffer.wrap(data))
				.toString();

		return s;
	}

	private void receiveType(InputStream stream) throws IOException {
		byte t[] = { -2 };
		stream.read(t, 0, 1);
		type = NetMessageType.valueOf(t[0]);
	}

	public void send(OutputStream stream) throws IOException {

		sendType(stream);
		switch (type) {
		case NONE:
			break;
		case PING:
			break;
		case REGISTER:
			sendInt32(stream, clientID);
			sendString(stream, InetAddress.getLocalHost().getHostName());
			break;
		case REGISTER_RESPONSE:
			sendInt32(stream, clientID);
			break;
		case SUBSCRIBE:
			sendInt32(stream, clientID);
			sendString(stream, msg);
			break;
		case EVENT:
			sendInt32(stream, clientID);
			sendString(stream, msg);
			break;
		}
		stream.flush();

	}

	private void sendInt32(OutputStream stream, int i2) throws IOException {
		int i = swabInt(i2);
		byte[] data = { (byte) (i >>> 24), (byte) (i >>> 16), (byte) (i >>> 8),
				(byte) i };

		stream.write(data);

	}

	private void sendString(OutputStream stream, String s) throws IOException {
		byte[] data = s.getBytes("US-ASCII"); // System.Text.Encoding.ASCII.GetBytes(s);
		sendInt32(stream, data.length);
		stream.write(data);
	}

	private void sendType(OutputStream stream) throws IOException {
		stream.write(type.toByte());
	}
}
