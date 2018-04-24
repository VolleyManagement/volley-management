﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.3.0.0
//      SpecFlow Generator Version:2.3.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace VolleyManagement.Specs.TeamsContext
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class ManageTeamPlayersFeature : Xunit.IClassFixture<ManageTeamPlayersFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "ManageTeamPlayers.feature"
#line hidden
        
        public ManageTeamPlayersFeature(ManageTeamPlayersFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Manage Team Players", "\tIn order to manage team roster\r\n\tAs a tournament administrator\r\n\tI want to be ab" +
                    "le to add and remove players from the team", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(DisplayName="Add player to team")]
        [Xunit.TraitAttribute("FeatureTitle", "Manage Team Players")]
        [Xunit.TraitAttribute("Description", "Add player to team")]
        public virtual void AddPlayerToTeam()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add player to team", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
 testRunner.Given("Team A exists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 8
 testRunner.And("I have added Jane Doe as a team player", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
 testRunner.When("I execute AddPlayersToTeam", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 10
 testRunner.Then("players are added", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="Add several players to team", Skip="Ignored")]
        [Xunit.TraitAttribute("FeatureTitle", "Manage Team Players")]
        [Xunit.TraitAttribute("Description", "Add several players to team")]
        public virtual void AddSeveralPlayersToTeam()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add several players to team", new string[] {
                        "ignore"});
#line 13
this.ScenarioSetup(scenarioInfo);
#line 14
 testRunner.Given("Team A exists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 15
    testRunner.And("Ivan Ivanov is a team player", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
 testRunner.And("I have added Jane Doe as a team player", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
 testRunner.And("I have added John Smith as a team player", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
 testRunner.When("I execute AddPlayersToTeam", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 19
 testRunner.Then("players are added", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="Remove player from team", Skip="Ignored")]
        [Xunit.TraitAttribute("FeatureTitle", "Manage Team Players")]
        [Xunit.TraitAttribute("Description", "Remove player from team")]
        public virtual void RemovePlayerFromTeam()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Remove player from team", new string[] {
                        "ignore"});
#line 22
this.ScenarioSetup(scenarioInfo);
#line 23
 testRunner.Given("Team A exists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 24
    testRunner.And("Jane Doe is a team player", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 25
 testRunner.And("I have removed Jane Doe", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 26
 testRunner.When("I execute RemovePlayersFromTeam", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 27
 testRunner.Then("players are removed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="Remove several player from team", Skip="Ignored")]
        [Xunit.TraitAttribute("FeatureTitle", "Manage Team Players")]
        [Xunit.TraitAttribute("Description", "Remove several player from team")]
        public virtual void RemoveSeveralPlayerFromTeam()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Remove several player from team", new string[] {
                        "ignore"});
#line 30
this.ScenarioSetup(scenarioInfo);
#line 31
 testRunner.Given("Team A exists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 32
    testRunner.And("Jane Doe is a team player", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
    testRunner.And("John Smith is a team player", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
    testRunner.And("Ivan Ivanov is a team player", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
 testRunner.And("I have added Jane Doe as a team player", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
 testRunner.And("I have added John Smith as a team player", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
 testRunner.When("I execute RemovePlayersFromTeam", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 38
 testRunner.Then("players are removed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="Remove captain from team", Skip="Ignored")]
        [Xunit.TraitAttribute("FeatureTitle", "Manage Team Players")]
        [Xunit.TraitAttribute("Description", "Remove captain from team")]
        public virtual void RemoveCaptainFromTeam()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Remove captain from team", new string[] {
                        "ignore"});
#line 41
this.ScenarioSetup(scenarioInfo);
#line 42
 testRunner.Given("Team A exists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 43
    testRunner.And("Jane Doe is a team captain", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
 testRunner.And("I have removed Jane Doe", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
 testRunner.When("I execute RemovePlayersFromTeam", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 46
 testRunner.Then("InvalidOperationException is thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.3.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                ManageTeamPlayersFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ManageTeamPlayersFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
