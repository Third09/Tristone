namespace Company.OfficeApp.UI;

/// <summary>
/// Hides a TabControl's tab strip and border, so only the open tab's page shows and fills the whole control.
/// Used for the pages whose tabs are listed in the sidebar instead (owner request 2026-11-03). The TabControl
/// still works as before (SelectedTab, SelectedIndexChanged, tab titles with alerts); it just isn't drawn.
/// </summary>
public static class HiddenTabs
{
    public static void Hide(TabControl tabs) => _ = new StripHider(tabs);

    /// <summary>Answers Windows' "where do the pages go?" question (TCM_ADJUSTRECT) with "the whole control".</summary>
    sealed class StripHider : NativeWindow
    {
        const int TCM_ADJUSTRECT = 0x1328;

        public StripHider(TabControl tabs)
        {
            tabs.HandleCreated += (_, _) => Attach(tabs);
            tabs.HandleDestroyed += (_, _) => ReleaseHandle();
            if (tabs.IsHandleCreated) Attach(tabs);
        }

        void Attach(TabControl tabs)
        {
            AssignHandle(tabs.Handle);
            foreach (TabPage p in tabs.TabPages) p.Bounds = tabs.DisplayRectangle;   // re-place pages already laid out
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == TCM_ADJUSTRECT) { m.Result = 1; return; }
            base.WndProc(ref m);
        }
    }
}
