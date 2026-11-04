using System.Diagnostics;
using Company.OfficeApp.Data;

namespace Company.Launcher;

/// <summary>
/// The Demo version's own database (owner decision 2026-11-04). The Setup file carries a small PostgreSQL (pgsql\) and the
/// sample data (demo\demo.dump). The first start creates the database in %LOCALAPPDATA%\TriStone Demo and loads the
/// samples; every start runs it on this PC only (127.0.0.1, port <see cref="AppEdition.DemoPort"/>), and closing
/// Tri-Stone stops it. It never touches a PostgreSQL already on the PC. Reset = delete that folder and start fresh.
/// </summary>
internal static class DemoDatabase
{
    static string Root => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TriStone Demo");
    static string DataDir => Path.Combine(Root, "pgdata");
    static string Files => Path.Combine(Root, "files");
    static string ReadyMark => Path.Combine(Root, "ready.txt");
    static string LogFile => Path.Combine(Root, "database.log");
    static string Bin(string exe) => Path.Combine(AppContext.BaseDirectory, "pgsql", "bin", exe + ".exe");

    /// <summary>Starts the database, preparing it the first time (with a small "please wait" window). False = can't run.</summary>
    public static bool Start()
    {
        string? problem = null;
        var ready = File.Exists(ReadyMark);
        using var wait = new Form
        {
            Text = "Tri-Stone Demo", Size = new Size(420, 130), StartPosition = FormStartPosition.CenterScreen,
            FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, ControlBox = false,
        };
        wait.Controls.Add(new Label
        {
            Text = ready ? "Starting the demo..." : "Preparing the demo for the first time (under a minute)...",
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter,
        });
        wait.Shown += async (_, _) =>
        {
            try { await Task.Run(StartCore); }
            catch (Exception ex) { problem = ex.Message; }
            wait.Close();
        };
        wait.ShowDialog();
        if (problem == null) return true;
        MessageBox.Show("The demo database couldn't start.\n\n" + problem, "Tri-Stone Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    static void StartCore()
    {
        Directory.CreateDirectory(Root);
        var ready = File.Exists(ReadyMark);
        if (!ready)
        {
            if (Directory.Exists(DataDir)) Directory.Delete(DataDir, recursive: true);   // a first start that didn't finish
            Run(Bin("initdb"), $"-D \"{DataDir}\" -U postgres -A trust -E UTF8 --locale=C");
        }
        if (!IsRunning())
        {
            // Not redirected: the database keeps running after pg_ctl ends, and would hold a redirected pipe open.
            RunDetached(Bin("pg_ctl"), $"-D \"{DataDir}\" -w -t 60 -l \"{LogFile}\" " +
                                       $"-o \"-p {AppEdition.DemoPort} -c listen_addresses=127.0.0.1\" start");
        }
        if (ready) return;

        var connect = $"-h 127.0.0.1 -p {AppEdition.DemoPort} -U postgres";
        Run(Bin("psql"), $"{connect} -d postgres -v ON_ERROR_STOP=1 -c \"CREATE DATABASE {AppEdition.DemoDatabaseName}\"");
        var dump = Path.Combine(AppContext.BaseDirectory, "demo", "demo.dump");
        Run(Bin("pg_restore"), $"{connect} --no-owner --no-acl -d {AppEdition.DemoDatabaseName} \"{dump}\"");
        Directory.CreateDirectory(Files);
        Run(Bin("psql"), $"{connect} -d {AppEdition.DemoDatabaseName} -v ON_ERROR_STOP=1 -c " +
                         $"\"UPDATE app_settings SET value = '{Files}' WHERE key = 'file_storage_path'\"");
        File.WriteAllText(ReadyMark, "Sample data loaded " + DateTime.Now);
    }

    static bool IsRunning()
    {
        if (!Directory.Exists(DataDir)) return false;
        using var p = Process.Start(new ProcessStartInfo(Bin("pg_ctl"), $"-D \"{DataDir}\" status") { UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true })!;
        p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        return p.ExitCode == 0;
    }

    /// <summary>Stops the database when Tri-Stone closes.</summary>
    public static void Stop()
    {
        try
        {
            if (Directory.Exists(DataDir)) RunDetached(Bin("pg_ctl"), $"-D \"{DataDir}\" -m fast -w -t 30 stop", failOk: true);
        }
        catch { /* the next start finds it still running and carries on */ }
    }

    /// <summary>"Reset Tri-Stone Demo" in the Start menu: back to the original sample data.</summary>
    public static void Reset()
    {
        if (MessageBox.Show("Delete everything changed in the demo and go back to the original sample data?", "Reset Tri-Stone Demo",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        Stop();
        if (Directory.Exists(Root)) Directory.Delete(Root, recursive: true);
        MessageBox.Show("Done. The next start of Tri-Stone Demo begins with fresh sample data.", "Reset Tri-Stone Demo");
    }

    static void Run(string exe, string args)
    {
        using var p = Process.Start(new ProcessStartInfo(exe, args) { UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true })
                      ?? throw new InvalidOperationException("Can't start " + exe);
        var output = p.StandardOutput.ReadToEndAsync();
        var errors = p.StandardError.ReadToEndAsync();
        p.WaitForExit();
        if (p.ExitCode != 0) throw new InvalidOperationException($"{Path.GetFileName(exe)} failed:\n{errors.Result}{output.Result}");
    }

    static void RunDetached(string exe, string args, bool failOk = false)
    {
        using var p = Process.Start(new ProcessStartInfo(exe, args) { UseShellExecute = false, CreateNoWindow = true })
                      ?? throw new InvalidOperationException("Can't start " + exe);
        p.WaitForExit();
        if (p.ExitCode != 0 && !failOk)
            throw new InvalidOperationException($"{Path.GetFileName(exe)} failed. See {LogFile}");
    }
}
