using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using WixSharp;
using WixSharp.Controls;
using Action = WixSharp.Action;
using CompressionLevel = WixSharp.CompressionLevel;
using File = WixSharp.File;


namespace YingDev.WGWinInstall
{
    class Program
    {
        //same with WG1
        const string UPGRADE_CODE = "00b924a0-e9c8-4c83-ac45-0e4c22b1eabc";
        static Guid Id = Guid.NewGuid();

        static void Main()
        {
            var builderDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            var pathParts = builderDir.Split('\\');
            var solutionDir = string.Join("\\", pathParts.Take(pathParts.Length - 3));

            var wgDir = Path.Combine(solutionDir, "WGestures.App", "bin", "Release");

            var icon = Path.Combine(builderDir, "logo.ico");
            var wgVer = FileVersionInfo.GetVersionInfo(Path.Combine(wgDir, @"WGestures.exe"));

            var project = new Project("WGestures " + wgVer.ProductVersion, new WixObject[]
            {
                new CloseApplication("WGestures.exe", false, false){ ElevatedCloseMessage = false, ElevatedEndSessionMessage = true },

                new MediaTemplate { CompressionLevel = CompressionLevel.high, EmbedCab = true },

                new Dir(new Id("INSTALL_DIR"), @"%ProgramFiles%\WGestures", new Files(wgDir + @"\*.*")),

                new Dir(@"%ProgramMenu%", new ExeFileShortcut(wgVer.ProductName, $"[INSTALL_DIR]WGestures.exe", "")),
            })
            {
                GUID = Id, //每次新生成的安装包必须不同
                //必须是 perMachine, 不然无法自动删除旧版本
                InstallScope = InstallScope.perMachine,
                InstallPrivileges = InstallPrivileges.elevated,
                UpgradeCode = new Guid(UPGRADE_CODE), //所有版本必须相同
                LicenceFile = "lic.rtf",
                ReinstallMode = "ams", //版本检查导致更新时丢失文件...

                MajorUpgrade = MajorUpgrade.Default,
                Version = Version.Parse(wgVer.ProductVersion),
                Language = "zh-CN",
                OutFileName = "Install WGestures " +wgVer.ProductVersion,
                ControlPanelInfo =
                {
                    HelpLink = "https://yingdev.com/projects/wgestures",
                    ProductIcon = icon,
                    Manufacturer = wgVer.CompanyName
                },

                BannerImage = "banner.png",
                BackgroundImage = "banner-left.jpg",
                Actions = new Action[]
                {
                    new PathFileAction(@"explorer.exe", "WGestures.exe", "INSTALL_DIR", Return.ignore, When.After, Step.InstallFinalize,
                        Condition.NOT_Installed) { Impersonate = false },
                },

                UI = WUI.WixUI_Minimal,
                DigitalSignature = new DigitalSignature()
                {
                    TimeUrl = new Uri("http://timestamp.digicert.com"),
                    PfxFilePath = Path.Combine(solutionDir, "YingDevSPC.pfx"),
                    Password = System.IO.File.ReadAllText(Path.Combine(solutionDir, "SIGNPASS.bat")).Split('=')[1],
                    OptionalArguments = "/fd sha256",
                },
                LocalizationFile = "Strings.wxl"
            };
            project.Media.Clear();

            project.MajorUpgrade.AllowSameVersionUpgrades = true;
            project.MajorUpgrade.DowngradeErrorMessage = "!(loc.NewerVersionAlreadyInstalled)";
            project.MajorUpgrade.Schedule = UpgradeSchedule.afterInstallValidate;

            // project.LaunchConditions.Add(new LaunchCondition("Installed OR (VersionNT >= 601)", "!(loc.OSNotSupported)"));

            project.BuildMsi();
        }
    }
}