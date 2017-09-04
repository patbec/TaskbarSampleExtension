// Copyright(c) 2017 Patrick Becker
//
// Visit the Project page for more information.
// https://github.com/patbec/TaskbarSampleExtension


using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TaskbarSampleExt.Interop;

namespace TaskbarSampleExt
{
    /// <summary>
    /// Basic class for a DeskBand object
    /// </summary>
    /// <example>
    /// [ComVisible(true)]
    /// [Guid("00000000-0000-0000-0000-000000000000")]
    /// [DeskBandInfo("Beispiel Erweiterung", "Diese ist eine Beispiel Erweiterung für die Taskleiste.")]
    /// public class SampleExtension : DeskBand
    /// { /*...*/ }
    /// </example>
    public class DeskBand : UserControl, IObjectWithSite, IDeskBand2
    {

        private const int S_OK = 0;
        private const int E_NOTIMPL = unchecked((int)0x80004001);

        protected IInputObjectSite DeskBandSite;

        public DeskBand()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Name = "DeskBand";
        }

        #region Properties

        /// <summary>
        /// Title of the band object, displayed by default on the left or top of the object.
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        public String Title { get; set; }

        /// <summary>
        /// Minimum size of the band object. Default value of -1 sets no minimum constraint.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Size), "-1,-1")]
        public Size MinSize { get; set; }

        /// <summary>
        /// Maximum size of the band object. Default value of -1 sets no maximum constraint.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Size), "-1,-1")]
        public Size MaxSize { get; set; }

        /// <summary>
        /// Minimum vertical size of the band object. Default value of -1 sets no maximum constraint. (Used when the taskbar is aligned vertically.)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Size), "-1,-1")]
        public Size MinSizeVertical { get; set; }

        /// <summary>
        /// Says that band object's size must be multiple of this size. Defauilt value of -1 does not set this constraint.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Size), "-1,-1")]
        public Size IntegralSize { get; set; }

        #endregion

        #region IObjectWithSite

        public void SetSite([In, MarshalAs(UnmanagedType.IUnknown)] Object pUnkSite)
        {
            if (DeskBandSite != null)
                Marshal.ReleaseComObject(DeskBandSite);

            DeskBandSite = (IInputObjectSite)pUnkSite;
        }

        public void GetSite(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out Object ppvSite)
        {
            ppvSite = DeskBandSite;
        }

        #endregion

        #region IDeskBand2

        public virtual int CanRenderComposited(out bool pfCanRenderComposited)
        {
            pfCanRenderComposited = true;
            return S_OK;
        }

        public int SetCompositionState(bool fCompositionEnabled)
        {
            fCompositionEnabled = true;
            return S_OK;
        }

        public int GetCompositionState(out bool pfCompositionEnabled)
        {
            pfCompositionEnabled = false;
            return S_OK;
        }

        public int GetBandInfo(uint dwBandID, DESKBANDINFO.DBIF dwViewMode, ref DESKBANDINFO pdbi)
        {
            if (pdbi.dwMask.HasFlag(DESKBANDINFO.DBIM.DBIM_MINSIZE))
            {
                // Support for a vertical taskbar
                // Most examples have no support for a vertical taskbar. Who in hell uses their taskbar vertically? Me! Very practical on a 21:9 monitor.
                if (dwViewMode.HasFlag(DESKBANDINFO.DBIF.DBIF_VIEWMODE_FLOATING) || dwViewMode.HasFlag(DESKBANDINFO.DBIF.DBIF_VIEWMODE_VERTICAL))
                {
                    pdbi.ptMinSize.Y = this.MinSizeVertical.Width;
                    pdbi.ptMinSize.X = this.MinSizeVertical.Height;
                }
                else
                {
                    pdbi.ptMinSize.X = this.MinSize.Width;
                    pdbi.ptMinSize.Y = this.MinSize.Height;
                }
            }
            if (pdbi.dwMask.HasFlag(DESKBANDINFO.DBIM.DBIM_MAXSIZE))
            {
                if (dwViewMode.HasFlag(DESKBANDINFO.DBIF.DBIF_VIEWMODE_FLOATING) || dwViewMode.HasFlag(DESKBANDINFO.DBIF.DBIF_VIEWMODE_VERTICAL))
                {
                    pdbi.ptMaxSize.Y = this.MaxSize.Width;
                    pdbi.ptMaxSize.X = this.MaxSize.Height;
                }
                else
                {
                    pdbi.ptMaxSize.X = this.MaxSize.Width;
                    pdbi.ptMaxSize.Y = this.MaxSize.Height;
                }
            }
            if (pdbi.dwMask.HasFlag(DESKBANDINFO.DBIM.DBIM_INTEGRAL))
            {
                if (dwViewMode.HasFlag(DESKBANDINFO.DBIF.DBIF_VIEWMODE_FLOATING) || dwViewMode.HasFlag(DESKBANDINFO.DBIF.DBIF_VIEWMODE_VERTICAL))
                {
                    pdbi.ptIntegral.Y = this.IntegralSize.Width;
                    pdbi.ptIntegral.X = this.IntegralSize.Height;
                }
                else
                {
                    pdbi.ptIntegral.X = this.IntegralSize.Width;
                    pdbi.ptIntegral.Y = this.IntegralSize.Height;
                }
            }

            if (pdbi.dwMask.HasFlag(DESKBANDINFO.DBIM.DBIM_ACTUAL))
            {
                if (dwViewMode.HasFlag(DESKBANDINFO.DBIF.DBIF_VIEWMODE_FLOATING) || dwViewMode.HasFlag(DESKBANDINFO.DBIF.DBIF_VIEWMODE_VERTICAL))
                {
                    pdbi.ptActual.Y = this.Size.Width;
                    pdbi.ptActual.X = this.Size.Height;
                }
                else
                {
                    pdbi.ptActual.X = this.Size.Width;
                    pdbi.ptActual.Y = this.Size.Height;
                }
            }

            if (pdbi.dwMask.HasFlag(DESKBANDINFO.DBIM.DBIM_TITLE))
            {
                pdbi.wszTitle = this.Title;
            }

            pdbi.dwModeFlags = DESKBANDINFO.DBIMF.DBIMF_ALWAYSGRIPPER | DESKBANDINFO.DBIMF.DBIMF_NORMAL | DESKBANDINFO.DBIMF.DBIMF_VARIABLEHEIGHT;
            pdbi.dwMask = pdbi.dwMask | DESKBANDINFO.DBIM.DBIM_BKCOLOR | DESKBANDINFO.DBIM.DBIM_TITLE; // Testen

            return S_OK;
        }

        public int GetWindow(out IntPtr phwnd)
        {
            phwnd = Handle;
            return S_OK;
        }

        public int ContextSensitiveHelp(bool fEnterMode)
        {
            return S_OK;
        }

        public int ShowDW([In] bool fShow)
        {
            if (fShow)
                Show();
            else
                Hide();

            return S_OK;
        }

        public int CloseDW([In] uint dwReserved)
        {
            Dispose(true);
            return S_OK;
        }

        public int ResizeBorderDW(RECT prcBorder, [In, MarshalAs(UnmanagedType.IUnknown)] IntPtr punkToolbarSite, bool fReserved)
        {
            return E_NOTIMPL;
        }

        #endregion

        #region Register / Unregister

        [ComRegisterFunctionAttribute]
        public static void Register(Type t)
        {
            string guid = t.GUID.ToString("B");

            DeskBandInfoAttribute[] deskBandInfo = (DeskBandInfoAttribute[])
            t.GetCustomAttributes(typeof(DeskBandInfoAttribute), false);

            // Register only the extension if the attribute DeskBandInfo is used.
            if (deskBandInfo.Length == 1)
            {
                RegistryKey rkClass = Registry.ClassesRoot.CreateSubKey(@"CLSID\" + guid);
                RegistryKey rkCat = rkClass.CreateSubKey("Implemented Categories");

                string _displayName = t.Name;
                string _helpText = t.Name;


                if (deskBandInfo[0].DisplayName != null)
                {
                    _displayName = deskBandInfo[0].DisplayName;
                }

                if (deskBandInfo[0].HelpText != null)
                {
                    _helpText = deskBandInfo[0].HelpText;
                }

                rkClass.SetValue(null, _displayName);
                rkClass.SetValue("MenuText", _displayName);
                rkClass.SetValue("HelpText", _helpText);

                // TaskBar
                rkCat.CreateSubKey("{00021492-0000-0000-C000-000000000046}");

                Console.WriteLine(String.Format("{0} {1} {2}", guid, _displayName, "successfully registered."));
            } else {
                Console.WriteLine(guid + " has no attributes");
            }
        }
      
        [ComUnregisterFunctionAttribute]
        public static void Unregister(Type t)
        {
            string guid = t.GUID.ToString("B");

            DeskBandInfoAttribute[] deskBandInfo = (DeskBandInfoAttribute[])
            t.GetCustomAttributes(typeof(DeskBandInfoAttribute), false);

            if (deskBandInfo.Length == 1)
            {
                string _displayName = t.Name;

                if (deskBandInfo[0].DisplayName != null)
                {
                    _displayName = deskBandInfo[0].DisplayName;
                }

                Registry.ClassesRoot.CreateSubKey(@"CLSID").DeleteSubKeyTree(guid);

                Console.WriteLine(String.Format("{0} {1} {2}", guid, _displayName, "successfully removed."));
            } else {
                Console.WriteLine(guid + " has no attributes");
            }
}

        #endregion
    }
}
