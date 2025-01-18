﻿using System;
using Box2DSharp.Testbed.Unity;
using Box2DSharp.Testbed.Unity.Inspection;
using ImGuiNET;
using Testbed.Abstractions;
using UnityEngine;

namespace ET
{
    public struct UnitProfile
    {
        public string UnitName;
        public System.Numerics.Vector2 LinearVelocity;
        public float AngularVelocity;
        public System.Numerics.Vector2 Position;
        public string BehaviorName;
        public string MoveType;
    }

    public struct UpdateUnitProfileCallback
    {
        public long instanceId;
    }

    //Loader层，负责渲染形状，接收输入
    public class b2Game: MonoBehaviour
    {
        private FpsCounter fpsCounter;

        private FixedUpdate fixedUpdate;

        public UnityDraw unityDraw;

        public DebugDraw DebugDraw;

        private UnityTestSettings Settings;

        public Vector3 Difference;

        public Vector3 Origin;

        public bool Drag;

        private UnityInput UnityInput;

        private b2GUIController controller;
        
        public void Awake()
        {
            fpsCounter = new();
            Settings = TestSettingHelper.Load();
            Global.Settings = this.Settings;
            Global.Camera.Width = this.Settings.WindowWidth;
            Global.Camera.Height = this.Settings.WindowHeight;
            Screen.SetResolution(this.Settings.WindowWidth, this.Settings.WindowHeight, this.Settings.FullScreenMode);
            
            UnityInput = new UnityInput();
            Global.Input = this.UnityInput;

            unityDraw = UnityDraw.GetDraw();
            DebugDraw = new DebugDraw { Draw = unityDraw };
            Global.DebugDraw = DebugDraw;

            Application.quitting += () => TestSettingHelper.Save(Settings);

            fixedUpdate = new FixedUpdate(TimeSpan.FromSeconds(1 / 60d), Tick);
            controller = new b2GUIController(this);
        }

        public void Start()
        {
            fixedUpdate.Start();
        }

        private void Update()
        {
            fixedUpdate.Update();
        }

        private void Tick()
        {
            fpsCounter.SetFps();
        }

        #region RenderUI

        public Action PreRenderCallback;

        private void OnPreRender()
        {
            PreRenderCallback?.Invoke();
        }

        private void OnEnable()
        {
            ImGuiUn.Layout += RenderUI;
        }

        private void OnDisable()
        {
            ImGuiUn.Layout -= RenderUI;
        }

        private void RenderUI()
        {
            controller.Render();
            DebugDraw.DrawString(5, 10, @"(F1) Reload  (F2) Pause  (F3) Single Step");
            if (Global.Settings.Pause)
            {
                DebugDraw.DrawString(5, 30, "****PAUSED***");
            }
            
            this.DebugDraw.DrawString(5,Global.Camera.Height - 60,$"Step: {Settings.StepCount}");
            DebugDraw.DrawString(5, Global.Camera.Height - 40, $"{fpsCounter.Ms:0.0} ms");
            DebugDraw.DrawString(5, Global.Camera.Height - 20, $"{fpsCounter.Fps:F1} fps");

            //Draw Profile
            if (Global.Settings.instanceId == 0) return;

            //call update unit profile
            // EventSystem.Instance?.Invoke(new UpdateUnitProfileCallback() { instanceId = Global.Settings.instanceId });

            DebugDraw.DrawString(5, 50, $"UnitName: {Profile.UnitName}");
            DebugDraw.DrawString(5, 65, $"LinearVelocity:{Profile.LinearVelocity}");
            DebugDraw.DrawString(5, 80, $"AngularVelocity:{Profile.AngularVelocity}");
            DebugDraw.DrawString(5, 95, $"Position:{Profile.Position}");
            DebugDraw.DrawString(5, 110, $"Behavior: ({Profile.BehaviorName})");
            DebugDraw.DrawString(5,125,$"MoveType: {Profile.MoveType}");
        }

        public UnitProfile Profile;

        #endregion
    }
}