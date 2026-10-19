using System;
using System.ComponentModel;
using System.Windows.Media;

namespace LteRefSensTester.Models
{
    /// <summary>
    /// DataGrid row for reference sensitivity test results (TPC MAX + MIN dual pass)
    /// </summary>
    public class RefSensRow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        void N(string p) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));

        public string BandDisplay { get; init; } = "";
        public string DuplexMode { get; init; } = "";
        public string Channel { get; init; } = "";
        public int Earfcn { get; init; }
        public int DlRb { get; init; }
        public int UlRb { get; init; }

        // ── TPC MAX phase ──────────────────────────────────────────
        double _rsMax = double.NaN;
        public double RefSensMax
        {
            get => _rsMax;
            set
            {
                _rsMax = value;
                N(nameof(RefSensMax));
                N(nameof(RefSensMaxStr));
                N(nameof(SenLevelMax));
                N(nameof(SenLevelMaxStr));
                N(nameof(SensColor));
                N(nameof(RefSensDiff));
                N(nameof(RefSensDiffStr));
                N(nameof(DiffColor));
                N(nameof(ResultText));
                N(nameof(ResultColor));
            }
        }
        public string RefSensMaxStr => double.IsNaN(_rsMax) ? "—" : $"{_rsMax:F1}";
        public double SenLevelMax => double.IsNaN(_rsMax) ? double.NaN : _rsMax + 27.8;
        public string SenLevelMaxStr => double.IsNaN(_rsMax) ? "—" : $"{_rsMax + 27.8:F1}";

        // ── TPC MIN phase ──────────────────────────────────────────
        double _rsMin = double.NaN;
        public double RefSensMin
        {
            get => _rsMin;
            set
            {
                _rsMin = value;
                N(nameof(RefSensMin));
                N(nameof(RefSensMinStr));
                N(nameof(SenLevelMin));
                N(nameof(SenLevelMinStr));
                N(nameof(SensColor));
                N(nameof(RefSensDiff));
                N(nameof(RefSensDiffStr));
                N(nameof(DiffColor));
                N(nameof(ResultText));
                N(nameof(ResultColor));
            }
        }
        public string RefSensMinStr => double.IsNaN(_rsMin) ? "—" : $"{_rsMin:F1}";
        public double SenLevelMin => double.IsNaN(_rsMin) ? double.NaN : _rsMin + 27.8;
        public string SenLevelMinStr => double.IsNaN(_rsMin) ? "—" : $"{_rsMin + 27.8:F1}";

        // ── UE TX power at convergence level (dBm) ────────────────
        double _uePwrMax = double.NaN;
        public double UePowerMax
        {
            get => _uePwrMax;
            set
            {
                _uePwrMax = value;
                N(nameof(UePowerMax));
                N(nameof(UePowerMaxStr));
            }
        }
        public string UePowerMaxStr => double.IsNaN(_uePwrMax) ? "—" : $"{_uePwrMax:F1}";

        double _uePwrMin = double.NaN;
        public double UePowerMin
        {
            get => _uePwrMin;
            set
            {
                _uePwrMin = value;
                N(nameof(UePowerMin));
                N(nameof(UePowerMinStr));
            }
        }
        public string UePowerMinStr => double.IsNaN(_uePwrMin) ? "—" : $"{_uePwrMin:F1}";

        // ── Diff: MAX − MIN (dB) ───────────────────────────────────
        public double RefSensDiff =>
            (double.IsNaN(_rsMax) || double.IsNaN(_rsMin)) ? double.NaN : (_rsMax - _rsMin);
        public string RefSensDiffStr =>
            double.IsNaN(RefSensDiff) ? "—" : $"{RefSensDiff:+0.0;-0.0;0.0}";

        public Brush SensColor =>
            (double.IsNaN(_rsMax) && double.IsNaN(_rsMin)) ? Brushes.White : Brushes.LightGreen;

        public Brush DiffColor => double.IsNaN(RefSensDiff) ? Brushes.White : Brushes.LightYellow;

        // ── Shared probe count / status / note ─────────────────────
        int _sc;
        public int SearchCount
        {
            get => _sc;
            set
            {
                _sc = value;
                N(nameof(SearchCount));
            }
        }

        string _st = "";
        public string StatusText
        {
            get => _st;
            set
            {
                _st = value;
                N(nameof(StatusText));
                N(nameof(StatusColor));
                N(nameof(ResultText));
                N(nameof(ResultColor));
            }
        }
        public Brush StatusColor => _st == "DONE" ? Brushes.LightGreen :
                                    _st == "ERROR" ? Brushes.Tomato :
                                    _st == "SKIP" ? Brushes.Gray : Brushes.White;

        // ── Result: PASS / FAIL based on Diff ∈ [-1, 1] ─────────────
        // 仅在 DONE 且 Diff 有效时判定；ERROR/SKIP 透传 StatusText
        public string ResultText
        {
            get
            {
                if (_st == "ERROR") return "ERROR";
                if (_st == "SKIP") return "SKIP";
                if (_st == "DONE")
                {
                    double d = RefSensDiff;
                    if (double.IsNaN(d)) return "—";
                    return (d >= -1.0 && d <= 1.0) ? "PASS" : "FAIL";
                }
                return "";
            }
        }

        public Brush ResultColor
        {
            get => ResultText switch
            {
                "PASS" => Brushes.LightGreen,
                "FAIL" => Brushes.Tomato,
                "ERROR" => Brushes.Tomato,
                "SKIP" => Brushes.Gray,
                _ => Brushes.White
            };
        }

        string _note = "";
        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                N(nameof(Note));
            }
        }
    }
}
