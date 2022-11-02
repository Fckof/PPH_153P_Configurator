using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace PPH_153P_Configurator
{
  public partial class Can : IDisposable
  {
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void CallBackHandler(Int16 Error);
    public delegate void CanReceiveHandler(Can sender);
    public delegate void CanTransferHandler(Can sender);
    public delegate void CanErrorHandler(Can sender, CanEventError Error);    

    const string ChaiDllLocation = @"chai.dll";

    [DllImport(ChaiDllLocation, CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 CiInit();
    [DllImport(ChaiDllLocation, CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 CiOpen(Byte Channel, Byte Flag);
    [DllImport(ChaiDllLocation, CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 CiClose(Byte Channel);
    [DllImport(ChaiDllLocation, CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 CiStart(Byte Channel);
    [DllImport(ChaiDllLocation, CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 CiStop(Byte Channel);
    [DllImport(ChaiDllLocation, CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 CiSetFilter(Byte Channel, UInt32 Code, UInt32 Mask);
    [DllImport(ChaiDllLocation, CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 CiSetBaud(Byte Channel, Byte Bt0, Byte Bt1);
    [DllImport(ChaiDllLocation, CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 CiWrite(Byte Channel, ref CanMessage mBuf, Byte Cnt);
    [DllImport(ChaiDllLocation, CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 CiRead(Byte Channel, [Out] CanMessage[] mBuf, Int16 Cnt);
    [DllImport(ChaiDllLocation, CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 CiSetCB(Byte Channel, Byte EventType, CallBackHandler Handler);
    [DllImport(ChaiDllLocation, CallingConvention = CallingConvention.Cdecl)]
    private static extern Int32 CiRcGetCnt(Byte Channel);
    [DllImport(ChaiDllLocation, CallingConvention = CallingConvention.Cdecl)]
    private static extern UInt32 CiGetLibVer();
    [DllImport(ChaiDllLocation, CallingConvention = CallingConvention.Cdecl)]
    private static extern UInt32 CiGetDrvVer();
    [DllImport(ChaiDllLocation, CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 CiBoardInfo(ref CanCiBoardInfo Info);
    [DllImport(ChaiDllLocation, CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 CiHwReset(Byte Channel);

    public event CanReceiveHandler OnReceive;
    public event CanTransferHandler OnTransfer;
    public event CanErrorHandler OnError;

    private CallBackHandler ReceiveEventDelegate;
    private CallBackHandler TransferEventDelegate;
    private CallBackHandler ErrorEventDelegate;

    private Byte channel;
    private static Boolean initialized = false;
    public string WriteStatus{ get; private set; }
    public string DeviceStatus{ get; private set; }
        public bool DeviceFound { get; private set; } = true;
    public Byte Channel { get { return this.channel; } }

    public Can(Byte Channel)
    {
      Init();
      this.channel = Channel;
    }

    /*public*/ private static void Init()
    {
      if (!initialized)
      {
        Int16 result = CiInit();
        if (result < 0) throw new CanInitializationException();
        initialized = true;
      }
    }

    public static CanBoardInfo BoardInfo(Byte Number)
    {
      Init();
      CanCiBoardInfo cbi = new CanCiBoardInfo();
      cbi.Number = Number;
      CiBoardInfo(ref cbi);
      CanBoardInfo ci;
      ci.Number = cbi.Number;
      ci.HwVersion = cbi.HwVersion;
      ci.Chip = cbi.Chip;
      ci.Name = new String(cbi.Name).Remove((new String(cbi.Name)).IndexOf('\0'));
      ci.Manufacture = new String(cbi.Manufacture).Remove((new String(cbi.Manufacture)).IndexOf('\0'));
      return ci;
    }

    public static Version GetLibraryVersion()
    {
      UInt32 ver = CiGetLibVer();
      if (ver < 0) return null;
      else return new Version((Int32)(ver >> 16) & 0xFF, (Int32)(ver >> 8) & 0xFF, (Int32)ver & 0xFF);
    }

    public static Version GetDriverVersion()
    {
      UInt32 ver = CiGetDrvVer();
      if (ver < 0) return null;
      else return new Version((Int32)(ver >> 16) & 0xFF, (Int32)(ver >> 8) & 0xFF, (Int32)ver & 0xFF);
    }

    public void Open(CanOpenFlag Flag)
    {
      Int16 result = CiOpen(this.channel, (Byte)Flag);
            if (result < 0)
            {
                //throw CreateException(result);
                DeviceFound = false;
            }
            else DeviceFound = true;
      SetEvents();
    }

    public void Close()
    {
      Int16 result = CiClose(this.channel);
      if (result < 0)
      {
        //throw CreateException(result);
        DeviceFound=false;
      }
    }

    public void Start()
    {
      Int16 result = CiStart(this.channel);
            /*if (result < 0)
            {
              throw CreateException(result);
            }*/
            if (result < 0) DeviceFound=false;
            else DeviceFound = true;
        }

    public void Stop()
    {
      Int16 result = CiStop(this.channel);
      if (result < 0)
      {
        //throw CreateException(result);
        DeviceFound = false;
      }
    }

    public void SetFilter(UInt32 Code, UInt32 Mask)
    {
      Int16 result = CiSetFilter(this.channel, Code, Mask);
      if (result < 0)
      {
        throw CreateException(result);
      }
    }

    public void SetBaud(CanBaudRate BaudRate)
    {
      Int16 result = CiSetBaud(this.channel, (Byte)((UInt32)BaudRate & 0xff), (Byte)(((UInt32)BaudRate >> 8) & 0xff));
            /*if (result < 0)
            {
              throw CreateException(result);
            }*/
            if (result < 0) DeviceFound = false;
            else DeviceFound = true;
        }

    public void SetBaud(Int16 bt)
    {
      Int16 result = CiSetBaud(this.channel, (Byte)(bt & 0xff), (Byte)((bt >> 8) & 0xff));
      if (result < 0)
      {
        throw CreateException(result);
      }
    }

    public void SetBaud(Byte bt0, Byte bt1)
    {
      Int16 result = CiSetBaud(this.channel, bt0, bt1);
      if (result < 0)
      {
        throw CreateException(result);
      }
    }

    public void Write(UInt32 Id, Byte[] Data, bool isExtended, bool rtr)
    {
      CanMessage message = new CanMessage();
      message.Id = Id;
      message.Size = (Byte)Data.Length;
      if (message.Size > 8)
        throw new ArgumentException("Данные имеют размер больше допустимого (8 байт)");
      message.Data = new Byte[8];
      Data.CopyTo(message.Data, 0);
      if (rtr)
        message.Flag |= 1;
      if (isExtended)
        message.Flag |= 4;
      Int16 result = CiWrite(this.channel, ref message, 1);
      if (result < 0)
      {
        throw CreateException(result);
      }
    }

    public void Write(CanMessage message)
    {
      if (message.Size > 8)
        throw new ArgumentException("Данные имеют размер больше допустимого (8 байт)");
      if (message.Data.Length != 8)
      {
        byte[] tmpData = message.Data;
        message.Data = new byte[8];
        Array.Copy(tmpData, message.Data, message.Size);
      }
      Int16 result = CiWrite(this.channel, ref message, 1);
            if (result < 0)
            {
                //throw CreateException(result);
                WriteStatus = " - Write Error";
            }
            else WriteStatus = "";
        }

    public int Count()
    {
      Int32 cnt = CiRcGetCnt(this.channel);
      if (cnt < 0)
            {
                //throw CreateException((Int16)cnt);
                DeviceFound = false;
                cnt = 0;
            }
      else DeviceFound = true;
        
        
      return cnt;
    }

    public CanMessage? Read()
    {
      CanMessage[] messages = Read(1);
      if (messages.Length < 1)
        return null;
      else
        return messages[0];
    }

    public CanMessage[] Read(Int16 Count)
    {
      CanMessage[] buffer = new CanMessage[Count];
      Int16 result = CiRead(this.channel, buffer, Count);
      if (result >= 0)
      {
        CanMessage[] messages = new CanMessage[result];
        for (Int32 i = 0; i < result; i++)
          messages[i] = buffer[i];
        return messages;
      }
      else
      {
        //throw CreateException(result);
        DeviceFound=false;
        return new CanMessage[0];
            }
    }

    public CanMessage[] ReadAll()
    {
      int cnt = Count();
      CanMessage[] messages = Read((Int16)cnt);
      return messages;
    }

    public void Reset()
    {
      Int16 result = CiHwReset(this.channel);
      if (result < 0)
      {
        throw CreateException(result);
      }
    }

    private Exception CreateException(Int16 Error)
    {
      Exception exception;
      switch (Error)
      {
        case -1:
          exception = new CanGenericException();
          break;
        case -2:
          exception = new CanDeviceIsBusyException();
          break;
        case -3:
          exception = new CanMemoryFaultException();
          break;
        case -4:
          exception = new CanIncorrectStateException();
          break;
        case -5:
          exception = new CanInvalidCallException();
          break;
        case -6:
          exception = new CanInvalidParameterException();
          break;
        case -7:
          exception = new CanCannotAccessException();
          break;
        case -8:
          exception = new CanNotImplementedException();
          break;
        case -9:
          exception = new CanIOErrorException();
          break;
        case -10:
          exception = new CanNoDeviceException();
          break;
        case -11:
          exception = new CanCallWasInterruptedException();
          break;
        case -12:
          exception = new CanNoResourcesException();
          break;
        case -13:
          exception = new CanTimeoutException();
          break;
        default:
          exception = new CanUnknownException();
          break;
      }

      return exception;
    }

    private void SetEvents()
    {
      ReceiveEventDelegate = new CallBackHandler(this.CallReciveEvent);
      Int16 result = CiSetCB(this.channel, (Byte)CanEventType.Receive, ReceiveEventDelegate);
            if (result < 0) DeviceStatus = " - NoDeviceFound"; else DeviceStatus = "";//throw CreateException(result);

      TransferEventDelegate = new CallBackHandler(this.CallTransferEvent);
      result = CiSetCB(this.channel, (Byte)CanEventType.Transfer, TransferEventDelegate);
      if (result < 0) DeviceStatus = " - NoDeviceFound"; else DeviceStatus = "";

      ErrorEventDelegate = new CallBackHandler(this.CallErrorEvent);
      result = CiSetCB(this.channel, (Byte)CanEventType.Error, ErrorEventDelegate);
      if (result < 0) DeviceStatus = " - NoDeviceFound"; else DeviceStatus = "";
    }

    private void CallReciveEvent(Int16 Error)
    {
      if (OnReceive != null)
        OnReceive(this);
    }

    private void CallTransferEvent(Int16 Error)
    {
      if (OnTransfer != null)
        OnTransfer(this);
    }

    private void CallErrorEvent(Int16 Error)
    {
      if (OnError != null)
        OnError(this, (CanEventError)Error);
    }

    #region Члены IDisposable

    public void Dispose()
    {
      Stop();
      Close();
    }

    #endregion
  }
}
