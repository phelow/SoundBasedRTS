/**
 * Copyright (c) 2014,2015,2016 Enzien Audio Ltd.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, and/or
 * sublicense copies of the Software, strictly on a non-commercial basis,
 * and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 *
 * DO NOT MODIFY. THIS CODE IS MACHINE GENERATED BY THE ENZIEN AUDIO HEAVY COMPILER.
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;
using AOT;

[RequireComponent (typeof (AudioSource))]

public class Hv_TriTone_LibWrapper : MonoBehaviour {
  
  // Public parameters are used to send messages into the patch context (thread-safe).
  // Example usage:
  /*
    void Start () {
        Hv_TriTone_LibWrapper script = GetComponent<Hv_TriTone_LibWrapper>();

        // Set the public parameter. Will also clamp the value, update the editor and pass it to the patch context.
        script.freq = 0.5f;

        // Send a float value directly to the patch context.
        script.SendFloatToReceiver(Hv_TriTone_LibWrapper.Parameter.Freq, 0.5f);

        // Sends a bang to this parameter.
        script.SendBangToReceiver(Hv_TriTone_LibWrapper.Parameter.Freq);
    }
  */
  public float freq = 324.0f; // freq
  public float metroVal = 300.0f; // metroVal
  public float metroVal2 = 300.0f; // metroVal2
  public float metroVal3 = 300.0f; // metroVal3
  public float octaveLength = 10.0f; // octaveLength
  public float waveToggle = 1.0f; // waveToggle
  public float waveToggle2 = 1.0f; // waveToggle2
  public float waveToggle3 = 1.0f; // waveToggle3

  public enum Parameter : uint {
    Freq = 0x345FC008,
    Metroval = 0xFFED8BCB,
    Metroval2 = 0x89B63A51,
    Metroval3 = 0x26A7FAB1,
    Octavelength = 0x358AAE38,
    Wavetoggle = 0x9B5289D6,
    Wavetoggle2 = 0x7045DC9E,
    Wavetoggle3 = 0xAFF59635,
  }
  
  // Delegate method for receiving float messages from the patch context (thread-safe).
  // Example usage:
  /*
    void Start () {
        Hv_TriTone_LibWrapper script = GetComponent<Hv_TriTone_LibWrapper>();
        script.RegisterSendHook();
        script.FloatReceivedCallback += OnFloatMessage;
    }

    void OnFloatMessage(Hv_TriTone_LibWrapper.FloatMessage message) {
        Debug.Log(message.receiverName + ": " + message.value);
    }
  */
  public class FloatMessage {
    public string receiverName;
    public float value;

    public FloatMessage(string name, float x) {
      receiverName = name;
      value = x;
    }
  }
  public delegate void FloatMessageReceived(FloatMessage message);
  public FloatMessageReceived FloatReceivedCallback;

  // internal state
  private float _freq = 324.0f;
  private float _metroVal = 300.0f;
  private float _metroVal2 = 300.0f;
  private float _metroVal3 = 300.0f;
  private float _octaveLength = 10.0f;
  private float _waveToggle = 1.0f;
  private float _waveToggle2 = 1.0f;
  private float _waveToggle3 = 1.0f;
  private Hv_TriTone_Context _context;

  public void RegisterSendHook() {
    _context.RegisterSendHook();
  }

  public void SendBangToReceiver(uint receiverHash) {
    // see Hv_TriTone_LibWrapper.Parameter for receiver hash definitions
    _context.SendBangToReceiver(receiverHash);
  }

  public void SendFloatToReceiver(uint receiverHash, float x) {
    // see Hv_TriTone_LibWrapper.Parameter for receiver hash definitions
    _context.SendFloatToReceiver(receiverHash, x);
  }

  public void FillTableWithMonoAudioClip(string tableName, AudioClip clip) {
    if (clip.channels > 1) {
      Debug.LogWarning("Hv_TriTone_LibWrapper: Only loading first channel of '" +
          clip.name + "' into table '" +
          tableName + "'. Multi-channel files are not supported.");
    }
    float[] buffer = new float[clip.samples]; // copy only the 1st channel
    clip.GetData(buffer, 0);
    _context.FillTableWithFloatBuffer(tableName, buffer);
  }

  public void FillTableWithFloatBuffer(string tableName, float[] buffer) {
    _context.FillTableWithFloatBuffer(tableName, buffer);
  }

  private void Awake() {
    _context = new Hv_TriTone_Context((double) AudioSettings.outputSampleRate);
  }
  
  private void Start() {
    _context.SendFloatToReceiver((uint) Parameter.Freq, Mathf.Clamp(freq, 0.0f, 1000.0f));
    _context.SendFloatToReceiver((uint) Parameter.Metroval, Mathf.Clamp(metroVal, 0.0f, 10000.0f));
    _context.SendFloatToReceiver((uint) Parameter.Metroval2, Mathf.Clamp(metroVal2, 0.0f, 10000.0f));
    _context.SendFloatToReceiver((uint) Parameter.Metroval3, Mathf.Clamp(metroVal3, 0.0f, 10000.0f));
    _context.SendFloatToReceiver((uint) Parameter.Octavelength, Mathf.Clamp(octaveLength, 0.0f, 10000.0f));
    _context.SendFloatToReceiver((uint) Parameter.Wavetoggle, Mathf.Clamp(waveToggle, 0.0f, 1.0f));
    _context.SendFloatToReceiver((uint) Parameter.Wavetoggle2, Mathf.Clamp(waveToggle2, 0.0f, 1.0f));
    _context.SendFloatToReceiver((uint) Parameter.Wavetoggle3, Mathf.Clamp(waveToggle3, 0.0f, 1.0f));
  }
  
  private void Update() {
    // retreive sent messages
    if (_context.IsSendHookRegistered()) {
      Hv_TriTone_LibWrapper.FloatMessage tempMessage;
      while ((tempMessage = _context.msgQueue.GetNextMessage()) != null) {
        FloatReceivedCallback(tempMessage);
      }
    }
  }
  
  private void OnValidate() {
    if (_context != null) {
      if (_freq != freq) {
        _freq = freq = Mathf.Clamp(freq, 0.0f, 1000.0f);
        _context.SendFloatToReceiver((uint) Parameter.Freq, _freq);
      }
      if (_metroVal != metroVal) {
        _metroVal = metroVal = Mathf.Clamp(metroVal, 0.0f, 10000.0f);
        _context.SendFloatToReceiver((uint) Parameter.Metroval, _metroVal);
      }
      if (_metroVal2 != metroVal2) {
        _metroVal2 = metroVal2 = Mathf.Clamp(metroVal2, 0.0f, 10000.0f);
        _context.SendFloatToReceiver((uint) Parameter.Metroval2, _metroVal2);
      }
      if (_metroVal3 != metroVal3) {
        _metroVal3 = metroVal3 = Mathf.Clamp(metroVal3, 0.0f, 10000.0f);
        _context.SendFloatToReceiver((uint) Parameter.Metroval3, _metroVal3);
      }
      if (_octaveLength != octaveLength) {
        _octaveLength = octaveLength = Mathf.Clamp(octaveLength, 0.0f, 10000.0f);
        _context.SendFloatToReceiver((uint) Parameter.Octavelength, _octaveLength);
      }
      if (_waveToggle != waveToggle) {
        _waveToggle = waveToggle = Mathf.Clamp(waveToggle, 0.0f, 1.0f);
        _context.SendFloatToReceiver((uint) Parameter.Wavetoggle, _waveToggle);
      }
      if (_waveToggle2 != waveToggle2) {
        _waveToggle2 = waveToggle2 = Mathf.Clamp(waveToggle2, 0.0f, 1.0f);
        _context.SendFloatToReceiver((uint) Parameter.Wavetoggle2, _waveToggle2);
      }
      if (_waveToggle3 != waveToggle3) {
        _waveToggle3 = waveToggle3 = Mathf.Clamp(waveToggle3, 0.0f, 1.0f);
        _context.SendFloatToReceiver((uint) Parameter.Wavetoggle3, _waveToggle3);
      }
    }
  }
  
  private void OnAudioFilterRead(float[] buffer, int numChannels) {
    Assert.AreEqual(numChannels, _context.GetNumOutputChannels()); // invalid channel configuration
    OnValidate(); // process parameter changes
    _context.Process(buffer, buffer.Length / numChannels); // process dsp
  }
}

class Hv_TriTone_Context {

#if UNITY_IOS && !UNITY_EDITOR
  private const string _dllName = "__Internal";
#else
  private const string _dllName = "Hv_TriTone_LibWrapper";
#endif

  // Thread-safe message queue
  public class SendMessageQueue {
    private readonly object _msgQueueSync = new object();
    private readonly Queue<Hv_TriTone_LibWrapper.FloatMessage> _msgQueue = new Queue<Hv_TriTone_LibWrapper.FloatMessage>();

    public Hv_TriTone_LibWrapper.FloatMessage GetNextMessage() {
      lock (_msgQueueSync) {
        return (_msgQueue.Count != 0) ? _msgQueue.Dequeue() : null;
      }
    }

    public void AddMessage(string receiverName, float value) {
      Hv_TriTone_LibWrapper.FloatMessage msg = new Hv_TriTone_LibWrapper.FloatMessage(receiverName, value);
      lock (_msgQueueSync) {
        _msgQueue.Enqueue(msg);
      }
    }
  }

  public readonly SendMessageQueue msgQueue = new SendMessageQueue();
  private readonly GCHandle gch;
  private readonly IntPtr _context; // handle into unmanaged memory
  private SendHook _sendHook = null;

  [DllImport (_dllName)]
  private static extern IntPtr hv_TriTone_new_with_options(double sampleRate, int poolKb, int queueKb);

  [DllImport (_dllName)]
  private static extern int hv_processInlineInterleaved(IntPtr ctx,
      [In] float[] inBuffer, [Out] float[] outBuffer, int numSamples);

  [DllImport (_dllName)]
  private static extern void hv_delete(IntPtr ctx);

  [DllImport (_dllName)]
  private static extern double hv_getSampleRate(IntPtr ctx);

  [DllImport (_dllName)]
  private static extern int hv_getNumInputChannels(IntPtr ctx);

  [DllImport (_dllName)]
  private static extern int hv_getNumOutputChannels(IntPtr ctx);

  [DllImport (_dllName)]
  private static extern void hv_setSendHook(IntPtr ctx, SendHook sendHook);

  [DllImport (_dllName)]
  private static extern void hv_setPrintHook(IntPtr ctx, PrintHook printHook);

  [DllImport (_dllName)]
  private static extern int hv_setUserData(IntPtr ctx, IntPtr userData);

  [DllImport (_dllName)]
  private static extern IntPtr hv_getUserData(IntPtr ctx);

  [DllImport (_dllName)]
  private static extern void hv_sendBangToReceiver(IntPtr ctx, uint receiverHash);

  [DllImport (_dllName)]
  private static extern void hv_sendFloatToReceiver(IntPtr ctx, uint receiverHash, float x);

  [DllImport (_dllName)]
  private static extern uint hv_msg_getTimestamp(IntPtr message);

  [DllImport (_dllName)]
  private static extern bool hv_msg_hasFormat(IntPtr message, string format);

  [DllImport (_dllName)]
  private static extern float hv_msg_getFloat(IntPtr message, int index);

  [DllImport (_dllName)]
  private static extern int hv_table_resize(IntPtr ctx, uint tableHash, int newSize);

  [DllImport (_dllName)]
  private static extern IntPtr hv_table_getBuffer(IntPtr ctx, uint tableHash);

  [DllImport (_dllName)]
  private static extern float hv_samplesToMilliseconds(IntPtr ctx, uint numSamples);

  [DllImport (_dllName)]
  private static extern uint hv_stringToHash(string s);

  private delegate void PrintHook(IntPtr context, string printName, string str, IntPtr message);

  private delegate void SendHook(IntPtr context, string sendName, uint sendHash, IntPtr message);

  public Hv_TriTone_Context(double sampleRate, int poolKb=10, int queueKb=2) {
    gch = GCHandle.Alloc(msgQueue);
    _context = hv_TriTone_new_with_options(sampleRate, poolKb, queueKb);
    hv_setPrintHook(_context, new PrintHook(OnPrint));
    hv_setUserData(_context, GCHandle.ToIntPtr(gch));
  }

  ~Hv_TriTone_Context() {
    hv_delete(_context);
    GC.KeepAlive(_context);
    GC.KeepAlive(_sendHook);
    gch.Free();
  }

  public void RegisterSendHook() {
    // Note: send hook functionality only applies to messages containing a single float value
    if (_sendHook == null) {
      _sendHook = new SendHook(OnMessageSent);
      hv_setSendHook(_context, _sendHook);
    }
  }

  public bool IsSendHookRegistered() {
    return (_sendHook != null);
  }

  public double GetSampleRate() {
    return hv_getSampleRate(_context);
  }

  public int GetNumInputChannels() {
    return hv_getNumInputChannels(_context);
  }

  public int GetNumOutputChannels() {
    return hv_getNumOutputChannels(_context);
  }

  public void SendBangToReceiver(uint receiverHash) {
    hv_sendBangToReceiver(_context, receiverHash);
  }

  public void SendFloatToReceiver(uint receiverHash, float x) {
    hv_sendFloatToReceiver(_context, receiverHash, x);
  }

  public void FillTableWithFloatBuffer(string tableName, float[] buffer) {
    uint tableHash = hv_stringToHash(tableName);
    if (hv_table_getBuffer(_context, tableHash) != IntPtr.Zero) {
      hv_table_resize(_context, tableHash, buffer.Length);
      Marshal.Copy(buffer, 0, hv_table_getBuffer(_context, tableHash), buffer.Length);
    } else {
      Debug.Log(string.Format("Table '{0}' doesn't exist in the patch context.", tableName));
    }
  }

  public uint StringToHash(string s) {
    return hv_stringToHash(s);
  }

  public int Process(float[] buffer, int numFrames) {
    return hv_processInlineInterleaved(_context, buffer, buffer, numFrames);
  }

  [MonoPInvokeCallback(typeof(PrintHook))]
  private static void OnPrint(IntPtr context, string printName, string str, IntPtr message) {
    float timeInSecs = hv_samplesToMilliseconds(context, hv_msg_getTimestamp(message)) / 1000.0f;
    Debug.Log(string.Format("{0} [{1:0.000}]: {2}", printName, timeInSecs, str));
  }

  [MonoPInvokeCallback(typeof(SendHook))]
  private static void OnMessageSent(IntPtr context, string sendName, uint sendHash, IntPtr message) {
    if (hv_msg_hasFormat(message, "f")) {
      SendMessageQueue msgQueue = (SendMessageQueue) GCHandle.FromIntPtr(hv_getUserData(context)).Target;
      msgQueue.AddMessage(sendName, hv_msg_getFloat(message, 0));
    }
  }
}
