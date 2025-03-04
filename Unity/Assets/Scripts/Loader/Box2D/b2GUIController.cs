﻿using System;
using Box2DSharp.Testbed.Unity.Inspection;
using ImGuiNET;
using Testbed.Abstractions;
using Timeline;
using UnityEngine;

namespace ET
{
    public struct PausedCallback
    {
        public bool Pause;
    }

    public struct SingleStepCallback
    {
        public bool SingleStep;
    }

    public struct UpdateBehaviorCallback
    {
        public long instanceId;

        public string behaviorName;
    }

    public class b2GUIController
    {
        private readonly b2Game Game;

        public b2GUIController(b2Game game)
        {
            Game = game;
        }

        public void Render()
        {
            UpdateText();
            UpdateUI();
        }

        private readonly Vector4 _textColor = new Vector4(0.9f, 0.6f, 0.6f, 1);

        private void UpdateText()
        {
            if (this.Game.DebugDraw.ShowUI)
            {
                ImGui.SetNextWindowPos(new Vector2(0.0f, 0.0f));
                ImGui.SetNextWindowSize(new Vector2(Global.Camera.Width, Global.Camera.Height));
                ImGui.SetNextWindowBgAlpha(0);
                ImGui.Begin("Overlay",
                    ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoScrollbar);
                ImGui.End();

                while (this.Game.DebugDraw.Texts.TryDequeue(out (System.Numerics.Vector2 Position, string Text) text))
                {
                    ImGui.Begin("Overlay",
                        ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoScrollbar);
                    ImGui.SetCursorPos(text.Position.ToUnityVector2());
                    ImGui.TextColored(_textColor, text.Text);
                    ImGui.End();
                }
            }
        }

        private void UpdateUI()
        {
            const int MenuWidth = 220;

            //Tools
            if (Game.DebugDraw.ShowUI)
            {
                //窗口MinBound
                ImGui.SetNextWindowPos(new Vector2((float)Global.Camera.Width - MenuWidth - 10, 10));
                //窗口size
                ImGui.SetNextWindowSize(new Vector2(MenuWidth, (float)Global.Camera.Height - 20));
                //ToolBar
                ImGui.Begin("Tools", ref Game.DebugDraw.ShowUI, ImGuiWindowFlags.NoResize);

                if (ImGui.BeginTabBar("b2World", ImGuiTabBarFlags.None))
                {
                    if (ImGui.BeginTabItem("b2World"))
                    {
                        ImGui.SliderInt("Vel Iters", ref Global.Settings.VelocityIterations, 0, 50);
                        ImGui.SliderInt("Pos Iters", ref Global.Settings.PositionIterations, 0, 50);
                        ImGui.SliderFloat("TimeScale", ref Global.Settings.TimeScale, 0.0f, 6.0f, "x %.1f");

                        ImGui.Separator();

                        ImGui.Checkbox("Sleep", ref Global.Settings.EnableSleep);
                        ImGui.Checkbox("Warm Starting", ref Global.Settings.EnableWarmStarting);
                        ImGui.Checkbox("Time of Impact", ref Global.Settings.EnableContinuous);
                        ImGui.Checkbox("Sub-Stepping", ref Global.Settings.EnableSubStepping);

                        ImGui.Separator();

                        ImGui.Checkbox("Shapes", ref Global.Settings.DrawShapes);
                        ImGui.Checkbox("Joints", ref Global.Settings.DrawJoints);
                        ImGui.Checkbox("AABBs", ref Global.Settings.DrawAABBs);
                        ImGui.Checkbox("Contact Points", ref Global.Settings.DrawContactPoints);
                        ImGui.Checkbox("Contact Normals", ref Global.Settings.DrawContactNormals);
                        ImGui.Checkbox("Contact Impulses", ref Global.Settings.DrawContactImpulse);
                        ImGui.Checkbox("Friction Impulses", ref Global.Settings.DrawFrictionImpulse);
                        ImGui.Checkbox("Center of Masses", ref Global.Settings.DrawCOMs);
                        ImGui.Checkbox("Draw GizmosUpdate", ref Global.Settings.DrawGizmos);
                        ImGui.Checkbox("Statistics", ref Global.Settings.DrawStats);
                        ImGui.Checkbox("Profile", ref Global.Settings.DrawProfile);

                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem("ET"))
                    {
                        //Project[0]
                        var nodeOpen = ImGui.TreeNodeEx("Project");

                        var leafNodeFlags = ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen;

                        if (nodeOpen)
                        {
                            ImGui.TreeNodeEx("Hot Reload", leafNodeFlags);
                            if (ImGui.IsItemClicked())
                            {
                                CodeLoader.Instance.LoadHotfix();
                                EventSystem.Instance.Load();
                                Log.Debug("hot reload success");
                            }

                            ImGui.TreeNodeEx("Pause", leafNodeFlags);
                            if (ImGui.IsItemClicked())
                            {
                                EventSystem.Instance?.Invoke(new PausedCallback() { Pause = !Global.Settings.Pause });
                            }

                            ImGui.TreePop();
                        }

                        //HitBox[1]
                        nodeOpen = ImGui.TreeNodeEx("HitBox");
                        if (nodeOpen)
                        {
                            ImGui.Checkbox("HitBox", ref Global.Settings.ShowHitBox);
                            ImGui.Checkbox("HurtBox", ref Global.Settings.ShowHurtBox);
                            ImGui.Checkbox("SquashBox", ref Global.Settings.ShowSquashBox);
                            ImGui.Checkbox("ThrowBox", ref Global.Settings.ShowThrowBox);
                            ImGui.Checkbox("ProximityBox", ref Global.Settings.ShowProximityBox);
                            ImGui.Checkbox("OtherBox", ref Global.Settings.ShowOtherBox);
                            ImGui.Checkbox("GizmosUpdate", ref Global.Settings.ShowGizmos);
                            ImGui.EndTabItem();
                        }
                    }

                    ImGui.EndTabBar();
                }

                ImGui.End();
            }

            //Behaviors
            if (Game.DebugDraw.ShowUI)
            {
                ImGui.SetNextWindowPos(new Vector2((float)Global.Camera.Width - MenuWidth - 10, 30));
                ImGui.SetNextWindowSize(new Vector2(MenuWidth, (float)Global.Camera.Height - 40));
                ImGui.Begin("Behaviors", ref this.Game.DebugDraw.ShowUI, ImGuiWindowFlags.NoResize);

                if (ImGui.BeginTabBar("Behaviors"))
                {
                    if (ImGui.BeginTabItem("Behaviors"))
                    {
                        Transform root = GameObject.Find("Global/UnitRoot").transform;

                        ImGuiTreeNodeFlags leafNodeFlags = ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen;
                        ImGuiTreeNodeFlags parentNodeFlags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.OpenOnDoubleClick;
                        
                        for (int i = 0; i < root.GetComponentsInChildren<TimelinePlayer>().Length; i++)
                        {
                            TimelinePlayer timelinePlayer = root.GetComponentsInChildren<TimelinePlayer>()[i];

                            ImGuiTreeNodeFlags nodeSelectedFlag = timelinePlayer.instanceId == Global.Settings.instanceId? ImGuiTreeNodeFlags.Selected : 0;
                            bool nodeOpen = ImGui.TreeNodeEx((IntPtr)i, parentNodeFlags | nodeSelectedFlag, $"{timelinePlayer.name}");
                            long instanceId = timelinePlayer.instanceId;
                            if (ImGui.IsItemClicked())
                            {
                                Global.Settings.instanceId = instanceId;
                            }

                            if (nodeOpen)
                            {
                                bool behaviorOpens = ImGui.TreeNodeEx("Behaviors", parentNodeFlags);
                                if (behaviorOpens)
                                {
                                    int j = 0;
                                    foreach (BBTimeline timeline in timelinePlayer.BBPlayable.timelineDict.Values)
                                    {
                                        ImGui.TreeNodeEx((IntPtr)j, leafNodeFlags, $"{timeline.timelineName}");
                                        if (ImGui.IsItemClicked())
                                        {
                                            EventSystem.Instance?.Invoke(new UpdateBehaviorCallback() { instanceId = instanceId ,behaviorName = timeline.timelineName});
                                        }

                                        j++;
                                    }

                                    ImGui.TreePop();
                                }
                                
                                ImGui.SliderFloat("Hertz", ref timelinePlayer.Hertz, 0f, 120f, "%.0f hz");
                                EventSystem.Instance.Invoke(new UpdateHertzCallback() { instanceId = timelinePlayer.instanceId,Hertz = (int)timelinePlayer.Hertz});
                                
                                ImGui.TreePop();
                            }
                        }
                    }
                    ImGui.EndTabBar();
                }

                ImGui.End();
            }
        }
    }
}