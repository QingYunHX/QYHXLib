namespace DosCommand
{
    #region

    using System;
    using System.IO;
    using System.Threading;

    #endregion
    //GetVers(Num)
    //GetJavaPath()
    //Dosin(string name ,string Version ,  string javapath)
    public class MainClass
    {
        public static FileStream _fs;

        public static TextWriter _tw;

        public static readonly AutoResetEvent Are = new AutoResetEvent(false);

        public static LauncherCore Core = LauncherCore.Create();
        public static string OtherJava;
        public static void Dosin(string name ,string Version ,  string javapath ,int MaxMem , int MinMem , bool FullScreen ,ushort high , ushort width)
        {
            TestTimer Timer = new TestTimer();
            OtherJava = javapath;
            /*
            try
            {
                var ping = new ServerPing("mc.hypixel.net", 25565);
                var server = ping.Ping();
                Console.WriteLine(Timer.ToString());
                Console.WriteLine(server.description.text);
                Console.WriteLine("{0} / {1}", server.players.online, server.players.max);
                Console.WriteLine(server.version.name);
                Console.WriteLine(server.modinfo);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine("服务器信息获取失败:"+ex.Message+"\n"+ Timer.ToString());
            }
            */

            using (_fs = new FileStream("mc.log", FileMode.Create))
            {
                using (_tw = new StreamWriter(_fs))
                {
                    //这里图方便没有检验LauncherCoreCreationOption.Create()返回的是不是null
                    var core = LauncherCore.Create();
                    core.GameExit += core_GameExit;
                    core.GameLog += core_GameLog;
                    var launch = new LaunchOptions
                    {
                        Version = core.GetVersion(Version),
                        //Authenticator = new YggdrasilRefresh(new Guid(),false),
                        Authenticator = new OfflineAuthenticator(name),
                        //Authenticator = new YggdrasilValidate(Guid.Parse("****"), Guid.Parse("****"), Guid.Parse("****"), "***"),
                        //Authenticator = new YggdrasilLogin("***@**", "***", true,Guid.Parse("****")),
                        //Authenticator = new YggdrasilAuto("***@**", "***", null, null, null, null),
                        //Server = new ServerInfo {Address = "mc.hypixel.net"},
                        Mode = null,
                        MaxMemory = MaxMem,
                        MinMemory = MinMem,
                        Size = new WindowSize {FullScreen =FullScreen, Height = high, Width = width }
                    };
                    var result = core.Launch(launch, (Action<MinecraftLaunchArguments>)(x => { }));
                    GC.Collect(0);
                    Are.WaitOne();
                    result = null;
                    GC.Collect(0);
                }
            }
        }
        public static void DosOn(string account, string Version, string javapath,string password, int MaxMem, int MinMem, bool FullScreen, ushort high, ushort width)
        {
            TestTimer Timer = new TestTimer();
            OtherJava = javapath;
            /*
            try
            {
                var ping = new ServerPing("mc.hypixel.net", 25565);
                var server = ping.Ping();
                Console.WriteLine(Timer.ToString());
                Console.WriteLine(server.description.text);
                Console.WriteLine("{0} / {1}", server.players.online, server.players.max);
                Console.WriteLine(server.version.name);
                Console.WriteLine(server.modinfo);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine("服务器信息获取失败:"+ex.Message+"\n"+ Timer.ToString());
            }
            */

            using (_fs = new FileStream("mc.log", FileMode.Create))
            {
                using (_tw = new StreamWriter(_fs))
                {
                    //这里图方便没有检验LauncherCoreCreationOption.Create()返回的是不是null
                    var core = LauncherCore.Create();
                    core.GameExit += core_GameExit;
                    core.GameLog += core_GameLog;
                    var launch = new LaunchOptions
                    {
                        Version = core.GetVersion(Version),
                        //Authenticator = new YggdrasilRefresh(new Guid(),false),
                        //Authenticator = new OfflineAuthenticator(name),
                        //Authenticator = new YggdrasilValidate(Guid.Parse("****"), Guid.Parse("****"), Guid.Parse("****"), "***"),
                        //Authenticator = new YggdrasilLogin("***@**", "***", true,Guid.Parse("****")),
                        Authenticator = new YggdrasilAuto(account, password, null, null, null, null),
                        //Server = new ServerInfo {Address = "mc.hypixel.net"},
                        Mode = null,
                        MaxMemory = MaxMem,
                        MinMemory = MinMem,
                        Size = new WindowSize { FullScreen = FullScreen, Height = high, Width = width }
                    };
                    var result = core.Launch(launch, (Action<MinecraftLaunchArguments>)(x => { }));
                    GC.Collect(0);
                    Are.WaitOne();
                    result = null;
                    GC.Collect(0);
                }
            }
        }

        public static void core_GameLog(LaunchHandle handle, string line)
        {
            Console.WriteLine(line);
            _tw.WriteLine(line);

            //handle.SetTitle("啦啦啦");
        }

        public static void core_GameExit(LaunchHandle handle, int code)
        {
            Are.Set();
        }
    }

    public class TestTimer
    {

        public int count = Environment.TickCount;
        public int now = Environment.TickCount;

        public int Used
        {
            get
            {
                int used = Environment.TickCount - this.now;
                this.now = Environment.TickCount;
                return used;
            }
        }

        public int Total
        {
            get { return Environment.TickCount - this.count; }
        }

        public override string ToString() => $" [消耗时长: {this.Used}毫秒, 共消耗: {this.Total}毫秒]";

    }
}