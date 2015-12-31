using DamienG.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LuapExplorer
{
    public partial class Form1 : Form
    {
        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);
            foreach (var directory in directoryInfo.GetDirectories())
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));
            foreach (var file in directoryInfo.GetFiles())
                directoryNode.Nodes.Add(new TreeNode(file.Name));
            return directoryNode;
        }

        Dictionary<string, fileheader_t> headers;
        Dictionary<string, string> fileNames = new Dictionary<string, string>();
        string fileName = "";

        public Form1(string[] args)
        {
            InitializeComponent();
            #region defines
            fileNames.Add("B2-01-E6-56-17-0A-9F-7A", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\AttackAction.lua");
            fileNames.Add("B2-30-93-2E-CF-44-F0-91", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\Checkpoint.lua");
            fileNames.Add("E9-D2-12-DF-8E-DA-92-13", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\Checkpoint_v2.lua");
            fileNames.Add("DF-6D-AF-69-D8-09-6E-EC", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\Civ_Combat.lua");
            fileNames.Add("74-92-A7-11-9D-E1-78-A4", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\Civ_Idle_MillingTest.lua");
            fileNames.Add("3E-B7-18-A7-2B-88-21-AD", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\Counter.lua");
            fileNames.Add("0A-7C-8B-0F-37-24-A8-9D", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\DeathWatcher.lua");
            fileNames.Add("15-4D-02-89-7A-3A-D0-D9", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\Enabler.lua");
            fileNames.Add("C4-CD-AE-C5-05-D5-CC-E4", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\Human_Idle.lua");
            fileNames.Add("52-4D-4C-C0-A7-35-FD-0D", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\IdleCiv.lua");
            fileNames.Add("A2-16-83-06-17-C0-72-EC", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\IdleNaziGrunt.lua");
            fileNames.Add("F7-58-65-5B-00-83-6D-C4", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\LocatorScriptController.lua");
            fileNames.Add("6F-06-B2-06-40-BC-05-75", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\MgrHarasser.lua");
            fileNames.Add("86-FA-37-71-E3-5C-BD-A0", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\MgrVendor.lua");
            fileNames.Add("D6-CA-ED-0F-83-93-36-AF", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\MISSION_CFrench.lua");
            fileNames.Add("AF-00-5A-B5-08-69-E7-3E", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\MISSION_DSimmons.lua");
            fileNames.Add("45-F9-C9-03-FA-ED-A6-25", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\MISSION_JBiegel.lua");
            fileNames.Add("C5-2E-E1-E1-72-79-9B-6C", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\MISSION_MFindley.lua");
            fileNames.Add("DE-8E-07-95-7B-4E-A5-44", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\MISSION_MMarzola.lua");
            fileNames.Add("D6-0B-10-9A-93-BB-2C-C6", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\MISSION_MTipul.lua");
            fileNames.Add("EC-15-3A-6A-55-96-92-51", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\MISSION_Random.lua");
            fileNames.Add("27-21-C0-3B-28-84-DC-0F", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\MISSION_SGillies.lua");
            fileNames.Add("3B-61-17-90-CC-03-85-C6", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\MISSION_TAbernathy.lua");
            fileNames.Add("AA-F4-9E-14-8F-15-48-AF", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\MISSION_ZRosencrantz.lua");
            fileNames.Add("11-72-EE-84-7E-2C-8A-AF", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\MoveToObjectAction.lua");
            fileNames.Add("FB-F2-52-95-9C-D3-C3-4A", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\NaziGeneralBase01_WE.lua");
            fileNames.Add("0F-44-21-01-18-12-A3-6A", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\NaziTest_Combat.lua");
            fileNames.Add("11-ED-2A-28-C6-8C-F1-9F", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\NaziTest_Idle.lua");
            fileNames.Add("5B-7F-D8-EE-DC-5A-F7-96", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\ScriptHelper.lua");
            fileNames.Add("B6-B4-4C-8D-F3-8C-49-D2", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\SoldierState_Combat.lua");
            fileNames.Add("F4-41-B0-23-CD-1B-96-D6", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\SoldierState_Configure.lua");
            fileNames.Add("1B-A6-71-63-4C-D0-4F-62", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\SoldierState_Hunt.lua");
            fileNames.Add("48-51-3B-AA-01-B1-41-E8", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\SoldierState_Idle.lua");
            fileNames.Add("23-FD-F1-BF-9C-DF-9D-8F", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\SoldierState_Investigate.lua");
            fileNames.Add("0F-1C-72-33-90-51-7E-04", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\SoldierState_InvestigateThreat.lua");
            fileNames.Add("D6-55-52-3E-F3-5F-61-6E", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\SoldierState_PaperCheckBackup.lua");
            fileNames.Add("71-81-C4-EC-26-59-06-74", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\SoldierState_PaperCheckLeader.lua");
            fileNames.Add("63-92-0D-E0-C4-E1-B9-6E", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\Soldier_Broadcasts.lua");
            fileNames.Add("23-01-24-2A-94-5C-E5-85", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\Soldier_Callbacks.lua");
            fileNames.Add("76-0B-05-3E-63-B7-CA-51", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\Soldier_Events.lua");
            fileNames.Add("AE-E6-4D-6F-9B-C8-D4-9A", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\Soldier_Internal.lua");
            fileNames.Add("53-99-2C-B4-F4-DF-49-46", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\testCrossingGuard.lua");
            fileNames.Add("1C-7F-89-26-4D-0E-93-62", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\TriggerWatcher.lua");
            fileNames.Add("80-D2-5F-C0-21-41-1C-8F", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\UsePathAction.lua");
            fileNames.Add("52-4E-08-E7-47-A4-B3-D1", @"projects\WildStar\pov\BinCommon\Scripts\Experimental\VR_HansGruber.lua");
            fileNames.Add("5D-32-E8-B3-31-95-AC-1A", @"projects\WildStar\pov\BinCommon\Scripts\Includes\List.lua");
            fileNames.Add("88-72-1E-E1-8C-66-37-EB", @"projects\WildStar\pov\BinCommon\Scripts\Includes\WRAPPER_Actor.lua");
            fileNames.Add("F1-80-7A-45-43-D3-31-AC", @"projects\WildStar\pov\BinCommon\Scripts\Includes\WRAPPER_Event.lua");
            fileNames.Add("F5-14-8B-DA-D5-22-E5-47", @"projects\WildStar\pov\BinCommon\Scripts\Includes\WRAPPER_Util.lua");
            fileNames.Add("41-F3-88-C2-EB-90-F7-72", @"projects\WildStar\pov\BinCommon\Scripts\Includes\WRAPPER_Vehicle.lua");
            fileNames.Add("16-57-0A-E6-44-B9-08-0A", @"projects\WildStar\pov\BinCommon\Scripts\Includes\__SabMissionIncludes.lua");
            fileNames.Add("16-0C-B2-D3-42-72-BD-BF", @"projects\WildStar\pov\BinCommon\Scripts\Includes\__UtilFunctions.lua");
            fileNames.Add("CF-78-0C-12-FE-84-73-E7", @"projects\WildStar\pov\BinCommon\Scripts\Managers\AchievementsManager.lua");
            fileNames.Add("25-55-74-1F-74-12-4E-59", @"projects\WildStar\pov\BinCommon\Scripts\Managers\GlobalMissionFile.lua");
            fileNames.Add("43-C2-BD-66-BA-26-A4-52", @"projects\WildStar\pov\BinCommon\Scripts\Managers\InteriorManager.lua");
            fileNames.Add("69-FE-C7-FD-E0-4C-FA-BA", @"projects\WildStar\pov\BinCommon\Scripts\Managers\RewardsManager.lua");
            fileNames.Add("6F-3A-99-39-02-AA-DF-A2", @"projects\WildStar\pov\BinCommon\Scripts\Managers\Sab_Message_Template.lua");
            fileNames.Add("48-68-A6-EB-CF-4E-1B-F9", @"projects\WildStar\pov\BinCommon\Scripts\Managers\Sab_Mission_Template.lua");
            fileNames.Add("47-EE-94-39-26-77-B8-89", @"projects\WildStar\pov\BinCommon\Scripts\Managers\ShopManager.lua");
            fileNames.Add("0C-BA-D2-A6-13-48-D4-CB", @"projects\WildStar\pov\BinCommon\Scripts\Managers\StarterManager.lua");
            fileNames.Add("D0-31-6C-28-2B-59-0D-5C", @"projects\WildStar\pov\BinCommon\Scripts\Managers\WorldSMEDNodes.lua");
            fileNames.Add("A7-12-EC-10-61-F2-01-66", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_1_BarFight.lua");
            fileNames.Add("46-B0-6D-D2-C2-17-56-D4", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_1_ConnectToBar.lua");
            fileNames.Add("ED-ED-1B-A5-03-48-2E-85", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_1_Escape.lua");
            fileNames.Add("72-58-3A-B5-D4-9B-AB-33", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_1_Factory.lua");
            fileNames.Add("C2-58-8E-74-F2-61-CA-1B", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_1_Farm.lua");
            fileNames.Add("B4-C5-CB-1A-3E-F0-E6-ED", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_1_GetCaught.lua");
            fileNames.Add("E1-46-14-F0-33-86-87-EC", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_1_Mission_2B.lua");
            fileNames.Add("D3-B5-BB-89-55-4B-78-19", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_1_Race.lua");
            fileNames.Add("7D-B8-E5-18-E5-F9-44-6F", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_1_RaceToGermany.lua");
            fileNames.Add("AA-36-1B-2F-08-37-67-28", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_1_ToGermany.lua");
            fileNames.Add("58-0A-6F-B9-02-F9-67-86", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_3_Mission_1.lua");
            fileNames.Add("77-8A-EE-01-BD-DD-93-BA", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_3_Mission_1_E3.lua");
            fileNames.Add("D9-DA-63-B6-6D-81-6F-08", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_3_Mission_2.lua");
            fileNames.Add("06-67-BC-32-B8-00-6D-DE", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_3_Mission_3.lua");
            fileNames.Add("E9-4E-B6-42-73-6B-01-0E", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_3_Mission_3_ConnectB.lua");
            fileNames.Add("B7-48-36-DD-EB-37-60-E0", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_3_Mission_4.lua");
            fileNames.Add("74-20-25-50-8E-28-5E-FE", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_3_Mission_5.lua");
            fileNames.Add("A9-97-E6-F9-D5-25-6E-79", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Act_3_TimeTrial.lua");
            fileNames.Add("DC-0B-2B-9B-08-EB-D9-23", @"projects\WildStar\pov\BinCommon\Scripts\Missions\AprilBelleDemo.lua");
            fileNames.Add("3F-27-64-68-B1-DF-B0-05", @"projects\WildStar\pov\BinCommon\Scripts\Missions\CFP_Chambord.lua");
            fileNames.Add("50-A4-E0-3F-76-D5-A3-12", @"projects\WildStar\pov\BinCommon\Scripts\Missions\CFP_DockDestroy.lua");
            fileNames.Add("A1-62-A0-F6-6D-4A-72-5E", @"projects\WildStar\pov\BinCommon\Scripts\Missions\CFP_GiselleRescue.lua");
            fileNames.Add("2C-8C-2D-73-BA-5E-A7-20", @"projects\WildStar\pov\BinCommon\Scripts\Missions\CFP_InfiltrateChateau.lua");
            fileNames.Add("2E-48-9A-68-30-E3-A2-CE", @"projects\WildStar\pov\BinCommon\Scripts\Missions\CFP_KoenigDestroy.lua");
            fileNames.Add("E2-33-0F-9B-AE-D8-A7-10", @"projects\WildStar\pov\BinCommon\Scripts\Missions\ConnectorMissionTemplate.lua");
            fileNames.Add("00-08-94-C8-E6-E2-81-E0", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_301_Luc_Con.lua");
            fileNames.Add("F6-0F-93-27-D8-42-82-FB", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_A1_M2b_MeetSkylar.lua");
            fileNames.Add("ED-44-36-DA-41-AC-C3-53", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_A1_M2c_JulesToTrack.lua");
            fileNames.Add("14-D7-0F-C9-A2-B6-AD-C1", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_A1_M5b_BellAnd3Months.lua");
            fileNames.Add("65-07-E9-0A-27-C7-37-BB", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_A3_M1b_ReturnToBelle.lua");
            fileNames.Add("EE-BE-D6-64-92-89-66-23", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_A3_M6b_BackToParis.lua");
            fileNames.Add("0C-00-28-7B-B6-0D-F8-FA", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_AmbientFP.lua");
            fileNames.Add("9D-B2-68-EF-EB-AF-94-72", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_Cin_301_Act3.lua");
            fileNames.Add("AD-CC-D1-7D-3F-F1-B9-49", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ConvoyPapers.lua");
            fileNames.Add("15-CA-2D-DE-D3-3B-CC-14", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_JulesisDeadCin.lua");
            fileNames.Add("31-8A-6D-A7-57-10-3C-D9", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_P2Papers.lua");
            fileNames.Add("E2-9D-F5-68-66-F2-D8-64", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_P3Papers.lua");
            fileNames.Add("E6-87-6A-FD-60-65-94-3F", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_P3PapersIntro.lua");
            fileNames.Add("59-AE-58-78-CB-7B-6A-E0", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_P3_M1b_KesslerAtDoppelsieg.lua");
            fileNames.Add("10-78-0B-C2-36-FF-63-47", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_PaySantos.lua");
            fileNames.Add("C5-0B-C2-23-AF-CD-47-17", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_S1_M6b_LaHavreWrapup.lua");
            fileNames.Add("DF-47-59-89-0D-E6-E9-E6", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_S1_M7c_FindFranziska.lua");
            fileNames.Add("3F-3C-F3-33-1D-FE-9E-C6", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_109_SkylarSex.lua");
            fileNames.Add("F9-7E-8E-1F-D7-7D-C0-65", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_121_Questioning.lua");
            fileNames.Add("D4-DB-89-E4-62-6E-3C-B6", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_209_DeliverLucMeds.lua");
            fileNames.Add("9B-51-27-DF-17-A3-97-45", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_212_ResistanceBackup.lua");
            fileNames.Add("76-7C-F8-1B-84-F9-93-A8", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_215b_SkylarRendevous.lua");
            fileNames.Add("72-1C-4F-76-0C-F1-3B-5E", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_302_ParisReturnVittore.lua");
            fileNames.Add("42-99-49-56-FE-59-E6-39", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_306_BishopMeeting.lua");
            fileNames.Add("37-E8-20-13-31-5F-08-49", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_307_ParkHangingBigGun.lua");
            fileNames.Add("5C-6E-E4-76-A8-1B-44-B3", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_316_VeroDistrustsSkylar.lua");
            fileNames.Add("31-20-78-83-49-C0-94-A6", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_318_RaceComing.lua");
            fileNames.Add("23-47-6F-DF-19-8B-EF-C8", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_320_PhoenixAurora.lua");
            fileNames.Add("60-B0-60-6F-CA-1B-DF-11", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_325_Escape.lua");
            fileNames.Add("9C-82-FB-73-3E-25-31-C3", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_405_BackToSaarbruken.lua");
            fileNames.Add("48-AA-3B-6A-DC-25-83-20", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_P2_Papers.lua");
            fileNames.Add("C6-08-B4-62-FE-55-31-6D", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_P3_Need.lua");
            fileNames.Add("B9-03-96-AB-57-63-72-4B", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_P3_Papers.lua");
            fileNames.Add("5D-E3-94-8A-87-03-80-56", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Connect_ST_Vittore2Belle.lua");
            fileNames.Add("96-58-F2-C0-40-4D-83-1B", @"projects\WildStar\pov\BinCommon\Scripts\Missions\E3_EiffelDemo.lua");
            fileNames.Add("F9-04-B0-C9-2B-7A-11-76", @"projects\WildStar\pov\BinCommon\Scripts\Missions\FP_AMB_ChambordStart.lua");
            fileNames.Add("22-DC-8F-F3-5C-8B-DB-A1", @"projects\WildStar\pov\BinCommon\Scripts\Missions\FP_AMB_ChemFactoryStart.lua");
            fileNames.Add("39-41-FC-3C-17-03-44-38", @"projects\WildStar\pov\BinCommon\Scripts\Missions\FP_AMB_PalaisStart.lua");
            fileNames.Add("65-45-E0-B4-3F-ED-46-08", @"projects\WildStar\pov\BinCommon\Scripts\Missions\FP_CountryRace_1.lua");
            fileNames.Add("64-11-9E-DB-18-1C-49-FE", @"projects\WildStar\pov\BinCommon\Scripts\Missions\FP_CountryRace_2.lua");
            fileNames.Add("36-9A-6A-52-66-B0-67-24", @"projects\WildStar\pov\BinCommon\Scripts\Missions\FP_Paris_Qualifier.lua");
            fileNames.Add("D4-6E-75-70-C4-A2-A6-50", @"projects\WildStar\pov\BinCommon\Scripts\Missions\FuelDepot_E3.lua");
            fileNames.Add("E5-A0-B8-B7-05-93-C1-FD", @"projects\WildStar\pov\BinCommon\Scripts\Missions\GAMESTART.lua");
            fileNames.Add("E6-40-04-7E-26-D7-4F-87", @"projects\WildStar\pov\BinCommon\Scripts\Missions\KillDierkerDebug.lua");
            fileNames.Add("01-32-3D-1E-FD-B0-02-A4", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_215a.lua");
            fileNames.Add("CB-C2-BE-3B-65-89-FA-2A", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_302.lua");
            fileNames.Add("C2-B4-49-43-86-30-E9-20", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_305.lua");
            fileNames.Add("D4-CB-BF-58-4C-BA-ED-28", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_307.lua");
            fileNames.Add("66-00-94-77-C0-D0-D0-AB", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_308a.lua");
            fileNames.Add("C3-17-6A-53-9D-D3-F5-5F", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_405.lua");
            fileNames.Add("06-DE-02-40-92-4A-C4-DC", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_AmbientFP.lua");
            fileNames.Add("2C-E7-1B-92-B2-90-43-43", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_AMB_Finish.lua");
            fileNames.Add("B2-B1-08-09-8E-CF-F9-38", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Note_Bryman1a_FoundMaria.lua");
            fileNames.Add("22-31-1A-FD-FE-4C-01-EF", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_BrymanPreAmbush.lua");
            fileNames.Add("80-69-EA-50-20-BD-9A-70", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_BrymanRadioSabotage.lua");
            fileNames.Add("01-52-91-F4-BF-FE-F8-F7", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_FatherDenis.lua");
            fileNames.Add("3F-91-9C-C6-AB-11-63-A0", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_FP_Ambient.lua");
            fileNames.Add("99-CC-AF-5D-B1-25-05-4C", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_FP_ChemicalFactory.lua");
            fileNames.Add("C6-02-0F-A5-1E-EA-CA-B0", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_FP_CountryChateau.lua");
            fileNames.Add("1C-0E-BD-2E-18-3C-C5-EF", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_FP_C_Race.lua");
            fileNames.Add("42-BB-E6-3A-B2-8C-DE-C1", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_FP_C_Race02.lua");
            fileNames.Add("07-75-E7-D3-F5-AC-15-D2", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_FP_P_Race.lua");
            fileNames.Add("13-93-61-E3-81-55-55-10", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_Jailbreak.lua");
            fileNames.Add("4F-23-3C-CE-19-B2-4C-C4", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Note_Mingo1_OkCorral.lua");
            fileNames.Add("CC-A0-D8-51-C4-16-79-B7", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_NaziWedding.lua");
            fileNames.Add("94-DA-40-7A-54-95-DB-61", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_P2_Papers.lua");
            fileNames.Add("35-1D-15-13-AF-43-F6-8F", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_P3_Papers.lua");
            fileNames.Add("4C-80-D6-CD-EA-2A-ED-E6", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_P4M1.lua");
            fileNames.Add("DE-10-B4-E4-54-1A-E9-69", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_P6M1.lua");
            fileNames.Add("8C-8A-08-09-92-8E-74-5C", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_Pre_Palais.lua");
            fileNames.Add("EF-5F-3E-00-B5-72-1A-4B", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_P_Qualifier.lua");
            fileNames.Add("21-1B-A9-82-BD-E6-57-E4", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_Santos01.lua");
            fileNames.Add("6F-1C-EB-B5-9B-5E-44-71", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_VeroniqueAngry.lua");
            fileNames.Add("AE-BA-61-59-E8-6A-C6-75", @"projects\WildStar\pov\BinCommon\Scripts\Missions\NOTE_VeroniqueBelle.lua");
            fileNames.Add("A3-B4-99-CD-89-D3-F6-2D", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P1FP_Carbomb.lua");
            fileNames.Add("C1-4D-BB-97-7F-57-47-D1", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P1FP_DestroyConvoy.lua");
            fileNames.Add("45-34-5C-2F-B3-A0-C4-FC", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P1FP_Entourage.lua");
            fileNames.Add("D4-10-17-47-6E-F9-3E-C3", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P1FP_EustacheSniper.lua");
            fileNames.Add("AE-88-EC-AE-9E-0D-E0-7E", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P1FP_Jailbreak.lua");
            fileNames.Add("BD-4C-65-2B-2B-B8-80-6D", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P1FP_KillCourtyard01.lua");
            fileNames.Add("6F-FD-F6-59-39-C5-4D-C0", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P1FP_MadBomber01.lua");
            fileNames.Add("61-B6-73-60-2B-7E-52-12", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P1FP_NaziParty.lua");
            fileNames.Add("81-BB-57-A4-B9-55-FD-C0", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P1FP_OnTheAir.lua");
            fileNames.Add("92-0E-9A-ED-32-6C-8C-24", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P1FP_PalaisBombe.lua");
            fileNames.Add("30-7A-D2-9D-F0-01-B8-05", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P1FP_RoofFetch01.lua");
            fileNames.Add("0C-06-9F-6B-2C-27-F2-DB", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P1FP_Traitor.lua");
            fileNames.Add("D1-CD-FF-16-F1-5F-25-F5", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P1M6b.lua");
            fileNames.Add("0D-9C-A8-81-C3-CB-87-B7", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P2FP_GrandSniper.lua");
            fileNames.Add("49-0F-BE-D7-DF-1E-E7-35", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P2FP_InfiltrateAbbey.lua");
            fileNames.Add("67-20-FE-76-51-79-EF-FB", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P2FP_MadeleineSniper.lua");
            fileNames.Add("02-81-CC-A3-CE-98-93-E5", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P2FP_RadioRescue.lua");
            fileNames.Add("D4-FC-F8-C8-08-CF-42-BF", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P2FP_RadioSwap.lua");
            fileNames.Add("F9-2E-99-08-89-25-38-17", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P2FP_Trap.lua");
            fileNames.Add("79-44-FC-A6-D9-2B-02-C0", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P2M5b.lua");
            fileNames.Add("D9-63-80-3C-03-D3-6C-CA", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P3FP_BiggerGun.lua");
            fileNames.Add("FC-A2-DF-4A-A2-E2-33-D1", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P3FP_FountainSniper.lua");
            fileNames.Add("7E-3A-35-B6-C6-E2-4D-94", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P3FP_Hit.lua");
            fileNames.Add("E3-58-2E-61-2B-74-BC-7B", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P3FP_Jardin.lua");
            fileNames.Add("87-4F-1E-72-39-5F-E5-FB", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P3FP_MadBomber03.lua");
            fileNames.Add("7A-7D-14-5D-AC-42-A4-8E", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P3FP_OKCorral.lua");
            fileNames.Add("A0-AF-D0-ED-FC-E5-B1-B6", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P3FP_RadioSabotage.lua");
            fileNames.Add("2D-74-E2-79-8B-05-4A-56", @"projects\WildStar\pov\BinCommon\Scripts\Missions\P4FP_MadBomber02.lua");
            fileNames.Add("3D-00-E5-3E-65-DA-69-80", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Paris_1_Mission_1.lua");
            fileNames.Add("6F-E5-20-BA-B5-FE-54-16", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Paris_1_Mission_1B.lua");
            fileNames.Add("6E-3B-61-A6-BA-36-07-86", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Paris_1_Mission_1B_Connect.lua");
            fileNames.Add("6C-F3-4B-43-A8-6D-C1-E1", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Paris_1_Mission_1_ConnectB.lua");
            fileNames.Add("60-05-68-BB-86-81-58-76", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Paris_1_Mission_6.lua");
            fileNames.Add("E8-8B-F6-FD-7E-8E-D7-D7", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Paris_2_Mission_5.lua");
            fileNames.Add("03-6C-C7-59-4B-42-01-B1", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Paris_3_Mission_1.lua");
            fileNames.Add("12-07-E7-73-70-02-B2-FC", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Paris_4_Mission_1.lua");
            fileNames.Add("22-B3-6F-92-46-38-AF-05", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Paris_4_Mission_1B.lua");
            fileNames.Add("AB-D8-6F-E0-AB-D2-10-7C", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Paris_5_Mission_3.lua");
            fileNames.Add("10-A0-14-43-56-5F-C0-95", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Paris_6_Mission_1.lua");
            fileNames.Add("6B-8E-9D-64-B1-FA-6B-DB", @"projects\WildStar\pov\BinCommon\Scripts\Missions\Paris_6_Mission_1_ConnectB.lua");
            fileNames.Add("19-7A-C0-F3-63-6D-B9-05", @"projects\WildStar\pov\BinCommon\Scripts\Missions\SeptemberTrailer.lua");
            fileNames.Add("05-D8-BF-17-39-3F-8C-77", @"projects\WildStar\pov\BinCommon\Scripts\Missions\SOE_1_Mission_7.lua");
            fileNames.Add("87-45-37-59-61-36-7A-45", @"projects\WildStar\pov\BinCommon\Scripts\Missions\SOE_1_Mission_7b.lua");
            fileNames.Add("33-C4-4D-B2-57-70-B5-10", @"projects\WildStar\pov\BinCommon\Scripts\Missions\SOE_2_Mission_2.lua");
            fileNames.Add("52-52-78-BB-9A-30-B5-51", @"projects\WildStar\pov\BinCommon\Scripts\Missions\SOE_2_Mission_2_ConnectB.lua");
            fileNames.Add("AE-FE-3C-84-F6-E5-55-7D", @"projects\WildStar\pov\BinCommon\Scripts\Missions\SOE_Zeppelin.lua");
            fileNames.Add("A8-A4-B9-8D-68-EE-97-13", @"projects\WildStar\pov\BinCommon\Scripts\Missions\ZeppelinInterior.lua");
            fileNames.Add("6E-D6-1F-34-AE-60-0B-E3", @"projects\WildStar\pov\BinCommon\Scripts\Modules\AmbientRubberStamp.lua");
            fileNames.Add("63-BB-54-2A-24-00-7C-8A", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\AttractionPts\AlarmSystems\AttractionPt_Alarm.lua");
            fileNames.Add("FE-1B-76-5E-72-18-F0-65", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\AttractionPts\AttractionPt.lua");
            fileNames.Add("B0-57-EE-B8-A4-EB-34-57", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\AttractionPts\AttractionPt_SuspBrothel.lua");
            fileNames.Add("6B-CB-2E-9C-F7-25-53-25", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\AttractionPts\AttractionPt_SuspBrothelDLC.lua");
            fileNames.Add("93-16-31-23-AF-DC-43-72", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\AttractionPts\AttractionPt_SuspBrothelHat.lua");
            fileNames.Add("60-B1-56-19-2A-D0-BC-5B", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\AttractionPts\AttractionPt_SuspKiss.lua");
            fileNames.Add("23-A3-C3-1E-ED-97-F4-E3", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\AttractionPts\DoorTeleporter.lua");
            fileNames.Add("46-30-FA-08-A2-98-31-33", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\AttractionPts\DoorTeleporterIntToInt.lua");
            fileNames.Add("79-61-9B-4B-49-C3-4B-06", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Human\Civilian\BrothelGirl.lua");
            fileNames.Add("F6-18-D1-72-86-1C-60-D2", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Human\Civilian\Civilian.lua");
            fileNames.Add("DB-94-9C-72-2F-69-AD-12", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Human\Civilian\KissingGirl.lua");
            fileNames.Add("5D-F1-53-B0-28-42-31-F5", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Human\Human_Null.lua");
            fileNames.Add("9C-15-16-54-4D-C7-E8-75", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Human\Nazi\Soldier.lua");
            fileNames.Add("F6-1B-A7-8C-1E-A7-52-59", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Human\Resistance\Resistance.lua");
            fileNames.Add("39-F8-B3-44-E3-69-54-F0", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Human\Resistance\Shopkeeper.lua");
            fileNames.Add("19-25-63-17-7B-4A-45-44", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Human\Resistance\Vendor.lua");
            fileNames.Add("14-38-E7-70-5E-87-4B-28", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Human\Starter\FreeplayStarter.lua");
            fileNames.Add("12-A4-7C-00-D6-BB-F7-41", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Human\Starter\MissionStarter.lua");
            fileNames.Add("F1-76-6A-A7-AC-94-A3-68", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Player\Saboteur.lua");
            fileNames.Add("5B-CC-4B-FF-82-37-E0-29", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\CafeRegion.lua");
            fileNames.Add("10-8A-BE-73-6F-85-19-37", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\DeathZone.lua");
            fileNames.Add("0F-11-95-DE-5A-5A-33-99", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\DeleteZone.lua");
            fileNames.Add("9B-E1-A6-22-E6-2B-15-7B", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\FightBackZone.lua");
            fileNames.Add("68-15-25-45-2F-4C-04-43", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\HostileZone.lua");
            fileNames.Add("AC-60-43-75-97-D4-8C-08", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\InteriorCheckPoint.lua");
            fileNames.Add("EC-96-56-66-27-67-D9-17", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\NoBirdZone.lua");
            fileNames.Add("9F-7C-AC-DC-AE-9D-58-B6", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\NoEscSpawnZone.lua");
            fileNames.Add("12-E7-F8-C1-19-B8-2F-E5", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\NoFlyZone.lua");
            fileNames.Add("F2-32-D2-11-9D-67-E1-27", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\OutOfBoundsDeathZone.lua");
            fileNames.Add("21-41-DC-AE-C0-5E-9D-A2", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\RedZone.lua");
            fileNames.Add("76-C1-0F-F1-75-49-FD-12", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\RestrictedArea.lua");
            fileNames.Add("3E-1D-52-52-D5-F8-A6-43", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\RestrictedArea2.lua");
            fileNames.Add("11-BD-46-36-20-78-A4-19", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\RestrictedArea3.lua");
            fileNames.Add("0C-9C-2A-4A-53-AF-97-1B", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\RestrictedArea4.lua");
            fileNames.Add("4F-2C-78-51-F6-9F-95-39", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\RestrictedArea5.lua");
            fileNames.Add("51-F0-C0-FB-14-FD-86-8F", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\RestrictedAreaPed.lua");
            fileNames.Add("A7-23-3D-FD-42-54-07-3C", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\RestrictedAreaVeh.lua");
            fileNames.Add("D5-A1-8A-06-C8-CC-77-F9", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\RoadBlockZone.lua");
            fileNames.Add("B1-6B-E7-D5-88-4D-61-29", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\SuspicionZone.lua");
            fileNames.Add("C0-8A-DD-0F-F7-C7-48-2F", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\SuspicionZonePed.lua");
            fileNames.Add("72-96-92-1E-09-D7-B2-03", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\SuspicionZoneVeh.lua");
            fileNames.Add("C8-8E-6E-16-8B-F6-A1-BE", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\Trigger.lua");
            fileNames.Add("AA-37-CC-FD-7D-4E-C2-AB", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Triggers\WorldBorder.lua");
            fileNames.Add("80-C3-98-84-DF-1F-8D-2B", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Behavior\Vehicles\Vehicle.lua");
            fileNames.Add("E0-6B-7F-FD-10-0A-3D-BB", @"projects\WildStar\pov\BinCommon\Scripts\Modules\France.lua");
            fileNames.Add("E3-9E-8B-92-E9-B3-41-AC", @"projects\WildStar\pov\BinCommon\Scripts\Modules\GameTips.lua");
            fileNames.Add("2B-94-DF-8A-D6-8F-88-BD", @"projects\WildStar\pov\BinCommon\Scripts\Modules\InteriorLevels\Belle_Interior.lua");
            fileNames.Add("07-8B-F1-44-12-F3-C4-FC", @"projects\WildStar\pov\BinCommon\Scripts\Modules\InteriorLevels\Belle_Interior_Destroyed.lua");
            fileNames.Add("DE-97-F0-BB-A9-A8-85-EA", @"projects\WildStar\pov\BinCommon\Scripts\Modules\InteriorLevels\Boulogne_Interior.lua");
            fileNames.Add("1E-95-C4-CC-25-41-7B-54", @"projects\WildStar\pov\BinCommon\Scripts\Modules\InteriorLevels\Catacombs_Interior.lua");
            fileNames.Add("0D-17-AA-57-4C-1D-9D-22", @"projects\WildStar\pov\BinCommon\Scripts\Modules\InteriorLevels\Cemetary_Interior.lua");
            fileNames.Add("0D-F6-98-DE-B4-99-A3-7F", @"projects\WildStar\pov\BinCommon\Scripts\Modules\InteriorLevels\Hotel_Interior.lua");
            fileNames.Add("4E-6B-43-FF-F1-0D-2D-73", @"projects\WildStar\pov\BinCommon\Scripts\Modules\InteriorLevels\InteriorTeleporter.lua");
            fileNames.Add("29-96-A8-A1-0C-59-37-98", @"projects\WildStar\pov\BinCommon\Scripts\Modules\InteriorLevels\LaVillette_Interior.lua");
            fileNames.Add("2C-DE-AB-9F-D3-92-8B-84", @"projects\WildStar\pov\BinCommon\Scripts\Modules\InteriorLevels\LeHavreHotel_Interior.lua");
            fileNames.Add("2F-BC-55-7B-96-FB-D0-C8", @"projects\WildStar\pov\BinCommon\Scripts\Modules\InteriorLevels\LeHavreHQ_Interior.lua");
            fileNames.Add("E8-C6-2E-9B-07-1F-AE-CB", @"projects\WildStar\pov\BinCommon\Scripts\Modules\InteriorLevels\Pantheon_Interior.lua");
            fileNames.Add("7B-6D-DC-57-FA-F7-40-1B", @"projects\WildStar\pov\BinCommon\Scripts\Modules\InteriorLevels\RedOx_Interior.lua");
            fileNames.Add("11-E5-82-89-C8-A9-6F-F8", @"projects\WildStar\pov\BinCommon\Scripts\Modules\InteriorLevels\SaarHQ_Interior.lua");
            fileNames.Add("63-64-F4-4B-16-F9-F6-98", @"projects\WildStar\pov\BinCommon\Scripts\Modules\InteriorLevels\Zeppelin_Int.lua");
            fileNames.Add("F4-22-FA-BE-C7-B1-BF-1E", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Libraries\AggroSpawner.lua");
            fileNames.Add("D3-FA-58-6D-D6-19-98-A6", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Libraries\Convo.lua");
            fileNames.Add("FF-9D-3F-FA-06-31-4B-69", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Libraries\ConvoHelper.lua");
            fileNames.Add("3D-96-22-25-68-28-24-5B", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Libraries\DestructionSequence.lua");
            fileNames.Add("7F-84-A2-8E-B2-09-76-3F", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Libraries\Formation.lua");
            fileNames.Add("16-59-F1-C4-D9-05-05-05", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Libraries\Joe.lua");
            fileNames.Add("78-E8-83-04-AB-29-8B-B0", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Libraries\ScriptSequence.lua");
            fileNames.Add("F1-21-DF-BB-04-76-8A-BB", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Libraries\TimerDestination.lua");
            fileNames.Add("FD-98-CC-B4-F8-5E-C5-7B", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Libraries\TipsLib.lua");
            fileNames.Add("1A-61-62-C7-29-6B-60-3E", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Libraries\TrainMGR.lua");
            fileNames.Add("87-1F-DC-5D-AA-FE-1B-22", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Libraries\Veh.lua");
            fileNames.Add("F8-F7-C7-D3-16-BB-7F-E0", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Locator_MissionLauncher.lua");
            fileNames.Add("9F-10-94-DD-71-43-2A-44", @"projects\WildStar\pov\BinCommon\Scripts\Modules\Main_Saboteur_Game.lua");
            fileNames.Add("24-FE-BA-72-8E-F5-61-81", @"projects\WildStar\pov\BinCommon\Scripts\Modules\SabTask.lua");
            fileNames.Add("CC-08-2C-CA-6E-E6-07-E9", @"projects\WildStar\pov\BinCommon\Scripts\Modules\SabTaskGameMaster.lua");
            fileNames.Add("C0-5D-55-CC-9C-3F-9D-AD", @"projects\WildStar\pov\BinCommon\Scripts\Modules\SabTaskMission.lua");
            fileNames.Add("C9-B6-87-91-AB-45-4C-C1", @"projects\WildStar\pov\BinCommon\Scripts\Modules\SabTaskObjective.lua");
            fileNames.Add("20-73-E4-57-4E-86-6E-E7", @"projects\WildStar\pov\BinCommon\Scripts\Modules\SabTaskObjectiveDeliver.lua");
            fileNames.Add("DF-E1-44-19-63-12-3B-61", @"projects\WildStar\pov\BinCommon\Scripts\Modules\SabTaskObjectiveDestroy.lua");
            fileNames.Add("90-01-69-CE-4E-15-F4-C1", @"projects\WildStar\pov\BinCommon\Scripts\Modules\SabTaskObjectiveEmpty.lua");
            fileNames.Add("9C-AC-40-94-E4-4E-B3-2A", @"projects\WildStar\pov\BinCommon\Scripts\Modules\SabTaskObjectiveEscalation.lua");
            fileNames.Add("FB-55-A2-C7-29-B5-A8-D0", @"projects\WildStar\pov\BinCommon\Scripts\Modules\SabTaskObjectiveInteract.lua");
            fileNames.Add("AA-67-32-F0-EE-C0-8F-84", @"projects\WildStar\pov\BinCommon\Scripts\Modules\__MagicNumbers.lua");
            fileNames.Add("C6-E3-D0-99-86-28-A8-F7", @"projects\WildStar\pov\BinCommon\Scripts\Modules\__MissionTypes.lua");
            fileNames.Add("E6-83-20-23-6C-71-FF-09", @"projects\WildStar\pov\BinCommon\Scripts\Modules\__SoldierSettings.lua");
            fileNames.Add("07-71-3F-2B-FC-50-08-AD", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\AAManager.lua");
            fileNames.Add("B7-38-12-AC-AC-60-69-38", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\AICombatGroup.lua");
            fileNames.Add("50-D8-63-DF-5D-67-1C-83", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\AmbientTankPatrol.lua");
            fileNames.Add("C0-57-A9-40-4D-C4-4D-39", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\BASE_LaVillette.lua");
            fileNames.Add("49-25-F7-35-BA-AA-BE-58", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\BelleInteriorSceneManager.lua");
            fileNames.Add("2D-64-2E-FD-7E-ED-E8-88", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\Bunker.lua");
            fileNames.Add("BA-26-63-F2-0B-B5-00-9D", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\CheckpointMgr.lua");
            fileNames.Add("C7-E5-B2-FB-DC-57-01-A3", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\CinematicSpawner.lua");
            fileNames.Add("6A-20-2C-AC-EB-88-9A-27", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\CoDSpawner.lua");
            fileNames.Add("93-9C-85-C9-E8-3C-62-02", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\ComplexConvo.lua");
            fileNames.Add("96-2E-23-A6-97-07-C7-65", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\ConversationTriggers.lua");
            fileNames.Add("CD-B8-F4-35-0E-E3-15-F3", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\DamageVehicle.lua");
            fileNames.Add("85-FB-B6-CB-06-D2-98-C6", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\ExplosionController.lua");
            fileNames.Add("9B-E9-B9-CD-F0-F6-A8-4B", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\HumanSpawner.lua");
            fileNames.Add("89-1B-86-AF-8A-F0-7B-EE", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\Null.lua");
            fileNames.Add("D6-2E-A8-6F-57-D3-49-FC", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\P1M1TransitionMonitor.lua");
            fileNames.Add("A5-A6-5D-7C-C6-E0-4D-A5", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\PantheonGun.lua");
            fileNames.Add("E2-79-9A-31-83-2A-01-58", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\PoisonField.lua");
            fileNames.Add("16-33-15-71-57-4F-E4-5C", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\ProximityDeathExplosion.lua");
            fileNames.Add("6A-DE-77-F4-2B-A7-A8-84", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\ProximityExplosionTrigger.lua");
            fileNames.Add("AB-04-BA-14-D0-5C-18-FB", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\RaceDayController.lua");
            fileNames.Add("A5-2F-8F-67-D6-33-30-B0", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\SimpleHumanSpawner.lua");
            fileNames.Add("78-00-F5-CC-45-5D-C3-4F", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\SimpleSpawner.lua");
            fileNames.Add("DE-5C-B0-55-1F-0E-BE-DA", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\SimpleVehicleSpawner.lua");
            fileNames.Add("4E-72-37-63-9F-E2-B5-85", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\Spawner.lua");
            fileNames.Add("9C-66-1C-F1-59-C0-A7-60", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\Teleporter.lua");
            fileNames.Add("4C-DF-49-D9-A9-17-82-14", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\TimedExplosionTrigger.lua");
            fileNames.Add("EE-9B-B6-F0-2F-81-77-65", @"projects\WildStar\pov\BinCommon\Scripts\ScriptControllers\VehicleSpawnCondition.lua");

            #endregion
            if (Directory.Exists("D:\\projects\\")) Directory.Delete("D:\\projects\\", true);
            if(args.Length != 0)
            {
                LoadLuap(string.Join(" ", args));
            }
        }

        public void LoadLuap(string filename)
        {
            fileName = filename;
            headers = UnpackLuap(filename);
            DirectoryInfo rootDirectoryInfo = new DirectoryInfo("D:\\projects\\WildStar\\pov\\BinCommon\\Scripts\\");
            treeView1.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fastColoredTextBox1.TabLength = 2;
            /*DialogResult dr = openFileDialog1.ShowDialog();
            if(dr == DialogResult.OK)
            {
                UnpackLuap(openFileDialog1.FileName);
            }*/
        }

        public Dictionary<string, fileheader_t> UnpackLuap(string fileName)
        {
            Dictionary<string, fileheader_t> headersList = new Dictionary<string, fileheader_t>();
            using (BinaryReader br = new BinaryReader(File.OpenRead(fileName)))
            {
                int numFiles = br.ReadInt32();
                fileheader_t[] headers = new fileheader_t[numFiles];
                for (int i = 0; i < numFiles; ++i)
                {
                    headers[i] = new fileheader_t(br);
                    
                }

                foreach (var header in headers)
                {
                    string scriptPath = GetScriptPath(br, header);

                    headersList.Add(scriptPath, header);
                    string outputPath = Path.Combine("D:\\", scriptPath);

                    FileInfo fi = new FileInfo(outputPath);
                    if (fi.Directory.Exists == false)
                    {
                        fi.Directory.Create();
                    }

                    br.BaseStream.Position = header.offset;
                    byte[] data = br.ReadBytes(header.length);
                    File.WriteAllBytes(outputPath, data);
                }
            }
            return headersList;
        }

        private string GetScriptPath(BinaryReader p_reader, fileheader_t p_header)
        {
            string hash = BitConverter.ToString(p_header.hash);
            foreach(KeyValuePair<string, string> hashkv in fileNames)
            {
                if (hashkv.Key == hash) return hashkv.Value;
            }
            return "";
            /*// yay, a horrible hack!

            p_reader.BaseStream.Position = p_header.offset + 12;

            int pathLength = p_reader.ReadInt32();
            string path = Encoding.ASCII.GetString(p_reader.ReadBytes(pathLength - 1));

            return path.Substring(path.IndexOf('\\') + 1);*/
        }

        public struct fileheader_t
        {
            public byte[] hash;
            public int offset;
            public int length;
            public int length2;
            public bool isModule;

            public fileheader_t(BinaryReader p_reader)
            {
                hash = p_reader.ReadBytes(8);
                offset = p_reader.ReadInt32();
                length = p_reader.ReadInt32();
                length2 = p_reader.ReadInt32();
                isModule = p_reader.ReadBoolean();
                //Debug.WriteLine(unknown1);
                Debug.Assert(length == length2);
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
            //DialogResult dr = MessageBox.Show("File " + e.Node.Text + " is opened!", e.Node.Text, MessageBoxButtons.);
        }

        private void repackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileName));
            bw.Write(headers.Count);
            int offset = 4; //int count
            foreach (KeyValuePair<string, fileheader_t> header in headers)
            {
                offset += 8; //8 bytes hash
                offset += 4 + 4 + 4 + 1; //offset 
            }
            foreach (KeyValuePair<string, fileheader_t> header in headers)
            {
                FileInfo inf = new FileInfo("D:\\" + header.Key);
                bw.Write(header.Value.hash);
                //bw.Write(new byte[] { 0x0F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                bw.Write(offset);
                offset += (int)inf.Length;
                //bw.Write(header.Value.length);
                //bw.Write(header.Value.length2);
                bw.Write((int)inf.Length);
                bw.Write((int)inf.Length);
                //bw.Write((byte) 0);
                bw.Write(header.Key.Contains("Module") ? (byte)1 : (byte)0);
                //bw.Write(header.Value.unknown1);
            }
            foreach (KeyValuePair<string, fileheader_t> header in headers)
            {
                bw.Write(File.ReadAllBytes("D:\\" + header.Key));
            }
            bw.Close();
            MessageBox.Show("Pack saved and repacked!", "Repacking done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        bool notSaved = false;

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
        }

        private void fastColoredTextBox1_Load(object sender, EventArgs e)
        {

        }

        private void fastColoredTextBox1_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            notSaved = true;
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Text.Contains(".lua"))
            {
                if (fastColoredTextBox1.Text == "") notSaved = false;
                if (notSaved)
                {
                    DialogResult dr = MessageBox.Show("You dont saved and compiled a last edited script!\n\nReally you want to continue?", "Not saved work", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                    notSaved = false;
                }
                string path = "D:\\projects\\WildStar\\pov\\BinCommon";
                string additPath = "";
                //path += e.Node.Parent.Parent.Text + "\\" + e.Node.Parent.Text + "\\" + e.Node.Text;
                TreeNode lastNode = e.Node;
                while (lastNode != null)
                {
                    additPath = "\\" + lastNode.Text + additPath;
                    lastNode = lastNode.Parent;
                }
                path = path + additPath;
                if (!File.Exists(path + "c"))
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.UseShellExecute = false;
                    psi.CreateNoWindow = true;
                    psi.WindowStyle = ProcessWindowStyle.Hidden;
                    psi.RedirectStandardOutput = true;
                    psi.FileName = "java";
                    psi.Arguments = "-jar unluac.jar \"" + path + "\"";
                    psi.WorkingDirectory = Application.StartupPath;
                    Process p = Process.Start(psi);
                    string output = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                    File.WriteAllText(path + "c", output);
                }
                fastColoredTextBox1.OpenFile(path + "c");
                notSaved = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "luac5.1.exe";
            string path = "D:\\projects\\WildStar\\pov\\BinCommon";
            string additPath = "";
            //path += e.Node.Parent.Parent.Text + "\\" + e.Node.Parent.Text + "\\" + e.Node.Text;
            TreeNode lastNode = treeView1.SelectedNode;
            while (lastNode != null)
            {
                additPath = "\\" + lastNode.Text + additPath;
                lastNode = lastNode.Parent;
            }
            path = path + additPath;
            //fastColoredTextBox1.SaveToFile(path + "c", Encoding.Default);
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.RedirectStandardError = true;
            psi.Arguments = "-o \""+path+"\" \"" + path + "c\"";
            psi.WorkingDirectory = Application.StartupPath;
            Process p = Process.Start(psi);
            string output = p.StandardError.ReadToEnd();
            p.WaitForExit();
            MessageBox.Show(output == "" ? "Saved and Compiled Successfully!" : "Compiling failed! Reason:\n" + output);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if(dr == DialogResult.OK)
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = System.AppDomain.CurrentDomain.FriendlyName;
                psi.Arguments = "\""+openFileDialog1.FileName+"\"";
                Process.Start(psi);
                Application.Exit();
            }
        }
    }
}
