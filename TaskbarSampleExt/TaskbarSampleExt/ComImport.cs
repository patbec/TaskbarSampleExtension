using System;
using System.Runtime.InteropServices;

namespace TaskbarSampleExt.Interop
{
    /// <summary>
    /// Provides a simple way to support communication between an object and its site in the container.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("FC4801A3-2BA9-11CF-A229-00AA003D7352")]
    public interface IObjectWithSite
    {
        /// <summary>
        /// Enables a container to pass an object a pointer to the interface for its site.
        /// </summary>
        /// <param name="pUnkSite">A pointer to the IUnknown interface pointer of the site managing this object.
        /// If NULL, the object should call Release on any existing site at which point the object no longer knows its site.</param>
        void SetSite([In, MarshalAs(UnmanagedType.IUnknown)] Object pUnkSite);

        /// <summary>
        /// Retrieves the latest site passed using SetSite.
        /// </summary>
        /// <param name="riid">The IID of the interface pointer that should be returned in ppvSite.</param>
        /// <param name="ppvSite">Address of pointer variable that receives the interface pointer requested in riid.</param>
        void GetSite(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out Object ppvSite);
    }

    /// <summary>
    /// Exposes a method that is used to communicate focus changes for a user input object contained in the Shell.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("F1DB8392-7331-11D0-8C99-00A0C92DBFE8")]
    public interface IInputObjectSite
    {
        /// <summary>
        /// Informs the browser that the focus has changed.
        /// </summary>
        /// <param name="punkObj">The address of the IUnknown interface of the object gaining or losing the focus.</param>
        /// <param name="fSetFocus">Indicates if the object has gained or lost the focus. If this value is nonzero, the object has gained the focus.
        /// If this value is zero, the object has lost the focus.</param>
        /// <returns>Returns S_OK if the method was successful, or a COM-defined error code otherwise.</returns>
        [PreserveSig]
        Int32 OnFocusChangeIS([MarshalAs(UnmanagedType.IUnknown)] Object punkObj, Int32 fSetFocus);
    }

    /// <summary>
    /// The IOleWindow interface provides methods that allow an application to obtain the handle to the various windows that participate in in-place activation, and also to enter and exit context-sensitive help mode.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000114-0000-0000-C000-000000000046")]
    public interface IOleWindow
    {
        /// <summary>
        /// Retrieves a handle to one of the windows participating in in-place activation (frame, document, parent, or in-place object window).
        /// </summary>
        /// <param name="phwnd">A pointer to a variable that receives the window handle.</param>
        /// <returns>This method returns S_OK on success.</returns>
        [PreserveSig]
        int GetWindow(out IntPtr phwnd);

        /// <summary>
        /// Determines whether context-sensitive help mode should be entered during an in-place activation session.
        /// </summary>
        /// <param name="fEnterMode">TRUE if help mode should be entered; FALSE if it should be exited.</param>
        /// <returns>This method returns S_OK if the help mode was entered or exited successfully, depending on the value passed in fEnterMode.</returns>
        [PreserveSig]
        int ContextSensitiveHelp(bool fEnterMode);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("012DD920-7B26-11D0-8CA9-00A0C92DBFE8")]
    public interface IDockingWindow : IOleWindow
    {
        #region IOleWindow

        [PreserveSig]
        new int GetWindow(out IntPtr phwnd);

        [PreserveSig]
        new int ContextSensitiveHelp(bool fEnterMode);

        #endregion

        /// <summary>
        /// Instructs the docking window object to show or hide itself.
        /// </summary>
        /// <param name="fShow">TRUE if the docking window object should show its window.
        /// FALSE if the docking window object should hide its window and return its border space by calling SetBorderSpaceDW with zero values.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int ShowDW([In] bool fShow);

        /// <summary>
        /// Notifies the docking window object that it is about to be removed from the frame.
        /// The docking window object should save any persistent information at this time.
        /// </summary>
        /// <param name="dwReserved">Reserved. This parameter should always be zero.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int CloseDW([In] UInt32 dwReserved);

        /// <summary>
        /// Notifies the docking window object that the frame's border space has changed.
        /// </summary>
        /// <param name="prcBorder">Pointer to a RECT structure that contains the frame's available border space.</param>
        /// <param name="punkToolbarSite">Pointer to the site's IUnknown interface. The docking window object should call the QueryInterface method for this interface, requesting IID_IDockingWindowSite.
        /// The docking window object then uses that interface to negotiate its border space. It is the docking window object's responsibility to release this interface when it is no longer needed.</param>
        /// <param name="fReserved">Reserved. This parameter should always be zero.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int ResizeBorderDW(RECT prcBorder, [In, MarshalAs(UnmanagedType.IUnknown)] IntPtr punkToolbarSite, bool fReserved);
    }

    /// <summary>
    /// Gets information about a band object.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("EB0FE172-1A3A-11D0-89B3-00A0C90A90AC")]
    public interface IDeskBand : IDockingWindow
    {
        #region IOleWindow

        [PreserveSig]
        new int GetWindow(out IntPtr phwnd);

        [PreserveSig]
        new int ContextSensitiveHelp(bool fEnterMode);

        #endregion

        #region IDockingWindow

        [PreserveSig]
        new int ShowDW([In] bool fShow);

        [PreserveSig]
        new int CloseDW([In] UInt32 dwReserved);

        [PreserveSig]
        new int ResizeBorderDW(RECT prcBorder, [In, MarshalAs(UnmanagedType.IUnknown)] IntPtr punkToolbarSite, bool fReserved);

        #endregion

        /// <summary>
        /// Gets state information for a band object.
        /// </summary>
        /// <param name="dwBandID">The identifier of the band, assigned by the container. The band object can retain this value if it is required.</param>
        /// <param name="dwViewMode">The view mode of the band object. One of the following values: DBIF_VIEWMODE_NORMAL, DBIF_VIEWMODE_VERTICAL, DBIF_VIEWMODE_FLOATING, DBIF_VIEWMODE_TRANSPARENT.</param>
        /// <param name="pdbi">Pointer to a DESKBANDINFO structure that receives the band information for the object. The dwMask member of this structure indicates the specific information that is being requested.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int GetBandInfo(UInt32 dwBandID, DESKBANDINFO.DBIF dwViewMode, ref DESKBANDINFO pdbi);
    }

    /// <summary>
    /// Exposes methods to enable and query translucency effects in a deskband object.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("79D16DE4-ABEE-4021-8D9D-9169B261D657")]
    public interface IDeskBand2 : IDeskBand
    {
        #region IOleWindow

        [PreserveSig]
        new int GetWindow(out IntPtr phwnd);

        [PreserveSig]
        new int ContextSensitiveHelp(bool fEnterMode);

        #endregion

        #region IDockingWindow

        [PreserveSig]
        new int ShowDW([In] bool fShow);

        [PreserveSig]
        new int CloseDW([In] UInt32 dwReserved);

        [PreserveSig]
        new int ResizeBorderDW(RECT prcBorder, [In, MarshalAs(UnmanagedType.IUnknown)] IntPtr punkToolbarSite, bool fReserved);

        #endregion

        #region IDeskBand

        [PreserveSig]
        new int GetBandInfo(UInt32 dwBandID, DESKBANDINFO.DBIF dwViewMode, ref DESKBANDINFO pdbi);

        #endregion

        /// <summary>
        /// Indicates the deskband's ability to be displayed as translucent.
        /// </summary>
        /// <param name="pfCanRenderComposited">When this method returns, contains a BOOL indicating ability.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int CanRenderComposited(out bool pfCanRenderComposited);

        /// <summary>
        /// Sets the composition state.
        /// </summary>
        /// <param name="fCompositionEnabled">TRUE to enable the composition state; otherwise, FALSE.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int SetCompositionState(bool fCompositionEnabled);

        /// <summary>
        /// Gets the composition state.
        /// </summary>
        /// <param name="pfCompositionEnabled">When this method returns, contains a BOOL that indicates state.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int GetCompositionState(out bool pfCompositionEnabled);
    }
}
