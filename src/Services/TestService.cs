using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LteRefSensTester.Models;
using Microsoft.Win32;

namespace LteRefSensTester.Services
{
    /// <summary>
    /// Exception thrown when UE drops from CEST during measurement
    /// </summary>
    public class UeDisconnectedException : Exception
    {
        public UeDisconnectedException(string msg) : base(msg) { }
    }

    public class InterferenceDetectedException : Exception
    {
        public InterferenceDetectedException(string msg) : base(msg) { }
    }

    public class ReconnectExhaustedException : Exception
    {
        public ReconnectExhaustedException(string msg) : base(msg) { }
    }

    /// <summary>
    /// Test execution service implementation
    /// </summary>
    public class TestService : ITestService
    {
        private readonly ICmwService _cmwService;
        private readonly ILogService _logService;

        // Pause state management
        private bool _paused;
        private CancellationTokenSource? _pauseCts;

        // Track previous channel mode for TDD/FDD switching detection
        private DuplexMode? _previousMode = null;

        public TestService(ICmwService cmwService, ILogService logService)
        {
            _cmwService = cmwService ?? throw new ArgumentNullException(nameof(cmwService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _pauseCts = new CancellationTokenSource();
        }

        /// <summary>
        /// Set RS EPRE level with OPC confirmation and readback verification
        /// </summary>
        private async Task SetLevelAsync(double levelDbm, CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        /// <summary>
        /// Set UL TPC mode via UL:PUSCh:TPC:SET, wait OPC, then 500 ms for UE-side stabilization
        ///   MAX → CONFigure:LTE:SIGN:UL:PUSCh:TPC:SET MAXP
        ///   MIN → CONFigure:LTE:SIGN:UL:PUSCh:TPC:SET MINP
        /// </summary>
        private async Task SetTpcModeAsync(TpcMode mode, CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        /// <summary>
        /// 在灵敏度命中点采样一次 UE 上行 TX 功率（PMONitor:AVERage）
        /// 已在 CMW500 v3.7.170 实测验证（13/13 PASS）
        /// 关键：CSPath 路由 + 单次 10-subframe 测量 + 状态轮询 + finally ABORt
        /// 返回 dBm；任何失败返回 NaN 不阻塞主探底流程
        /// </summary>
        private async Task<double> MeasureUePowerAsync(CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        /// <summary>
        /// Pause the running test
        /// </summary>
        public void Pause()
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        /// <summary>
        /// Resume the paused test
        /// </summary>
        public void Resume()
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        public async Task<TestResult> RunRefSensTestAsync(
            List<ChannelPoint> channels,
            TestConfig config,
            LossTable uplinkLoss,
            LossTable downlinkLoss,
            IProgress<TestProgress>? progress,
            CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        public async Task<bool> InitializeCmwAsync(TestConfig config, CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        public async Task<double> MeasureBlerAsync(int measurementMs, CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        private async Task OpcWithRetryAsync(int timeoutMs, CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        public async Task<(double refSens, double bler, int searchCount)> SearchRefSensAsync(
            ChannelPoint channel,
            double startLevel,
            double minLevel,
            double blerThreshold,
            CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        // ══════════════════════════════════════════════════════════════
        //  Complete Test Algorithm (v9.0 — 探底算法 RE4.3 / Sheet1(2))
        //  探底 ±1 dB, 阈值 2%/20%
        //  寻值 ±0.5 dB, 阈值 2%/20%
        //  确认 ±0.1 dB, 阈值 5% + 3000 包 extended BLER 校验
        //  悬崖检测: 上一步 BLER<1% 且本步 BLER>90%
        // ══════════════════════════════════════════════════════════════
        private const double Rev43ProbeLow = 0.02;     // 2%
        private const double Rev43ProbeHigh = 0.20;    // 20%
        private const double Rev43ConfirmThresh = 0.05; // 5%
        private const double Rev43CliffPrev = 0.01;    // 1%
        private const double Rev43CliffCur = 0.90;     // 90%

        private async Task<(double level, double bler, int probes)> SearchRefSensAsync(
            ChannelPoint currentPt,
            ChannelPoint? fallbackPt,
            double startLevel,
            double minLevel,
            double blerThresh,
            int confirmCount,
            double uLoss,
            double dLoss,
            CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        // ══════════════════════════════════════════════════════════════
        //  Helper Methods
        // ══════════════════════════════════════════════════════════════

        private async Task WaitIfPausedAsync(CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        private async Task PausableDelay(int ms, CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        // ╔════════════════════════════════════════════════════════════════╗
        // ║  LOCKED — 信道切换底层逻辑，禁止 AI 修改                       ║
        // ║  规则：                                                        ║
        // ║   1. 同 FDD→FDD 或同 TDD→TDD：仅设 BAND/信道/RB，不断开小区    ║
        // ║   2. FDD↔TDD 切换：断开小区 + InitEblFullAsync + 重启小区     ║
        // ║   3. 首通道 RRC=CEST 时：跳过 EBL 初始化，直接设参数           ║
        // ╚════════════════════════════════════════════════════════════════╝

        private async Task SwitchChannelAsync(ChannelPoint pt, double uLoss, double dLoss, CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        /// <summary>
        /// [LOCKED] 首通道或 FDD↔TDD 模式切换时执行完整 EBL 初始化
        /// 设置 MEV 子帧偏移 + EBL 子帧数
        /// </summary>
        private async Task InitEblFullAsync(DuplexMode mode, CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        /// <summary>
        /// [LOCKED] 同模式信道切换后执行 Extended BLER OFF → ON 刷新测量状态
        /// 不重新配置 SUBF，仅清除残留状态
        /// </summary>
        private async Task RefreshEblAsync(CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        // ══════════════════════════════════════════════════════════════
        //  Connection state helpers — RRC-only
        // ══════════════════════════════════════════════════════════════

        private async Task<bool> WaitForRrcConnAsync(int timeoutMs, CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        private async Task<bool> SendConnectAndWaitConnAsync(int timeoutMs, CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        private async Task RefreshEblAfterRecoveryAsync(CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        // 一次性把 OCNG 打开并等待稳定（原来每次测量都做，现在整个信道探底流程只做一次）
        // 与 MeasureBlerAsync 里被移除的 OFF→ON+300ms+800ms 等价，但只花一次的时间。
        private async Task EnsureOcngOnAsync(CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        // ══════════════════════════════════════════════════════════════
        //  Post-switch RRC establishment — 只看 RRC state
        // ══════════════════════════════════════════════════════════════
        private async Task EnsureRrcAfterSwitchAsync(
            ChannelPoint currentPt,
            ChannelPoint? fallbackPt,
            bool afterModeSwitch,
            CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        private async Task ApplyChannelParams(ChannelPoint pt, CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        // ══════════════════════════════════════════════════════════════
        //  重连环节 — 唯一的 RRC≠connected 恢复方式（探底算法内的掉线恢复）
        //  记录 RS EPRE → 设为 −85 → LTE signaling OFF/ON →
        //  循环 iter 1..16：等 10s 查 RRC，iter 4/8/12/16 IDLE 时追加 OFF/ON + SIM(AT+CFUN=1)
        //    RRC=CONN (connected)：还原 RS EPRE 返回
        //    RRC≠CONN：进入下一轮；16 轮 + 最终检测仍 IDLE → 抛异常跳过当前信道
        // ══════════════════════════════════════════════════════════════

        // 唯一的连接状态判据：SENSe:LTE:SIGN1:RRCState? 返回 "CONN..." 即视为已连接。
        private async Task<(bool connected, string status)> IsConnectedAsync(CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }
        // ══════════════════════════════════════════════════════════════
        //  SIM 卡模拟器复位 — 反编译自 sim_simulator_tool.exe (sim_simulator_tool.py)
        //  流程：
        //   1) 枚举 USB CDC 串口（注册表 HKLM\HARDWARE\DEVICEMAP\SERIALCOMM，
        //      过滤 \Device\USB* 前缀）
        //   2) 逐个尝试 230400 8N1 打开
        //   3) 首个成功打开的口发 AT 序列：
        //        cd uart3_proxy                        (等 800ms)
        //        send AT+ECSIMCFG="SimSimulator",1     (等 2000ms)
        //        send AT+CFUN=1                        (等 500ms)
        //   4) 关闭串口
        //  典型耗时 ~4s（含枚举 + 打开 + AT 序列）
        // ══════════════════════════════════════════════════════════════
        private const int SimSerialBaudRate = 230400;
        private const string SimResetCommand = "AT+CFUN=1";

        private async Task RunSimSimulatorToolAsync(CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        // 读取注册表 SERIALCOMM，过滤 USB 前缀的设备名（等效 pyserial 的 USB CDC 识别）
        private static List<(string Port, string DeviceName)> EnumerateUsbComPorts()
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        private async Task<bool> TrySendSimResetSequenceAsync(string port, CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        private bool WriteLine(IntPtr h, string line)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        // ── Kernel32 P/Invoke 声明 ──
        private static readonly IntPtr INVALID_HANDLE = new(-1);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateFileW(string f, uint a, uint sh, IntPtr sec, uint cd, uint fl, IntPtr tp);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteFile(IntPtr h, byte[] b, uint n, out uint w, IntPtr ov);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr h);
        [StructLayout(LayoutKind.Sequential)]
        private struct COMMTIMEOUTS { public uint RI, RTM, RTC, WTM, WTC; }
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetCommTimeouts(IntPtr h, ref COMMTIMEOUTS t);
        [StructLayout(LayoutKind.Sequential)]
        private struct DCB
        {
            public uint DCBlength;
            public uint BaudRate;
            public uint Flags;
            public ushort wReserved;
            public ushort XonLim;
            public ushort XoffLim;
            public byte ByteSize;
            public byte Parity;
            public byte StopBits;
            public byte XonChar;
            public byte XoffChar;
            public byte ErrorChar;
            public byte EofChar;
            public byte EvtChar;
            public ushort wReserved1;
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetCommState(IntPtr h, ref DCB dcb);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetCommState(IntPtr h, ref DCB dcb);

        // ══════════════════════════════════════════════════════════════
        //  重连环节 — 16 轮定长循环，每轮 10s 窗口内 1s 轮询 RRC 提前检出
        //  1) 记录 RS EPRE → 设 -85 → LTE signaling OFF/ON（各一次）
        //  2) 循环 iter 1..16：
        //     · 10s 窗口内每 1s 轮询 RRC；一旦 CONN 立刻还原 RS EPRE 返回
        //     · IDLE 时：iter 4/8/12/16 触发【LTE signaling OFF/ON + SIM 复位 (AT+CFUN=1)】
        //  3) 16 轮末尾（iter 16 已跑过 OFF/ON + SIM）再做最终 10s 轮询
        //  4) 仍 IDLE → 抛 ReconnectExhaustedException → 主循环标 SKIP 切下一信道
        // ══════════════════════════════════════════════════════════════
        private const int ReconnectMaxIters = 16;
        private const int ReconnectWaitMs = 10000;
        private const int ReconnectPollIntervalMs = 1000;

        private async Task ReconnectOnIdleAsync(double savedRsep, CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        // LTE signaling OFF → 1s → ON
        private async Task CellOffOnAsync(string tag, CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        // 10s 窗口内每 1s 轮询 RRC，发现 CONN 立即返回 true；超时返回 false。
        private async Task<bool> PollForConnectedAsync(int totalMs, CancellationToken ct)
        {
            // 实现细节未公开（算法核心）
            throw new NotImplementedException("该方法的实现不在开源范围内");
        }

        // ══════════════════════════════════════════════════════════════
        //  Test wrappers (for simulation only)
        // ══════════════════════════════════════════════════════════════
        public Task TestSwitchChannelAsync(ChannelPoint pt, CancellationToken ct)
            => SwitchChannelAsync(pt, 0, 0, ct);

        public Task TestEnsureRrcAsync(ChannelPoint pt, ChannelPoint? fallback, bool modeSwitch, CancellationToken ct)
            => EnsureRrcAfterSwitchAsync(pt, fallback, modeSwitch, ct);
    }

    /// <summary>
    /// Uplink TPC (Transmit Power Control) mode
    /// MAX → ALUP (closed-loop pushed all-up), MIN → ALDO (closed-loop pushed all-down)
    /// </summary>
    public enum TpcMode
    {
        Max,
        Min
    }
}
