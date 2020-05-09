using System.Threading;
using System.Threading.Tasks;
using System;

namespace DosCommand
{
	public interface IAuthenticator
	{
		/// <summary>
		///     获取验证器的类型
		/// </summary>
		string Type { get; }

		/// <summary>
		///     同步方式调用
		/// </summary>
		/// <returns>验证信息</returns>
		AuthenticationInfo Do();

		/// <summary>
		///     异步方式调用
		/// </summary>
		/// <returns>验证信息</returns>
		Task<AuthenticationInfo> DoAsync(CancellationToken token);
	}
	public abstract class LaunchMode
	{


		public static readonly MCLauncherMode MCLauncher = new MCLauncherMode();

		/// <summary>
		///     启动模式
		/// </summary>
		/// <returns>模式是否应用成功</returns>
		public abstract bool Operate(LauncherCore core, MinecraftLaunchArguments args);
	}


	/// <summary>
	///     模仿BMCL的启动模式
	/// </summary>


	/// <summary>
	///     模仿MCLauncher的启动模式
	/// </summary>
	public class MCLauncherMode : LaunchMode
	{
		public override bool Operate(LauncherCore core, MinecraftLaunchArguments args)
		{
			args.Tokens["game_directory"] = String.Format(@".\versions\{0}\", args.Version.Id);
			return true;
		}
	}

	/// <summary>
	///     简单的映射启动模式
	/// </summary>
	public class SimpleWarpedMode : LaunchMode
	{
		private readonly Func<LauncherCore, MinecraftLaunchArguments, bool> _operatorMethod;

		public SimpleWarpedMode(Func<LauncherCore, MinecraftLaunchArguments, bool> operatorMethod)
		{
			_operatorMethod = operatorMethod;
		}

		public override bool Operate(LauncherCore core, MinecraftLaunchArguments args)
		{
			return _operatorMethod.Invoke(core, args);
		}
	}
	public class AuthenticationInfo
	{
		/// <summary>
		///     玩家的名字
		/// </summary>
		public string DisplayName { get; set; }

		/// <summary>
		///     UUID不解释
		/// </summary>
		public Guid UUID { get; set; }

		/// <summary>
		///     Session不解释
		/// </summary>
		public String AccessToken { get; set; }

		/// <summary>
		///     各种属性（比如Twitch的Session）
		/// </summary>
		public string Properties { get; set; }

		/// <summary>
		///     错误信息，无错误则为null
		/// </summary>
		public string Error { get; set; }

		/// <summary>
		///     用户类型：Legacy or Mojang
		/// </summary>
		public string UserType { get; set; }

	}
}
