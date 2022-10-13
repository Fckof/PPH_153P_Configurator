using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CAN_Simulator
{
  public enum CanBaudRate : short
  {
    BCI_10K = 0x1c31,
    BCI_20K = 0x1c18,
    BCI_50K = 0x1c09,
    BCI_125K = 0x1c03,
    BCI_250K = 0x1c01,
    BCI_500K = 0x1c00,
    BCI_800K = 0x1600,
    BCI_1M = 0x1400,
  }

  public enum CanOpenFlag : ushort
  {
    Can11 = 2,
    Can29 = 4
  }

  public enum CanEventError : int
  {
    WarningLimit = 3,
    BusOff = 4,
    HardwareOverrun = 5,
    SoftwareOverrun = 7,
    WriteTimeout = 8
  }

  [StructLayout(LayoutKind.Sequential)]
  public struct CanMessage
  {
    public UInt32 Id;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public Byte[] Data;
    public Byte Size;
    public UInt16 Flag;
    public UInt32 Ts;
  }


  public struct CanBoardInfo
  {
    public Byte Number;
    public UInt32 HwVersion;
    public Int16[] Chip;
    public String Name;
    public String Manufacture;
  }

  public partial class Can
  {
    private enum CanEventType
    {
      Receive = 1,
      Transfer = 2,
      Error = 6
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct CanCiBoardInfo
    {
      public Byte Number;
      public UInt32 HwVersion;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public Int16[] Chip;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
      public Char[] Name;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
      public Char[] Manufacture;
    }
  }
}
